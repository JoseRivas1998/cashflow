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

    public int space;

    private PlayerTab tab;
    public GamePiece gamePiece { get; private set; }

    public bool downsized { get; private set; }
    public int downsizedTurns;
    public int charityTurnsLeft { get; private set; }

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

    public void Downsize()
    {
        this.SubtractMoney(incomeStatement.TotalExpenses);
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

}
