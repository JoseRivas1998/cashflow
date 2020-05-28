using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusinessInvestmentDisplay : MonoBehaviour
{
    public Text title;
    public Text cashFlow;
    public Text downPayment;

    public void SetInvestment(FastTrackSpace space)
    {
        if (space.type == FastTrackSpaceType.BusinessInvestments)
        {
            BusinessInvestment investment = (BusinessInvestment)space;
            title.text = investment.name;
            cashFlow.text = $"+{Utility.FormatMoney(investment.cashFlow)}/mo CF";
            downPayment.text = $"{Utility.FormatMoney(investment.down)} down";
        }
    }

}
