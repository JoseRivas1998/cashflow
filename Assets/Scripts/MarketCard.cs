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
    private struct PlexBuyerJSON
    {
        public int additional;
        public bool percent;
    }

    [System.Serializable]
    private struct BonusCardJSON
    {
        public string title;
        public string flavorText;
        public int cashFlowMax;
        public int cashFlowIncrease;
        public bool everyone;
    }

    [System.Serializable]
    private struct DamageCardJSON
    {
        public string title;
        public string flavorText;
        public string instruction;
        public int cost;
    }

    [System.Serializable]
    private struct StockSplitCardJSON
    {
        public string symbol;
        public string flavorText;
    }

    [System.Serializable]
    private struct GoldBuyerCardJSON 
    {
        public string title;
        public string flavorText;
        public int offer;
    }

    [System.Serializable]
    private struct MarketCardsJSON
    {
        public HomeBuyerJSON[] homeBuyers;
        public ApartmentBuyerJSON[] apartmentBuyers;
        public PlexBuyerJSON[] plexBuyers;
        public BonusCardJSON[] bonuses;
        public DamageCardJSON[] damages;
        public StockSplitCardJSON[] stockSplits;
        public GoldBuyerCardJSON[] goldBuyers;
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

    private static PlexBuyerCard[] PlexBuyers(MarketCardsJSON marketCards)
    {
        PlexBuyerCard[] plexBuyerCards = new PlexBuyerCard[marketCards.plexBuyers.Length];
        for (int i = 0; i < plexBuyerCards.Length; i++)
        {
            PlexBuyerJSON card = marketCards.plexBuyers[i];
            plexBuyerCards[i] = new PlexBuyerCard(card.additional, card.percent);
        }
        return plexBuyerCards;
    }

    private static BonusCard[] BonusCards(MarketCardsJSON marketCards)
    {
        BonusCard[] bonusCards = new BonusCard[marketCards.bonuses.Length];
        for (int i = 0; i < bonusCards.Length; i++)
        {
            BonusCardJSON card = marketCards.bonuses[i];
            bonusCards[i] = new BonusCard(card.title, card.flavorText, card.cashFlowMax, card.cashFlowIncrease, card.everyone);
        }
        return bonusCards;
    }

    private static DamageCard[] DamageCards(MarketCardsJSON marketCards)
    {
        DamageCard[] damageCards = new DamageCard[marketCards.damages.Length];
        for (int i = 0; i < damageCards.Length; i++)
        {
            DamageCardJSON card = marketCards.damages[i];
            damageCards[i] = new DamageCard(card.title, card.flavorText, card.cost, card.instruction);
        }
        return damageCards;
    }

    private static StockSplitCard[] StockSplitCards(MarketCardsJSON marketCards)
    {
        Dictionary<string, Stocks.Stock> stocks = GameData.Instance.GetStocks();
        StockSplitCard[] stockSplitCards = new StockSplitCard[marketCards.stockSplits.Length];
        for (int i = 0; i < stockSplitCards.Length; i++)
        {
            StockSplitCardJSON card = marketCards.stockSplits[i];
            stockSplitCards[i] = new StockSplitCard(stocks[card.symbol], card.flavorText);
        }
        return stockSplitCards;
    }

    private static GoldBuyerCard[] GoldBuyerCards(MarketCardsJSON marketCards)
    {
        GoldBuyerCard[] goldBuyerCards = new GoldBuyerCard[marketCards.goldBuyers.Length];
        for (int i = 0; i < goldBuyerCards.Length; i++)
        {
            GoldBuyerCardJSON card = marketCards.goldBuyers[i];
            goldBuyerCards[i] = new GoldBuyerCard(card.title, card.flavorText, card.offer);
        }
        return goldBuyerCards;
    }

    public static List<MarketCard> LoadMarketCards()
    {
        var marketCardsAsset = Resources.Load<TextAsset>("JSON/market_cards");
        MarketCardsJSON marketCardsJSON = JsonUtility.FromJson<MarketCardsJSON>(marketCardsAsset.text);
        List<MarketCard> marketCards = new List<MarketCard>();

        marketCards.AddRange(HomeBuyerCards(marketCardsJSON));
        marketCards.AddRange(ApartmentBuyerCards(marketCardsJSON));
        marketCards.AddRange(PlexBuyers(marketCardsJSON));
        marketCards.AddRange(BonusCards(marketCardsJSON));
        marketCards.AddRange(DamageCards(marketCardsJSON));
        marketCards.AddRange(StockSplitCards(marketCardsJSON));
        marketCards.AddRange(GoldBuyerCards(marketCardsJSON));

        return marketCards;
    }

    public override string ToString()
    {
        return "Market: " + this.type +
            "\t" + this.title +
            "\t" + this.flavorText;
    }

}
