using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockCard : DealCard
{
    public Stocks.Stock stock { get; private set; }
    public int price { get; private set; }
    public string flavorText { get; private set; }

    public StockCard(Stocks.Stock stock, int price, string flavorText)
    {
        this.type = DealType.Stock;
        this.smallDeal = true;
        this.stock = stock;
        this.price = price;
        this.flavorText = flavorText;
    }

    public override string ToString()
    {
        return base.ToString() +
            "\t" + (stock.isStock ? "Stock" : "Mutual Fund") + " - " + stock.title +
            "\tSymbol: " + stock.symbol +
            "\tToday's Price: $" + this.price + 
            "\tHistoric Trading Range: $" + stock.priceMin + " - $" + stock.priceMax +
            "\t" + flavorText;
    }

}
