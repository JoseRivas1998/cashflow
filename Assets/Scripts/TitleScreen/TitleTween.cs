using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTween : MonoBehaviour
{
    void Awake()
    {
        LeanTween.scale(gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic);
    }
}
