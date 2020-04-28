using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySellRealEstateState : GameState
{

    private enum Choices
    {
        Sell, Buy, Loan
    }

    private readonly int originalBuyerIndex;
    private readonly Player originalBuyer;
    private readonly RealEstateCard realEstateCard;
    private readonly DealCardGameObject dealCard;
    private readonly BuyPropertyOptions options;

    private bool selected;
    private bool isTakingLoan;
    private Choices choice;

    public BuySellRealEstateState(MainGameManager mgm, RealEstateCard realEstateCard, DealCardGameObject dealCard)
    {
        this.originalBuyerIndex = mgm.turnManager.GetCurrentPlayer();
        this.originalBuyer = mgm.GetPlayer(originalBuyerIndex);
        this.realEstateCard = realEstateCard;
        this.dealCard = dealCard;
        GameObject optionsObject = Object.Instantiate(mgm.buyPropertyOptionsPrefab, mgm.mainUICanvas.transform);
        optionsObject.transform.SetSiblingIndex(dealCard.transform.GetSiblingIndex() + 1);
        this.options = optionsObject.GetComponent<BuyPropertyOptions>();
        this.options.sellBtn.interactable = mgm.turnManager.NumPlayersIn() > 1;
        this.options.buyBtn.interactable = originalBuyer.ledger.GetCurretBalance() >= realEstateCard.downPayment;
        this.selected = false;
        this.isTakingLoan = false;
        if(mgm.turnManager.NumPlayersIn() > 1)
        {
            this.options.sellBtn.onClick.AddListener(() => {
                if (selected) return;
                selected = true;
                this.choice = Choices.Sell;
            });
        }
        this.options.buyBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            this.choice = Choices.Buy;
        });
        this.options.loanBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            this.choice = Choices.Loan;
        });
        mgm.gameStateDisplay.gameObject.SetActive(false);
    }


    public override GameState Update(MainGameManager mgm)
    {
        if (isTakingLoan)
        {
            selected = false;
            isTakingLoan = false;
            this.options.gameObject.SetActive(true);
            this.dealCard.gameObject.SetActive(true);
        }
        if (selected)
        {
            switch (this.choice)
            {
                case Choices.Sell:
                    Object.Destroy(this.options.gameObject);
                    this.dealCard.gameObject.SetActive(false);
                    return new SellingRealEstateCardState(mgm, realEstateCard, dealCard);
                case Choices.Buy:
                    if (originalBuyer.ledger.GetCurretBalance() >= realEstateCard.downPayment)
                    {
                        Object.Destroy(this.dealCard.gameObject);
                        Object.Destroy(this.options.gameObject);
                        originalBuyer.BuyRealEstate(this.realEstateCard);
                        return new PostTurnState(mgm);
                    }
                    else
                    {
                        selected = false;
                    }
                    break;
                case Choices.Loan:
                    isTakingLoan = true;
                    selected = false;
                    this.options.gameObject.SetActive(false);
                    this.dealCard.gameObject.SetActive(false);
                    return new LoanState(mgm, this);
                default:
                    break;
            }
        }
        this.options.buyBtn.interactable = originalBuyer.ledger.GetCurretBalance() >= realEstateCard.downPayment;
        return this;
    }

}
