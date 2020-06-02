using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnforeseenRepairsState : GameState
{

    private enum Choices
    {
        Pay, LoseAsset
    }

    private readonly Player player;
    private readonly int price;
    private UnforeseenRepairsOptions options;

    private bool done;
    private Choices choice;

    public UnforeseenRepairsState(MainGameManager mgm)
    {
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        price = Mathf.Min(player.ledger.GetCurretBalance(), player.fastTrackIncomeStatement.CashFlowDayIncome * 10);

        done = false;

        options = mgm.SpawnUIObjectBehindCashToggle<UnforeseenRepairsOptions>(mgm.unforeseenRepairsOPtionsPrefab);
        options.price.text = $"{Utility.FormatMoney(price)}";

        if (player.fastTrackIncomeStatement.NumEntries > 0)
        {
            options.lostAssetButton.interactable = true;
            options.lostAssetButton.onClick.AddListener(() =>
            {
                if (done) return;
                done = true;
                choice = Choices.LoseAsset;
            });
        }
        else
        {
            options.lostAssetButton.interactable = false;
        }
        options.payButton.onClick.AddListener(() =>
        {
            if (done) return;
            done = true;
            choice = Choices.Pay;
        });
        mgm.gameStateDisplay.gameObject.SetActive(false);
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (done)
        {
            switch (choice)
            {
                case Choices.Pay:
                    player.SubtractMoney(price);
                    break;
                case Choices.LoseAsset:
                    player.fastTrackIncomeStatement.RemoveLowestAsset();
                    break;
                default:
                    break;
            }
            Object.Destroy(options.gameObject);
            return new FastTrackPostTurnState(mgm);
        }
        return this;
    }
}
