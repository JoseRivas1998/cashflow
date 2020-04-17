using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProfessionCard : MonoBehaviour
{

    private enum FlipState
    {
        FlipReadyBack,
        TurningBackAway,
        TurningFrontTowards,
        FlipReadyFront,
        TurningFrontAway,
        TurningBackTowards,
        FlipDone
    }

    public RectTransform rect;
    public RawImage background;
    public Texture cardBack;
    public Texture cardFront;
    public GameObject cardDataContainer;
    public float flipTime;
    public float degThreshold;

    public Color mutedColor;
    public Text title;
    public Text salary;
    public Text taxes;
    public Text mortgagePayment;
    public Text schoolLoanPayment;
    public Text carLoanPayment;
    public Text creditCardPayment;
    public Text otherExpenses;
    public Text savings;
    public Text homeMortgage;
    public Text schoolLoans;
    public Text carLoans;
    public Text creditCardDebt;
    public Text salaryCalc;
    public Text totalIncome;
    public Text totalExpenses;
    public Text monthlyCashFlow;

    private Professions.Profession profession;
    private FlipState state;
    private float degPerSecond;

    // Start is called before the first frame update
    void Start()
    {
        ResetFlip();
    }

    // Update is called once per frame
    void Update()
    {
        degPerSecond = 180f / flipTime;
        this.state = NextState();
    }

    public void SetProfession(Professions.Profession profession)
    {
        title.text = profession.name.ToUpper();
        SetMoneyText(salary, profession.salary);
        SetMoneyText(taxes, profession.taxes);
        SetMoneyText(mortgagePayment, profession.mortgagePayment);
        SetMoneyText(schoolLoanPayment, profession.schoolPayment);
        SetMoneyText(carLoanPayment, profession.carPayment);
        SetMoneyText(creditCardPayment, profession.creditPayment);
        SetMoneyText(otherExpenses, profession.otherExpenses);
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

    public bool FlipDone()
    {
        return state == FlipState.FlipDone;
    }

    public bool FlipReadyBack()
    {
        return state == FlipState.FlipReadyBack;
    }

    public bool FlipReadyFront()
    {
        return state == FlipState.FlipReadyFront;
    }

    public void ResetFlip()
    {
        rect.eulerAngles = Vector3.zero;
        profession = null;
        background.texture = cardBack;
        cardDataContainer.SetActive(false);
        state = FlipState.FlipReadyBack;
    }

    public void BeginFlip()
    {
        if (profession != null && state == FlipState.FlipReadyBack)
        {
            this.state = FlipState.TurningBackAway;
        }
    }

    public void BeginFlipBack()
    {
        if (state == FlipState.FlipReadyFront)
        {
            this.state = FlipState.TurningFrontAway;
        }
    }

    private FlipState NextState()
    {
        switch (state)
        {
            case FlipState.TurningBackAway: return TurnBackAway();
            case FlipState.TurningFrontTowards: return TurnFrontTowards();
            case FlipState.TurningFrontAway: return TurnFrontAway();
            case FlipState.TurningBackTowards: return TurnBackTowards();
        }
        return this.state;
    }

    private FlipState TurnBackAway()
    {
        if (Mathf.Abs(90f - rect.eulerAngles.y) < degThreshold)
        {
            rect.eulerAngles = new Vector3(0, -90, 0);
            cardDataContainer.SetActive(true);
            background.texture = cardFront;
            return FlipState.TurningFrontTowards;
        }
        rect.Rotate(new Vector3(0, degPerSecond * Time.deltaTime));
        return FlipState.TurningBackAway;
    }

    private FlipState TurnFrontTowards()
    {
        if (Mathf.Abs(rect.eulerAngles.y) < degThreshold)
        {
            rect.eulerAngles = Vector3.zero;
            return FlipState.FlipReadyFront;
        }
        rect.Rotate(new Vector3(0, degPerSecond * Time.deltaTime));
        return FlipState.TurningFrontTowards;
    }

    private FlipState TurnFrontAway()
    {
        if (Mathf.Abs(-90f - (rect.eulerAngles.y - 360)) < degThreshold)
        {
            rect.eulerAngles = new Vector3(0, 90, 0);
            cardDataContainer.SetActive(false);
            background.texture = cardBack;
            return FlipState.TurningBackTowards;
        }
        rect.Rotate(new Vector3(0, -degPerSecond * Time.deltaTime));
        return FlipState.TurningFrontAway;
    }

    private FlipState TurnBackTowards()
    {
        if (Mathf.Abs(rect.eulerAngles.y) < degThreshold)
        {
            rect.eulerAngles = Vector3.zero;
            return FlipState.FlipDone;
        }
        rect.Rotate(new Vector3(0, -degPerSecond * Time.deltaTime));
        return FlipState.TurningBackTowards;
    }

}
