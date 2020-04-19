using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodadState : GameState
{
    private readonly DoodadCard doodad;
    private Player player;
    private DoodadDisplay display;
    private DoodadCardGameObject card;
    private bool optionsShowing;

    private float cancelTimer;
    private float cancelTime;
    private bool canceling;

    private bool willTakeLoan;
    private bool isTakingLoan;

    private bool done = false;

    public DoodadState(MainGameManager mgm)
    {
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        doodad = mgm.PullDoodadCard();
        GameObject doodadDisplay = Object.Instantiate(mgm.doodadDisplayPrefab, mgm.mainUICanvas.transform);
        doodadDisplay.transform.SetSiblingIndex(mgm.financialStatementToggle.transform.GetSiblingIndex() - 1);
        mgm.gameStateDisplay.gameObject.SetActive(false);
        display = doodadDisplay.GetComponent<DoodadDisplay>();
        display.Initialize(doodad);
        card = display.doodadCard;
        optionsShowing = false;
        cancelTimer = 1;
        cancelTime = 0;
        canceling = false;
        willTakeLoan = false;
        isTakingLoan = false;
        display.payBtn.onClick.AddListener(() => {
            if (done) return;
            player.SubtractMoney(doodad.cost);
            done = true;
        });
        display.loanBtn.onClick.AddListener(() => {
            if (willTakeLoan) return;
            willTakeLoan = true;
        });
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(willTakeLoan)
        {
            willTakeLoan = false;
            isTakingLoan = true;
            display.gameObject.SetActive(false);
            return new LoanState(mgm, this);
        }
        if(isTakingLoan)
        {
            isTakingLoan = false;
            display.gameObject.SetActive(true);
            return this;
        }
        if(done)
        {
            Object.Destroy(display.gameObject);
            return new PostTurnState(mgm);
        }
        if(canceling)
        {
            cancelTime += Time.deltaTime;
            if(cancelTime > cancelTimer) 
            {
                Object.Destroy(display.gameObject);
                return new PostTurnState(mgm);
            }
            return this;
        }
        if(Input.GetMouseButtonUp(0) && card.cardFlip.FlipReadyBack())
        {
            card.cardFlip.BeginFlip();
        }
        if(!optionsShowing && card.cardFlip.FlipReadyFront())
        {
            if (doodad.child && player.incomeStatement.numChildren == 0)
            {
                // if the player does not have a child and the doodad requires a child, cancel doodad card
                canceling = true;
                return this;
            }
            optionsShowing = true;
            display.payBtn.interactable = player.ledger.GetCurretBalance() - doodad.cost >= 0;
            display.options.SetActive(true);
        }
        if(optionsShowing)
        {
            display.payBtn.interactable = player.ledger.GetCurretBalance() - doodad.cost >= 0;
        }
        return this;
    }
}
