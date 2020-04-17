using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfessionReveal : MonoBehaviour
{

    public Text title;
    public ProfessionCard card;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(Player player)
    {
        title.text = player.name + "'s Profession";
        card.SetProfession(player.profession);
    }

}
