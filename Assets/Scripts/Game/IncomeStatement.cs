using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IncomeStatement
{
    // INCOME
    public int salary {get; private set;}
    // EXPENSES
    public int taxes {get; private set;}
    public int mortgagePayment {get; private set;}
    public int schoolPayment {get; private set;}
    public int carPayment {get; private set;}
    public int creditPayment {get; private set;}
    public int otherExpenses {get; private set;}
    public int BankLoanPayment { get { return bankLoan / 10; } }
    public int perChildExpenses { get; private set; }
    public int numChildren { get; private set; }
    public int ChildPayment { get { return perChildExpenses * numChildren; } }
    // ASSETS
    public int savings {get; private set; }
    private readonly List<StockEntry> stockEntries;
    private readonly List<RealEstateCard> realEstate;
    // LIABILITIES
    public int mortgage {get; private set;}
    public int schoolLoans {get; private set;}
    public int carLoans {get; private set;}
    public int creditCardDebt {get; private set;}
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
        this.stockEntries = new List<StockEntry>();
        this.realEstate = new List<RealEstateCard>();
    }

    public bool AddChild()
    {
        if(this.numChildren < 3)
        {
            numChildren++;
            return true;
        }
        return false;
    }

    public void AddStock(string symbol, int price, int numShares)
    {
        int foundIndex = -1;
        int currentShares = 0;
        for (int i = 0; i < this.stockEntries.Count && foundIndex == -1; i++)
        {
            if(this.stockEntries[i].Symbol.Equals(symbol) && this.stockEntries[i].Price == price)
            {
                foundIndex = i;
                currentShares = this.stockEntries[i].NumberOfShares;
            }
        }
        if(foundIndex != -1)
        {
            this.stockEntries.RemoveAt(foundIndex);
        }
        this.stockEntries.Add(new StockEntry(symbol, price, currentShares + numShares));
    }

    public void SellStock(string symbol, int numShares)
    {
        int remainingShares = numShares;
        int foundIndex;
        while(remainingShares > 0)
        {
            /*
             * step one: find a stock entry that is <= remaining shares
             */
            foundIndex = -1;
            for (int i = 0; i < this.stockEntries.Count && foundIndex == -1; i++)
            {
                StockEntry entry = stockEntries[i];
                if(entry.Symbol.Equals(symbol) && entry.NumberOfShares <= remainingShares)
                {
                    foundIndex = i;
                }
            }
            if (foundIndex != -1)
            {
                /* 
                 * step two: if such a stock was found, subtract shares in that entry
                 *           from remaining, remove entry from list and continue
                 */
                StockEntry entry = stockEntries[foundIndex];
                remainingShares -= entry.NumberOfShares;
                stockEntries.RemoveAt(foundIndex);
            }
            else
            {
                /*
                 * step three: if no such stock was found, find a stock where
                 *             the entry has more shares than the remaining
                 */
                foundIndex = -1;
                for (int i = 0; i < this.stockEntries.Count && foundIndex == -1; i++)
                {
                    StockEntry entry = stockEntries[i];
                    if (entry.Symbol.Equals(symbol) && entry.NumberOfShares > remainingShares)
                    {
                        foundIndex = i;
                    }
                }
                if (foundIndex == -1)
                {
                    return; // this should NEVER happen
                }
                else
                {
                    /*
                     * step four: calculate difference between
                     *             number of shares and remaining shares, set remaining
                     *             shares to zero, remove found stock, add new entry
                     *             with the same symbol and price, but the number of entries
                     *             is the difference
                     */
                    StockEntry entry = stockEntries[foundIndex];
                    int diff = entry.NumberOfShares - remainingShares;
                    remainingShares = 0;
                    stockEntries.RemoveAt(foundIndex);
                    this.AddStock(entry.Symbol, entry.Price, diff);
                }

            }
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

    public List<RealEstateCard> RealEstate()
    {
        List<RealEstateCard> cards = new List<RealEstateCard>();
        cards.AddRange(this.realEstate);
        return cards;
    }

}
