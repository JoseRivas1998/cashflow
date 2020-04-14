using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DealCard
{

    public DealType type { get; protected set; }
    public bool smallDeal { get; protected set; }

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
    private struct DealCardsJSON
    {
        public StockCardJSON[] stocks;
        public HomeCardJSON[] homes;
        public GoldCardJSON[] gold;
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

        return smallDeals;
    }

    public override string ToString()
    {
        return (smallDeal ? "Small" : "Big") + " Deal: " + type;
    }

}
