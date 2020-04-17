using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeStatement
{
    // INCOME
    public int salary {get; private set;}
    // EXPENSES
    public int taxes {get; private set;}
    public int mortgagePayment {get; private set;}
    public int schoolPayment {get; private set;}
    public int carPayment {get; private set;}
    public int creditPayment {get; private set;}
    public int otherExpenses {get; private set;}
    public int BankLoanPayment { get { return bankLoan / 10; } }
    public int perChildExpenses { get; private set; }
    public int numChildren { get; private set; }
    public int ChildPayment { get { return perChildExpenses * numChildren; } }
    // ASSETS
    public int savings {get; private set;}
    // LIABILITIES
    public int mortgage {get; private set;}
    public int schoolLoans {get; private set;}
    public int carLoans {get; private set;}
    public int creditCardDebt {get; private set;}
    public int bankLoan { get; set; }

    public int PassiveIncome { get { return 0; } }

    public int TotalIncome { get { return salary - PassiveIncome; } }

    public int TotalExpenses { get { return taxes + mortgagePayment + schoolPayment + carPayment + creditPayment + otherExpenses + BankLoanPayment + ChildPayment; } }

    public int MonthlyCashflow { get { return TotalIncome - TotalExpenses; } }

    public IncomeStatement(Professions.Profession profession)
    {
        this.salary = profession.salary;
        this.taxes = profession.taxes;
        this.mortgagePayment = profession.mortgagePayment;
        this.schoolPayment = profession.schoolPayment;
        this.carPayment = profession.carPayment;
        this.creditPayment = profession.creditPayment;
        this.otherExpenses = profession.otherExpenses;
        this.perChildExpenses = profession.perChildExpenses;
        this.numChildren = 0;
        this.savings = profession.savings;
        this.mortgage = profession.mortgage;
        this.schoolLoans = profession.schoolLoans;
        this.carLoans = profession.carLoans;
        this.creditCardDebt = profession.creditCardDebt;
        this.bankLoan = 0;
    }

}
