﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealEstateMarketCard : MarketCard
{

    public RealEstateMarketType realEstateMarketType { get; protected set; }

    public override string ToString()
    {
        return base.ToString() + "\t" + this.realEstateMarketType;
    }

}
