using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInTween : MonoBehaviour
{
    public float fadeInTime = 1f;
    public Image image;
    public GameObject title;
    public GameObject options;

    private void Start()
    {
        LeanTween.value(image.gameObject, 1f, 0f, fadeInTime)
                .setOnUpdate((val) =>
                {
                    Color c = image.color;
                    c.a = val;
                    image.color = c;
                })
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() =>
                {
                    title.SetActive(true);
                    options.SetActive(true);
                    Destroy(gameObject);
                });
    }

}
