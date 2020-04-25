using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{

    public Button button;
    public GameObject check;

    public bool Selected { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Selected = false;
        check.SetActive(false);
    }

    public void Toggle() 
    {
        Selected = !Selected;
        check.SetActive(Selected);
    }
}
