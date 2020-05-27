using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTrackCharityState : GameState
{

    private readonly Player player;
    private readonly int cost;
    private readonly FastTrackCharityChoices choices;

    private bool selected;
    private bool willDonate;

    public FastTrackCharityState(MainGameManager mgm)
    {
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        cost = player.fastTrackIncomeStatement.CashFlowDayIncome / 10;

        choices = mgm.SpawnUIObjectBehindCashToggle<FastTrackCharityChoices>(mgm.fastTrackCharityOptionsPrefab);
        choices.price.text = $"Pay {Utility.FormatMoney(cost)}";

        selected = false;

        if (player.ledger.GetCurretBalance() >= cost)
        {
            choices.yesBtn.interactable = true;
            choices.yesBtn.onClick.AddListener(() =>
            {
                if (selected) return;
                willDonate = true;
                selected = true;
            });
        }
        else
        {
            choices.yesBtn.interactable = false;
        }
        choices.noBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            willDonate = false;
            selected = true;
        });

        mgm.gameStateDisplay.gameObject.SetActive(false);
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (selected)
        {
            if (willDonate)
            {
                player.SubtractMoney(cost);
                player.AddCharity();
            }
            Object.Destroy(choices.gameObject);
            return new FastTrackPostTurnState(mgm);
        }
        return this;
    }
}
