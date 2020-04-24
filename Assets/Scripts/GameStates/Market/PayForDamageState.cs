using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayForDamageState : GameState
{

    private readonly DamageCard damageCard;
    private readonly MarketCardGameObject marketCard;
    private Player player;
    private readonly DamageOptions options;

    private bool selected;
    private bool willPay;
    private bool takingLoan;

    public PayForDamageState(MainGameManager mgm, DamageCard damageCard, MarketCardGameObject marketCard)
    {
        this.damageCard = damageCard;
        this.marketCard = marketCard;
        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        GameObject optionObject = Object.Instantiate(mgm.damageOptionsPrefab, mgm.mainUICanvas.transform);
        optionObject.transform.SetSiblingIndex(marketCard.transform.GetSiblingIndex() + 1);
        options = optionObject.GetComponent<DamageOptions>();

        options.payNow.interactable = this.player.ledger.GetCurretBalance() >= damageCard.cost;
        options.payNow.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            willPay = true;
        });

        options.takeLoan.onClick.AddListener(() => 
        {
            if (selected) return;
            selected = true;
            willPay = false;
        });

        selected = false;
        willPay = false;
        takingLoan = false;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (takingLoan)
        {
            selected = false;
            takingLoan = false;
            this.marketCard.gameObject.SetActive(true);
            this.options.gameObject.SetActive(true);
        }
        if (selected)
        {
            if (willPay)
            {
                this.player.SubtractMoney(this.damageCard.cost);
                Object.Destroy(this.marketCard.gameObject);
                Object.Destroy(this.options.gameObject);
                return new PostTurnState(mgm);
            }
            takingLoan = true;
            this.marketCard.gameObject.SetActive(false);
            this.options.gameObject.SetActive(false);
            return new LoanState(mgm, this);
        }
        options.payNow.interactable = this.player.ledger.GetCurretBalance() >= damageCard.cost;
        return this;
    }
}
