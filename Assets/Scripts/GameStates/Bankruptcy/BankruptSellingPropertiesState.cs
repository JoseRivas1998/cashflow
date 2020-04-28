using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BankruptSellingPropertiesState : GameState
{
    private readonly Player player;

    private readonly List<RealEstateCardOption> cardOptions;
    private readonly RealEstateSell realEstateSell;

    private bool cardsShowing = false;

    private bool done = false;
    private readonly BankruptOptionsState previousState;

    public BankruptSellingPropertiesState(MainGameManager mgm, BankruptOptionsState previousState)
    {
        this.previousState = previousState;

        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        GameObject options = Object.Instantiate(mgm.realEstateSellPrefab, mgm.mainUICanvas.transform);
        options.transform.SetSiblingIndex(mgm.cashLedgerToggle.transform.GetSiblingIndex());
        realEstateSell = options.GetComponent<RealEstateSell>();
        realEstateSell.confirm.onClick.AddListener(() => {
            if (done) return;
            done = true;
        });

        List<RealEstateCard> toSell = this.player.incomeStatement.RealEstate();
        GameObject currentRow = null;
        cardOptions = new List<RealEstateCardOption>();
        for (int i = 0; i < toSell.Count; i++)
        {
            if (i % 2 == 0)
            {
                currentRow = Object.Instantiate(mgm.realEstateRowPrefab, realEstateSell.content.transform);
            }
            cardOptions.Add(RealEstateCardOption.CreateCardOption(mgm, currentRow, toSell[i]));
        }

    }

    public override GameState Update(MainGameManager mgm)
    {
        if (!cardsShowing)
        {
            cardsShowing = true;
            foreach (RealEstateCardOption cardOption in cardOptions)
            {
                cardOption.dealCard.cardFlip.background.texture = cardOption.dealCard.cardFlip.cardFront;
                cardOption.dealCard.realEstateHolder.SetActive(true);
            }
        }
        if (done)
        {
            this.player.SellRealEstateToBank(cardOptions.Where(option => option.selected).Select(option => option.realEstate).ToList());
            Object.Destroy(this.realEstateSell.gameObject);
            mgm.financialStatementToggle.Close();
            mgm.cashLedgerToggle.Close();
            return previousState;
        }
        int capitalGains = cardOptions.Where(option => option.selected).Select(option => option.realEstate).Sum(realEstate => realEstate.downPayment / 2);
        this.realEstateSell.capitalGains.text = $"Amount Recieved: {Utility.FormatMoney(capitalGains)}";
        return this;
    }

    private class RealEstateCardOption
    {
        public readonly RealEstateCardButton cardButton;
        public readonly DealCardGameObject dealCard;
        public readonly RealEstateCard realEstate;

        public bool selected { get; private set; }

        public static RealEstateCardOption CreateCardOption(MainGameManager mgm, GameObject parent, RealEstateCard realEstate)
        {
            GameObject newObject = Object.Instantiate(mgm.realEstateButtonPrefab, parent.transform);
            return new RealEstateCardOption(newObject, realEstate);
        }

        private RealEstateCardOption(GameObject realEstateCardButton, RealEstateCard realEstate)
        {
            this.cardButton = realEstateCardButton.GetComponent<RealEstateCardButton>();
            this.cardButton.button.onClick.AddListener(() => {
                if (selected)
                {
                    Deselect();
                }
                else
                {
                    Select();
                }
            });
            this.dealCard = realEstateCardButton.GetComponent<DealCardGameObject>();
            this.realEstate = realEstate;
            this.dealCard.SetDeal(realEstate);
            this.dealCard.cardFlip.background.texture = this.dealCard.cardFlip.cardFront;
            this.dealCard.realEstateHolder.SetActive(true);
            selected = false;
        }

        public void Select()
        {
            selected = true;
            cardButton.checkMark.SetActive(true);
        }

        public void Deselect()
        {
            selected = false;
            cardButton.checkMark.SetActive(false);
        }

    }
}
