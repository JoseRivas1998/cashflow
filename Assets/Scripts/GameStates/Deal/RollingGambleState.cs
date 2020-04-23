using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingGambleState : GameState
{

    private readonly GambleCard gamble;
    private readonly DealCardGameObject dealCard;
    private readonly Player player;
    private readonly DiceContainer die;
    private readonly float doneTimer;

    private float doneTime;

    public RollingGambleState(MainGameManager mgm, GambleCard gamble, DealCardGameObject dealCard)
    {
        this.gamble = gamble;
        this.dealCard = dealCard;
        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        this.dealCard.transform.position += Vector3.right * dealCard.transform.position.x * 0.5f;
        this.dealCard.transform.position += Vector3.up * dealCard.transform.position.y * 0.45f;
        this.dealCard.transform.localScale = (Vector3.up + Vector3.right) * 0.75f;

        this.die = mgm.SpawnDice(1)[0];

        this.doneTime = 0;
        this.doneTimer = 1;

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.die.roller.Shake();
        }
        if (Input.GetMouseButtonUp(0))
        {
            mgm.mainCamTracker.TrackObject(this.die.transform);
            this.die.roller.Roll();
        }
        if(die.roller.RollComplete())
        {
            int dieValue = die.dir.DieValue();
            if(dieValue == -1)
            {
                die.roller.ResetRoll();
                die.roller.Roll();
                return this;
            }
            doneTime += Time.deltaTime;
            if(doneTime > doneTimer)
            {
                mgm.mainCamTracker.TrackObject(player.gamePiece.transform);
                Object.Destroy(die.gameObject);
                Object.Destroy(dealCard.gameObject);
                if (dieValue >= gamble.goodMin && dieValue <= gamble.goodMax)
                {
                    if(gamble.gold)
                    {
                        player.incomeStatement.AddGold(gamble.reward);
                    } 
                    else
                    {
                        player.AddMoney(gamble.reward);
                    }
                }
                return new PostTurnState(mgm);
            }
        } 
        else
        {
            doneTime = 0;
        }
        return this;
    }
}
