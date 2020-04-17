using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderState : GameState
{

    private DiceContainer die;
    private int currentPlayer;

    private float rollTime;
    private float rollTimer;
    private bool shaking;
    private bool resetting;
    private float resetTime;
    private float resetTimer;
    private Vector3 dieSpawnPos;

    public TurnOrderState(MainGameManager mgm)
    {
        currentPlayer = 0;
        LoadPlayer(mgm);
        rollTime = 0;
        rollTimer = 5f;
        shaking = false;
        mgm.turnOrder.gameObject.SetActive(true);
        die = mgm.SpawnDice(1)[0];
        dieSpawnPos = die.transform.position;
        resetting = false;
        resetTime = 0;
        resetTimer = 2f;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(die.roller.RollReady())
        {
            rollTime += Time.deltaTime;
            if(Input.GetMouseButtonDown(0))
            {
                shaking = true;
                die.roller.Shake();
            }
            else if(rollTime >= rollTimer)
            {
                die.roller.Roll();
            }
        }
        else
        {
            rollTime = 0;
        }
        if(shaking && Input.GetMouseButtonUp(0))
        {
            die.roller.Roll();
            shaking = false;
        }
        if(die.roller.RollComplete())
        {
            if(resetting) {
                resetTime += Time.deltaTime;
                if (resetTime >= resetTimer)
                {
                    resetTime = 0;
                    resetting = false;
                    if (currentPlayer == mgm.NumPlayers - 1)
                    {
                        Object.Destroy(die.gameObject);
                        mgm.turnOrder.gameObject.SetActive(false);
                        return new LoopState();
                    }
                    else
                    {
                        currentPlayer++;
                        LoadPlayer(mgm);
                        die.roller.ResetRoll();
                        die.transform.position = dieSpawnPos;
                    }
                }
            } 
            else
            {
                int dieValue = die.dir.DieValue();
                if(dieValue != -1)
                {
                    mgm.RegisterPlayerTurnRoll(currentPlayer, dieValue);
                    resetTime = 0;
                    resetting = true;
                    mgm.turnOrder.playerRolling.text = mgm.GetPlayer(currentPlayer).name + " rolled a " + die.dir.DieValue() + "!";
                } 
                else
                {
                    die.roller.ResetRoll();
                    die.roller.Roll();
                }
            }
        } else
        {
            resetTime = 0;
            resetting = false;
        }
        return this;
    }

    private void LoadPlayer(MainGameManager mgm)
    {
        mgm.turnOrder.playerRolling.text = mgm.GetPlayer(currentPlayer).name + " is rolling";
    }

}
