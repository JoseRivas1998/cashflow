using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTrackAnimation : MonoBehaviour
{
    public float openCloseTime = 0.5f;
    public float sustainTime = 2f;
    public float endWaitTime = 0.25f;

    public Image image;

    public AspectRatioFitter ratioFitter;

    public AudioSource audioSrc;

    public Sprite cashflowDay;
    public Sprite healthCare;
    public Sprite lawsuit;
    public Sprite taxAudit;
    public Sprite divorce;

    public bool Done { get; private set; }

    private bool playAudio;

    public enum FastTrackAnimationType
    {
        CashflowDay,
        HealthCare,
        Lawsuit,
        TaxAudit,
        Divorce
    }

    public void SetAnimationType(FastTrackAnimationType type)
    {
        playAudio = false;
        switch (type)
        {
            case FastTrackAnimationType.CashflowDay:
                image.sprite = cashflowDay;
                playAudio = true;
                break;
            case FastTrackAnimationType.HealthCare:
                image.sprite = healthCare;
                break;
            case FastTrackAnimationType.Lawsuit:
                image.sprite = lawsuit;
                break;
            case FastTrackAnimationType.TaxAudit:
                image.sprite = taxAudit;
                break;
            case FastTrackAnimationType.Divorce:
                image.sprite = divorce;
                break;
            default:
                image.sprite = cashflowDay; // cashflow day by default
                break;
        }
        ratioFitter.aspectRatio = ((float)image.sprite.texture.width) / ((float)image.sprite.texture.height);
    }

    // Start is called before the first frame update
    void Start()
    {
        Done = false;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        Done = false;
        if (playAudio) audioSrc.Play();
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
