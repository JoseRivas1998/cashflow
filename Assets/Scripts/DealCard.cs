using System.Collections;
using System.Collections.Generic;
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
    private struct DealCardsJSON
    {
        public StockCardJSON[] stocks;
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

    public static List<DealCard> SmallDeals()
    {
        var dealCardsFile = Resources.Load("JSON/deal_cards");
        DealCardsJSON dealCards = JsonUtility.FromJson<DealCardsJSON>(dealCardsFile.ToString());
        
        List<DealCard> smallDeals = new List<DealCard>();
        smallDeals.AddRange(LoadStockCards(dealCards));


        return smallDeals;
    }

    public override string ToString()
    {
        return (smallDeal ? "Small" : "Big") + " Deal: " + type;
    }

}
