using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyState : GameState
{

    private readonly bool skip;

    public BabyState(MainGameManager mgm) 
    {
        mgm.gameStateDisplay.gameObject.SetActive(false);
        Player player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        skip = !player.incomeStatement.AddChild();
        if (skip) return;
        mgm.babyAnimation.gameObject.SetActive(true);
        mgm.babyAnimation.ResetAnimation();
        mgm.babyAnimation.StartAnimation();
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(skip)
        {
            return new PostTurnState(mgm);
        }
        if(mgm.babyAnimation.AnimatorDone())
        {
            mgm.babyAnimation.ResetAnimation();
            mgm.babyAnimation.gameObject.SetActive(false);
            return new PostTurnState(mgm);
        }
        return this;
    }
}
