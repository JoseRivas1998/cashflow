using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingGoldCardState : GameState
{
    private readonly GoldCard goldCard;
    private readonly DealCardGameObject dealCard;
    private readonly SellCardOptions options;

    private Player targetPlayer;
    private Dictionary<int, PlayerSelectButton> buttons;

    private bool selected;

    public SellingGoldCardState(MainGameManager mgm, GoldCard goldCard, DealCardGameObject dealCard)
    {
        this.goldCard = goldCard;
        this.dealCard = dealCard;
        GameObject optionsObject = Object.Instantiate(mgm.sellCardOptionsPrefab, mgm.mainUICanvas.transform);
        optionsObject.transform.SetSiblingIndex(dealCard.transform.GetSiblingIndex() + 1);
        this.options = optionsObject.GetComponent<SellCardOptions>();
        this.targetPlayer = null;
        this.options.numberInput.confirmBtn.interactable = false;
        int currentPlayer = mgm.turnManager.GetCurrentPlayer();
        buttons = new Dictionary<int, PlayerSelectButton>();
        for (int i = 0; i < mgm.NumPlayers; i++)
        {
            if (i != currentPlayer && !mgm.turnManager.PlayerDroppedOut(i) && !mgm.GetPlayer(i).FastTrack)
            {
                GameObject buttonObject = Object.Instantiate(options.playerButtonPrefab, options.buyerButtons.transform);
                PlayerSelectButton button = buttonObject.GetComponent<PlayerSelectButton>();
                buttons[i] = button;
                button.Initialize(mgm.GetPlayer(i));
                button.button.onClick.AddListener(() => {
                    if (targetPlayer != null) buttons[targetPlayer.index].Deselect();
                    targetPlayer = mgm.GetPlayer(button.playerIndex);
                    button.Select();
                });
            }
        }
        this.selected = false;
        this.options.numberInput.confirmBtn.onClick.AddListener(() => {
            if (selected || targetPlayer == null || options.numberInput.Number <= 0) return;
            this.selected = true;
        });
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (selected)
        {
            Object.Destroy(this.options.gameObject);
            return new OfferingGoldCardState(mgm, goldCard, dealCard, this.options.numberInput.Number, targetPlayer.index);
        }
        this.options.numberInput.confirmBtn.interactable = targetPlayer != null && options.numberInput.Number > 0;
        return this;
    }
}
