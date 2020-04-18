using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateDisplay : MonoBehaviour
{
    public Text displayText;

    public void SetText(string s)
    {
        displayText.text = s;
        gameObject.SetActive(true);
    }

}
