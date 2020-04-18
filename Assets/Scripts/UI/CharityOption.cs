using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharityOption : MonoBehaviour
{

    public Button noBtn;
    public Button yesBtn;
    public Text costText;

    public void Initialize(Player player)
    {
        int cost = player.incomeStatement.TotalIncome / 10;
        costText.text = "Pay " + Utility.FormatMoney(cost);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
