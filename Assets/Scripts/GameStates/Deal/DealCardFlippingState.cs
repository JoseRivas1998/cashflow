using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealCardFlippingState : GameState
{

    private readonly DealCard dealCard;
    private readonly DealCardGameObject dealCardObject;

    public DealCardFlippingState(MainGameManager mgm, bool smallDeal)
    {
        GameObject gameObject = Object.Instantiate(mgm.dealCardPrefab, mgm.mainUICanvas.transform);
        gameObject.transform.SetSiblingIndex(mgm.cashLedgerToggle.transform.GetSiblingIndex());
        dealCard = smallDeal ? mgm.PullSmallDealCard() : mgm.PullBigDeal();
        dealCardObject = gameObject.GetComponent<DealCardGameObject>();
        dealCardObject.SetDeal(dealCard);
        mgm.gameStateDisplay.gameObject.SetActive(false);
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(dealCardObject.cardFlip.FlipReadyBack())
        {
            dealCardObject.cardFlip.BeginFlip();
        }
        if(dealCardObject.cardFlip.FlipReadyFront())
        {
            switch (dealCard.type)
            {
                case DealType.Stock:
                    return new BuyingStockState(mgm, dealCard, dealCardObject);
                case DealType.RealEstate:
                    break;
                case DealType.Gold:
                    break;
                case DealType.Gamble:
                    break;
                default:
                    break;
            }
        }
        return this;
    }
}
