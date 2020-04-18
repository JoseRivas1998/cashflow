using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieAmountSelect : MonoBehaviour
{

    public Text turnsLeft;
    public Button oneBtn;
    public Button twoBtn;

    public void Initialize(Player player)
    {
        if(player.charityTurnsLeft == 1)
        {
            turnsLeft.text = "This is your last charity turn";
        } 
        else
        {
            turnsLeft.text = player.charityTurnsLeft + " charity turns left";
        }
    }
}
