using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessInvestment : FastTrackSpace
{

    public string name { get; private set; }
    public int down { get; private set; }
    public int cashFlow { get; private set; }

    public BusinessInvestment(string name, int down, int cashFlow) : base(FastTrackSpaceType.BusinessInvestments)
    {
        this.name = name;
        this.down = down;
        this.cashFlow = cashFlow;
    }
}
