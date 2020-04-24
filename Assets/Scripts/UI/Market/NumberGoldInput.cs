using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberGoldInput : MonoBehaviour
{

    public NumberInput numberInput;

    private GoldBuyerCard goldBuyer;

    public void Initialize(GoldBuyerCard goldBuyer)
    {
        this.goldBuyer = goldBuyer;
    }

    // Update is called once per frame
    void Update()
    {
        if (goldBuyer != null)
        {
            numberInput.costText.text = $"{Utility.FormatMoney(goldBuyer.offer * numberInput.Number)}";
        }
    }
}
