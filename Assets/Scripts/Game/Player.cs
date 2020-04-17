using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public readonly int index;
    public readonly string name;
    public readonly string dream;
    public readonly Color color;
    public readonly Professions.Profession profession;

    public Player(int index, string name, string dream, Color color, Professions.Profession profession)
    {
        this.index = index;
        this.name = name;
        this.dream = dream;
        this.color = color;
        this.profession = profession;
    }

    public override string ToString()
    {
        return this.name + " is a " + this.profession.name + " with a dream: " + this.dream;
    }

}
