using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FastTrackRollingState : GameState
{

    private DiceContainer[] dice;
    private readonly int currentSpace;
    private Player player;


    public FastTrackRollingState(MainGameManager mgm, int numDice)
    {
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        dice = mgm.SpawnDice(numDice);
        currentSpace = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer()).space;
        ResetAll();
        FreezeAll();
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (Input.GetMouseButtonDown(0))
        {
            UnFreezeAll();
            ShakeAll();
        }
        if (Input.GetMouseButtonUp(0))
        {
            RollAll();
            mgm.mainCamTracker.TrackObject(dice[0].transform);
        }
        if (AllDone())
        {
            if (!RerollCocked())
            {
                mgm.mainCamTracker.TrackObject(player.gamePiece.transform);
                int sum = dice.Sum(die => die.dir.DieValue());
                foreach (DiceContainer die in dice)
                {
                    Object.Destroy(die.gameObject);
                }
                return new PreTurn(mgm);
            }
        }
        return this;
    }

    private void FreezeAll()
    {
        foreach (DiceContainer die in dice)
        {
            die.roller.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        }
    }

    private void UnFreezeAll()
    {
        foreach (DiceContainer die in dice)
        {
            die.roller.rb.constraints = RigidbodyConstraints.None;
        }
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
            if (die.dir.DieValue() == -1)
            {
                die.roller.ResetRoll();
                die.roller.Roll();
                anyRolled = true;
            }
        }
        return anyRolled;
    }
}
