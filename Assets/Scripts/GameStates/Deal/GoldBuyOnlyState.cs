using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBuyOnlyState : GameState
{

    private readonly GoldCard goldCard;
    private readonly DealCardGameObject dealCard;
    private readonly Player buyer;
    private RealEstateBuyOnlyOption options;

    private bool selected;
    private bool takingLoan;
    private bool willBuy;

    public GoldBuyOnlyState(MainGameManager mgm, GoldCard goldCard, DealCardGameObject dealCard, int buyerIndex)
    {
        this.goldCard = goldCard;
        this.dealCard = dealCard;
        this.buyer = mgm.GetPlayer(buyerIndex);

        GameObject optionsObject = Object.Instantiate(mgm.goldBuyOnlyOptionPrefab, mgm.mainUICanvas.transform);
        optionsObject.transform.SetSiblingIndex(dealCard.transform.GetSiblingIndex() + 1);
        options = optionsObject.GetComponent<RealEstateBuyOnlyOption>();

        options.buyBtn.interactable = this.buyer.ledger.GetCurretBalance() >= goldCard.cost;
        options.buyBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            willBuy = true;
            selected = true;
        });

        options.loanBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            willBuy = false;
            selected = true;
        });

        selected = false;
        takingLoan = false;
        willBuy = false;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (takingLoan)
        {
            takingLoan = false;
            selected = false;
            this.dealCard.gameObject.SetActive(true);
            this.options.gameObject.SetActive(true);
        }
        if (selected)
        {
            if (willBuy)
            {
                this.buyer.SubtractMoney(this.goldCard.cost);
                this.buyer.incomeStatement.AddGold(this.goldCard.coins);
                Object.Destroy(this.dealCard.gameObject);
                Object.Destroy(this.options.gameObject);
                Player currentPlayer = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
                mgm.mainCamTracker.TrackObject(currentPlayer.gamePiece.transform);
                return new PostTurnState(mgm);
            }
            takingLoan = true;
            this.dealCard.gameObject.SetActive(false);
            this.options.gameObject.SetActive(false);
            return new LoanState(mgm, this, this.buyer.index);
        }
        options.buyBtn.interactable = this.buyer.ledger.GetCurretBalance() >= goldCard.cost;
        return this;
    }
}
