using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnManager 
{

    private Stack<int> turnStack;
    private bool[] outPlayers;

    private readonly int numPlayers;
    private int currentPlayer;
    private int startingPlayer;
    private int highestRoll;

    public TurnManager(int numPlayers)
    {
        turnStack = new Stack<int>();
        outPlayers = new bool[numPlayers];
        for (int i = 0; i < outPlayers.Length; i++)
        {
            outPlayers[i] = false;
        }
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
        do
        {
            if (currentPlayer == -1)
            {
                currentPlayer = startingPlayer;
            }
            else
            {
                currentPlayer = (currentPlayer + 1) % numPlayers;
            }
        } while (outPlayers[currentPlayer]);
        return currentPlayer;
    }

    public void DropOutPlayer(int index)
    {
        outPlayers[index] = true;
    }

    public bool PlayerDroppedOut(int index)
    {
        return outPlayers[index];
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public int[] TurnOrder()
    {
        int[] order = new int[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            int player = (startingPlayer + i) % numPlayers;
            order[i] = player;
        }
        return order;
    }

    public int NumPlayersIn()
    {
        return outPlayers.Sum(outPlayer => outPlayer ? 0 : 1);
    }

}
