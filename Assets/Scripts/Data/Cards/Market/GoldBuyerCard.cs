using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBuyerCard : MarketCard
{
    public int offer { get; private set; }

    public GoldBuyerCard(string title, string flavorText, int offer)
    {
        this.type = MarketType.Gold;
        this.title = title;
        this.flavorText = flavorText;
        this.offer = offer;
    }

    public override string ToString()
    {
        return base.ToString() +
            "\tOffer per coin: " + Utility.FormatMoney(this.offer);
    }

}
