using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCount : MonoBehaviour
{
    public int maxPlayers;
    public int minPlayers;
    public Text playerCountText;
    public Button upArrow;
    public Button downArrow;

    public int count { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        count = 1;
        UpdateInteractable();
        upArrow.onClick.AddListener(() => {
            count = Mathf.Min(count + 1, maxPlayers);
            UpdateInteractable();
        });
        downArrow.onClick.AddListener(() => {
            count = Mathf.Max(count - 1, minPlayers);
            UpdateInteractable();
        });
    }

    private void UpdateInteractable()
    {
        upArrow.interactable = count < maxPlayers;
        downArrow.interactable = count > minPlayers;
    }

    // Update is called once per frame
    void Update()
    {
        playerCountText.text = "" + count;
    }
}
