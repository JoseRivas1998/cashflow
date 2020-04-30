using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTrackSpace
{
    public FastTrackSpaceType type { get; private set; }

    public FastTrackSpace(FastTrackSpaceType type)
    {
        this.type = type;
    }

}
