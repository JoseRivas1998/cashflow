using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToFastTrackStartingSpaceState : GameState
{

    private bool done;
    private float doneTime;
    private const float doneTimer = 1f;
    private const float moveTimer = 0.4f;

    public MovingToFastTrackStartingSpaceState(MainGameManager mgm, in int dieValue)
    {

        Player player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        int targetSpace = mgm.board.FastTractStartSpace(dieValue);
        Vector3 spaceCenter = mgm.board.FastTrackSpaceCenter(targetSpace);
        Vector3 targetPosition = new Vector3(spaceCenter.x, player.gamePiece.transform.position.y, spaceCenter.z);
        
        LeanTween.move(player.gamePiece.gameObject, targetPosition, moveTimer)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() => done = true);

        player.EnterFastTrack(targetSpace);

        done = false;
        doneTime = 0f;
        mgm.gameStateDisplay.gameObject.SetActive(false);
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(done)
        {
            doneTime += Time.deltaTime;
        }
        else
        {
            doneTime = 0;
        }
        return this;
    }
}
