using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStack<T>
{
    private readonly Stack<T> cards;
    private readonly List<T> discard;

    public CardStack(List<T> initialList)
    {
        cards = new Stack<T>();
        discard = new List<T>();
        discard.AddRange(initialList);
        DumpDiscardToStack();
    }

    private void DumpDiscardToStack()
    {
        Utility.ShuffleList(discard);
        foreach (T card in discard)
        {
            cards.Push(card);
        }
        discard.Clear();
    }

    public T Pop()
    {
        if(cards.Count == 0)
        {
            DumpDiscardToStack();
        }
        T card = cards.Pop();
        discard.Add(card);
        return card;
    }

}
