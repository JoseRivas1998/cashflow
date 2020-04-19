using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoodadDisplay : MonoBehaviour
{

    public DoodadCardGameObject doodadCard;
    public GameObject options;
    public Button payBtn;
    public Button loanBtn;

    public void Initialize(DoodadCard doodad)
    {
        options.SetActive(false);
        doodadCard.SetDoodad(doodad);
    }

}
