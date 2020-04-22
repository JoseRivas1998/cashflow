using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealEstateBuyOnlyState : GameState
{

    private readonly Player buyer;
    private readonly RealEstateBuyOnlyOption option;
    private readonly RealEstateCard realEstate;
    private readonly DealCardGameObject dealCard;

    private bool selected;
    private bool willBuy;
    private bool takingLoan;

    public RealEstateBuyOnlyState(MainGameManager mgm, RealEstateCard realEstate, DealCardGameObject dealCard, int buyerIndex)
    {
        this.buyer = mgm.GetPlayer(buyerIndex);
        this.realEstate = realEstate;
        this.dealCard = dealCard;

        selected = false;
        willBuy = false;
        takingLoan = true;

        GameObject optionsObject = Object.Instantiate(mgm.realEstateBuyOnlyOptionPrefab, mgm.mainUICanvas.transform);
        optionsObject.transform.SetSiblingIndex(this.dealCard.transform.GetSiblingIndex() + 1);
        option = optionsObject.GetComponent<RealEstateBuyOnlyOption>();
        option.buyBtn.interactable = this.buyer.ledger.GetCurretBalance() >= realEstate.downPayment;

        option.buyBtn.onClick.AddListener(() =>
        {
            if (selected || this.buyer.ledger.GetCurretBalance() < realEstate.downPayment) return;
            selected = true;
            willBuy = true;
        });
        option.loanBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            willBuy = false;
        });

    }

    public override GameState Update(MainGameManager mgm)
    {
        if(takingLoan)
        {
            selected = false;
            willBuy = false;
            takingLoan = false;
            mgm.gameStateDisplay.SetText(buyer.name);
            option.gameObject.SetActive(true);
            dealCard.gameObject.SetActive(true);
        }
        if(selected)
        {
            if(willBuy)
            {
                buyer.BuyRealEstate(this.realEstate);
                Object.Destroy(option.gameObject);
                Object.Destroy(dealCard.gameObject);
                Player currentPlayer = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
                mgm.mainCamTracker.TrackObject(currentPlayer.gamePiece.transform);
                return new PostTurnState(mgm);
            }
            takingLoan = true;
            option.gameObject.SetActive(false);
            dealCard.gameObject.SetActive(false);
            return new LoanState(mgm, this, buyer.index);
        }
        option.buyBtn.interactable = this.buyer.ledger.GetCurretBalance() >= realEstate.downPayment;
        return this;
    }
}
