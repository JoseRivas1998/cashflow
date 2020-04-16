using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputDataState : GameState
{

    private Text nextButtonText;
    private bool done;

    public PlayerInputDataState(MainGameManager mgm)
    {
        nextButtonText = mgm.playerNameDreamColor.nextButton.GetComponentInChildren<Text>();
        nextButtonText.text = mgm.numPlayers == 1 ? "Assign Professions!" : "Next";
        done = false;
        mgm.playerNameDreamColor.nextButton.onClick.AddListener(() =>
        {
            string name = mgm.playerNameDreamColor.nameField.text.Trim();
            mgm.playerNameDreamColor.nameField.text = "";
            string dream = mgm.playerNameDreamColor.dreamField.text.Trim();
            mgm.playerNameDreamColor.dreamField.text = "";
            Color color = mgm.playerNameDreamColor.colorSelector.selectedButton.image.color;
            if (mgm.playerNameDreamColor.currentPlayer == mgm.numPlayers - 1)
            {
                done = true;
            }
            else 
            {
                if (mgm.playerNameDreamColor.currentPlayer + 1 == mgm.numPlayers - 1)
                {
                    nextButtonText.text = "Assign Professions!";
                }
            }
            mgm.playerNameDreamColor.NextPlayer();
            Debug.Log(name + " " + dream + " " + color);
        });
        mgm.playerNameDreamColor.Activate();
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (done)
        {
            mgm.playerNameDreamColor.gameObject.SetActive(false);
            return new LoopState();
        }
        return this;
    }
}
