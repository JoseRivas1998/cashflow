using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple debug state meant to stall game progress. This should NOT be used in production.
/// </summary>
public class LoopState : GameState
{
    public override GameState Update(MainGameManager mgm)
    {
        return this;
    }
}
