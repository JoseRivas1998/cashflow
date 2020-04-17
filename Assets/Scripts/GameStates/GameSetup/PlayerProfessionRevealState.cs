using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfessionRevealState : GameState
{

    private int playerIndex;
    private float revealTime;
    private float revealTimer;
    public PlayerProfessionRevealState(MainGameManager mgm)
    {
        mgm.playerNameDreamColor.gameObject.SetActive(false);
        playerIndex = 0;
        mgm.playerProfessionReveal.SetPlayer(mgm.GetPlayer(playerIndex));
        mgm.playerProfessionReveal.gameObject.SetActive(true);
        revealTime = 0;
        revealTimer = 10;
    }

    public override GameState Update(MainGameManager mgm)
    {
        CardFlip cardFlip = mgm.playerProfessionReveal.card.cardFlip;
        if (cardFlip.FlipReadyBack())
        {
            cardFlip.BeginFlip();
        }
        if (cardFlip.FlipReadyFront())
        {
            revealTime += Time.deltaTime;
            if (revealTime >= revealTimer || Input.GetMouseButtonUp(0))
            {
                revealTime = 0;
                cardFlip.BeginFlipBack();
            }
        }
        else
        {
            revealTime = 0;
        }
        if (cardFlip.FlipDone())
        {
            if (playerIndex == mgm.NumPlayers - 1)
            {
                mgm.playerProfessionReveal.gameObject.SetActive(false);
                return new TurnOrderState(mgm);
            }
            cardFlip.ResetFlip();
            NextPlayer(mgm);
        }
        return this;
    }

    private void NextPlayer(MainGameManager mgm)
    {
        mgm.playerProfessionReveal.SetPlayer(mgm.GetPlayer(++playerIndex));
    }

}
