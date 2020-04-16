using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCountSelectState : GameState
{

    private bool selecting;

    public PlayerCountSelectState(MainGameManager mgm)
    {
        selecting = true;
        mgm.playerCount.playGame.onClick.AddListener(() => {
            selecting = false;
            mgm.numPlayers = mgm.playerCount.count;
            mgm.playerCount.gameObject.SetActive(false);
        });
    }

    public override GameState Update()
    {
        if (selecting)
        {
            return this;
        }
        return new LoopState();
    }


}
