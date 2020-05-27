using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTrackDiceSelectState : GameState
{

    private readonly DieAmountSelect amountSelect;

    private int numDice;
    private bool selected;

    public FastTrackDiceSelectState(MainGameManager mgm)
    {
        Player player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        amountSelect = mgm.SpawnUIObjectBehindCashToggle<DieAmountSelect>(mgm.dieAmountSelectPrefab);

        amountSelect.Initialize(player);
        player.SubtractFromCharity();

        selected = false;
        amountSelect.oneBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            numDice = 1;
        });
        amountSelect.twoBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            numDice = 2;
        });

        mgm.gameStateDisplay.gameObject.SetActive(false);

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (selected)
        {
            Object.Destroy(amountSelect.gameObject);
            return new FastTrackRollingState(mgm, numDice);
        }
        return this;
    }
}
