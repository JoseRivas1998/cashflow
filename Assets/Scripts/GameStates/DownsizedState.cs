using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownsizedState : GameState
{


    private Player player;
    private Vector3 targetPosition;
    private Vector3 startPos;
    private float moveTime;
    private readonly float moveTimer = 0.4f;

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
        startPos = player.gamePiece.transform.position;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (Vector3.SqrMagnitude(player.gamePiece.transform.position - targetPosition) > mgm.board.sqRatRaceSpaceCenterThreshold)
        {
            float step = (1f / moveTimer) * Time.deltaTime;
            player.gamePiece.transform.position = Vector3.MoveTowards(player.gamePiece.transform.position, targetPosition, step);
            moveTime += Time.deltaTime;
        }
        else
        {
            moveTime = 0;
            player.gamePiece.transform.position = targetPosition;
            startPos = player.gamePiece.transform.position;
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
