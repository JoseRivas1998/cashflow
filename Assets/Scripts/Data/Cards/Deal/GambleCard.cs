using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambleCard : DealCard
{

    public string instruction { get; private set; }
    public int badMin { get; private set; }
    public int badMax { get; private set; }
    public int goodMin { get; private set; }
    public int goodMax { get; private set; }
    public int reward { get; private set; }
    public bool gold { get; private set; }
    public string badText { get; private set; }
    public string rewardText { get; private set; }
    public int cost { get; private set; }
    public bool mlm { get; private set; }
    public GambleCard(string title, string flavorText, string instruction, int goodMin, int goodMax, int badMin, int badMax, int reward, bool gold, string rewardText, string badText, int cost, bool mlm)
    {
        this.type = DealType.Gamble;
        this.smallDeal = true;
        this.title = title;
        this.flavorText = flavorText;
        this.instruction = instruction;
        this.goodMin = goodMin;
        this.goodMax = goodMax;
        this.badMin = badMin;
        this.badMax = badMax;
        this.reward = reward;
        this.gold = gold;
        this.rewardText = rewardText;
        this.badText = badText;
        this.cost = cost;
        this.mlm = mlm;
    }

    public string RewardRange { get { return diceRange(goodMin, goodMax); } }
    public string BadRange { get { return diceRange(badMin, badMax); } }

    private string diceRange(int min, int max)
    {
        if(min == max)
        {
            return "" + min;
        }
        return min + "-" + max;
    }

    public override string ToString()
    {
        return base.ToString() +
            "\tType: " + (this.mlm ? "MLM" : "Regular") + 
            "\t" + this.instruction +
            "\tDie = " + BadRange + ", " + this.badText +
            "\tDie = " + RewardRange + ", " + this.rewardText +
            "\tReward: " + (this.gold ? (this.reward + " Gold Coins") : Utility.FormatMoney(this.reward)) +
            "\tCost: " + Utility.FormatMoney(this.cost);
    }

}
