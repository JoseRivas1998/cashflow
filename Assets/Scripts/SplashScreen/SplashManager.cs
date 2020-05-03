using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{

    public GameObject logo;
    public AudioSource soundEffect;
    public Image fadeOut;

    public float introTime = 0.5f;
    public float fadeOutTime = 1f;

    public bool fadingOut;

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.rotate(logo, Vector3.zero, introTime).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(logo, Vector3.one, introTime).setEase(LeanTweenType.easeOutElastic);
        fadingOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!soundEffect.isPlaying && !fadingOut)
        {
            LeanTween.value(fadeOut.gameObject, 0f, 1f, fadeOutTime)
                .setOnUpdate((val) =>
                {
                    Color c = fadeOut.color;
                    c.a = val;
                    fadeOut.color = c;
                })
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() =>
                {
                    SceneManager.LoadScene("TitleScreen");
                });
            fadingOut = true;
        }
    }
}
