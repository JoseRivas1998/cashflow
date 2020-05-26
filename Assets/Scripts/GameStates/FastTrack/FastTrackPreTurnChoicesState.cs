using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTrackPreTurnChoicesState : GameState
{
    private readonly FastTrackSingleButton choices;

    private bool done;

    public FastTrackPreTurnChoicesState(MainGameManager mgm)
    {
        choices = mgm.SpawnUIObjectBehindCashToggle(mgm.fastTrackPreTurnChoicesPrefab).GetComponent<FastTrackSingleButton>();
        choices.btn.onClick.AddListener(() =>
        {
            if (done) return;
            done = true;
        });
        done = false;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (done)
        {
            Object.Destroy(choices.gameObject);
            // TODO handle charity
            return new FastTrackRollingState(mgm, 1);
        }
        return this;
    }
}
