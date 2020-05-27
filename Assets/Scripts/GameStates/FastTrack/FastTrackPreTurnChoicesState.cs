using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTrackPreTurnChoicesState : GameState
{
    private readonly FastTrackSingleButton choices;

    private bool done;
    private readonly Player player;

    public FastTrackPreTurnChoicesState(MainGameManager mgm)
    {
        choices = mgm.SpawnUIObjectBehindCashToggle(mgm.fastTrackPreTurnChoicesPrefab).GetComponent<FastTrackSingleButton>();
        choices.btn.onClick.AddListener(() =>
        {
            if (done) return;
            done = true;
        });
        done = false;
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (done)
        {
            Object.Destroy(choices.gameObject);
            if (player.charityTurnsLeft > 0)
            {
                return new FastTrackDiceSelectState(mgm);
            }
            return new FastTrackRollingState(mgm, 1);
        }
        return this;
    }
}
