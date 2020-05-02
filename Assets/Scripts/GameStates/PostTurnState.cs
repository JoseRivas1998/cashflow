using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostTurnState : GameState
{
    private enum Choices { Loan, EndTurn, Debt }

    private bool selected;
    private Choices selectedChoice;

    private bool takingLoan;
    private bool payingDebt;

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
        choices.debtBtn.onClick.AddListener(() => {
            if (selected) return;
            selected = true;
            selectedChoice = Choices.Debt;
        });
        selected = false;
        takingLoan = false;
        payingDebt = false;
        mgm.gameStateDisplay.SetText(player.name + "'s post turn");
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (takingLoan || payingDebt)
        {
            selected = false;
            takingLoan = false;
            payingDebt = false;
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
                    if(player.incomeStatement.PassiveIncome > player.incomeStatement.TotalExpenses)
                    {
                        return new EnterFastTrackOptionsState(mgm);
                    }
                    return new PreTurn(mgm);
                case Choices.Debt:
                    choices.gameObject.SetActive(false);
                    mgm.gameStateDisplay.gameObject.SetActive(false);
                    this.payingDebt = true;
                    this.selected = false;
                    return new PayingDebtState(mgm, this);
            }
        }
        return this;
    }
}
