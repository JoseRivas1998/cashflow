using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySellGoldState : GameState
{

    private enum Choices
    {
        Sell, Buy, Loan
    }

    private readonly int originalBuyerIndex;
    private readonly Player originalBuyer;
    private readonly GoldCard goldCard;
    private readonly DealCardGameObject dealCard;
    private readonly BuyPropertyOptions options;

    private bool selected;
    private bool isTakingLoan;
    private Choices choice;

    public BuySellGoldState(MainGameManager mgm, GoldCard goldCard, DealCardGameObject dealCard)
    {
        this.originalBuyerIndex = mgm.turnManager.GetCurrentPlayer();
        this.originalBuyer = mgm.GetPlayer(this.originalBuyerIndex);
        this.goldCard = goldCard;
        this.dealCard = dealCard;

        GameObject optionsObject = Object.Instantiate(mgm.buyGoldOptionsPrefab, mgm.mainUICanvas.transform);
        optionsObject.transform.SetSiblingIndex(dealCard.transform.GetSiblingIndex() + 1);
        options = optionsObject.GetComponent<BuyPropertyOptions>();

        options.sellBtn.interactable = mgm.turnManager.NumPlayersIn() > 1;
        options.sellBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            choice = Choices.Sell;
        });

        options.buyBtn.interactable = originalBuyer.ledger.GetCurretBalance() >= goldCard.cost;
        options.buyBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            choice = Choices.Buy;
        });

        options.loanBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            choice = Choices.Loan;
        });

        selected = false;
        isTakingLoan = false;

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (isTakingLoan)
        {
            isTakingLoan = false;
            selected = false;
            dealCard.gameObject.SetActive(true);
            options.gameObject.SetActive(true);
        }
        if(selected)
        {
            switch (this.choice)
            {
                case Choices.Sell:
                    dealCard.gameObject.SetActive(false);
                    Object.Destroy(options.gameObject);
                    return new SellingGoldCardState(mgm, goldCard, dealCard);
                case Choices.Buy:
                    originalBuyer.SubtractMoney(goldCard.cost);
                    originalBuyer.incomeStatement.AddGold(goldCard.coins);
                    Object.Destroy(dealCard.gameObject);
                    Object.Destroy(options.gameObject);
                    return new PostTurnState(mgm);
                case Choices.Loan:
                    isTakingLoan = true;
                    dealCard.gameObject.SetActive(false);
                    options.gameObject.SetActive(false);
                    return new LoanState(mgm, this);
            }
        }
        options.buyBtn.interactable = originalBuyer.ledger.GetCurretBalance() >= goldCard.cost;
        return this;
    }

}
