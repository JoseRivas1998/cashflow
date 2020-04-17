using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTabContainer : MonoBehaviour
{
    public GameObject tabObject;

    private int numTabs;

    // Start is called before the first frame update
    void Start()
    {
        numTabs = 0;
    }

    public PlayerTab AddTab(Player player)
    {
        GameObject tab = Instantiate(tabObject, this.transform);
        RectTransform rect = tab.GetComponent<RectTransform>();
        rect.transform.localScale = Vector3.one;
        float x = numTabs * rect.rect.width;
        rect.localPosition = new Vector3(x, rect.localPosition.y);
        numTabs++;
        PlayerTab playerTab = tab.GetComponent<PlayerTab>();
        playerTab.Initialize(player);
        return playerTab;
    }
}
