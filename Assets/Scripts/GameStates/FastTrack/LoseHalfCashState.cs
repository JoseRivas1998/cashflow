using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseHalfCashState : GameState
{
    private readonly FastTrackAnimation animation;

    public LoseHalfCashState(MainGameManager mgm, FastTrackSpaceType spaceType)
    {
        Player player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        player.SubtractMoney(player.ledger.GetCurretBalance() / 2);
        mgm.gameStateDisplay.SetText($"{player.name} must pay half their cash!");

        animation = mgm.SpawnUIObjectBehindCashToggle(mgm.fastTrackAnimation).GetComponent<FastTrackAnimation>();
        switch (spaceType)
        {
            case FastTrackSpaceType.Lawsuit:
                animation.SetAnimationType(FastTrackAnimation.FastTrackAnimationType.Lawsuit);
                break;
            case FastTrackSpaceType.TaxAudit:
                break;
            case FastTrackSpaceType.Divorce:
                break;
            default:
                break;
        }
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
