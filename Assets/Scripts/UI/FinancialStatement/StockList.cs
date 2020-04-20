using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StockList : MonoBehaviour
{

    public GameObject listItemPrefab;
    public GameObject dividerPrefab;
    public Transform contentList;

    private readonly List<GameObject> listItems = new List<GameObject>();

    public void ResetList(Player player)
    {
        foreach (GameObject item in listItems)
        {
            Destroy(item);
        }
        listItems.Clear();
        List<IncomeStatement.StockEntry> stocks = player.incomeStatement.StockEntries().OrderBy(entry => entry.Symbol).ThenBy(entry => entry.Price).ToList();
        foreach (IncomeStatement.StockEntry entry in stocks)
        {
            GameObject li = Instantiate(listItemPrefab, contentList);
            StockListItem item = li.GetComponent<StockListItem>();
            item.symbol.text = entry.Symbol;
            item.numberOfShares.text = Utility.FormatNumberCommas(entry.NumberOfShares);
            item.price.text = Utility.FormatMoney(entry.Price);
            listItems.Add(li);
            listItems.Add(Instantiate(dividerPrefab, contentList));
        }
        
    }

}
