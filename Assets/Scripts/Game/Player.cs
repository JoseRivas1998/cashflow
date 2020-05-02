using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player
{
    public readonly int index;
    public readonly string name;
    public readonly string dream;
    public readonly Color color;
    public readonly Professions.Profession profession;
    public readonly Ledger ledger;
    public readonly IncomeStatement incomeStatement;

    public int space;

    private PlayerTab tab;
    public GamePiece gamePiece { get; private set; }

    public bool downsized { get; private set; }
    public int downsizedTurns;
    public int charityTurnsLeft { get; private set; }

    public bool FastTrack { get; private set; }

    public FastTrackIncomeStatement fastTrackIncomeStatement { get; private set; }

    public Player(int index, string name, string dream, Color color, Professions.Profession profession)
    {
        this.index = index;
        this.name = name;
        this.dream = dream;
        this.color = color;
        this.profession = profession;
        this.ledger = new Ledger(profession);
        this.incomeStatement = new IncomeStatement(profession);
        this.gamePiece = null;
        this.space = 0;
        this.downsized = false;
        this.downsizedTurns = 0;
        this.charityTurnsLeft = 0;
        this.FastTrack = false;
        this.fastTrackIncomeStatement = null;
    }

    public override string ToString()
    {
        return this.name + " is a " + this.profession.name + " with a dream: " + this.dream;
    }

    public void SetTab(PlayerTab tab)
    {
        this.tab = tab;
    }

    public bool GamePieceExists()
    {
        return gamePiece != null;
    }

    public void SetGamePiece(GamePiece gamePiece)
    {
        this.gamePiece = gamePiece;
        this.gamePiece.SetColor(this.color);
    }

    public void AddMoney(int amount)
    {
        ledger.AddMoney(amount);
        tab.UpdatePlayerBalance(this);
    }

    public void SubtractMoney(int amount)
    {
        ledger.SubtractMoney(amount);
        tab.UpdatePlayerBalance(this);
    }

    public void Downsize(int cost)
    {
        this.SubtractMoney(cost);
        this.downsized = true;
        this.downsizedTurns = 0;
        this.charityTurnsLeft = 0;
    }

    public void RemoveDownsize()
    {
        this.downsized = false;
        this.downsizedTurns = 0;
    }

    public void AddCharity()
    {
        this.charityTurnsLeft += 3;
    }

    public void SubtractFromCharity()
    {
        this.charityTurnsLeft = Mathf.Max(0, charityTurnsLeft - 1);
    }

    public void TakeOutLoan(int amount)
    {
        this.AddMoney(amount);
        this.incomeStatement.bankLoan += amount;
    }

    public void BuyStock(StockCard stockCard, int numShares)
    {
        this.BuyStock(stockCard.stock.symbol, stockCard.price, numShares);
    }

    private void BuyStock(string symbol, int price, int numShares)
    {
        int cost = price * numShares;
        SubtractMoney(cost);
        incomeStatement.AddStock(symbol, price, numShares);
    }

    public void SellStock(StockCard stockCard, int numShares)
    {
        int revenue = stockCard.price * numShares;
        AddMoney(revenue);
        incomeStatement.SellStock(stockCard.stock.symbol, numShares);
    }

    public void BuyRealEstate(RealEstateCard realEstate)
    {
        this.SubtractMoney(realEstate.downPayment);
        this.incomeStatement.AddRealEstate(realEstate);
    }

    public void InvestMLM(GambleCard gambleCard)
    {
        this.SubtractMoney(gambleCard.cost);
        this.incomeStatement.AddMLM(gambleCard);
    }

    public void SellGold(int price, int numGold) 
    {
        int goldSell = Mathf.Min(numGold, incomeStatement.goldCoins);
        incomeStatement.SubtractGold(goldSell);
        AddMoney(goldSell * price);
    }

    public void SplitStock(string symbol) 
    {
        int currentShares = this.incomeStatement.NumShares(symbol);
        this.incomeStatement.AddStock(symbol, 0, currentShares);
    }

    public void ReverseSplitStock(string symbol)
    {
        int currentShares = this.incomeStatement.NumShares(symbol);
        this.incomeStatement.SellStock(symbol, currentShares / 2);
    }

    public int CapitalGains(RealEstateMarketCard offer, List<RealEstateCard> realEstates)
    {
        int revenue = realEstates.Sum(card => RealEstateMarketCard.OfferAmount(offer, card));
        int cost = realEstates.Sum(card => card.mortgage);
        return revenue - cost;
    }

    public void SellRealEstates(RealEstateMarketCard offer, List<RealEstateCard> realEstates)
    {
        int capitalGains = CapitalGains(offer, realEstates);
        this.AddMoney(capitalGains);
        foreach (RealEstateCard card in realEstates)
        {
            this.incomeStatement.RemoveRealEstate(card);
        }
    }

    public void SellRealEstateToBank(List<RealEstateCard> realEstates)
    {
        int capitalGains = realEstates.Sum(realEstate => realEstate.downPayment / 2);
        this.AddMoney(capitalGains);
        foreach (RealEstateCard card in realEstates)
        {
            this.incomeStatement.RemoveRealEstate(card);
        }
    }

    public void DropOut()
    {
        this.tab.DropOut();
        Object.Destroy(this.gamePiece.gameObject);
    }

    public void EnterFastTrack(int fastTrackSpace)
    {
        this.gamePiece.onFastTrack = true;
        this.gamePiece.SetFastTrackSpace(fastTrackSpace);
        this.AddMoney(Utility.RoundToNearestPowerOfTen(this.incomeStatement.PassiveIncome * 100, 3));
        this.fastTrackIncomeStatement = new FastTrackIncomeStatement(this.incomeStatement);
        this.charityTurnsLeft = 0;
        this.space = fastTrackSpace;
        this.FastTrack = true;
    }

}
