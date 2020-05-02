using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTween : MonoBehaviour
{
    void Awake()
    {
        LeanTween.scale(gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(0.25f);
    }
}
