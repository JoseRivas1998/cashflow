using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankruptDropOutOptionsState : GameState
{

    private readonly Player player;
    private readonly YesNoOptions options;
    private readonly BankruptOptions bankruptOptions;
    private readonly BankruptOptionsState previousState;

    private bool done;
    private bool willDrop;

    public BankruptDropOutOptionsState(MainGameManager mgm, BankruptOptions bankruptOptions, BankruptOptionsState previousState)
    {
        this.bankruptOptions = bankruptOptions;
        this.previousState = previousState;
        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        GameObject yesNoObject = Object.Instantiate(mgm.yesNoOptionsPrefab, mgm.mainUICanvas.transform);
        yesNoObject.transform.SetSiblingIndex(bankruptOptions.transform.GetSiblingIndex());
        this.options = yesNoObject.GetComponent<YesNoOptions>();

        this.options.prompt.text = "Will you drop out?";

        this.options.yes.onClick.AddListener(() =>
        {
            if (done) return;
            done = true;
            willDrop = true;
        });

        this.options.no.onClick.AddListener(() =>
        {
            if (done) return;
            done = true;
            willDrop = false;
        });

        done = false;
        willDrop = false;

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (done)
        {
            Object.Destroy(this.options.gameObject);
            if (willDrop)
            {
                Object.Destroy(this.bankruptOptions.gameObject);
                mgm.DropOutPlayer(this.player.index);
                mgm.mainCamTracker.TrackObject(null);
                return new PreTurn(mgm);
            }
            return this.previousState;
        }
        return this;
    }

}
