using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TakeMoneyButton : MonoBehaviour, IPointerClickHandler
{
    public MainGameManager mgm;
    public int amount = -100;

    public void OnPointerClick(PointerEventData eventData)
    {
        Player p = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        if (amount > 0)
        {
            p.AddMoney(amount);
        }
        else
        {
            p.SubtractMoney(Mathf.Abs(amount));
        }
    }
}
