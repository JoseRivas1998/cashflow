using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager 
{

    private Stack<int> turnStack;

    private readonly int numPlayers;
    private int currentPlayer;
    private int startingPlayer;
    private int highestRoll;

    public TurnManager(int numPlayers)
    {
        turnStack = new Stack<int>();
        this.numPlayers = numPlayers;
        currentPlayer = -1;
        startingPlayer = 1;
        highestRoll = -1;
    }

    public void RegisterRoll(int playerNumber, int rollValue)
    {
        if(rollValue > highestRoll)
        {
            startingPlayer = playerNumber;
            highestRoll = rollValue;
        }
        else if(rollValue == highestRoll)
        {
            startingPlayer = Random.Range(0f, 1f) < 0.5 ? playerNumber : startingPlayer;
        }
    }

    public void Push()
    {
        turnStack.Push(currentPlayer);
    }

    public int Pop()
    {
        if(turnStack.Count > 0)
        {
            currentPlayer = turnStack.Pop();
        }
        return currentPlayer;
    }

    public int NextPlayer()
    {
        if(currentPlayer == -1)
        {
            currentPlayer = startingPlayer;
        } 
        else
        {
            currentPlayer = (currentPlayer + 1) % numPlayers;
        }
        return currentPlayer;
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public string TurnOrder()
    {
        var s = "{";
        for (int i = 0; i < numPlayers; i++)
        {
            int player = (startingPlayer + i) % numPlayers;
            s += player;
            if(i < numPlayers - 1)
            {
                s += ", ";
            }
        }
        return s + "}";
    }

}
