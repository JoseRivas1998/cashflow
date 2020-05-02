using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownsizedState : GameState
{


    private Player player;
    private Vector3 targetPosition;
    private const float moveTimer = 0.4f;

    private bool moving;
    private bool atDestination;

    public DownsizedState(MainGameManager mgm)
    {
        int playerIndex = mgm.turnManager.GetCurrentPlayer();
        player = mgm.GetPlayer(playerIndex);
        player.downsizedTurns++;
        if (player.downsizedTurns <= 2)
        {
            mgm.gameStateDisplay.SetText(player.name + " is downsized!");
        } else
        {
            mgm.gameStateDisplay.SetText(player.name + " is no longer downsized!");
        }
        targetPosition = mgm.board.DownSizedSpace(player.downsizedTurns);
        targetPosition.y = player.gamePiece.transform.position.y;
        moving = false;
        atDestination = false;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (!moving && !atDestination)
        {
            LeanTween.move(player.gamePiece.gameObject, targetPosition, moveTimer)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() => {
                    moving = false;
                    atDestination = true;
                });
            moving = true;
        }
        else if (atDestination)
        {
            atDestination = false;
            player.gamePiece.transform.position = targetPosition;
            if (player.downsizedTurns > 2)
            {
                player.RemoveDownsize();
                return new PreTurnChoicesState(mgm);
            }
            return new PostTurnState(mgm);
        }
        return this;
    }
}
