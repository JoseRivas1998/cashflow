using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTrackPostTurnState : GameState
{

    private bool done;

    private readonly FastTrackSingleButton fastTrackSingleButton;

    public FastTrackPostTurnState(MainGameManager mgm)
    {
        done = false;
        Player player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        mgm.gameStateDisplay.SetText($"{player.name}'s Post Turn");
        fastTrackSingleButton = mgm.SpawnUIObjectBehindCashToggle(mgm.fastTrackPostTurnChoicesPrefab).GetComponent<FastTrackSingleButton>();
        fastTrackSingleButton.btn.onClick.AddListener(() =>
        {
            if (done) return;
            done = true;
        });
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (done)
        {
            Object.Destroy(fastTrackSingleButton.gameObject);
            return new PreTurn(mgm);
        }
        return this;
    }
}
