using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashflowDayState : GameState
{

    private readonly CashflowDayAnimation animation;
    private readonly int diceSum;

    public CashflowDayState(MainGameManager mgm, int diceSum, int cashflowDays)
    {
        this.diceSum = diceSum;
        Player player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        int cashflowDayIncome = player.fastTrackIncomeStatement.CashFlowDayIncome;
        for (int i = 0; i < cashflowDays; i++)
        {
            player.AddMoney(cashflowDayIncome);
        }
        animation = GameObject.Instantiate(mgm.cashflowDayAnimation, mgm.mainUICanvas.transform).GetComponent<CashflowDayAnimation>();
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (animation.Done)
        {
            GameObject.Destroy(animation.gameObject);
            return new FastTrackMoveState(mgm, diceSum);
        }
        return this;
    }
}
