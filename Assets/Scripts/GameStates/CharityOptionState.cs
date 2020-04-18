using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharityOptionState : GameState
{

    private bool selecting;
    private bool willBuy;
    private Player player;
    private CharityOption charityOption;

    public CharityOptionState(MainGameManager mgm)
    {
        selecting = true;
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        GameObject optionObject = Object.Instantiate(mgm.charityOptionPrefab, mgm.mainUICanvas.transform);
        optionObject.transform.SetSiblingIndex(mgm.financialStatementToggle.transform.GetSiblingIndex() - 1);
        charityOption = optionObject.GetComponent<CharityOption>();
        charityOption.Initialize(player);
        charityOption.yesBtn.onClick.AddListener(() => {
            if (!selecting) return;
            selecting = false;
            willBuy = true;
        });
        charityOption.noBtn.onClick.AddListener(() => {
            if (!selecting) return;
            selecting = false;
            willBuy = false;
        });
        mgm.gameStateDisplay.gameObject.SetActive(false);
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(!selecting)
        {
            Object.Destroy(charityOption.gameObject);
            if(willBuy)
            {
                int cost = player.incomeStatement.TotalIncome / 10;
                // TODO CHECK FOR LOAN
                player.SubtractMoney(cost);
                player.AddCharity();
            }
            // TODO GO TO END TURN
            return new PreTurn(mgm);
        }
        return this;
    }
}
