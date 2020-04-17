using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRollerTester : MonoBehaviour
{

    public DiceContainer dice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dice.roller.Shake();
        }
        if (Input.GetMouseButtonUp(0))
        {
            dice.roller.Roll();
        }
        if (dice.roller.RollComplete())
        {
            dice.roller.ResetRoll();
            int num = dice.dir.DieValue();
            if(num == -1)
            {
                dice.roller.Roll();
            }
        }

    }
}
