using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingDieAmountState : GameState
{
    private int diceAmount;
    private Player player;
    private DieAmountSelect diceAmountSelect;
    public SelectingDieAmountState(MainGameManager mgm)
    {
        diceAmount = -1;
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        GameObject amountSelect = Object.Instantiate(mgm.dieAmountSelectPrefab, mgm.mainUICanvas.transform);
        amountSelect.transform.SetSiblingIndex(mgm.financialStatementToggle.transform.GetSiblingIndex() - 1);
        mgm.gameStateDisplay.gameObject.SetActive(false);
        diceAmountSelect = amountSelect.GetComponent<DieAmountSelect>();
        diceAmountSelect.Initialize(player);
        player.SubtractFromCharity();
        diceAmountSelect.oneBtn.onClick.AddListener(() => {
            if (diceAmount == -1)
            {
                diceAmount = 1;
            }
        });
        diceAmountSelect.twoBtn.onClick.AddListener(() => {
            if (diceAmount == -1)
            {
                diceAmount = 2;
            }
        });
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (diceAmount != -1)
        {
            Object.Destroy(diceAmountSelect.gameObject);
            return new PlayerRollDiceState(mgm, diceAmount);
        }
        return this;
    }
}
