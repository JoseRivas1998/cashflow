﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealEstateCard : DealCard
{
    
    public RealEstateType propertyType { get; private set; }
    public string title { get; private set; }
    public string flavorText { get; private set; }
    public int cost { get; private set; }
    public int mortgage { get; private set; }
    public int downPayment { get; private set; }
    public int cashFlow { get; private set; }


    public RealEstateCard(bool smallDeal, RealEstateType propertyType, string title, string flavorText, int cost, int mortgage, int downPayment, int cashFlow)
    {
        this.type = DealType.RealEstate;
        this.smallDeal = smallDeal;
        this.propertyType = propertyType;
        this.title = title;
        this.flavorText = flavorText;
        this.cost = cost;
        this.mortgage = mortgage;
        this.downPayment = downPayment;
        this.cashFlow = cashFlow;
    }

    public override string ToString()
    {
        return base.ToString() +
            "\t" + propertyType +
            "\t" + title +
            "\t" + flavorText +
            "\t Cost: " + Utility.FormatMoney(cost) +
            "\t Mortgage: " + Utility.FormatMoney(mortgage) +
            "\t Down Payment: " + Utility.FormatMoney(downPayment) +
            "\t Cash Flow: " + Utility.FormatMoney(cashFlow);
    }

}
