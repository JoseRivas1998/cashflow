using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCard : RealEstateCard
{
    public int units { get; private set; }

    public MultiCard(RealEstateType propertyType, string title, string flavorText, int cost, int mortgage, int downPayment, int cashFlow, int units)
        : base(false, propertyType, title, flavorText, cost, mortgage, downPayment, cashFlow)
    {
        this.units = units;
    }

    public override string ToString()
    {
        return base.ToString() +
            "\tUnits: " + units;
    }
}
