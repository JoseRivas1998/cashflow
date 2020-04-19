using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataTester : MonoBehaviour
{

    public DoodadCardGameObject doodadCard;

    private Professions professions;
    int professionIndex;

    private List<DoodadCard> doodads;
    private int doodadIndex;

    // Start is called before the first frame update
    void Start()
    {
        professions = GameData.Instance.GetProfessions();
        List<DealCard> smallDeals = DealCard.SmallDeals();
        foreach (DealCard card in smallDeals)
        {
            Debug.Log(card);
        }
        Debug.Log("================================");
        List<DealCard> bigDeals = DealCard.BigDeals();
        foreach (DealCard card in bigDeals)
        {
            Debug.Log(card);
        }
        Debug.Log("================================");
        doodads = DoodadCard.LoadDoodadCards();
        doodadIndex = 0;
        List<MarketCard> marketCards = MarketCard.LoadMarketCards();
        foreach (MarketCard card in marketCards)
        {
            Debug.Log(card);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(doodadCard.cardFlip.FlipReadyBack())
            {
                doodadCard.SetDoodad(doodads[doodadIndex]);
                doodadIndex = (doodadIndex + 1) % doodads.Count;
                doodadCard.cardFlip.BeginFlip();
            }
            else if(doodadCard.cardFlip.FlipReadyFront())
            {
                doodadCard.cardFlip.BeginFlipBack();
            }
        }
        if(doodadCard.cardFlip.FlipDone())
        {
            doodadCard.cardFlip.ResetFlip();
        }
    }
}
