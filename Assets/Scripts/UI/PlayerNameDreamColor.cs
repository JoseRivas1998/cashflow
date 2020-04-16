using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameDreamColor : MonoBehaviour
{

    public ColorSelector colorSelector;
    public Text title;
    public InputField nameField;
    public InputField dreamField;
    public Button nextButton;

    public int currentPlayer { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Activate()
    {
        colorSelector.Initialize();
        currentPlayer = 0;
        title.text = "Player " + (currentPlayer + 1) + " Details";
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        title.text = "Player " + (currentPlayer + 1) + " Details";
        nextButton.interactable = nameField.text.Trim().Length > 0 && dreamField.text.Trim().Length > 0 && colorSelector.selectedButton != null;
    }

    public void NextPlayer()
    {
        colorSelector.ResetSelected();
        currentPlayer++;
    }

}
