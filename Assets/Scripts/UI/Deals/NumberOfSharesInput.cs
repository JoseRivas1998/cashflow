using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOfSharesInput : MonoBehaviour
{
    public NumberInput numberInput;

    private StockCard stock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(StockCard stock)
    {
        this.stock = stock;
    }

    // Update is called once per frame
    void Update()
    {
        if(stock != null)
        {
            numberInput.costText.text = "Total Cost: " + Utility.FormatMoney(this.stock.price * numberInput.Number);
        }
    }
}
