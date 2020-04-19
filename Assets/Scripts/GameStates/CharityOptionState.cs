using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharityOptionState : GameState
{

    private bool selecting;
    private bool willBuy;
    private Player player;
    private CharityOption charityOption;
    private bool takingLoan;

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
        this.takingLoan = false;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(takingLoan)
        {
            charityOption.gameObject.SetActive(true);
            takingLoan = false;
            selecting = true;
        }
        if(!selecting)
        {
            if(willBuy)
            {
                int cost = player.incomeStatement.TotalIncome / 10;
                if (player.ledger.GetCurretBalance() - cost >= 0)
                {
                    player.SubtractMoney(cost);
                    player.AddCharity();
                }
                else
                {
                    charityOption.gameObject.SetActive(false);
                    takingLoan = true;
                    return new LoanState(mgm, this);
                }
            }
            Object.Destroy(charityOption.gameObject);
            return new PostTurnState(mgm);
        }
        return this;
    }
}
