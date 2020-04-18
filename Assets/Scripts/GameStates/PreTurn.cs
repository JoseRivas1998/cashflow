using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreTurn : GameState
{

    public PreTurn(MainGameManager mgm)
    {
        int playerIndex = mgm.turnManager.NextPlayer();
        Player player = mgm.GetPlayer(playerIndex);
        if(!player.GamePieceExists())
        {
            Vector3 spawnPosition = mgm.board.SpaceCenter(0) + (Vector3.up * 0.5f);
            mgm.SpawnGamePiece(playerIndex);
            player.gamePiece.origin = mgm.board.transform.position + mgm.board.ratRaceCenterOffset;
            player.gamePiece.transform.position = spawnPosition;
        }
        mgm.mainCamTracker.TrackObject(player.gamePiece.transform);
    } 

    public override GameState Update(MainGameManager mgm)
    {
        if(mgm.mainCamTracker.SquareDistanceFromTarget < 1)
        {
            // TODO check rules here
            return new PlayerRollDiceState(mgm, 1);
        }
        return this;
    }

}
