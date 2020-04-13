using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stocks 
{
    
    [System.Serializable]
    public struct Stock
    {
        public string symbol;
        public string title;
        public int priceMin;
        public int priceMax;
        public bool isStock;
    }

    [System.Serializable]
    public struct StockList
    {
        public Stock[] stocks;
    }

    public static Dictionary<string, Stock> CreateFromJSON(string jsonString)
    {
        StockList stockList = JsonUtility.FromJson<StockList>(jsonString);
        Stock[] stocks = stockList.stocks;
        Dictionary<string, Stock> stockDict = new Dictionary<string, Stock>();
        foreach (Stock stock in stocks)
        {
            stockDict[stock.symbol] = stock;
        }
        return stockDict;
    }

}
