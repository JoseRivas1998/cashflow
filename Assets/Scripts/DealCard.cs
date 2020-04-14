using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DealCard
{

    public DealType type { get; protected set; }
    public bool smallDeal { get; protected set; }
    public string title { get; protected set; }
    public string flavorText { get; protected set; }

    [System.Serializable]
    private struct StockCardJSON
    {
        public string symbol;
        public int price;
        public string flavor;
    }

    [System.Serializable]
    private struct HomeCardJSON
    {
        public bool smallDeal;
        public string type;
        public string title;
        public string flavorText;
        public int cost;
        public int mortgage;
        public int downPayment;
        public int cashFlow;
        public int bed;
        public int bath;
    }

    [System.Serializable]
    private struct GoldCardJSON
    {
        public string title;
        public string flavorText;
        public int coins;
        public int cost;
    }

    [System.Serializable]
    private struct GambleCardJSON
    {
        public string title;
        public string flavorText;
        public string instruction;
        public int badMin;
        public int badMax;
        public int goodMin;
        public int goodMax;
        public int reward;
        public bool gold;
        public string badText;
        public string rewardText;
        public int cost;
        public bool mlm;
    }

    [System.Serializable]
    private struct MultiCardJSON
    {
        public string type;
        public string title;
        public string flavorText;
        public int cost;
        public int mortgage;
        public int downPayment;
        public int cashFlow;
        public int units;
    }

    [System.Serializable]
    private struct DealCardsJSON
    {
        public StockCardJSON[] stocks;
        public HomeCardJSON[] homes;
        public GoldCardJSON[] gold;
        public GambleCardJSON[] gamble;
        public MultiCardJSON[] multi;
    }

    private static DealCard[] LoadStockCards(DealCardsJSON dealCards)
    {
        Dictionary<string, Stocks.Stock> stocks = GameData.Instance.GetStocks();
        StockCard[] stockCards = new StockCard[dealCards.stocks.Length];
        for (int i = 0; i < stockCards.Length; i++)
        {
            StockCardJSON card = dealCards.stocks[i];
            stockCards[i] = new StockCard(stocks[card.symbol], card.price, card.flavor);
        }
        return stockCards;
    }

    private static HomeCard[] LoadHomeCards(DealCardsJSON dealCards)
    {
        HomeCard[] homeCards = new HomeCard[dealCards.homes.Length];
        for (int i = 0; i < homeCards.Length; i++)
        {
            HomeCardJSON card = dealCards.homes[i];
            RealEstateType propertyType = (RealEstateType)Enum.Parse(typeof(RealEstateType), card.type, true);
            homeCards[i] = new HomeCard(card.smallDeal, propertyType, card.title, card.flavorText, card.cost, card.mortgage, card.downPayment, card.cashFlow, card.bed, card.bath);
        }
        return homeCards;
    }

    private static GoldCard[] LoadGoldCards(DealCardsJSON dealCards)
    {
        GoldCard[] goldCards = new GoldCard[dealCards.gold.Length];
        for (int i = 0; i < goldCards.Length; i++)
        {
            GoldCardJSON card = dealCards.gold[i];
            goldCards[i] = new GoldCard(card.title, card.flavorText, card.coins, card.cost);
        }
        return goldCards;
    }

    private static GambleCard[] LoadGambleCards(DealCardsJSON dealCards)
    {
        GambleCard[] gambleCards = new GambleCard[dealCards.gamble.Length];
        for (int i = 0; i < gambleCards.Length; i++)
        {
            GambleCardJSON card = dealCards.gamble[i];
            gambleCards[i] = new GambleCard(card.title, card.flavorText, card.instruction, card.goodMin, card.goodMax, card.badMin, card.badMax, card.reward, card.gold, card.rewardText, card.badText, card.cost, card.mlm);
        }
        return gambleCards;
    }

    public static List<DealCard> SmallDeals()
    {
        var dealCardsFile = Resources.Load("JSON/deal_cards");
        DealCardsJSON dealCards = JsonUtility.FromJson<DealCardsJSON>(dealCardsFile.ToString());
        
        List<DealCard> smallDeals = new List<DealCard>();
        smallDeals.AddRange(LoadStockCards(dealCards));

        HomeCard[] homeCards = LoadHomeCards(dealCards);
        foreach (HomeCard homeCard in homeCards)
        {
            if(homeCard.smallDeal)
            {
                smallDeals.Add(homeCard);
            }
        }

        smallDeals.AddRange(LoadGoldCards(dealCards));
        smallDeals.AddRange(LoadGambleCards(dealCards));

        return smallDeals;
    }

    private static MultiCard[] LoadMultiCards(DealCardsJSON dealCards)
    {
        MultiCard[] multiCards = new MultiCard[dealCards.multi.Length];
        for (int i = 0; i < multiCards.Length; i++)
        {
            MultiCardJSON card = dealCards.multi[i];
            RealEstateType propertyType = (RealEstateType)Enum.Parse(typeof(RealEstateType), card.type, true);
            multiCards[i] = new MultiCard(propertyType, card.title, card.flavorText, card.cost, card.mortgage, card.downPayment, card.cashFlow, card.units);
        }
        return multiCards;
    }

    public static List<DealCard> BigDeals()
    {
        var dealCardsFile = Resources.Load("JSON/deal_cards");
        DealCardsJSON dealCards = JsonUtility.FromJson<DealCardsJSON>(dealCardsFile.ToString());

        List<DealCard> bigDeals = new List<DealCard>();

        HomeCard[] homeCards = LoadHomeCards(dealCards);
        foreach (HomeCard homeCard in homeCards)
        {
            if(!homeCard.smallDeal)
            {
                bigDeals.Add(homeCard);
            }
        }

        bigDeals.AddRange(LoadMultiCards(dealCards));

        return bigDeals;

    }

    public override string ToString()
    {
        return (smallDeal ? "Small" : "Big") + " Deal: " + type +
            "\t" + title +
            "\t" + flavorText;
    }

}
