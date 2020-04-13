using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Professions professions = GameData.Instance.GetProfessions();
        List<DealCard> smallDeals = DealCard.SmallDeals();
        foreach (DealCard card in smallDeals)
        {
            Debug.Log(card);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
