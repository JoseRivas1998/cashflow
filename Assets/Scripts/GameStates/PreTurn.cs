using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreTurn : GameState
{
    private readonly bool skip;
    public PreTurn(MainGameManager mgm)
    {
        if (mgm.turnManager.NumPlayersIn() == 0)
        {
            skip = true;
            return;
        }
        skip = false;
        int playerIndex = mgm.turnManager.NextPlayer();
        Player player = mgm.GetPlayer(playerIndex);
        if (!player.GamePieceExists())
        {
            float angle = Random.Range(0, Mathf.PI * 2);
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 0.5f;
            Vector3 spawnPosition = mgm.board.SpaceCenter(0) + (new Vector3(offset.x, 0.5f, offset.y));
            mgm.SpawnGamePiece(playerIndex);
            player.gamePiece.origin = mgm.board.transform.position + mgm.board.ratRaceCenterOffset;
            player.gamePiece.transform.position = spawnPosition;
        }
        mgm.mainCamTracker.TrackObject(player.gamePiece.transform);
        mgm.gameStateDisplay.SetText(player.name + "'s Turn");
        mgm.gameStateDisplay.gameObject.SetActive(true);
        mgm.financialStatementToggle.Close();
        mgm.financialStatementToggle.gameObject.SetActive(true);
        mgm.cashLedgerToggle.Close();
        mgm.cashLedgerToggle.gameObject.SetActive(true);
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (skip)
        {
            return new GameOverState(mgm);
        }
        if (mgm.mainCamTracker.SquareDistanceFromTarget < 1)
        {
            int playerIndex = mgm.turnManager.GetCurrentPlayer();
            Player player = mgm.GetPlayer(playerIndex);
            if (player.downsized)
            {
                return new DownsizedState(mgm);
            }
            return new PreTurnChoicesState(mgm);
        }
        return this;
    }

}
