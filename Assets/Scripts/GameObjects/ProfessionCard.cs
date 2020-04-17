using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProfessionCard : MonoBehaviour
{

    public CardFlip cardFlip;
    public Color mutedColor;
    public Text title;
    public Text salary;
    public Text taxes;
    public Text mortgagePayment;
    public Text schoolLoanPayment;
    public Text carLoanPayment;
    public Text creditCardPayment;
    public Text otherExpenses;
    public Text perChildExpenses;
    public Text savings;
    public Text homeMortgage;
    public Text schoolLoans;
    public Text carLoans;
    public Text creditCardDebt;
    public Text salaryCalc;
    public Text totalIncome;
    public Text totalExpenses;
    public Text monthlyCashFlow;
    public Image icon;

    private Professions.Profession profession;

    // Start is called before the first frame update
    void Start()
    {
        cardFlip.AddResetAction(() => profession = null);
        cardFlip.SetFlipCondition(() => profession != null);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetProfession(Professions.Profession profession)
    {
        title.text = profession.name.ToUpper();
        icon.sprite = GameData.Instance.GetProfessionIcon(profession.row, profession.col);
        SetMoneyText(salary, profession.salary);
        SetMoneyText(taxes, profession.taxes);
        SetMoneyText(mortgagePayment, profession.mortgagePayment);
        SetMoneyText(schoolLoanPayment, profession.schoolPayment);
        SetMoneyText(carLoanPayment, profession.carPayment);
        SetMoneyText(creditCardPayment, profession.creditPayment);
        SetMoneyText(otherExpenses, profession.otherExpenses);
        SetMoneyText(perChildExpenses, profession.perChildExpenses);
        SetMoneyText(savings, profession.savings);
        SetMoneyText(homeMortgage, profession.mortgage);
        SetMoneyText(schoolLoans, profession.schoolLoans);
        SetMoneyText(carLoans, profession.carLoans);
        SetMoneyText(creditCardDebt, profession.creditCardDebt);
        salaryCalc.text = Utility.FormatNumberCommas(profession.salary);
        totalIncome.text = Utility.FormatNumberCommas(profession.salary);
        totalExpenses.text = Utility.FormatNumberCommas(profession.Expenses);
        monthlyCashFlow.text = Utility.FormatNumberCommas(profession.CashFlow);
        this.profession = profession;
    }

    private void SetMoneyText(Text text, int amount)
    {
        text.color = amount > 0 ? Color.black : mutedColor;
        text.text = Utility.FormatMoney(amount);
    }

}
