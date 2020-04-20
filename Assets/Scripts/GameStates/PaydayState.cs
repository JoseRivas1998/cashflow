using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaydayState : GameState
{

    private readonly int diceSum;

    public PaydayState(MainGameManager mgm, int diceSum, int payDays) 
    {
        mgm.gameStateDisplay.gameObject.SetActive(false);
        Player player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        int monthlyCashFlow = player.incomeStatement.MonthlyCashflow;
        for (int i = 0; i < payDays; i++)
        {
            player.AddMoney(monthlyCashFlow);
        }
        mgm.payDayAnimation.gameObject.SetActive(true);
        mgm.payDayAnimation.ResetAnimation();
        mgm.payDayAnimation.StartAnimation();
        this.diceSum = diceSum;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(mgm.payDayAnimation.AnimatorDone())
        {
            mgm.payDayAnimation.ResetAnimation();
            mgm.payDayAnimation.gameObject.SetActive(false);
            return new PlayerMoving(mgm, diceSum);
        }
        return this;
    }
}
