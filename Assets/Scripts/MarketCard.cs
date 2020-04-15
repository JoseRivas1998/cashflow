using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketCard
{
    public MarketType type { get; protected set; }
    public string title { get; protected set; }
    public string flavorText { get; protected set; }

    [System.Serializable]
    private struct HomeBuyerJSON
    {
        public string title;
        public string flavorText;
        public int baseOffer;
        public bool offerOriginal;
        public int additional;
        public bool percent;
        public bool acceptCondo;
        public int bed;
        public int bath;
    }

    [System.Serializable]
    private struct ApartmentBuyerJSON
    {
        public string title;
        public string flavorText;
        public int offer;
        public int minUnits;
    }


    [System.Serializable]
    private struct MarketCardsJSON
    {
        public HomeBuyerJSON[] homeBuyers;
        public ApartmentBuyerJSON[] apartmentBuyers;
    }

    private static HomeBuyerCard[] HomeBuyerCards(MarketCardsJSON marketCards)
    {
        HomeBuyerCard[] homeBuyerCards = new HomeBuyerCard[marketCards.homeBuyers.Length];
        for (int i = 0; i < homeBuyerCards.Length; i++)
        {
            HomeBuyerJSON card = marketCards.homeBuyers[i];
            homeBuyerCards[i] = new HomeBuyerCard(card.title, card.flavorText, card.baseOffer, card.offerOriginal, card.additional, card.percent, card.acceptCondo, card.bed, card.bath);
        }
        return homeBuyerCards;
    }

    private static ApartmentBuyerCard[] ApartmentBuyerCards(MarketCardsJSON marketCards)
    {
        ApartmentBuyerCard[] apartmentBuyerCards = new ApartmentBuyerCard[marketCards.apartmentBuyers.Length];
        for (int i = 0; i < apartmentBuyerCards.Length; i++)
        {
            ApartmentBuyerJSON card = marketCards.apartmentBuyers[i];
            apartmentBuyerCards[i] = new ApartmentBuyerCard(card.title, card.flavorText, card.offer, card.minUnits);
        }
        return apartmentBuyerCards;
    }

    public static List<MarketCard> LoadMarketCards()
    {
        var marketCardsAsset = Resources.Load<TextAsset>("JSON/market_cards");
        MarketCardsJSON marketCardsJSON = JsonUtility.FromJson<MarketCardsJSON>(marketCardsAsset.text);
        List<MarketCard> marketCards = new List<MarketCard>();

        marketCards.AddRange(HomeBuyerCards(marketCardsJSON));
        marketCards.AddRange(ApartmentBuyerCards(marketCardsJSON));

        return marketCards;
    }

    public override string ToString()
    {
        return "Market: " + this.type +
            "\t" + this.title +
            "\t" + this.flavorText;
    }

}