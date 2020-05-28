using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessInvestmentState : GameState
{

    private readonly Player player;
    private readonly BusinessInvestment businessInvestment;
    private readonly BusinessInvestmentOptions options;

    private bool done;
    private bool willInvest;

    public BusinessInvestmentState(MainGameManager mgm, BusinessInvestment businessInvestment)
    {
        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        this.businessInvestment = businessInvestment;
        this.options = mgm.SpawnUIObjectBehindCashToggle<BusinessInvestmentOptions>(mgm.businessInvestmentOptionsPrefab);

        done = false;

        this.options.businessInvestmentDisplay.SetInvestment(businessInvestment);

        if (this.player.ledger.GetCurretBalance() >= businessInvestment.down)
        {
            this.options.invest.interactable = true;
            this.options.invest.onClick.AddListener(() =>
            {
                if (done) return;
                done = true;
                willInvest = true;
            });
        }
        else
        {
            this.options.invest.interactable = false;
        }
        this.options.dontInvest.onClick.AddListener(() =>
        {
            if (done) return;
            done = true;
            willInvest = false;
        });

        mgm.gameStateDisplay.gameObject.SetActive(false);
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (done)
        {
            if (willInvest)
            {
                player.SubtractMoney(businessInvestment.down);
                player.fastTrackIncomeStatement.AddEntry(businessInvestment.name, businessInvestment.cashFlow);
            }
            Object.Destroy(options.gameObject);
            mgm.financialStatementToggle.Close();
            mgm.cashLedgerToggle.Close();
            return new FastTrackPostTurnState(mgm);
        }
        return this;
    }
}
