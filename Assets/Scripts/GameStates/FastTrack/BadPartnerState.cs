using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadPartnerState : GameState
{

    private readonly FastTrackAnimation animation;

    private bool skip;

    public BadPartnerState(MainGameManager mgm)
    {
        Player player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        if (player.fastTrackIncomeStatement.NumEntries > 0)
        {
            skip = false;
            animation = mgm.SpawnUIObjectBehindCashToggle<FastTrackAnimation>(mgm.fastTrackAnimation);
            animation.SetAnimationType(FastTrackAnimation.FastTrackAnimationType.badPartner);
            player.fastTrackIncomeStatement.RemoveLowestAsset();
            mgm.gameStateDisplay.SetText($"{player.name} loses their lowest cash flowing asset! Click to continue.");
        }
        else
        {
            skip = true;
            animation = null;
            mgm.gameStateDisplay.SetText($"{player.name} does not have any assets. Click to continue.");
        }

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (skip || (animation != null && animation.Done))
        {
            if (Input.GetButtonUp("Fire1"))
            {
                return new FastTrackPostTurnState(mgm);
            }
        }
        return this;
    }
}
