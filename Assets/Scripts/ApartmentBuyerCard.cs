using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentBuyerCard : RealEstateMarketCard
{
    public int offer { get; private set;}
    public int minUnits { get; private set; }

    public ApartmentBuyerCard(string title, string flavorText, int offer, int minUnits)
    {
        this.type = MarketType.RealEstate;
        this.realEstateMarketType = RealEstateMarketType.Apartment;
        this.title = title;
        this.flavorText = flavorText;
        this.offer = offer;
        this.minUnits = minUnits;
    }

    public override string ToString()
    {
        return base.ToString() +
            "\tOffer Per Unit: " + Utility.FormatMoney(this.offer) + 
            "\tMin Units: " + this.minUnits;
    }

}
