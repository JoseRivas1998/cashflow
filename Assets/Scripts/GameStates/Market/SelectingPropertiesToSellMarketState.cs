using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SelectingPropertiesToSellMarketState : GameState
{
    private readonly RealEstateMarketCard buyerCard;
    private readonly MarketCardGameObject marketCard;
    private readonly Player player;

    private readonly List<RealEstateCardOption> cardOptions;
    private readonly RealEstateSell realEstateSell;

    private bool cardsShowing = false;

    private bool done = false;
    private readonly SellingRealEstateMarketState previousState;

    public SelectingPropertiesToSellMarketState(MainGameManager mgm, RealEstateMarketCard buyer, MarketCardGameObject marketCard, SellingRealEstateMarketState previousState)
    {
        this.buyerCard = buyer;
        this.marketCard = marketCard;
        this.previousState = previousState;

        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

        GameObject options = Object.Instantiate(mgm.realEstateSellPrefab, mgm.mainUICanvas.transform);
        options.transform.SetSiblingIndex(this.marketCard.transform.GetSiblingIndex() + 1);
        realEstateSell = options.GetComponent<RealEstateSell>();
        realEstateSell.confirm.onClick.AddListener(() => {
            if (done) return;
            done = true;
        });

        List<RealEstateCard> toSell = this.player.incomeStatement.RealEstate().Where(RealEstateMarketCard.GetRealEstatePredicate(this.buyerCard)).ToList();
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
        if(!cardsShowing)
        {
            cardsShowing = true;
            foreach (RealEstateCardOption cardOption in cardOptions)
            {
                cardOption.dealCard.cardFlip.background.texture = cardOption.dealCard.cardFlip.cardFront;
                cardOption.dealCard.realEstateHolder.SetActive(true);
            }
        }
        if(done)
        {
            this.player.SellRealEstates(buyerCard, cardOptions.Where(option => option.selected).Select(option => option.realEstate).ToList());
            Object.Destroy(this.realEstateSell.gameObject);
            mgm.financialStatementToggle.Close();
            mgm.cashLedgerToggle.Close();
            return previousState;
        }
        int capitalGains = player.CapitalGains(buyerCard, cardOptions.Where(option => option.selected).Select(option => option.realEstate).ToList());
        this.realEstateSell.capitalGains.text = $"Capital Gains: {Utility.FormatMoney(capitalGains)}";
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
                if(selected)
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
