using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeMoneyButton : MonoBehaviour
{
    public MainGameManager mgm;
    public int amount = -100;
    public void OnClick()
    {
        Player p = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        if(amount > 0)
        {
            p.AddMoney(amount);
        }
        else
        {
            p.SubtractMoney(Mathf.Abs(amount));
        }
    }

}
