using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCard : RealEstateCard
{

    public int bed { get; private set; }
    public int bath { get; private set; }

    public HomeCard(bool smallDeal, RealEstateType propertyType, string title, string flavorText, int cost, int mortgage, int downPayment, int cashFlow, int bed, int bath)
        : base(smallDeal, propertyType, title, flavorText, cost, mortgage, downPayment, cashFlow)
    {
        this.bed = bed;
        this.bath = bath;
    }

    public override string ToString()
    {
        return base.ToString() +
            "\tBed: " + bed +
            "\tBath: " + bath;
    }
}
