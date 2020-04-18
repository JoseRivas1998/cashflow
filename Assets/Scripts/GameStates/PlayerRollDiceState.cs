using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRollDiceState : GameState
{

    private DiceContainer[] dice;
    private readonly int currentSpace;

    public PlayerRollDiceState(MainGameManager mgm, int numDice)
    {
        dice = mgm.SpawnDice(numDice);
        currentSpace = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer()).space;
    } 

    public override GameState Update(MainGameManager mgm)
    {
        if(Input.GetMouseButtonDown(0))
        {
            ShakeAll();
        }
        if(Input.GetMouseButtonUp(0))
        {
            RollAll();
        }
        if(AllDone())
        {
            if(!RerollCocked())
            {
                int sum = dice.Sum(die => die.dir.DieValue());
                Debug.Log(sum);
                foreach (DiceContainer die in dice)
                {
                    Object.Destroy(die.gameObject);
                }
                int payDays = mgm.board.PayDays(currentSpace, sum);
                if (payDays == 0)
                {
                    return new PlayerMoving(mgm, sum);
                }
                // TODO check for MLM
                return new PaydayState(mgm, sum, payDays);
            }
        }
        return this;
    }

    private void ShakeAll()
    {
        foreach (DiceContainer die in dice)
        {
            die.roller.Shake();
        }
    }

    private void RollAll()
    {
        foreach (DiceContainer die in dice)
        {
            die.roller.Roll();
        }
    }

    private void ResetAll()
    {
        foreach (DiceContainer die in dice)
        {
            die.roller.ResetRoll();
        }
    }

    private bool AllDone()
    {
        bool result = true;
        for (int i = 0; i < dice.Length && result; i++)
        {
            result = result && dice[i].roller.RollComplete();
        }
        return result;
    }

    private bool RerollCocked() 
    {
        bool anyRolled = false;
        foreach (DiceContainer die in dice) 
        { 
            if(die.dir.DieValue() == -1)
            {
                die.roller.ResetRoll();
                die.roller.Roll();
                anyRolled = true;
            }
        }
        return anyRolled;
    }

}
