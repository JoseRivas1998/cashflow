using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayDebtScreen : MonoBehaviour
{
    [System.Serializable]
    public struct CheckboxAmount
    {
        public CheckBox checkBox;
        public Text amount;
    }

    [System.Serializable]
    public struct ArrowsAmount
    {
        public Slider slider;
        public Text amount;
    }

    public CheckboxAmount mortgage;
    public CheckboxAmount schoolLoans;
    public CheckboxAmount carLoans;
    public CheckboxAmount creditCardDebt;
    public ArrowsAmount bankLoan;

    public Text totalText;
    public Button confirmBtn;

    public int LoanAmount { get; private set; }

    private int maxLoan;
    private int minLoan;

    public void Initialize(Player player)
    {
        if (player.incomeStatement.mortgage == 0)
        {
            mortgage.checkBox.button.interactable = false;
        }
        mortgage.amount.text = Utility.FormatMoney(player.incomeStatement.mortgage);

        if (player.incomeStatement.schoolLoans == 0)
        {
            schoolLoans.checkBox.button.interactable = false;
        }
        schoolLoans.amount.text = Utility.FormatMoney(player.incomeStatement.schoolLoans);

        if (player.incomeStatement.carLoans == 0)
        {
            carLoans.checkBox.button.interactable = false;
        }
        carLoans.amount.text = Utility.FormatMoney(player.incomeStatement.carLoans);

        if (player.incomeStatement.creditCardDebt == 0)
        {
            creditCardDebt.checkBox.button.interactable = false;
        }
        creditCardDebt.amount.text = Utility.FormatMoney(player.incomeStatement.creditCardDebt);
        bankLoan.slider.minValue = 0;
        bankLoan.slider.maxValue = player.incomeStatement.bankLoan / 1000;
        bankLoan.slider.value = 0;
        bankLoan.slider.wholeNumbers = true;
        minLoan = 0;
        maxLoan = player.incomeStatement.bankLoan;
        LoanAmount = 0;
        bankLoan.amount.text = Utility.FormatMoney(player.incomeStatement.bankLoan);
        bankLoan.slider.onValueChanged.AddListener((value) =>
        {
            LoanAmount = (int)(value * 1000);
        });

    }

    // Update is called once per frame
    void Update()
    {
        //bankLoan.addBtn.interactable = LoanAmount < maxLoan;
        //bankLoan.subBtn.interactable = LoanAmount > minLoan;
        bankLoan.amount.text = Utility.FormatMoney(LoanAmount);
    }

}
