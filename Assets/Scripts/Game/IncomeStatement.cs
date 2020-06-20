using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditorInternal;

public class IncomeStatement
{
    // INCOME
    public int salary { get; private set; }
    // EXPENSES
    public int taxes { get; private set; }
    public int mortgagePayment { get; private set; }
    public int schoolPayment { get; private set; }
    public int carPayment { get; private set; }
    public int creditPayment { get; private set; }
    public int otherExpenses { get; private set; }
    public int BankLoanPayment { get { return bankLoan / 10; } }
    public int perChildExpenses { get; private set; }
    public int numChildren { get; private set; }
    public int ChildPayment { get { return perChildExpenses * numChildren; } }
    // ASSETS
    public int savings { get; private set; }
    public int goldCoins { get; private set; }
    private readonly List<StockEntry> stockEntries;
    private readonly List<RealEstateCard> realEstate;
    private readonly List<GambleCard> mlms;
    // LIABILITIES
    public int mortgage { get; private set; }
    public int schoolLoans { get; private set; }
    public int carLoans { get; private set; }
    public int creditCardDebt { get; private set; }
    public int bankLoan { get; set; }

    public int PassiveIncome { get { return this.realEstate.Sum(card => card.cashFlow); } }

    public int TotalIncome { get { return salary + PassiveIncome; } }

    public int TotalExpenses { get { return taxes + mortgagePayment + schoolPayment + carPayment + creditPayment + otherExpenses + BankLoanPayment + ChildPayment; } }

    public int MonthlyCashflow { get { return TotalIncome - TotalExpenses; } }

    public struct StockEntry
    {
        public readonly string Symbol;
        public readonly int Price;
        public readonly int NumberOfShares;
        public StockEntry(string symbol, int price, int numShares)
        {
            this.Symbol = symbol;
            this.Price = price;
            this.NumberOfShares = numShares;
        }

        public bool MatchSymbol(string symbol)
        {
            return this.Symbol.Equals(symbol);
        }

        public bool MatchPrice(int price)
        {
            return this.Price == price;
        }

        public bool MatchesStock(string symbol, int price)
        {
            return MatchSymbol(symbol) && MatchPrice(price);
        }

    }

    public IncomeStatement(Professions.Profession profession)
    {
        this.salary = profession.salary;
        this.taxes = profession.taxes;
        this.mortgagePayment = profession.mortgagePayment;
        this.schoolPayment = profession.schoolPayment;
        this.carPayment = profession.carPayment;
        this.creditPayment = profession.creditPayment;
        this.otherExpenses = profession.otherExpenses;
        this.perChildExpenses = profession.perChildExpenses;
        this.numChildren = 0;
        this.savings = profession.savings;
        this.mortgage = profession.mortgage;
        this.schoolLoans = profession.schoolLoans;
        this.carLoans = profession.carLoans;
        this.creditCardDebt = profession.creditCardDebt;
        this.bankLoan = 0;
        this.goldCoins = 0;
        this.stockEntries = new List<StockEntry>();
        this.realEstate = new List<RealEstateCard>();
        this.mlms = new List<GambleCard>();
    }

    public bool AddChild()
    {
        if (this.numChildren < 3)
        {
            numChildren++;
            return true;
        }
        return false;
    }

    public void AddStock(string symbol, int price, int numShares)
    {
        new StockAdder(symbol, price, numShares, this).AddStock();
    }

    private class StockAdder
    {
        readonly string symbol;
        readonly int price;
        readonly int numShares;
        readonly IncomeStatement statement;
        public StockAdder(string symbol, int price, int numShares, IncomeStatement statement)
        {
            this.symbol = symbol;
            this.price = price;
            this.numShares = numShares;
            this.statement = statement;
        }

        public void AddStock()
        {
            int foundIndex = GetStockIndex();
            int currentShares = GetCurrentShares(foundIndex);
            RemoveCurrentEntryIfFound(foundIndex);
            AddStockEntry(currentShares);
        }

        int GetStockIndex()
        {
            int foundIndex = -1;
            for (int i = 0; i < statement.stockEntries.Count && foundIndex == -1; i++)
            {
                if (statement.stockEntries[i].MatchesStock(symbol, price))
                {
                    foundIndex = i;
                }
            }
            return foundIndex;
        }

        int GetCurrentShares(int stockIndex)
        {
            return stockIndex >= 0 ? statement.stockEntries[stockIndex].NumberOfShares : 0;
        }

        void RemoveCurrentEntryIfFound(int stockIndex)
        {
            if (stockIndex != -1)
            {
                statement.stockEntries.RemoveAt(stockIndex);
            }
        }

        void AddStockEntry(int currentShares)
        {
            statement.stockEntries.Add(new StockEntry(symbol, price, currentShares + numShares));
        }

    }

    public void SellStock(string symbol, int numShares)
    {
        new StockSeller(symbol, numShares, this).SellShares();
    }

    private class StockSeller
    {
        readonly string symbol;
        readonly IncomeStatement statement;
        int remainingShares;
        public StockSeller(string symbol, int numShares, IncomeStatement statement)
        {
            this.symbol = symbol;
            this.statement = statement;
            this.remainingShares = numShares;
        }

        public void SellShares()
        {
            RemoveAllEntriesThatFitWithinRemainingShares();
            RemoveLeftOverShares();
        }

        bool SharesLeftToSell()
        {
            return this.remainingShares > 0;
        }

