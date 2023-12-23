using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FastTrackCashGambleState : GameState
{

    private readonly Player player;
    private readonly TitleDescriptionCost gamble;
    private readonly FastTrackGambleOptions fastTrackGambleOptions;

    private bool isDone;
    private DiceContainer die;

    public FastTrackCashGambleState(MainGameManager mgm, FastTrackSpaceType spaceType)
    {
        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        this.gamble = this.GetGamble(spaceType);
        this.fastTrackGambleOptions = this.CreateOptions(mgm);
        this.isDone = false;
    }

    private TitleDescriptionCost GetGamble(FastTrackSpaceType space)
    {
        switch (space)
        {
            case FastTrackSpaceType.SoftwareCoIPO:
                return new TitleDescriptionCost {
                    title = "Software Co. IPO",
                    description = "Buy 250,000 shares at 10¢/share.\nIf you roll a 6 on one die, shares\ngo to $2/share - get $500,000 cash\nfrom bank. Roll less than 6, get $0.",
                    cost = 25000,
                    threshold = 6,
                    reward = 500000,
                };
            case FastTrackSpaceType.BioTecCoIPO:
                return new TitleDescriptionCost {
                    title = "Bio-Tech Co. IPO",
                    description = "If you invest, pay $50,000 and roll one die. If you roll a 5 or 6, collect $500,000! If you roll less than a 5, you lose your investment and collect nothing.",
                    cost = 50000,
                    threshold = 5,
                    reward = 500000,
                };
        }
        return new TitleDescriptionCost {
            title = "",
            cost = 0,
            description = "",
            threshold = 100,
            reward = 0,
        };
    }

    private FastTrackGambleOptions CreateOptions(MainGameManager mgm)
    {
        var options = mgm.SpawnUIObjectBehindCashToggle<FastTrackGambleOptions>(mgm.fastTrackGambleOptionsPrefab);

        options.title.text = this.gamble.title;
        options.description.text = this.gamble.description;
        options.investment.text = $"{Utility.FormatMoney(this.gamble.cost)} Investment";

        if (this.player.ledger.GetCurretBalance() >= this.gamble.cost)
        {
            options.invest.interactable = true;
            options.invest.onClick.AddListener(() => {
                mgm.StartCoroutine(this.Invest(mgm));
            });
        }
        else
        {
            options.invest.interactable = false;
        }

        options.dontInvest.interactable = true;
        options.dontInvest.onClick.AddListener(() => {
            mgm.StartCoroutine(this.ExitState(mgm, true));
        });

        return options;
    }

    IEnumerator Invest(MainGameManager mgm)
    {
        this.DestroyOptions(mgm);
        mgm.gameStateDisplay.SetText($"{player.name} is investing in {this.gamble.title}");
        player.SubtractMoney(this.gamble.cost);
        yield return SpawnDie(mgm);
    }

    IEnumerator SpawnDie(MainGameManager mgm)
    {
        this.die = mgm.SpawnDice(1, true)[0];
        mgm.mainCamTracker.TrackObject(die.transform);
        yield return ShakeDie(mgm);
    }

    IEnumerator ShakeDie(MainGameManager mgm)
    {
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
        yield return RolLDie(mgm);
    }

    IEnumerator RolLDie(MainGameManager mgm)
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
        yield return ProcessResult(mgm, value);
    }

    IEnumerator ProcessResult(MainGameManager mgm, int dieValue)
    {
        yield return new WaitForEndOfFrame();
        if (dieValue >= this.gamble.threshold)
        {
            this.player.AddMoney(this.gamble.reward);
        }
        mgm.gameStateDisplay.SetText($"{player.name} rolled a {dieValue}!");
        yield return new WaitForSeconds(1);
        Object.Destroy(die.gameObject);
        yield return ExitState(mgm);
    }

    IEnumerator ExitState(MainGameManager mgm, bool destroyOptions = false)
    {
        yield return new WaitForEndOfFrame();
        mgm.mainCamTracker.TrackObject(this.player.gamePiece.transform);
        if (destroyOptions) this.DestroyOptions(mgm);
        this.isDone = true;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (!this.isDone) return this;
        return new FastTrackPostTurnState(mgm);
    }

    private void DestroyOptions(MainGameManager mgm)
    {
        Object.Destroy(this.fastTrackGambleOptions.gameObject);
        mgm.financialStatementToggle.Close();
        mgm.cashLedgerToggle.Close();
    }

    private class TitleDescriptionCost
    {
        public string title;
        public int cost;
        public string description;
        public int threshold;
        public int reward;
    }

}

