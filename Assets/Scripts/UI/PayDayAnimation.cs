using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayDayAnimation : MonoBehaviour
{
    public Animator animator;
    public AudioSource soundEffect;

    public bool AnimatiorReady()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("AnimatorReady");
    }

    public bool AnimatorDone()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("AnimatorDone");
    }

    public void StartAnimation()
    {
        if(AnimatiorReady())
        {
            soundEffect.Play();
            NextState();
        }
    }

    public void ResetAnimation()
    {
        if(AnimatorDone())
        {
            NextState();
        }
    }

    private void NextState()
    {
        animator.SetTrigger("NextState");
    }

}
