using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTrackPreTurnChoicesState : GameState
{
    private readonly FastTrackPreTurnChoices choices;

    private bool done;

    public FastTrackPreTurnChoicesState(MainGameManager mgm)
    {
        choices = mgm.SpawnUIObjectBehindCashToggle(mgm.fastTrackPreTurnChoicesPrefab).GetComponent<FastTrackPreTurnChoices>();
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
            return new PreTurn(mgm);
        }
        return this;
    }
}
