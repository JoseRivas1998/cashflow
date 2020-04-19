using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostTurnState : GameState
{
    private enum Choices { Loan, EndTurn, Debt }

    private bool selected;
    private Choices selectedChoice;

    private bool takingLoan;

    private readonly Player player;
    private readonly PostTurnChoices choices;

    public PostTurnState(MainGameManager mgm)
    {
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        GameObject choicesObject = Object.Instantiate(mgm.postTurnChoicesPrefab, mgm.mainUICanvas.transform);
        choicesObject.transform.SetSiblingIndex(mgm.financialStatementToggle.transform.GetSiblingIndex() - 1);
        choices = choicesObject.GetComponent<PostTurnChoices>();
        choices.endTurnBtn.onClick.AddListener(() => {
            if (selected) return;
            selected = true;
            selectedChoice = Choices.EndTurn;
        });
        choices.loanBtn.onClick.AddListener(() => {
            if (selected) return;
            selected = true;
            selectedChoice = Choices.Loan;
        });
        selected = false;
        takingLoan = false;
        mgm.gameStateDisplay.SetText(player.name + "'s post turn");
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (takingLoan)
        {
            selected = false;
            takingLoan = false;
            choices.gameObject.SetActive(true);
            mgm.gameStateDisplay.SetText(player.name + "'s post turn");
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
                case Choices.EndTurn:
                    Object.Destroy(choices.gameObject);
                    return new PreTurn(mgm);
            }
        }
        return this;
    }
}
