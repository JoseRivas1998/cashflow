using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCard : DealCard
{
    public int coins { get; private set; }
    public int cost { get; private set; }

    public GoldCard(string title, string flavorText, int coins, int cost)
    {
        this.type = DealType.Gold;
        this.smallDeal = true;
        this.title = title;
        this.flavorText = flavorText;
        this.coins = coins;
        this.cost = cost;
    }

    public override string ToString()
    {
        return base.ToString() +
            "\tCoins: " + coins +
            "\tCost: " + Utility.FormatMoney(cost);
    }

}
