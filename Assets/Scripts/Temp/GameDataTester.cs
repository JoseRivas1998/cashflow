using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameDataTester : MonoBehaviour
{

    public BoardManager board;
    public BusinessInvestmentDisplay investmentDisplay;

    private int currentSpace;

    // Start is called before the first frame update
    void Start()
    {
        currentSpace = -1;
        NextSpace();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            NextSpace();
        }
    }

    void NextSpace()
    {
        do
        {
            currentSpace = board.NormalizeFastTrackSpace(currentSpace + 1);
        } while (board.GetFastTrackSpaceType(currentSpace) != FastTrackSpaceType.BusinessInvestments);
        investmentDisplay.SetInvestment(board.GetFastTrackSpace(currentSpace));
    }

}
