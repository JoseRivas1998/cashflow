using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoanDisplay : MonoBehaviour
{

    public NumberInput numberInput;
    public Button cancelButton;

    public int loanAmount { get { return numberInput.Number; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
