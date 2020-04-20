using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealCardGameObject : MonoBehaviour
{
    [System.Serializable]
    public struct StockCardText
    {
        public Text title;
        public Text flavorText;
        public Text symbol;
        public Text todaysPrice;
        public Text historicTradingRange;
    }

    [System.Serializable]
    public struct RealEstateText
    {
        public Text title;
        public Text flavorText;
        public Text cost;
        public Text mortgage;
        public Text downPayment;
        public Text cashFlow;
    }

    [System.Serializable]
    public struct GoldCardText
    {
        public Text title;
        public Text flavorText;
        public Text cost;
    }

    [System.Serializable]
    public struct GambleCardText
    {
        public Text title;
        public Text flavorText;
        public Text instruction;
        public Text badText;
        public Text rewardText;
        public Text cost;
    }

    public Texture smallDealBackground;
    public Texture bigDealBackground;
    public CardFlip cardFlip;
    public StockCardText stockCard;
    public GameObject stockCardHolder;
    public RealEstateText realEstateCard;
    public GameObject realEstateHolder;
    public GoldCardText goldCard;
    public GameObject goldHolder;
    public GambleCardText gambleCard;
    public GameObject gambleHolder;

    private DealCard dealCard;

    void Start()
    {
        cardFlip.AddResetAction(() => dealCard = null);
        cardFlip.SetFlipCondition(() => dealCard != null);
    }

    public void SetDeal(DealCard dealCard)
    {
        this.dealCard = dealCard;
        switch (dealCard.type)
        {
            case DealType.Stock:
                SetStockCard();
                break;
            case DealType.RealEstate:
                SetRealEstateCard();
                break;
            case DealType.Gold:
                SetGoldCard();
                break;
            case DealType.Gamble:
                SetGamble();
                break;
            default:
                this.dealCard = null;
                break;
        }
    }

    private void SetStockCard()
    {
        StockCard card = (StockCard)(this.dealCard);
        cardFlip.cardBack = smallDealBackground;
        cardFlip.cardDataContainer = stockCardHolder;
        stockCard.title.text = (card.stock.isStock ? "Stock" : "Mutual Fund") + " - " + card.stock.title;
        stockCard.flavorText.text = card.flavorText;
        stockCard.symbol.text = "Symbol: " + card.stock.symbol;
        stockCard.todaysPrice.text = "Today's Price: " + Utility.FormatMoney(card.price);
        stockCard.historicTradingRange.text = "Historic Trading Range: " + Utility.FormatMoney(card.stock.priceMin) + " - " + Utility.FormatMoney(card.stock.priceMax);
    }

    private void SetRealEstateCard()
    {
        RealEstateCard card = (RealEstateCard)(this.dealCard);
        cardFlip.cardBack = card.smallDeal ? smallDealBackground : bigDealBackground;
        cardFlip.cardDataContainer = realEstateHolder;
        realEstateCard.title.text = card.title;
        realEstateCard.flavorText.text = card.flavorText;
        realEstateCard.cost.text = "Cost: " + Utility.FormatMoney(card.cost);
        realEstateCard.mortgage.text = "Mortgage: " + Utility.FormatMoney(card.mortgage);
        realEstateCard.downPayment.text = "Down Payment: " + Utility.FormatMoney(card.downPayment);
        realEstateCard.cashFlow.text = "Cash Flow: " + (card.cashFlow > 0 ? "+" : "") + Utility.FormatMoney(card.cashFlow);
    }

    private void SetGoldCard()
    {
        GoldCard card = (GoldCard)(this.dealCard);
        cardFlip.cardBack = smallDealBackground;
        cardFlip.cardDataContainer = goldHolder;
        goldCard.title.text = card.title;
        goldCard.flavorText.text = card.flavorText;
        goldCard.cost.text = "Cost: " + Utility.FormatMoney(card.cost);
    }

    private void SetGamble()
    {
        GambleCard card = (GambleCard)(this.dealCard);
        cardFlip.cardBack = smallDealBackground;
        cardFlip.cardDataContainer = gambleHolder;
        gambleCard.title.text = card.title;
        gambleCard.flavorText.text = card.flavorText;
        gambleCard.instruction.text = "<b>" + card.instruction + "</b>";
        gambleCard.badText.text = "<b>Die = " + card.BadRange + ",</b> " + card.badText;
        gambleCard.rewardText.text = "<b>Die = " + card.RewardRange + ",</b> " + card.rewardText;
        gambleCard.cost.text = "Cost: " + Utility.FormatMoney(card.cost);
    }

}
