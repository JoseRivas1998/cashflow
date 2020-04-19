using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoodadCardGameObject : MonoBehaviour
{

    public CardFlip cardFlip;
    public Text title;
    public Text cost;
    public Text ifChild;

    private DoodadCard doodadCard = null;

    // Start is called before the first frame update
    void Start()
    {
        cardFlip.AddResetAction(() => doodadCard = null);
        cardFlip.SetFlipCondition(() => doodadCard != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDoodad(DoodadCard doodadCard)
    {
        title.text = doodadCard.title.ToUpper();
        cost.text = "Pay " + Utility.FormatMoney(doodadCard.cost);
        ifChild.gameObject.SetActive(doodadCard.child);
        this.doodadCard = doodadCard;
    }

}
