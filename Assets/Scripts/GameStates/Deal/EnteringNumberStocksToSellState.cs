using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteringNumberStocksToSellState : GameState
{
    private readonly StockCard stockCard;
    private readonly DealCardGameObject dealCard;
    private readonly NumberOfSharesInput sharesInput;
    private readonly Player player;
    private bool done;
    private readonly SellingStockState previousState;

    public EnteringNumberStocksToSellState(MainGameManager mgm, StockCard stock, DealCardGameObject dealCard, SellingStockState previousState)
    {
        this.previousState = previousState;
        this.stockCard = stock;
        this.dealCard = dealCard;
        this.dealCard.gameObject.SetActive(false);

        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        GameObject gameObject = Object.Instantiate(mgm.numberStocksInputPrefab, mgm.mainUICanvas.transform);
        gameObject.transform.SetSiblingIndex(this.dealCard.transform.GetSiblingIndex() + 1);
        sharesInput = gameObject.GetComponent<NumberOfSharesInput>();
        sharesInput.numberInput.maxNumber = player.incomeStatement.NumShares(this.stockCard.stock.symbol);
        sharesInput.Initialize(this.stockCard);
        done = false;
        sharesInput.numberInput.confirmBtn.onClick.AddListener(() =>
        {
            if (done) return;
            done = true;
        });
        mgm.gameStateDisplay.gameObject.SetActive(false);
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (done)
        {
            int numShares = sharesInput.numberInput.Number;
            Object.DestroyImmediate(sharesInput.gameObject);
            player.SellStock(stockCard, numShares);
            dealCard.gameObject.SetActive(true);
            mgm.financialStatementToggle.Close();
            mgm.cashLedgerToggle.Close();
            return this.previousState;
        }
        return this;
    }
}
