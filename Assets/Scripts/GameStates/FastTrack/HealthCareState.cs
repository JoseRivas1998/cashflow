using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCareState : GameState
{

    private readonly Player player;
    private readonly FastTrackAnimation fastTrackAnimation;
    private readonly DiceContainer die;

    public bool done;

    public HealthCareState(MainGameManager mgm)
    {
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        fastTrackAnimation = mgm.SpawnUIObjectBehindCashToggle(mgm.fastTrackAnimation).GetComponent<FastTrackAnimation>();
        fastTrackAnimation.SetAnimationType(FastTrackAnimation.FastTrackAnimationType.HealthCare);

        die = mgm.SpawnDice(1, true)[0];

        mgm.mainCamTracker.TrackObject(die.transform);

        done = false;

        mgm.gameStateDisplay.SetText($"{player.name} is rolling healthcare!");

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (done)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                mgm.mainCamTracker.TrackObject(player.gamePiece.transform);
                Object.Destroy(die.gameObject);
                Object.Destroy(fastTrackAnimation.gameObject);
                return new FastTrackPostTurnState(mgm);
            }
        }
        else if (fastTrackAnimation.Done)
        {
            if (die.roller.RollReady() && Input.GetButtonDown("Fire1"))
            {
                mgm.mainCamTracker.TrackObject(null);
                die.roller.Shake();
            }
            if (Input.GetButtonUp("Fire1"))
            {
                mgm.mainCamTracker.TrackObject(die.transform);
                die.roller.Roll();
            }
            if (die.roller.RollComplete())
            {
                int dieValue = die.dir.DieValue();
                if (dieValue == -1)
                {
                    die.roller.ResetRoll();
                    die.roller.Roll();
                }
                else
                {
                    int cost;
                    if (dieValue <= 3)
                    {
                        cost = player.ledger.GetCurretBalance() / 2;
                        mgm.gameStateDisplay.SetText("Pay half your cash! Click to continue");
                    }
                    else
                    {
                        cost = player.ledger.GetCurretBalance();
                        mgm.gameStateDisplay.SetText("Pay all your cash! Click to continue");
                    }
                    player.SubtractMoney(cost);
                    done = true;
                }
            }
        }
        return this;
    }
}
