using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public readonly int index;
    public readonly string name;
    public readonly string dream;
    public readonly Color color;
    public readonly Professions.Profession profession;
    public readonly Ledger ledger;
    public readonly IncomeStatement incomeStatement;

    private PlayerTab tab;

    public Player(int index, string name, string dream, Color color, Professions.Profession profession)
    {
        this.index = index;
        this.name = name;
        this.dream = dream;
        this.color = color;
        this.profession = profession;
        this.ledger = new Ledger(profession);
        this.incomeStatement = new IncomeStatement(profession);
    }

    public override string ToString()
    {
        return this.name + " is a " + this.profession.name + " with a dream: " + this.dream;
    }

    public void SetTab(PlayerTab tab)
    {
        this.tab = tab;
    }

}
