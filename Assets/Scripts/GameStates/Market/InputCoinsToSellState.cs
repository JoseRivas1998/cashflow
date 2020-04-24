using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCoinsToSellState : GameState
{
    private readonly GoldBuyerCard goldBuyer;
    private readonly NumberGoldInput input;
    private readonly Player player;
    private readonly SellingGoldState previousState;

    private bool done;

    public InputCoinsToSellState(MainGameManager mgm, GoldBuyerCard goldBuyer, MarketCardGameObject marketCard, SellingGoldState previousState)
    {
        this.goldBuyer = goldBuyer;
        this.previousState = previousState;
        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        GameObject numInput = Object.Instantiate(mgm.numberCoinsInput, mgm.mainUICanvas.transform);
        numInput.transform.SetSiblingIndex(marketCard.transform.GetSiblingIndex());
        input = numInput.GetComponent<NumberGoldInput>();
        input.Initialize(goldBuyer);

        input.numberInput.maxNumber = player.incomeStatement.goldCoins;
        input.numberInput.confirmBtn.onClick.AddListener(() => {
            if (done) return;
            done = true;
        });

        done = false;

        mgm.gameStateDisplay.gameObject.SetActive(false);

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (done)
        {
            int numCoins = input.numberInput.Number;
            Object.Destroy(input.gameObject);
            if(numCoins > 0)
            {
                player.SellGold(goldBuyer.offer, numCoins);
            }
            mgm.financialStatementToggle.Close();
            mgm.cashLedgerToggle.Close();
            return this.previousState;
        }
        return this;
    }
}
