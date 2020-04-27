using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanState : GameState
{

    private readonly GameState previousState;
    private readonly Player player;
    private readonly LoanDisplay loanDisplay;

    private bool done;

    public LoanState(MainGameManager mgm, GameState previousState) : this(mgm, previousState, mgm.turnManager.GetCurrentPlayer()) { }

    public LoanState(MainGameManager mgm, GameState previousState, int playerIndex)
    {
        player = mgm.GetPlayer(playerIndex);
        this.previousState = previousState;
        GameObject display = Object.Instantiate(mgm.loanDisplayPrefab, mgm.mainUICanvas.transform);
        display.transform.SetSiblingIndex(mgm.financialStatementToggle.transform.GetSiblingIndex() - 1);
        mgm.gameStateDisplay.gameObject.SetActive(false);
        done = false;
        loanDisplay = display.GetComponent<LoanDisplay>();
        loanDisplay.cancelButton.onClick.AddListener(() =>
        {
            if (done) return;
            done = true;
        });
        loanDisplay.numberInput.confirmBtn.onClick.AddListener(() =>
        {
            if (done) return;
            player.TakeOutLoan(loanDisplay.loanAmount);
            done = true;
        });
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (done)
        {
            Object.Destroy(loanDisplay.gameObject);
            return previousState;
        }
        return this;
    }
}
