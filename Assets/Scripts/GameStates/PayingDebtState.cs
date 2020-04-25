using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayingDebtState : GameState
{

    private readonly PayDebtScreen payDebtScreen;
    private readonly GameState previousState;
    private readonly Player player;

    private bool done;

    public PayingDebtState(MainGameManager mgm, GameState previousState)
    {
        this.previousState = previousState;
        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        GameObject screen = Object.Instantiate(mgm.payDebtScreenPrefab, mgm.mainUICanvas.transform);
        screen.transform.SetSiblingIndex(mgm.cashLedgerToggle.transform.GetSiblingIndex());
        payDebtScreen = screen.GetComponent<PayDebtScreen>();
        payDebtScreen.Initialize(player);

        payDebtScreen.confirmBtn.onClick.AddListener(() => {
            if (done) return;
            done = true;
        });

        done = false;
    }

    public override GameState Update(MainGameManager mgm)
    {
        int total = CalculateTotal();
        if (done)
        {
            if(total == 0)
            {
                Object.Destroy(this.payDebtScreen.gameObject);
                return this.previousState;
            }
            player.SubtractMoney(total);
            if (payDebtScreen.mortgage.checkBox.Selected)
            {
                player.incomeStatement.RemoveMortgate();
            }
            if (payDebtScreen.schoolLoans.checkBox.Selected)
            {
                player.incomeStatement.RemoveSchoolLoans();
            }
            if (payDebtScreen.carLoans.checkBox.Selected)
            {
                player.incomeStatement.RemoveCarLoans();
            }
            if (payDebtScreen.creditCardDebt.checkBox.Selected)
            {
                player.incomeStatement.RemoveCreditCardDebt();
            }
            if (payDebtScreen.LoanAmount > 0)
            {
                player.incomeStatement.SubtractBankLoan(payDebtScreen.LoanAmount);
            }
            Object.Destroy(this.payDebtScreen.gameObject);
            return this.previousState;
        }
        payDebtScreen.totalText.text = Utility.FormatMoney(total);
        payDebtScreen.confirmBtn.interactable = player.ledger.GetCurretBalance() >= total;
        return this;
    }

    private int CalculateTotal()
    {
        int total = payDebtScreen.LoanAmount;
        if (payDebtScreen.mortgage.checkBox.Selected)
        {
            total += player.incomeStatement.mortgage;
        }
        if (payDebtScreen.schoolLoans.checkBox.Selected)
        {
            total += player.incomeStatement.schoolLoans;
        }
        if (payDebtScreen.carLoans.checkBox.Selected)
        {
            total += player.incomeStatement.carLoans;
        }
        if (payDebtScreen.creditCardDebt.checkBox.Selected)
        {
            total += player.incomeStatement.creditCardDebt;
        }
        return total;
    }

}
