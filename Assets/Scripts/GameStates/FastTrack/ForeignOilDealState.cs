using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeignOilDealState : GameState
{

    private readonly Player player;
    private readonly ForeignOilDealOptions foreignOilDealOptions;

    private const int investmentCost = 750000;
    private const int cashFlow = 75000;

    private bool isSelected;
    private bool willInvest;
    private DiceContainer die;

    private bool isDone;

    public ForeignOilDealState(MainGameManager mgm)
    {
        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        this.foreignOilDealOptions = mgm.SpawnUIObjectBehindCashToggle<ForeignOilDealOptions>(mgm.foreignOilDealOptionsPrefab);
        this.isSelected = false;

        this.isDone = false;

        this.InitInvestButton(mgm);
        this.InitDontInvestButton();

    }

    private void InitInvestButton(MainGameManager mgm)
    {
        if (this.player.ledger.GetCurretBalance() < investmentCost)
        {
            this.foreignOilDealOptions.invest.interactable = false;
            return;
        }
        this.foreignOilDealOptions.invest.interactable = true;
        this.foreignOilDealOptions.invest.onClick.AddListener(() => {
            if (this.isSelected) return;
            this.willInvest = true;
            this.isSelected = true;
            mgm.StartCoroutine(Invest(mgm));
        });
    }

    private void InitDontInvestButton()
    {
        this.foreignOilDealOptions.dontInvest.onClick.AddListener(() => { 
            if (this.isSelected) return;
            this.willInvest = false;
            this.isSelected = true;
        });
    }

    IEnumerator Invest(MainGameManager mgm)
    {
        player.SubtractMoney(investmentCost);
        mgm.gameStateDisplay.SetText($"{player.name} is investing in Foreign Oil");
        this.DestroyOptions(mgm);
        yield return SpawnDie(mgm);
    }

    IEnumerator SpawnDie(MainGameManager mgm)
    {
        this.die = mgm.SpawnDice(1, true)[0];
        yield return ShakeDie(mgm);
    }

    IEnumerator ShakeDie(MainGameManager  mgm)
    {
        mgm.mainCamTracker.TrackObject(die.transform);
        die.roller.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        while (Input.GetMouseButton(0))
        {
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        while (!Input.GetMouseButton(0))
        {
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        while (Input.GetMouseButton(0))
        {
            die.roller.rb.constraints = RigidbodyConstraints.None;
            mgm.mainCamTracker.TrackObject(null);
            this.die.roller.Shake();
            yield return null;
        }
        yield return RolleDie(mgm);
    }

    IEnumerator RolleDie(MainGameManager mgm)
    {
        mgm.mainCamTracker.TrackObject(die.transform);
        int value = -1;
        while (value == -1)
        {
            this.die.roller.ResetRoll();
            this.die.roller.Roll();
            while (!this.die.roller.RollComplete())
            {
                yield return null;
            }
            value = this.die.dir.DieValue();
        }
        yield return new WaitForSeconds(1f);
        yield return ExitState(mgm, value);
    }

    IEnumerator ExitState(MainGameManager mgm, int dieValue)
    {
        yield return new WaitForEndOfFrame();
        player.fastTrackIncomeStatement.AddEntry("Foreign Oil Deal", dieValue == 6 ? cashFlow : 0);
        mgm.gameStateDisplay.SetText($"{player.name} rolled a {dieValue}!");
        yield return new WaitForSeconds(1f);
        Object.Destroy(die.gameObject);
        mgm.mainCamTracker.TrackObject(this.player.gamePiece.transform);
        this.isDone = true;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (!this.isSelected) return this;
        if (!this.willInvest)
        {
            this.DestroyOptions(mgm);
            return new FastTrackPostTurnState(mgm);
        }
        if (!this.isDone) return this;
        return new FastTrackPostTurnState(mgm);
    }

    private void DestroyOptions(MainGameManager mgm)
    {
        Object.Destroy(this.foreignOilDealOptions.gameObject);
        mgm.financialStatementToggle.Close();
        mgm.cashLedgerToggle.Close();
    }

}
