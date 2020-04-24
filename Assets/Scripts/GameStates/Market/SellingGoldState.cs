using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingGoldState : GameState
{
    private readonly int startingPlayerIndex;
    private readonly GoldBuyerCard goldBuyer;
    private readonly MarketCardGameObject marketCard;
    private readonly YesNoOptions options;

    private int currentPlayerIndex;
    private Player currentPlayer;

    private bool done;
    private bool next;
    private bool selected;
    private bool willSell;
    private bool selling;

    public SellingGoldState(MainGameManager mgm, GoldBuyerCard goldBuyer, MarketCardGameObject marketCard)
    {
        mgm.turnManager.Push();
        this.startingPlayerIndex = mgm.turnManager.GetCurrentPlayer();
        this.goldBuyer = goldBuyer;
        this.marketCard = marketCard;

        GameObject yesNo = Object.Instantiate(mgm.yesNoOptionsPrefab, mgm.mainUICanvas.transform);
        yesNo.transform.SetSiblingIndex(marketCard.gameObject.transform.GetSiblingIndex() + 1);
        this.options = yesNo.GetComponent<YesNoOptions>();
        this.options.prompt.text = "Sell Some Gold?";

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
        next = false;
        selected = false;
        willSell = false;
        selling = false;

        currentPlayerIndex = startingPlayerIndex;
        currentPlayer = mgm.GetPlayer(currentPlayerIndex);

        if(currentPlayer.incomeStatement.goldCoins == 0)
        {
            do
            {
                currentPlayerIndex = mgm.turnManager.NextPlayer();
                currentPlayer = mgm.GetPlayer(currentPlayerIndex);
            } while (currentPlayerIndex != startingPlayerIndex && currentPlayer.incomeStatement.goldCoins == 0);
            if(currentPlayerIndex == startingPlayerIndex)
            {
                done = true;
            }
        }

        if (!done)
        {
            mgm.mainCamTracker.TrackObject(currentPlayer.gamePiece.transform);
            mgm.gameStateDisplay.SetText($"{currentPlayer.name} is selling gold.");
        }

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (selling)
        {
            willSell = false;
            next = true;
            selected = false;
            selling = false;
            this.options.gameObject.SetActive(true);
            this.marketCard.gameObject.SetActive(true);
        }
        if (next)
        {
            next = false;
            do
            {
                currentPlayerIndex = mgm.turnManager.NextPlayer();
                currentPlayer = mgm.GetPlayer(currentPlayerIndex);
            } while (currentPlayerIndex != startingPlayerIndex && currentPlayer.incomeStatement.goldCoins == 0);
            if (currentPlayerIndex == startingPlayerIndex)
            {
                done = true;
            }
            else
            {
                mgm.mainCamTracker.TrackObject(currentPlayer.gamePiece.transform);
                mgm.gameStateDisplay.SetText($"{currentPlayer.name} is selling gold.");
            }
        }
        if (done)
        {
            Object.Destroy(this.options.gameObject);
            Object.Destroy(this.marketCard.gameObject);
            currentPlayerIndex = mgm.turnManager.Pop();
            currentPlayer = mgm.GetPlayer(currentPlayerIndex);
            mgm.mainCamTracker.TrackObject(currentPlayer.gamePiece.transform);
            return new PostTurnState(mgm);
        }
        return this;
    }
}
