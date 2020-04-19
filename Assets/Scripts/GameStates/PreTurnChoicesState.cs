using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreTurnChoicesState : GameState
{

    private enum Choices { Loan, Dice, Debt }

    private bool selected;
    private Choices selectedChoice;

    private bool takingLoan;

    private readonly Player player;
    private readonly PreTurnChoices choices;

    public PreTurnChoicesState(MainGameManager mgm)
    {
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        GameObject choicesObject = Object.Instantiate(mgm.preTurnChoicesPrefab, mgm.mainUICanvas.transform);
        choicesObject.transform.SetSiblingIndex(mgm.financialStatementToggle.transform.GetSiblingIndex() - 1);
        choices = choicesObject.GetComponent<PreTurnChoices>();
        choices.rollDiceBtn.onClick.AddListener(() => {
            if (selected) return;
            selected = true;
            selectedChoice = Choices.Dice;
        });
        choices.loanBtn.onClick.AddListener(() => {
            if (selected) return;
            selected = true;
            selectedChoice = Choices.Loan;
        });
        selected = false;
        takingLoan = false;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (takingLoan)
        {
            selected = false;
            takingLoan = false;
            choices.gameObject.SetActive(true);
            mgm.gameStateDisplay.SetText(player.name + "'s turn");
        }
        if (selected)
        {
            switch (selectedChoice)
            {
                case Choices.Loan:
                    choices.gameObject.SetActive(false);
                    this.takingLoan = true;
                    this.selected = false;
                    return new LoanState(mgm, this);
                case Choices.Dice:
                    Object.Destroy(choices.gameObject);
                    if (player.charityTurnsLeft > 0)
                    {
                        return new SelectingDieAmountState(mgm);
                    }
                    return new PlayerRollDiceState(mgm, 1);
            }
        }
        return this;
    }
}