        void RemoveAllEntriesThatFitWithinRemainingShares()
        {
            if (!SharesLeftToSell()) return;
            int stockEntryIndex;
            do
            {
                stockEntryIndex = FindEntryIndexWithLessOrEqualSharesThanRemainingShares();
                RemoveEntryFromRemainingShares(stockEntryIndex);
            } while (SharesLeftToSell() && stockEntryIndex != -1);
        }

        void RemoveEntryFromRemainingShares(int entryIndex)
        {
            if (entryIndex == -1) return;
            StockEntry entry = statement.stockEntries[entryIndex];
            remainingShares -= entry.NumberOfShares;
            statement.stockEntries.RemoveAt(entryIndex);
        }

        int FindEntryIndexWithLessOrEqualSharesThanRemainingShares()
        {
            int foundIndex = -1;
            for (int i = 0; i < statement.stockEntries.Count && foundIndex == -1; i++)
            {
                StockEntry entry = statement.stockEntries[i];
                if (entry.Symbol.Equals(symbol) && entry.NumberOfShares <= remainingShares)
                {
                    foundIndex = i;
                }
            }
            return foundIndex;
        }

        void RemoveLeftOverShares()
        {
            int foundIndex = FindEntryIndexWithMoreSharesThanRemainingShares();
            RemoveEntryWithMoreSharesThanRemaining(foundIndex);
        }

        int FindEntryIndexWithMoreSharesThanRemainingShares()
        {
            int foundIndex = -1;
            for (int i = 0; i < statement.stockEntries.Count && foundIndex == -1; i++)
            {
                StockEntry entry = statement.stockEntries[i];
                if (entry.Symbol.Equals(symbol) && entry.NumberOfShares > remainingShares)
                {
                    foundIndex = i;
                }
            }
            return foundIndex;
        }

        void RemoveEntryWithMoreSharesThanRemaining(int entryIndex)
        {
            if (entryIndex == -1) return;
            StockEntry entry = statement.stockEntries[entryIndex];
            int sharesAfterRemoving = entry.NumberOfShares - remainingShares;
            RemoveEntryFromRemainingShares(entryIndex);
            statement.AddStock(entry.Symbol, entry.Price, sharesAfterRemoving);
        }

    }

    public int NumShares(string symbol)
    {
        return stockEntries.Where(entry => entry.Symbol.Equals(symbol)).Sum(entry => entry.NumberOfShares);
    }

    public List<StockEntry> StockEntries()
    {
        List<StockEntry> entries = new List<StockEntry>();
        entries.AddRange(this.stockEntries);
        return entries;
    }

    public void AddRealEstate(RealEstateCard card)
    {
        this.realEstate.Add(card);
    }

    public void RemoveRealEstate(RealEstateCard card)
    {
        int index = -1;
        for (int i = 0; i < this.realEstate.Count && index == -1; i++)
        {
            if (this.realEstate[i].cardId == card.cardId)
            {
                index = i;
            }
        }
        if (index != -1)
        {
            this.realEstate.RemoveAt(index);
        }

    }

    public List<RealEstateCard> RealEstate()
    {
        List<RealEstateCard> cards = new List<RealEstateCard>();
        cards.AddRange(this.realEstate);
        return cards;
    }

    public void AddGold(int coins)
    {
        this.goldCoins += coins;
    }

    public void SubtractGold(int coins)
    {
        this.goldCoins -= coins;
    }

    public int MLMCount { get { return this.mlms.Count; } }
    public bool HasMLM { get { return this.MLMCount > 0; } }

    public void AddMLM(GambleCard gambleCard)
    {
        this.mlms.Add(gambleCard);
    }

    public GambleCard GetMLM(int index) { return this.mlms[index]; }

    public void BoostBusinesses(int maxCashFlow, int boostAmount)
    {
        List<RealEstateCard> toAdd = new List<RealEstateCard>();
        for (int i = this.realEstate.Count - 1; i >= 0; i--)
        {
            if (this.realEstate[i].propertyType == RealEstateType.Business && this.realEstate[i].cashFlow <= maxCashFlow)
            {
                toAdd.Add(new RealEstateCard(
                    this.realEstate[i].smallDeal,
                    this.realEstate[i].propertyType,
                    this.realEstate[i].title,
                    this.realEstate[i].flavorText,
                    this.realEstate[i].cost,
                    this.realEstate[i].mortgage,
                    this.realEstate[i].downPayment,
                    this.realEstate[i].cashFlow + boostAmount));
                this.realEstate.RemoveAt(i);
            }
        }
        this.realEstate.AddRange(toAdd);
    }

    public void RemoveMortgate()
    {
        this.mortgage = 0;
        this.mortgagePayment = 0;
    }

    public void RemoveSchoolLoans()
    {
        this.schoolLoans = 0;
        this.schoolPayment = 0;
    }

    public void RemoveCarLoans()
    {
        this.carLoans = 0;
        this.carPayment = 0;
    }

    public void RemoveCreditCardDebt()
    {
        this.creditCardDebt = 0;
        this.creditPayment = 0;
    }

    public void SubtractBankLoan(int amount)
    {
        this.bankLoan = Mathf.Max(this.bankLoan - amount, 0);
    }

    public void Clear()
    {
        this.bankLoan = 0;
        this.stockEntries.Clear();
        this.realEstate.Clear();
        this.mlms.Clear();
        this.goldCoins = 0;
    }

}
