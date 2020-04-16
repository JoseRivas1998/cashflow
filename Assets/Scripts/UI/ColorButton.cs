using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{

    public RectTransform rect;
    public Button button;
    public Image image;
    public GameObject selectedCheckmark;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        selectedCheckmark.SetActive(true);
    }

    public void Deselect()
    {
        selectedCheckmark.SetActive(false);
    }

}
