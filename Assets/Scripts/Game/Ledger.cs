using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledger
{

    private readonly struct Entry
    {
        public Entry(bool add, int amount)
        {
            Add = add;
            Amount = amount;
        }

        public bool Add { get; }
        public int Amount { get; }

        public override string ToString() => (Add ? "+" : "-") + Amount;
    }

    public readonly int startingBalance;
    private List<Entry> entries;

    public Ledger(Professions.Profession profession)
    {
        startingBalance = profession.CashFlow + profession.savings;
        entries = new List<Entry>();
    }

    public int GetCurretBalance()
    {
        int balance = startingBalance;
        foreach (Entry entry in entries)
        {
            if (entry.Add)
            {
                balance += entry.Amount;
            }
            else
            {
                balance -= entry.Amount;
            }
        }
        return balance;
    }

    public void AddMoney(int amount)
    {
        AddEntry(true, amount);
    }

    public void SubtractMoney(int amount)
    {
        AddEntry(false, amount);
    }

    private void AddEntry(bool add, int amount)
    {
        entries.Add(new Entry(add, amount));
    }

}
