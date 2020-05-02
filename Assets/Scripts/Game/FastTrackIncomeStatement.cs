using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FastTrackIncomeStatement
{

    public struct Entry : System.IComparable<Entry>
    {
        private static int NumEntries = 0;
        public readonly int ID;
        public readonly string Business;
        public readonly int MonthlyCashFlow;

        public Entry(string business, int monthlyCashFlow)
        {
            this.ID = ++Entry.NumEntries;
            this.Business = business;
            this.MonthlyCashFlow = monthlyCashFlow;
        }

        public int CompareTo(Entry other)
        {
            return MonthlyCashFlow.CompareTo(other.MonthlyCashFlow);
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }
            Entry other = (Entry)obj;
            return other.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return 1213502048 + ID.GetHashCode();
        }
    }

    public readonly int StartingCashFlowDayIncome;
    public readonly int CashFlowDayGoal;
    public readonly int StartingPassiveIncome;
    private List<Entry> entries;

    public int NumEntries { get { return entries.Count; } }

    public int CashFlowDayIncome { get { return StartingCashFlowDayIncome + entries.Sum(entry => entry.MonthlyCashFlow); } }

    public FastTrackIncomeStatement(IncomeStatement incomeStatement)
    {
        this.StartingPassiveIncome = incomeStatement.PassiveIncome;
        this.entries = new List<Entry>();
        this.StartingCashFlowDayIncome = Utility.RoundToNearestPowerOfTen(incomeStatement.PassiveIncome * 100, 3);
        this.CashFlowDayGoal = StartingCashFlowDayIncome + 50000;
    }

    public void AddEntry(string business, int monthlyCashFlow)
    {
        entries.Add(new Entry(business, monthlyCashFlow));
    }

    public Entry GetLowestCashFlowingAsset()
    {
        return entries.Min();
    }

    public void RemoveLowestAsset()
    {
        entries.Remove(GetLowestCashFlowingAsset());
    }

    public List<Entry> GetEntries()
    {
        return (from entry in entries select entry).ToList();
    }
}
