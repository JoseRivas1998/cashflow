using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataTester : MonoBehaviour
{

    public ProfessionCard professionCard;

    private Professions professions;
    int professionIndex;

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
        List<DoodadCard> doodads = DoodadCard.LoadDoodadCards();
        foreach (DoodadCard card in doodads)
        {
            Debug.Log(card);
        }
        Debug.Log("================================");
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
            if(professionCard.FlipReadyBack())
            {
                professionCard.SetProfession(professions.professions[professionIndex]);
                professionIndex = (professionIndex + 1) % professions.professions.Length;
                professionCard.BeginFlip();
            }
            else if(professionCard.FlipReadyFront())
            {
                professionCard.BeginFlipBack();
            }
        }
        if(professionCard.FlipDone())
        {
            professionCard.ResetFlip();
        }
    }
}
