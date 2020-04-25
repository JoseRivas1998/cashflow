using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SellingRealEstateMarketState : GameState
{
    private readonly RealEstateMarketCard buyerCard;
    private readonly MarketCardGameObject marketCard;
    private readonly int startingPlayerIndex;
    private readonly YesNoOptions options;

    private int currentPlayerIndex;
    private Player currentPlayer;

    private bool selected;
    private bool willSell;
    private bool selling;
    private bool next;
    private bool done;

    public SellingRealEstateMarketState(MainGameManager mgm, RealEstateMarketCard buyerCard, MarketCardGameObject marketCard)
    {
        this.buyerCard = buyerCard;
        this.marketCard = marketCard;

        GameObject yesNoOptions = Object.Instantiate(mgm.yesNoOptionsPrefab, mgm.mainUICanvas.transform);
        yesNoOptions.transform.SetSiblingIndex(marketCard.transform.GetSiblingIndex() + 1);
        this.options = yesNoOptions.GetComponent<YesNoOptions>();
        this.options.prompt.text = "Sell Real Estate?";
        this.options.yes.onClick.AddListener(() => {
            if (selected) return;
            selected = true;
            willSell = true;
        });
        this.options.no.onClick.AddListener(() => {
            if (selected) return;
            selected = true;
            willSell = false;
        });

        done = false;
        selected = false;
        willSell = false;
        selling = false;
        next = false;

        this.startingPlayerIndex = mgm.turnManager.GetCurrentPlayer();
        mgm.turnManager.Push();

        currentPlayerIndex = this.startingPlayerIndex;
        currentPlayer = mgm.GetPlayer(currentPlayerIndex);

        if(!CurrentPlayerHasRealEstateToSell())
        {
            do
            {
                this.currentPlayerIndex = mgm.turnManager.NextPlayer();
                this.currentPlayer = mgm.GetPlayer(currentPlayerIndex);
            } while (this.currentPlayerIndex != this.startingPlayerIndex && !this.CurrentPlayerHasRealEstateToSell());
            if(this.currentPlayerIndex == this.startingPlayerIndex)
            {
                done = true;
            }
        }
        if(!done)
        {
            mgm.mainCamTracker.TrackObject(this.currentPlayer.gamePiece.transform);
            mgm.gameStateDisplay.SetText($"{this.currentPlayer.name} is selling real estate.");
        }

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (selling)
        {
            next = true;
            selected = false;
            willSell = false;
            selling = false;
            this.options.gameObject.SetActive(true);
            this.marketCard.gameObject.SetActive(true);
        }
        if (next)
        {
            next = false;
            do
            {
                this.currentPlayerIndex = mgm.turnManager.NextPlayer();
                this.currentPlayer = mgm.GetPlayer(currentPlayerIndex);
            } while (this.currentPlayerIndex != this.startingPlayerIndex && !this.CurrentPlayerHasRealEstateToSell());
            if (this.currentPlayerIndex == this.startingPlayerIndex)
            {
                done = true;
            }
            else
            {
                mgm.mainCamTracker.TrackObject(this.currentPlayer.gamePiece.transform);
                mgm.gameStateDisplay.SetText($"{this.currentPlayer.name} is selling real estate.");
            }
        }
        if (done)
        {
            Object.Destroy(this.marketCard.gameObject);
            Object.Destroy(this.options.gameObject);
            currentPlayerIndex = mgm.turnManager.Pop();
            currentPlayer = mgm.GetPlayer(currentPlayerIndex);
            mgm.mainCamTracker.TrackObject(currentPlayer.gamePiece.transform);
            return new PostTurnState(mgm);
        }
        if (selected)
        {
            if (willSell)
            {
                this.options.gameObject.SetActive(false);
                this.marketCard.gameObject.SetActive(false);
                selling = true;
                return new SelectingPropertiesToSellMarketState(mgm, buyerCard, marketCard, this);
            }
            selected = false;
            next = true;
        }
        return this;
    }

    private bool CurrentPlayerHasRealEstateToSell()
    {
        return currentPlayer.incomeStatement.RealEstate().Where(RealEstateMarketCard.GetRealEstatePredicate(buyerCard)).Any();
    }

}
