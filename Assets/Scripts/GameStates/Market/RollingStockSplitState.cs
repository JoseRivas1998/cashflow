using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingStockSplitState : GameState
{

    private readonly StockSplitCard stockSplit;
    private readonly MarketCardGameObject marketCard;
    private readonly DiceContainer die;
    private readonly Player player;

    private bool done;
    private int dieValue;

    private float doneTime;
    private readonly float doneTimer = 2f;

    public RollingStockSplitState(MainGameManager mgm, StockSplitCard stockSplit, MarketCardGameObject marketCard)
    {
        this.stockSplit = stockSplit;
        this.marketCard = marketCard;
        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        this.die = mgm.SpawnDice(1)[0];

        this.marketCard.transform.position += (Vector3.right * this.marketCard.transform.position.x * 0.5f) + (Vector3.up * this.marketCard.transform.position.y * 0.45f);
        this.marketCard.transform.localScale = (Vector3.right + Vector3.up) * 0.75f;

        done = false;
        dieValue = -1;
        doneTime = 0;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(done)
        {
            doneTime += Time.deltaTime;
            if(doneTime > doneTimer)
            {
                mgm.mainCamTracker.TrackObject(this.player.gamePiece.transform);
                Object.Destroy(this.marketCard.gameObject);
                Object.Destroy(this.die.gameObject);
                return new PostTurnState(mgm);
            }
            return this;
        }
        if(Input.GetMouseButtonDown(0))
        {
            this.die.roller.Shake();
        }
        if(Input.GetMouseButtonUp(0))
        {
            this.die.roller.Roll();
            mgm.mainCamTracker.TrackObject(this.die.transform);
        }
        if(this.die.roller.RollComplete()) 
        {
            dieValue = this.die.dir.DieValue();
            if(dieValue == -1)
            {
                this.die.roller.ResetRoll();
                this.die.roller.Roll();
                return this;
            }
            Stocks.Stock stock = this.stockSplit.stock;
            if(dieValue <= 3)
            {
                mgm.gameStateDisplay.SetText($"{stock.symbol} splits!");
                for (int i = 0; i < mgm.NumPlayers; i++)
                {
                    Player player = mgm.GetPlayer(i);
                    if(player.incomeStatement.NumShares(stock.symbol) > 0)
                    {
                        player.SplitStock(stock.symbol);
                    }
                }
            } 
            else
            {
                mgm.gameStateDisplay.SetText($"{stock.symbol} reverse splits!");
                for (int i = 0; i < mgm.NumPlayers; i++)
                {
                    Player player = mgm.GetPlayer(i);
                    if (player.incomeStatement.NumShares(stock.symbol) > 0)
                    {
                        player.ReverseSplitStock(stock.symbol);
                    }
                }
            }
            done = true;
        } 
        else
        {
            this.doneTime = 0;
        }
        return this;
    }
}
