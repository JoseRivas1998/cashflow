using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MarketCardFlipState : GameState
{

    private readonly Player player;
    private readonly MarketCard marketCard;
    private readonly MarketCardGameObject marketCardObject;

    private bool skip;
    private float skipTime;
    private readonly float skipTimer = 1f;

    public MarketCardFlipState(MainGameManager mgm)
    {
        marketCard = mgm.PullMarketCard();

        GameObject cardObject = Object.Instantiate(mgm.marketCardPrefab, mgm.mainUICanvas.transform);
        cardObject.transform.SetSiblingIndex(mgm.cashLedgerToggle.transform.GetSiblingIndex());
        marketCardObject = cardObject.GetComponent<MarketCardGameObject>();

        marketCardObject.SetMarket(marketCard);

        mgm.gameStateDisplay.gameObject.SetActive(false);

        skip = false;

        this.player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());

    }

    public override GameState Update(MainGameManager mgm)
    {
        if(skip)
        {
            skipTime += Time.deltaTime;
            if (skipTime > skipTimer)
            {
                Object.Destroy(marketCardObject.gameObject);
                return new PostTurnState(mgm);
            }
            return this;
        }
        if (marketCardObject.cardFlip.FlipReadyBack())
        {
            if (Input.GetMouseButtonUp(0))
            {
                marketCardObject.cardFlip.BeginFlip();
            }
        }
        if (marketCardObject.cardFlip.FlipReadyFront())
        {
            switch (this.marketCard.type)
            {
                case MarketType.RealEstate:
                    bool playerHaveRealEstate = false;
                    RealEstateMarketCard buyer = (RealEstateMarketCard)this.marketCard;
                    for (int i = 0; i < mgm.NumPlayers && !playerHaveRealEstate; i++)
                    {
                        List<RealEstateCard> realEstate = mgm.GetPlayer(i).incomeStatement.RealEstate();
                        if(realEstate.Where(RealEstateMarketCard.GetRealEstatePredicate(buyer)).Any())
                        {
                            playerHaveRealEstate = true;
                        }
                    }
                    if (playerHaveRealEstate)
                    {
                        skip = true;
                        return new SellingRealEstateMarketState(mgm, (RealEstateMarketCard)this.marketCard, this.marketCardObject);
                    }
                    skip = true;
                    mgm.gameStateDisplay.SetText("No one has this kind of property");
                    return this;
                case MarketType.StockSplit:
                    bool playersHaveStock = false;
                    StockSplitCard stockSplit = (StockSplitCard)this.marketCard;
                    for (int i = 0; i < mgm.NumPlayers && !playersHaveStock; i++)
                    {
                        if(mgm.GetPlayer(i).incomeStatement.NumShares(stockSplit.stock.symbol) > 0)
                        {
                            playersHaveStock = true;
                        }
                    }
                    if(playersHaveStock)
                    {
                        return new RollingStockSplitState(mgm, (StockSplitCard)this.marketCard, marketCardObject);
                    }
                    skip = true;
                    mgm.gameStateDisplay.SetText("No one has this stock");
                    return this;
                case MarketType.Damage:
                    if(this.player.incomeStatement.RealEstate().Count == 0)
                    {
                        skip = true;
                        return this;
                    }
                    return new PayForDamageState(mgm, (DamageCard)this.marketCard, this.marketCardObject);
                case MarketType.Gold:
                    bool hasGold = false;
                    for (int i = 0; i < mgm.NumPlayers && !hasGold; i++)
                    {
                        if(mgm.GetPlayer(i).incomeStatement.goldCoins > 0)
                        {
                            hasGold = true;
                        }
                    }
                    if (hasGold)
                    {
                        return new SellingGoldState(mgm, (GoldBuyerCard)marketCard, marketCardObject);
                    }
                    skip = true;
                    return this;
                case MarketType.Bonus:
                    BonusCard bonusCard = (BonusCard)this.marketCard;
                    if (bonusCard.everyone)
                    {
                        for (int i = 0; i < mgm.NumPlayers; i++)
                        {
                            mgm.GetPlayer(i).incomeStatement.BoostBusinesses(bonusCard.cashFlowMax, bonusCard.cashFlowIncrease);
                        }
                    }
                    else
                    {
                        this.player.incomeStatement.BoostBusinesses(bonusCard.cashFlowMax, bonusCard.cashFlowIncrease);
                    }
                    skip = true;
                    return this;
                default:
                    break;
            }
        }
        return this;
    }
}
