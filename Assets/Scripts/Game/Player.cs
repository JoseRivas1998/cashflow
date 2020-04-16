using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public readonly int index;
    public readonly string name;
    public readonly string dream;
    public readonly Color color;

    public Player(int index, string name, string dream, Color color)
    {
        this.index = index;
        this.name = name;
        this.dream = dream;
        this.color = color;
    }

}
