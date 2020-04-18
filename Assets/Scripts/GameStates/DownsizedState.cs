using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownsizedState : GameState
{


    private Player player;
    private Vector3 targetPosition;

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
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (Vector3.SqrMagnitude(player.gamePiece.transform.position - targetPosition) > mgm.board.sqRatRaceSpaceCenterThreshold)
        {
            player.gamePiece.transform.position += (targetPosition - player.gamePiece.transform.position) / 40f;
        }
        else
        {
            player.gamePiece.transform.position = targetPosition;
            if (player.downsizedTurns > 2)
            {
                player.RemoveDownsize();
                // TODO make this options state
                return new PlayerRollDiceState(mgm, 1);
            }
            return new PreTurn(mgm);
        }
        return this;
    }
}
