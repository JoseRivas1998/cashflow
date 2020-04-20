using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameDataTester : MonoBehaviour
{

    public GameObject dealCardPrefab;
    public Canvas canvas;

    private List<DealCard> dealCards;
    private int dealIndex;
    private DealCardGameObject dealCard;


    // Start is called before the first frame update
    void Start()
    {
        dealCards = new List<DealCard>();
        dealCards.AddRange(DealCard.SmallDeals());
        dealCards.AddRange(DealCard.BigDeals());
        dealIndex = 0;
        dealCard = null;
        List<MarketCard> marketCards = MarketCard.LoadMarketCards();
        foreach (MarketCard card in marketCards)
        {
            Debug.Log(card);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(dealCard == null)
            {
                GameObject card = Instantiate(dealCardPrefab, canvas.transform);
                dealCard = card.GetComponent<DealCardGameObject>();
                dealCard.SetDeal(dealCards[dealIndex]);
                dealIndex = (dealIndex + 1) % dealCards.Count;
            }
            if (dealCard != null && dealCard.cardFlip.FlipReadyBack())
            {
                dealCard.cardFlip.BeginFlip();
            }
            else if (dealCard != null && dealCard.cardFlip.FlipReadyFront())
            {
                dealCard.cardFlip.BeginFlipBack();
            }
        }
        if (dealCard != null && dealCard.cardFlip.FlipDone())
        {
            Destroy(dealCard.gameObject);
            dealCard = null;
        }
    }
}
