using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteringNumberStocksToBuyState : GameState
{

    private readonly StockCard stockCard;
    private readonly DealCardGameObject dealCard;
    private readonly NumberOfSharesInput sharesInput;
    private readonly Player player;
    private bool done;

    public EnteringNumberStocksToBuyState(MainGameManager mgm, DealCard deal, DealCardGameObject dealCard)
    {
        this.stockCard = (StockCard)deal;
        this.dealCard = dealCard;
        this.dealCard.gameObject.SetActive(false);

        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        GameObject gameObject = Object.Instantiate(mgm.numberStocksInputPrefab, mgm.mainUICanvas.transform);
        gameObject.transform.SetSiblingIndex(this.dealCard.transform.GetSiblingIndex() + 1);
        sharesInput = gameObject.GetComponent<NumberOfSharesInput>();
        sharesInput.numberInput.maxNumber = player.ledger.GetCurretBalance() / this.stockCard.price;
        sharesInput.Initialize(this.stockCard);
        done = false;
        sharesInput.numberInput.confirmBtn.onClick.AddListener(() =>
        {
            if (done) return;
            done = true;
        });
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(done)
        {
            int numShares = sharesInput.numberInput.Number;
            Object.DestroyImmediate(sharesInput.gameObject);
            player.BuyStock(stockCard, numShares);
            return new LoopState();
        }
        return this;
    }
}
