﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static string FormatMoney(int money)
    {
        return string.Format("${0:n0}", money);
    }
}
