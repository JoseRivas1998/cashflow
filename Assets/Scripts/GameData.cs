using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Stocks;

public class GameData : Singleton<GameData>
{

    private bool professionsLoaded = false;
    private bool stocksLoaded = false;

    private Professions professions;
    private Dictionary<string, Stock> stocks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Professions GetProfessions()
    {
        if(professionsLoaded)
        {
            return professions;
        }
        var professionsJSON = Resources.Load("JSON/professions");
        professions = Professions.CreateFromJSON(professionsJSON.ToString());
        professionsLoaded = true;
        return professions;
    }

    public Dictionary<string, Stock> GetStocks()
    {
        if(stocksLoaded)
        {
            return stocks;
        }
        var stocksJSON = Resources.Load("JSON/stocks");
        stocks = Stocks.CreateFromJSON(stocksJSON.ToString());
        stocksLoaded = true;
        return stocks;
    }

}
