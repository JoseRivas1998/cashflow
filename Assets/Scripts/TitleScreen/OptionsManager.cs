using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public bool locked { get; private set; }

    
    void Start()
    {
        locked = false;
    }

    public void Lock()
    {
        if (!locked)
        {
            locked = true;
        }
    }

}
