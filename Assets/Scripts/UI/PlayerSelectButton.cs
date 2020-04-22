using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectButton : MonoBehaviour
{

    public Button button;
    public Image image;
    public GameObject selectedCheckmark;
    public Text playerName;
    public int playerIndex { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(Player player)
    {
        playerIndex = player.index;
        image.color = player.color;
        playerName.text = player.name;
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
