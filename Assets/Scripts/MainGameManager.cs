using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{

    public PlayerCount playerCount;

    public int numPlayers;

    private GameState currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = new PlayerCountSelectState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Update();
    }
}
