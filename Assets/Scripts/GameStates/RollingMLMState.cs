using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingMLMState : GameState
{

    private readonly Player player;
    private DiceContainer die;
    private GambleCard gamble;
    private readonly DealCardGameObject dealCard;

    private readonly int payDays;
    private readonly int diceSum;
    private int currentMLM;
    private bool cardShowing;
    private bool dieSpawn;

    private float waitTime;
    private readonly float waitTimer = 1;

    public RollingMLMState(MainGameManager mgm, int diceSum, int payDays)
    {
        this.diceSum = diceSum;
        this.payDays = payDays;

        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        GameObject card = Object.Instantiate(mgm.dealCardPrefab, mgm.mainUICanvas.transform);
        card.transform.SetSiblingIndex(mgm.cashLedgerToggle.transform.GetSiblingIndex());
        this.dealCard = card.GetComponent<DealCardGameObject>();

        this.dealCard.transform.position += Vector3.right * dealCard.transform.position.x * 0.5f;
        this.dealCard.transform.position += Vector3.up * dealCard.transform.position.y * 0.45f;
        this.dealCard.transform.localScale = (Vector3.up + Vector3.right) * 0.75f;

        this.currentMLM = 0;
        this.UpdateCard();
        this.cardShowing = false;

        dieSpawn = false;

        this.waitTime = 0;

        mgm.gameStateDisplay.SetText(player.name + " is rolling their Multi Level Networking");

    }

    public override GameState Update(MainGameManager mgm)
    {
        if(!cardShowing)
        {
            cardShowing = true;
            this.dealCard.cardFlip.background.texture = this.dealCard.cardFlip.cardFront;
            this.dealCard.gambleHolder.SetActive(true);
        }
        if(!dieSpawn)
        {
            if (mgm.mainCamTracker.SquareDistanceFromTarget < 1)
            {
                dieSpawn = true;
                this.die = mgm.SpawnDice(1)[0];
            }
        }
        if (this.die != null && Input.GetMouseButtonDown(0))
        {
            this.die.roller.Shake();
        }
        if (this.die != null && Input.GetMouseButtonUp(0))
        {
            this.die.roller.Roll();
            mgm.mainCamTracker.TrackObject(die.transform);
        }
        if (this.die != null && this.die.roller.RollComplete())
        {
            int dieValue = this.die.dir.DieValue();
            if(dieValue == -1)
            {
                this.die.roller.ResetRoll();
                this.die.roller.Roll();
                return this;
            }
            waitTime += Time.deltaTime;
            if(waitTime > waitTimer)
            {
                mgm.mainCamTracker.TrackObject(player.gamePiece.transform);
                if(mgm.mainCamTracker.SquareDistanceFromTarget < 1) 
                {
                    if(dieValue >= gamble.goodMin && dieValue <= gamble.goodMax)
                    {
                        player.AddMoney(gamble.reward);
                    }
                    Object.Destroy(this.die.gameObject);
                    this.die = null;
                    this.currentMLM++;
                    if(this.currentMLM == player.incomeStatement.MLMCount)
                    {
                        Object.Destroy(this.dealCard.gameObject);
                        mgm.gameStateDisplay.gameObject.SetActive(false);
                        return new PaydayState(mgm, diceSum, payDays);
                    }
                    this.dieSpawn = false;
                    UpdateCard();
                }
            }
        }
        else
        {
            this.waitTime = 0;
        }
        return this;
    }

    private void UpdateCard()
    {
        this.gamble = this.player.incomeStatement.GetMLM(this.currentMLM);
        this.dealCard.SetDeal(gamble);
        this.dealCard.cardFlip.background.texture = this.dealCard.cardFlip.cardFront;
        this.dealCard.gambleHolder.SetActive(true);
    }

}
