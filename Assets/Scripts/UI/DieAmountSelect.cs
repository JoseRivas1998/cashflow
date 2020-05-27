using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieAmountSelect : MonoBehaviour
{

    public Text turnsLeft;
    public Button oneBtn;
    public Button twoBtn;
    public Sprite fastTrackBackground;
    public Image oneImage;
    public Image twoImage;

    public void Initialize(Player player)
    {
        if (player.FastTrack)
        {
            Color c = new Color(1f, 1f, 1f, oneImage.color.a);
            oneImage.sprite = fastTrackBackground;
            twoImage.sprite = fastTrackBackground;
            oneImage.color = c;
            twoImage.color = c;
        }
        if (player.charityTurnsLeft == 1)
        {
            turnsLeft.text = "This is your last charity turn";
        }
        else
        {
            turnsLeft.text = player.charityTurnsLeft + " charity turns left";
        }
    }
}
