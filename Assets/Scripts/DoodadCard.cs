using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodadCard
{

    public string title { get; private set; }
    public int cost { get; private set; }
    public bool child { get; private set; }

    private DoodadCard(DoodadJSON doodad)
    {
        this.title = doodad.title;
        this.cost = doodad.cost;
        this.child = doodad.child;
    }

    [System.Serializable]
    private struct DoodadJSON
    {
        public string title;
        public int cost;
        public bool child;
    }

    [System.Serializable]
    private struct DoodadCardsJSON
    {
        public DoodadJSON[] doodads;
    }

    public static List<DoodadCard> LoadDoodadCards()
    {
        var doodadsAsset = Resources.Load<TextAsset>("JSON/doodads");
        DoodadCardsJSON cardsObj = JsonUtility.FromJson<DoodadCardsJSON>(doodadsAsset.text);
        DoodadJSON[] cards = cardsObj.doodads;
        List<DoodadCard> doodadCards = new List<DoodadCard>();
        foreach (DoodadJSON card in cards)
        {
            doodadCards.Add(new DoodadCard(card));
        }
        return doodadCards;
    }

    public override string ToString()
    {
        return "Doodad: " + this.title + "\tPay: " + Utility.FormatMoney(this.cost) + (this.child ? "(If you have a child)" : "");
    }

}
