using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaydayState : GameState
{

    private readonly int diceSum;
    private readonly int payDays;

    private readonly bool skip;

    public PaydayState(MainGameManager mgm, int diceSum, int payDays)
    {
        mgm.gameStateDisplay.gameObject.SetActive(false);
        this.diceSum = diceSum;
        this.payDays = payDays;
        Player player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        int monthlyCashFlow = player.incomeStatement.MonthlyCashflow;
        if (player.ledger.GetCurretBalance() + monthlyCashFlow < 0)
        {
            skip = true;
            return;
        }
        skip = false;
        for (int i = 0; i < payDays; i++)
        {
            player.AddMoney(monthlyCashFlow);
        }
        mgm.payDayAnimation.gameObject.SetActive(true);
        mgm.payDayAnimation.ResetAnimation();
        mgm.payDayAnimation.StartAnimation();
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (skip)
        {
            return new BankruptOptionsState(mgm, this.diceSum, this.payDays);
        }
        if (mgm.payDayAnimation.AnimatorDone())
        {
            mgm.payDayAnimation.ResetAnimation();
            mgm.payDayAnimation.gameObject.SetActive(false);
            return new PlayerMoving(mgm, diceSum);
        }
        return this;
    }
}
