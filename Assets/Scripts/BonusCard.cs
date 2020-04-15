using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCard : MarketCard
{
    public int cashFlowMax { get; private set; }
    public int cashFlowIncrease { get; private set; }
    public bool everyone { get; private set; }

    public BonusCard(string title, string flavorText, int cashFlowMax, int cashFlowIncrease, bool everyone)
    {
        this.type = MarketType.Bonus;
        this.title = title;
        this.flavorText = flavorText;
        this.cashFlowMax = cashFlowMax;
        this.cashFlowIncrease = cashFlowIncrease;
        this.everyone = everyone;
    }

    public override string ToString()
    {
        return base.ToString() +
            "\tALL businesses with a cash flow of " + Utility.FormatMoney(this.cashFlowMax) + ", or less, have their cash flow increased by " + Utility.FormatMoney(cashFlowIncrease) + "." +
            "\t" + (everyone ? "All Affected" : "Only Card Holder Affected");
    }

}
