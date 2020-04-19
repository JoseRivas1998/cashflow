using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoanDisplay : MonoBehaviour
{

    public Button upBtn;
    public Button downBtn;
    public Button cancelBtn;
    public Button confirmBtn;
    public Text amountText;

    public int loanAmount { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        loanAmount = 1000;
        amountText.text = Utility.FormatMoney(loanAmount);
        downBtn.interactable = false;
        upBtn.onClick.AddListener(() => loanAmount += 1000);
        downBtn.onClick.AddListener(() => loanAmount = Mathf.Max(loanAmount - 1000, 1000));
    }

    // Update is called once per frame
    void Update()
    {
        amountText.text = Utility.FormatMoney(loanAmount);
        downBtn.interactable = loanAmount > 1000;
    }
}
