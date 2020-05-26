using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LawsuitState : GameState
{
    private readonly FastTrackAnimation animation;

    public LawsuitState(MainGameManager mgm)
    {
        Player player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        player.SubtractMoney(player.ledger.GetCurretBalance() / 2);
        mgm.gameStateDisplay.SetText($"{player.name} must pay half their cash!");

        animation = mgm.SpawnUIObjectBehindCashToggle(mgm.fastTrackAnimation).GetComponent<FastTrackAnimation>();
        animation.SetAnimationType(FastTrackAnimation.FastTrackAnimationType.Lawsuit);
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(animation.Done)
        {
            Object.Destroy(animation.gameObject);
            return new FastTrackPostTurnState(mgm);
        }
        return this;
    }
}
