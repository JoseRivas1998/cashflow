using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameDataTester : MonoBehaviour
{
    public GameObject marketCardPrefab;
    public Canvas canvas;

    private List<MarketCard> marketCards;
    private int marketIndex;
    private MarketCardGameObject marketCard;

    // Start is called before the first frame update
    void Start()
    {

        marketCards = MarketCard.LoadMarketCards();
        marketIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(marketCard == null)
            {
                GameObject card = Instantiate(marketCardPrefab, canvas.transform);
                marketCard = card.GetComponent<MarketCardGameObject>();
                marketCard.SetMarket(marketCards[0]);
            }
            if (marketCard != null && marketCard.cardFlip.FlipReadyBack())
            {
                marketCard.cardFlip.BeginFlip();
            }
            else if (marketCard != null && marketCard.cardFlip.FlipReadyFront())
            {
                marketCard.cardFlip.BeginFlipBack();
            }
        }
        if (marketCard != null && marketCard.cardFlip.FlipDone())
        {
            marketIndex = (marketIndex + 1) % marketCards.Count;
            marketCard.cardFlip.ResetFlip();
            marketCard.SetMarket(marketCards[marketIndex]);
        }
    }
}
