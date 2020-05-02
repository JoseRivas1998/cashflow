using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingFastTrackSpaceState : GameState
{

    private DiceContainer die;

    public RollingFastTrackSpaceState(MainGameManager mgm)
    {

        die = mgm.SpawnDice(1)[0];

        mgm.gameStateDisplay.SetText("Roll to determine starting space");
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            die.roller.Shake();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            die.roller.Roll();
            mgm.mainCamTracker.TrackObject(die.transform);
        }
        if(die.roller.RollComplete())
        {
            int dieValue = die.dir.DieValue();
            if(dieValue == -1)
            {
                die.roller.ResetRoll();
                die.roller.Roll();
                return this;
            }
            mgm.mainCamTracker.TrackObject(mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer()).gamePiece.transform);
            Object.Destroy(die.gameObject);
            return new MovingToFastTrackStartingSpaceState(mgm, dieValue);
        }
        return this;
    }
}
