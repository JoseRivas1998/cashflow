using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankruptOptionsState : GameState
{
    private enum Choices
    {
        SellProperty, DropOut, CollectPayDay, PayDebt
    }

    private readonly int diceSum;
    private readonly int payDays;

    private readonly Player player;
    private readonly BankruptOptions options;

    private bool selected;
    private Choices choice;
    private bool onOtherScreen;

    public BankruptOptionsState(MainGameManager mgm, int diceSum, int payDays)
    {
        this.diceSum = diceSum;
        this.payDays = payDays;

        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        GameObject optionsObject = Object.Instantiate(mgm.bankruptOptionsPrefab, mgm.mainUICanvas.transform);
        optionsObject.transform.SetSiblingIndex(mgm.cashLedgerToggle.transform.GetSiblingIndex());
        this.options = optionsObject.GetComponent<BankruptOptions>();

        this.options.collectPayday.interactable = false;
        this.options.collectPayday.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            choice = Choices.CollectPayDay;
        });

        this.options.payDebt.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            choice = Choices.PayDebt;
        });

        this.options.sellProperty.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            choice = Choices.SellProperty;
        });


        this.options.dropOut.onClick.AddListener(() => 
        {
            if (selected) return;
            selected = true;
            choice = Choices.DropOut;
        });

        selected = false;
        onOtherScreen = false;

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (onOtherScreen)
        {
            onOtherScreen = false;
            selected = false;
            this.options.gameObject.SetActive(true);
        }
        if (selected)
        {
            switch (this.choice)
            {
                case Choices.SellProperty:
                    onOtherScreen = true;
                    this.options.gameObject.SetActive(false);
                    return new BankruptSellingPropertiesState(mgm, this);
                case Choices.DropOut:
                    try
                    {
                        Object.Destroy(this.options.gameObject);
                        mgm.DropOutPlayer(this.player.index);
                    } catch(MissingReferenceException mre) { }
                    mgm.mainCamTracker.TrackObject(null);
                    return this;
                case Choices.CollectPayDay:
                    Object.Destroy(this.options.gameObject);
                    return new PaydayState(mgm, diceSum, payDays);
                case Choices.PayDebt:
                    onOtherScreen = true;
                    this.options.gameObject.SetActive(false);
                    return new PayingDebtState(mgm, this);
                default:
                    break;
            }
        }
        this.options.collectPayday.interactable = player.incomeStatement.MonthlyCashflow > 0;
        return this;
    }
}
