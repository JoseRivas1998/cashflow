using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanState : GameState
{

    private readonly GameState previousState;

    public LoanState(MainGameManager mgm, GameState previousState)
    {
        this.previousState = previousState;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(Input.GetMouseButtonUp(0))
        {
            return previousState;
        }
        return this;
    }
}
