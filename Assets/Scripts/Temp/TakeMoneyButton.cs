using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeMoneyButton : MonoBehaviour
{
    public MainGameManager mgm;

    public void OnClick()
    {
        Player p = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        p.SubtractMoney(p.ledger.GetCurretBalance() - 100);
    }

}
