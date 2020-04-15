using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCard : MarketCard
{

    public int cost { get; private set; }
    public string instructions { get; private set; }

    public DamageCard(string title, string flavorText, int cost, string instructions)
    {
        this.type = MarketType.Damage;
        this.title = title;
        this.flavorText = flavorText;
        this.cost = cost;
        this.instructions = instructions;
    }

    public override string ToString()
    {
        return base.ToString() +
            "\t" + this.instructions +
            "\tCost: " + Utility.FormatMoney(this.cost);
    }

}
