using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketCardGameObject : MonoBehaviour
{

    [System.Serializable]
    public struct RealEstateMarketDataContainer
    {
        public Text title;
        public Text flavorText;
    }

    [System.Serializable]
    public struct BonusMarketDataContainer
    {
        public Text title;
        public Text flavorText;
        public Text instruction;
    }

    [System.Serializable]
    public struct DamageMarketDataContainer
    {
        public Text title;
        public Text flavorText;
        public Text instruction;
    }

    [System.Serializable]
    public struct StockSplitMarketDataContainer
    {
        public Text title;
        public Text flavorText;
        public Text splitText;
        public Text reverseSplitText;
        public Text symbol;
    }

    [System.Serializable]
    public struct GoldBuyerMarketDataContainer
    {
        public Text title;
        public Text flavorText;
    }

    public RealEstateMarketDataContainer realEstateMarketData;
    public GameObject realEstateMarketContainer;

    public BonusMarketDataContainer bonusMarketData;
    public GameObject bonusMarketContainer;

    public DamageMarketDataContainer damageMarketData;
    public GameObject damageMarketContainer;

    public StockSplitMarketDataContainer stockSplitMarketData;
    public GameObject stockSplitMarketContainer;

    public GoldBuyerMarketDataContainer goldBuyerMarketData;
    public GameObject goldBuyerMarketContainer;

    public CardFlip cardFlip;

    private MarketCard marketCard;

    // Start is called before the first frame update
    void Start()
    {
        cardFlip.AddResetAction(() => marketCard = null);
        cardFlip.SetFlipCondition(() => marketCard != null);
    }

    public void SetMarket(MarketCard marketCard)
    {
        this.marketCard = marketCard;
        switch (marketCard.type)
        {
            case MarketType.RealEstate:
                SetRealEstate();
                break;
            case MarketType.StockSplit:
                SetStockSplit();
                break;
            case MarketType.Damage:
                SetDamage();
                break;
            case MarketType.Gold:
                SetGold();
                break;
            case MarketType.Bonus:
                SetBonus();
                break;
            default:
                this.marketCard = null;
                break;
        }
    }

    private void SetRealEstate()
    {
        this.cardFlip.cardDataContainer = this.realEstateMarketContainer;       
        this.realEstateMarketData.title.text = this.marketCard.title;
        this.realEstateMarketData.flavorText.text = this.marketCard.flavorText;
    }

    private void SetStockSplit()
    {
        StockSplitCard stockSplit = (StockSplitCard)(this.marketCard);
        this.cardFlip.cardDataContainer = this.stockSplitMarketContainer;
        this.stockSplitMarketData.title.text = stockSplit.title;
        this.stockSplitMarketData.flavorText.text = stockSplit.flavorText;
        this.stockSplitMarketData.splitText.text = $"Die = 1-3, STOCK SPLITS! <b>Everyone</b> who owns {stockSplit.stock.symbol} DOUBLES their number of shares.";
        this.stockSplitMarketData.reverseSplitText.text = $"Die = 4-6, STOCK REVERSE SPLITS! <b>Everyone</b> who owns {stockSplit.stock.symbol} loses half their shares.";
        this.stockSplitMarketData.symbol.text = $"<b>Symbol: {stockSplit.stock.symbol}</b>";
    }

    private void SetDamage()
    {
        DamageCard damageCard = (DamageCard)this.marketCard;
        this.cardFlip.cardDataContainer = this.damageMarketContainer;
        this.damageMarketData.title.text = damageCard.title;
        this.damageMarketData.flavorText.text = damageCard.flavorText;
        this.damageMarketData.instruction.text = damageCard.instructions;
    }

    private void SetGold()
    {
        this.cardFlip.cardDataContainer = this.goldBuyerMarketContainer;
        this.goldBuyerMarketData.title.text = this.marketCard.title;
        this.goldBuyerMarketData.flavorText.text = this.marketCard.flavorText;
    }

    private void SetBonus()
    {
        BonusCard bonusCard = (BonusCard)this.marketCard;
        this.cardFlip.cardDataContainer = this.bonusMarketContainer;
        this.bonusMarketData.title.text = bonusCard.title;
        this.bonusMarketData.flavorText.text = bonusCard.flavorText;
        this.bonusMarketData.instruction.text = $"All businesses with a cash flow of " +
            $"{Utility.FormatMoney(bonusCard.cashFlowMax)} or less, have their cashflow increased by {Utility.FormatMoney(bonusCard.cashFlowIncrease)}.";
    }

}
