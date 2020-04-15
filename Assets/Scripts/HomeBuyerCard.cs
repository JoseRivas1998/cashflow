using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBuyerCard : RealEstateMarketCard
{
    public int baseOffer { get; private set; }
    public bool offerOriginal { get; private set; }
    public int additional { get; private set; }
    public bool percent { get; private set; }
    public bool acceptCondo { get; private set; }
    public int bed { get; private set; }
    public int bath { get; private set; }

    public HomeBuyerCard(string title, string flavorText, int baseOffer, bool offerOriginal, int additional, bool percent, bool acceptCondo, int bed, int bath)
    {
        this.type = MarketType.RealEstate;
        this.realEstateMarketType = RealEstateMarketType.Home;
        this.title = title;
        this.flavorText = flavorText;
        this.baseOffer = baseOffer;
        this.offerOriginal = offerOriginal;
        this.additional = additional;
        this.percent = percent;
        this.acceptCondo = acceptCondo;
        this.bed = bed;
        this.bath = bath;
    }

    public override string ToString()
    {
        return base.ToString() +
            "\tOffer: " + (this.offerOriginal ? "Original Cost" : Utility.FormatMoney(this.baseOffer)) +
            (this.additional > 0 ? (" + " + (this.percent ? (this.additional + "%") : (Utility.FormatMoney(additional)))) : "") +
            "\tHouse" + (this.acceptCondo ? " Or Condo" : "") + " " + this.bed + "/" + this.bath;
    }

}
