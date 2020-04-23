using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambleCardOptionsState : GameState
{
    private enum Choices
    {
        Yes, No, Loan
    }

    private readonly GambleCard gamble;
    private readonly DealCardGameObject dealCard;
    private readonly Player player;
    private readonly GambleOptions gambleOptions;

    private bool selected;
    private bool takingLoan;
    private Choices choice;

    public GambleCardOptionsState(MainGameManager mgm, GambleCard gamble, DealCardGameObject dealCard)
    {
        this.gamble = gamble;
        this.dealCard = dealCard;
        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        GameObject options = Object.Instantiate(mgm.gambleOptionsPrefab, mgm.mainUICanvas.transform);
        options.transform.SetSiblingIndex(dealCard.transform.GetSiblingIndex() + 1);

        gambleOptions = options.GetComponent<GambleOptions>();
        gambleOptions.yesBtn.onClick.AddListener(() => {
            if (selected) return;
            selected = true;
            choice = Choices.Yes;
        });
        gambleOptions.noBtn.onClick.AddListener(() => {
            if (selected) return;
            selected = true;
            choice = Choices.No;
        });
        gambleOptions.loanBtn.onClick.AddListener(() => {
            if (selected) return;
            selected = true;
            choice = Choices.Loan;
        });
        gambleOptions.yesBtn.interactable = player.ledger.GetCurretBalance() >= gamble.cost;

        selected = false;
        takingLoan = false;

    }

    public override GameState Update(MainGameManager mgm)
    {
        if(takingLoan)
        {
            takingLoan = false;
            selected = false;
            dealCard.gameObject.SetActive(true);
            gambleOptions.gameObject.SetActive(true);
        }
        if(selected)
        {
            switch (choice)
            {
                case Choices.Yes:
                    Object.Destroy(gambleOptions.gameObject);
                    if(gamble.mlm)
                    {
                        Object.Destroy(dealCard.gameObject);
                        player.InvestMLM(gamble);
                        return new PostTurnState(mgm);
                    }
                    player.SubtractMoney(gamble.cost);
                    return new RollingGambleState(mgm, gamble, dealCard);
                case Choices.No:
                    Object.Destroy(dealCard.gameObject);
                    Object.Destroy(gambleOptions.gameObject);
                    return new PostTurnState(mgm);
                case Choices.Loan:
                    takingLoan = true;
                    dealCard.gameObject.SetActive(false);
                    gambleOptions.gameObject.SetActive(false);
                    return new LoanState(mgm, this);
            }
        }
        gambleOptions.yesBtn.interactable = player.ledger.GetCurretBalance() >= gamble.cost;
        return this;
    }
}
