using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{

    public PlayerCount playerCount;
    public PlayerNameDreamColor playerNameDreamColor;


    private GameState currentState;
    private Player[] players;
    public int NumPlayers { get { return players != null ? players.Length : 0; } }

    // Start is called before the first frame update
    void Start()
    {
        currentState = new PlayerCountSelectState(this);
    }

    public void SetNumPlayers(int numPlayers)
    {
        if(players == null)
        {
            players = new Player[numPlayers];
        } 
        else
        {
            throw new System.Exception("The player count was already set!");
        }
    }

    public void RegisterPlayer(int index, string name, string dream, Color color)
    {
        players[index] = new Player(index, name, dream, color);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Update(this);
    }
}
