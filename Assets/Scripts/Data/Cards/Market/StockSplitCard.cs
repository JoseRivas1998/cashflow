using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockSplitCard : MarketCard
{
    public Stocks.Stock stock { get; private set; }
    
    public StockSplitCard(Stocks.Stock stock, string flavorText)
    {
        this.type = MarketType.StockSplit;
        this.stock = stock;
        this.title = "Stock - " + this.stock.title;
        this.flavorText = flavorText;
    }

}
