using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour
{
    private enum FlipState
    {
        FlipReadyBack,
        TurningBackAway,
        TurningFrontTowards,
        FlipReadyFront,
        TurningFrontAway,
        TurningBackTowards,
        FlipDone
    }

    public RectTransform rect;
    public RawImage background;
    public Texture cardBack;
    public Texture cardFront;
    public GameObject cardDataContainer;
    public float flipTime = 2f;

    private float flipTimer = 0;

    private FlipState state;
    private float degPerSecond;

    
    public delegate bool FlipCondition();
    public delegate void ResetAction();
    private ResetAction onReset;
    private FlipCondition canFlip;


    // Start is called before the first frame update
    void Start()
    {
        onReset = null;
        canFlip = null;
        ResetFlip();
    }

    // Update is called once per frame
    void Update()
    {
        degPerSecond = 180f / flipTime;
        this.state = NextState();
    }

    public void AddResetAction(ResetAction action)
    {
        if(onReset == null)
        {
            onReset = action;
        } else
        {
            onReset += action;
        }
    }

    public void SetFlipCondition(FlipCondition cond)
    {
        canFlip = cond;
    }

    public bool FlipDone()
    {
        return state == FlipState.FlipDone;
    }

    public bool FlipReadyBack()
    {
        return state == FlipState.FlipReadyBack;
    }

    public bool FlipReadyFront()
    {
        return state == FlipState.FlipReadyFront;
    }

    public void ResetFlip()
    {
        onReset?.Invoke();
        rect.eulerAngles = Vector3.zero;
        background.texture = cardBack;
        cardDataContainer.SetActive(false);
        state = FlipState.FlipReadyBack;

        flipTimer = 0;
    }

    public void BeginFlip()
    {
        if (state == FlipState.FlipReadyBack && (canFlip == null || canFlip()))
        {
            this.state = FlipState.TurningBackAway;
            flipTimer = 0;
        }
    }

    public void BeginFlipBack()
    {
        if (state == FlipState.FlipReadyFront)
        {
            this.state = FlipState.TurningFrontAway;
            flipTimer = 0;
        }
    }

    private FlipState NextState()
    {
        switch (state)
        {
            case FlipState.TurningBackAway: return TurnBackAway();
            case FlipState.TurningFrontTowards: return TurnFrontTowards();
            case FlipState.TurningFrontAway: return TurnFrontAway();
            case FlipState.TurningBackTowards: return TurnBackTowards();
        }
        return this.state;
    }

    private float percent { get { return flipTimer / (flipTime / 2f); } }

    private FlipState TurnBackAway()
    {
        if (flipTimer > (flipTime / 2f))
        {
            rect.eulerAngles = new Vector3(0, -90, 0);
            cardDataContainer.SetActive(true);
            background.texture = cardFront;
            flipTimer = 0f;
            return FlipState.TurningFrontTowards;
        }
        flipTimer += Time.deltaTime;
        rect.eulerAngles = new Vector3(rect.eulerAngles.x, Mathf.Lerp(0f, 90f, percent), rect.eulerAngles.z);
        return FlipState.TurningBackAway;
    }

    private FlipState TurnFrontTowards()
    {
        if (flipTimer > (flipTime / 2f))
        {
            rect.eulerAngles = Vector3.zero;
            flipTimer = 0f;
            return FlipState.FlipReadyFront;
        }
        flipTimer += Time.deltaTime;
        rect.eulerAngles = new Vector3(rect.transform.eulerAngles.x, Mathf.Lerp(270f, 360f, percent), rect.transform.eulerAngles.z);
        return FlipState.TurningFrontTowards;
    }

    private FlipState TurnFrontAway()
    {
        if (flipTimer > (flipTime / 2f))
        {
            rect.eulerAngles = new Vector3(0, 90, 0);
            cardDataContainer.SetActive(false);
            background.texture = cardBack;
            flipTimer = 0f;
            return FlipState.TurningBackTowards;
        }
        flipTimer += Time.deltaTime;
        rect.eulerAngles = new Vector3(rect.transform.eulerAngles.x, Mathf.Lerp(270f, 360f, 1 - percent), rect.transform.eulerAngles.z);
        return FlipState.TurningFrontAway;
    }

    private FlipState TurnBackTowards()
    {
        if (flipTimer > (flipTime / 2f))
        {
            rect.eulerAngles = Vector3.zero;
            flipTimer = 0f;
            return FlipState.FlipDone;
        }
        flipTimer += Time.deltaTime;
        rect.eulerAngles = new Vector3(rect.transform.eulerAngles.x, Mathf.Lerp(0f, 90f, 1 - percent), rect.transform.eulerAngles.z);
        return FlipState.TurningBackTowards;
    }
}
