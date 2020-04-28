using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameDataTester : MonoBehaviour
{

    public ProfessionCard card;

    // Start is called before the first frame update
    void Start()
    {
        card.SetProfession(GameData.Instance.GetProfessions().professions[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (card.cardFlip.FlipReadyBack())
            {
                card.cardFlip.BeginFlip();
            }
            else if (card.cardFlip.FlipReadyFront())
            {
                card.cardFlip.BeginFlipBack();
            }
        }
        if (card.cardFlip.FlipDone())
        {
            card.cardFlip.ResetFlip();
            card.SetProfession(GameData.Instance.GetProfessions().professions[0]);
        }
    }
}
