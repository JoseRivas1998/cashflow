using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlexBuyerCard : RealEstateMarketCard
{
    
    public int additional { get; private set; }
    public bool percent { get; private set; }

    public PlexBuyerCard(int additional, bool percent)
    {
        this.type = MarketType.RealEstate;
        this.realEstateMarketType = RealEstateMarketType.Plex;
        this.title = "Plex Buyer";
        this.additional = additional;
        this.percent = percent;
        string additionStr = percent ? (additional + "%") : Utility.FormatMoney(additional);
        this.flavorText = "Buyer looking for all Apartment and Plexes in any combination of units. Offers your original cost plus " + additionStr + ".";
    }

}
