using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealEstateMarketCard : MarketCard
{

    public RealEstateMarketType realEstateMarketType { get; protected set; }

    public override string ToString()
    {
        return base.ToString() + "\t" + this.realEstateMarketType;
    }

    private static readonly Dictionary<RealEstateMarketType, System.Func<RealEstateCard, RealEstateMarketCard, bool>> RealEstatePredicates = new Dictionary<RealEstateMarketType, System.Func<RealEstateCard, RealEstateMarketCard, bool>>
    {
        {RealEstateMarketType.Home, (realEstate, realEstateMarket) => {
            if(realEstateMarket.realEstateMarketType != RealEstateMarketType.Home) return false;
            if(realEstate.propertyType != RealEstateType.House && realEstate.propertyType != RealEstateType.Condo) return false;
            HomeBuyerCard homeBuyer  =(HomeBuyerCard) realEstateMarket;
            if(!homeBuyer.acceptCondo && realEstate.propertyType == RealEstateType.Condo) return false;
            HomeCard home = (HomeCard) realEstate;
            return home.bed == homeBuyer.bed && home.bath == homeBuyer.bath;
        }},
        {RealEstateMarketType.Apartment, (realEstate, realEstateMarket) => {
            if(realEstateMarket.realEstateMarketType != RealEstateMarketType.Apartment) return false;
            if(realEstate.propertyType != RealEstateType.Apartment) return false;
            ApartmentBuyerCard apartmentBuyer = (ApartmentBuyerCard)realEstateMarket;
            MultiCard apartment = (MultiCard) realEstate;
            return apartment.units >= apartmentBuyer.minUnits;
        }},
        {RealEstateMarketType.Plex, (realEstate, realEstateMarket) => {
            if(realEstateMarket.realEstateMarketType != RealEstateMarketType.Plex) return false;
            if(realEstate.propertyType != RealEstateType.Apartment && realEstate.propertyType != RealEstateType.Plex) return false;
            return true;
        }}
    };

    public static System.Func<RealEstateCard, bool> GetRealEstatePredicate(RealEstateMarketCard marketCard)
    {
        return (realEstateCard) => RealEstatePredicates[marketCard.realEstateMarketType].Invoke(realEstateCard, marketCard);
    }

    public static int OfferAmount(RealEstateMarketCard marketCard, RealEstateCard card)
    {
        int offer = 0;
        switch (marketCard.realEstateMarketType)
        {
            case RealEstateMarketType.Home:
                HomeBuyerCard homeBuyer = (HomeBuyerCard)marketCard;
                offer = homeBuyer.offerOriginal ? card.cost : homeBuyer.baseOffer;
                offer += homeBuyer.percent ? (card.cost * homeBuyer.additional / 100) : homeBuyer.additional;
                break;
            case RealEstateMarketType.Apartment:
                ApartmentBuyerCard apartmentBuyer = (ApartmentBuyerCard)marketCard;
                offer = ((MultiCard)card).units * apartmentBuyer.offer;
                break;
            case RealEstateMarketType.Plex:
                PlexBuyerCard plexBuyer = (PlexBuyerCard)marketCard;
                offer = card.cost;
                offer += plexBuyer.percent ? (card.cost * plexBuyer.additional / 100) : plexBuyer.additional;
                break;
            default:
                break;
        }
        return offer;
    }

}
