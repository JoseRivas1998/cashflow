using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : GameState
{

    private int currentSpace;
    private int targetSpace;
    private Player player;
    private Vector3 targetPosition;

    public PlayerMoving(MainGameManager mgm, int dieCount)
    {
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        currentSpace = player.space;
        targetSpace = mgm.board.NormalizeSpace(currentSpace + dieCount);
        targetPosition = mgm.board.SpaceCenter(mgm.board.NormalizeSpace(currentSpace + 1));
        targetPosition.y = player.gamePiece.transform.position.y;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (Vector3.SqrMagnitude(player.gamePiece.transform.position - targetPosition) > mgm.board.sqRatRaceSpaceCenterThreshold)
        {
            player.gamePiece.transform.position += (targetPosition - player.gamePiece.transform.position) / 25f;
        }
        else
        {
            currentSpace = mgm.board.NormalizeSpace(currentSpace + 1);
            if (currentSpace == targetSpace)
            {
                player.gamePiece.transform.position = targetPosition;
                player.space = targetSpace;
                return new PreTurn(mgm);
            }
            targetPosition = mgm.board.SpaceCenter(mgm.board.NormalizeSpace(currentSpace + 1));
            targetPosition.y = player.gamePiece.transform.position.y;
        }
        return this;
    }
}
