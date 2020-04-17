using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{

    public PlayerCount playerCount;
    public PlayerNameDreamColor playerNameDreamColor;


    private GameState currentState;
    private Player[] players;
    private Stack<Professions.Profession> professions;
    public int NumPlayers { get { return players != null ? players.Length : 0; } }

    // Start is called before the first frame update
    void Start()
    {
        LoadProfessions();
        currentState = new PlayerCountSelectState(this);
    }

    private void LoadProfessions()
    {
        List<Professions.Profession> professionList = new List<Professions.Profession>();
        professionList.AddRange(GameData.Instance.GetProfessions().professions);
        Utility.ShuffleList(professionList);
        professions = new Stack<Professions.Profession>();
        foreach (Professions.Profession profession in professionList)
        {
            professions.Push(profession);
        }
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
        if(professions.Count == 0)
        {
            throw new System.Exception("No more professions!");
        }
        players[index] = new Player(index, name, dream, color, professions.Pop());
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Update(this);
    }
}
