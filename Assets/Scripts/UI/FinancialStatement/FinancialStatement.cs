using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinancialStatement : MonoBehaviour
{

    public RectTransform rect;

    public Text Auditor;
    public Text Dream;
    public Text Profession;
    public Text SalaryRight;
    public Text PassiveIncome;
    public Text TotalIncome;
    public Text TotalExpenses;
    public Text MonthlyCashFlow;
    public Text Salary;
    public Text Taxes;
    public Text MortgagePayment;
    public Text SchoolLoanPayment;
    public Text CarLoanPayment;
    public Text CreditCardPayment;
    public Text OtherExpenses;
    public Text BankLoanPayment;
    public Text ChildExpeses;
    public Text PerChildExpense;
    public Text NumberOfChildren;
    public Text Savings;
    public Text PreciousMetals;
    public Text Mortgage;
    public Text SchoolLoans;
    public Text CarLoans;
    public Text CreditCardDebt;
    public Text BankLoan;

    public MainGameManager mainGameManager;

    public Color defaultColor;
    public Color mutedColor;

    public StockList stockList;
    public RealEstateLists realEstateLists;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetWidth()
    {
        Vector3[] worlds = new Vector3[4];
        rect.GetWorldCorners(worlds);
        return worlds[2].x - worlds[0].x;
    }

    public void UpdateToCurrentPlayer()
    {
        int playerIndex = mainGameManager.turnManager.GetCurrentPlayer();
        int auditorIndex = (playerIndex + (mainGameManager.NumPlayers - 1)) % mainGameManager.NumPlayers;
        Auditor.text = mainGameManager.GetPlayer(auditorIndex).name;

        Player player = mainGameManager.GetPlayer(playerIndex);
        Dream.text = player.dream;
        Profession.text = player.profession.name;
        SetAmountText(SalaryRight, player.incomeStatement.salary, false);
        SetAmountText(PassiveIncome, player.incomeStatement.PassiveIncome, false);
        SetAmountText(TotalIncome, player.incomeStatement.TotalIncome, false);
        SetAmountText(TotalExpenses, player.incomeStatement.TotalExpenses, false);
        SetAmountText(MonthlyCashFlow, player.incomeStatement.MonthlyCashflow, false);
        SetAmountText(Salary, player.incomeStatement.salary, true);
        SetAmountText(Taxes, player.incomeStatement.taxes, true);
        SetAmountText(MortgagePayment, player.incomeStatement.mortgagePayment, true);
        SetAmountText(SchoolLoanPayment, player.incomeStatement.schoolPayment, true);
        SetAmountText(CarLoanPayment, player.incomeStatement.carPayment, true);
        SetAmountText(CreditCardPayment, player.incomeStatement.creditPayment, true);
        SetAmountText(OtherExpenses, player.incomeStatement.otherExpenses, true);
        SetAmountText(BankLoanPayment, player.incomeStatement.BankLoanPayment, true);
        SetAmountText(ChildExpeses, player.incomeStatement.ChildPayment, true);
        SetAmountText(PerChildExpense, player.incomeStatement.perChildExpenses, true);
        SetAmountText(NumberOfChildren, player.incomeStatement.numChildren, false);
        SetAmountText(Savings, player.incomeStatement.savings, true);
        SetAmountText(PreciousMetals, player.incomeStatement.goldCoins, false);
        PreciousMetals.text += " Gold Coins";
        SetAmountText(Mortgage, player.incomeStatement.mortgage, true);
        SetAmountText(SchoolLoans, player.incomeStatement.schoolLoans, true);
        SetAmountText(CarLoans, player.incomeStatement.carLoans, true);
        SetAmountText(CreditCardDebt, player.incomeStatement.creditCardDebt, true);
        SetAmountText(BankLoan, player.incomeStatement.bankLoan, true);
        stockList.ResetList(player);
        realEstateLists.ResetPlayer(player);
    }

    private void SetAmountText(Text text, int amount, bool money)
    {
        text.color = amount == 0 ? mutedColor : defaultColor;
        text.text = money ? Utility.FormatMoney(amount) : Utility.FormatNumberCommas(amount);
    }

}
