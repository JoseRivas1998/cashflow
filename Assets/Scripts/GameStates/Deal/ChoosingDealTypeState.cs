using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingDealTypeState : GameState
{
    private bool selected;
    private bool smallDeal;
    private readonly DealTypeChoices choices;

    public ChoosingDealTypeState(MainGameManager mgm)
    {
        GameObject choicesObject = Object.Instantiate(mgm.dealTypeChoicesPrefab, mgm.mainUICanvas.transform);
        choicesObject.transform.SetSiblingIndex(mgm.cashLedgerToggle.transform.GetSiblingIndex());
        choices = choicesObject.GetComponent<DealTypeChoices>();
        selected = false;
        mgm.gameStateDisplay.SetText("Select a deal type");
        choices.bigDealBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            smallDeal = false;
        });
        choices.smallDealBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            smallDeal = true;
        });
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (selected)
        {
            Object.Destroy(choices.gameObject);
            return new DealCardFlippingState(mgm, smallDeal);
        }
        return this;
    }
}
