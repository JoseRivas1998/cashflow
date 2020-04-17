using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Professions
{

    [System.Serializable]
    public class Profession
    {
        public string name;
        public int salary;
        public int taxes;
        public int mortgagePayment;
        public int schoolPayment;
        public int carPayment;
        public int creditPayment;
        public int otherExpenses;
        public int savings;
        public int mortgage;
        public int schoolLoans;
        public int carLoans;
        public int creditCardDebt;
        public int row;
        public int col;
        public int Expenses { get { return taxes + mortgagePayment + schoolPayment + carPayment + creditPayment + otherExpenses; } }
        public int CashFlow { get { return salary - Expenses; } }

        public override string ToString()
        {
            return "Profession: " + this.name;
        }

    }

    public Profession[] professions;

    public static Professions CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Professions>(jsonString);
    }

}
