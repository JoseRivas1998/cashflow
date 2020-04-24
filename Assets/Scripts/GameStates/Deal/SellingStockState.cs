using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingStockState : GameState
{
    private readonly int startingPlayerIndex;
    private int currentPlayerIndex;
    private Player currentPlayer;

    private readonly StockCard stockCard;
    private readonly DealCardGameObject dealCard;
    private readonly YesNoOptions options;
    private bool done;
    private bool next;
    private bool selected;
    private bool willSell;
    private bool isSelling;

    public SellingStockState(MainGameManager mgm, StockCard stockCard, DealCardGameObject dealCard)
    {
        this.stockCard = stockCard;
        this.dealCard = dealCard;
        GameObject sellOptions = Object.Instantiate(mgm.yesNoOptionsPrefab, mgm.mainUICanvas.transform);
        sellOptions.transform.SetSiblingIndex(this.dealCard.transform.GetSiblingIndex() + 1);
        this.options = sellOptions.GetComponent<YesNoOptions>();
        this.options.prompt.text = "Sell this stock?";
        this.dealCard.gameObject.SetActive(true);
        mgm.turnManager.Push(); // save this in case things get messed, this might be unnessasary 
        startingPlayerIndex = mgm.turnManager.GetCurrentPlayer();
        currentPlayerIndex = startingPlayerIndex;
        currentPlayer = mgm.GetPlayer(currentPlayerIndex);
        done = false;
        next = false;
        if (currentPlayer.incomeStatement.NumShares(stockCard.stock.symbol) == 0)
        {
            // if the player who drew the card does not have shares, look for the other players and find someone who does have the stock, otherwise we will return to the startingplayer
            do
            {
                currentPlayerIndex = mgm.turnManager.NextPlayer();
                currentPlayer = mgm.GetPlayer(currentPlayerIndex);
            } while (currentPlayerIndex != startingPlayerIndex && currentPlayer.incomeStatement.NumShares(stockCard.stock.symbol) == 0);
            // if we found no player with this stock, we would have looped back to the starting player which means that we skip the sell state
            if (currentPlayerIndex == startingPlayerIndex)
            {
                done = true;
            }
        }
        if (!done)
        {
            mgm.mainCamTracker.TrackObject(currentPlayer.gamePiece.transform);
            mgm.gameStateDisplay.SetText(currentPlayer.name + " is selling " + stockCard.stock.symbol);
        }
        selected = false;
        options.yes.onClick.AddListener(() => 
        {
            if (selected) return;
            selected = true;
            willSell = true;
        });
        options.no.onClick.AddListener(() => 
        {
            if (selected) return;
            selected = true;
            willSell = false;
        });
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(isSelling)
        {
            isSelling = false;
            willSell = false;
            next = true;
            options.gameObject.SetActive(true);
        }
        if (next)
        {
            do
            {
                currentPlayerIndex = mgm.turnManager.NextPlayer();
                currentPlayer = mgm.GetPlayer(currentPlayerIndex);
            } while (currentPlayerIndex != startingPlayerIndex && currentPlayer.incomeStatement.NumShares(stockCard.stock.symbol) == 0);
            if (currentPlayerIndex == startingPlayerIndex)
            {
                done = true;
            }
            else
            {
                mgm.mainCamTracker.TrackObject(currentPlayer.gamePiece.transform);
                mgm.gameStateDisplay.SetText(currentPlayer.name + " is selling " + stockCard.stock.symbol);
            }
            selected = false; // open up options again
            next = false;
        }
        if (done)
        {
            Object.Destroy(options.gameObject);
            Object.Destroy(dealCard.gameObject);
            mgm.mainCamTracker.TrackObject(currentPlayer.gamePiece.transform);
            currentPlayerIndex = mgm.turnManager.Pop(); // just in case restore original player index
            currentPlayer = mgm.GetPlayer(currentPlayerIndex);
            mgm.mainCamTracker.TrackObject(currentPlayer.gamePiece.transform);
            return new PostTurnState(mgm);
        }
        if (selected)
        {
            if(willSell)
            {
                isSelling = true;
                options.gameObject.SetActive(false);
                return new EnteringNumberStocksToSellState(mgm, this.stockCard, this.dealCard, this);
            } 
            else
            {
                next = true;
            }
        }
        return this;
    }
}
