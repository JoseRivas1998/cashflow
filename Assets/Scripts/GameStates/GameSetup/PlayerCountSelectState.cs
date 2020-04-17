using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCountSelectState : GameState
{

    private bool selecting;

    public PlayerCountSelectState(MainGameManager mgm)
    {
        selecting = true;
        mgm.playerCount.gameObject.SetActive(true);
        mgm.playerCount.playGame.onClick.AddListener(() => {
            selecting = false;
            mgm.SetNumPlayers(mgm.playerCount.count);
            mgm.playerCount.gameObject.SetActive(false);
        });
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (selecting)
        {
            return this;
        }
        return new PlayerInputDataState(mgm);
    }


}
