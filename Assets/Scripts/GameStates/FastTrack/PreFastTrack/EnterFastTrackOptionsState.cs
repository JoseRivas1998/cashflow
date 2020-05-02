using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterFastTrackOptionsState : GameState
{
    private EnterFastTrackOption option;

    private bool done;

    public EnterFastTrackOptionsState(MainGameManager mgm)
    {
        GameObject optionObject = Object.Instantiate(mgm.enterFastTrackOptionPrefab, mgm.mainUICanvas.transform);
        optionObject.transform.SetSiblingIndex(mgm.cashLedgerToggle.transform.GetSiblingIndex());

        option = optionObject.GetComponent<EnterFastTrackOption>();
        option.btn.onClick.AddListener(() => {
            if (done) return;
            done = true;
        });

        Player p = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        mgm.gameStateDisplay.SetText($"{p.name} has made it to the fast track!");

        done = false;
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(done)
        {
            Object.Destroy(option.gameObject);
            return new RollingFastTrackSpaceState(mgm);
        }
        return this;
    }
}
