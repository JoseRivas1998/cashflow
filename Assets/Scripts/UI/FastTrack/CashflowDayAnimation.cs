using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashflowDayAnimation : MonoBehaviour
{
    public float openCloseTime = 0.5f;
    public float sustainTime = 2f;
    public float endWaitTime = 0.25f;

    public bool Done { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Done = false;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        Done = false;
        var scale = LeanTween.scale(gameObject, Vector3.one, openCloseTime).setEase(LeanTweenType.easeOutBack);
        var rotate = LeanTween.rotate(gameObject, Vector3.zero, openCloseTime).setEase(LeanTweenType.easeOutBack);
        while (LeanTween.isTweening(scale.id) || LeanTween.isTweening(rotate.id))
        {
            yield return null;
        }
        yield return new WaitForSeconds(sustainTime);
        scale = LeanTween.scale(gameObject, Vector3.zero, openCloseTime).setEase(LeanTweenType.easeInBack);
        rotate = LeanTween.rotate(gameObject, Vector3.forward * 180, openCloseTime).setEase(LeanTweenType.easeInBack);
        while (LeanTween.isTweening(scale.id) || LeanTween.isTweening(rotate.id))
        {
            yield return null;
        }
        yield return new WaitForSeconds(endWaitTime);
        Done = true;
    }
}
