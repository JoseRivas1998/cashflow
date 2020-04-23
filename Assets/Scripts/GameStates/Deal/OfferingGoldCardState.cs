using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferingGoldCardState : GameState
{

    private readonly GoldCard goldCard;
    private readonly DealCardGameObject dealCard;
    private readonly int offerAmount;
    private readonly CardOffer offer;
    private readonly Player currentPlayer;
    private readonly Player targetPlayer;

    private bool selected;
    private bool takeDeal;

    public OfferingGoldCardState(MainGameManager mgm, GoldCard goldCard, DealCardGameObject dealCard, int offerAmount, int targetPlayer)
    {
        this.goldCard = goldCard;
        this.dealCard = dealCard;
        this.offerAmount = offerAmount;
        this.currentPlayer = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        this.targetPlayer = mgm.GetPlayer(targetPlayer);
        if (!this.targetPlayer.GamePieceExists())
        {
            Vector3 spawnPosition = mgm.board.SpaceCenter(0) + (Vector3.up * 0.5f);
            mgm.SpawnGamePiece(this.targetPlayer.index);
            this.targetPlayer.gamePiece.origin = mgm.board.transform.position + mgm.board.ratRaceCenterOffset;
            this.targetPlayer.gamePiece.transform.position = spawnPosition;
        }
        mgm.mainCamTracker.TrackObject(this.targetPlayer.gamePiece.transform);

        GameObject offerObject = Object.Instantiate(mgm.cardOfferPrefab, mgm.mainUICanvas.transform);
        offerObject.transform.SetSiblingIndex(dealCard.transform.GetSiblingIndex() + 1);
        offer = offerObject.GetComponent<CardOffer>();
        offer.offerText.text = $"{currentPlayer.name} has offered this card for {Utility.FormatMoney(offerAmount)}";
        selected = false;
        takeDeal = false;
        offer.noDealBtn.onClick.AddListener(() => {
            if (selected) return;
            selected = true;
            takeDeal = false;
        });
        offer.takeDealBtn.onClick.AddListener(() => {
            if (selected || this.targetPlayer.ledger.GetCurretBalance() < offerAmount) return;
            selected = true;
            takeDeal = true;
        });
        offer.takeDealBtn.interactable = this.targetPlayer.ledger.GetCurretBalance() > offerAmount;
        this.dealCard.gameObject.SetActive(true);
        mgm.gameStateDisplay.SetText(this.targetPlayer.name);

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (selected)
        {
            Object.Destroy(this.offer.gameObject);
            if (takeDeal)
            {
                targetPlayer.SubtractMoney(offerAmount);
                currentPlayer.AddMoney(offerAmount);
                return new GoldBuyOnlyState(mgm, goldCard, dealCard, targetPlayer.index);
            }
            else
            {
                mgm.gameStateDisplay.gameObject.SetActive(false);
                mgm.mainCamTracker.TrackObject(currentPlayer.gamePiece.transform);
                dealCard.gameObject.SetActive(false);
                return new SellingGoldCardState(mgm, goldCard, dealCard);
            }
        }
        offer.takeDealBtn.interactable = this.targetPlayer.ledger.GetCurretBalance() > offerAmount;
        return this;
    }
}
