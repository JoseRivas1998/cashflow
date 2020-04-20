using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberInput : MonoBehaviour
{

    public Button oneBtn;
    public Button twoBtn;
    public Button threeBtn;
    public Button fourBtn;
    public Button fiveBtn;
    public Button sixBtn;
    public Button sevenBtn;
    public Button eightBtn;
    public Button nineBtn;
    public Button zeroBtn;
    public Button backspaceBtn;
    public Button confirmBtn;
    public Text amountText;
    public Text costText;
    public bool isMoney;
    public int maxNumber = int.MaxValue;
    public int minNumber = 0;

    public int Number { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        amountText.text = isMoney ? Utility.FormatMoney(Number) : Utility.FormatNumberCommas(Number);   
    }

    public void Add(int digit)
    {
        if(digit < 10)
        {
            Number = Number * 10 + digit;
            Number = Mathf.Min(Mathf.Max(Number, minNumber), maxNumber);
        }
    }

    public void Backspace()
    {
        Number /= 10;
    }

}
