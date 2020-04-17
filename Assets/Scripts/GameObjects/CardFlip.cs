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
    public float degThreshold = 1.5f;

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
    }

    public void BeginFlip()
    {
        if (state == FlipState.FlipReadyBack && (canFlip == null || canFlip()))
        {
            this.state = FlipState.TurningBackAway;
        }
    }

    public void BeginFlipBack()
    {
        if (state == FlipState.FlipReadyFront)
        {
            this.state = FlipState.TurningFrontAway;
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

    private FlipState TurnBackAway()
    {
        if (Mathf.Abs(90f - rect.eulerAngles.y) < degThreshold)
        {
            rect.eulerAngles = new Vector3(0, -90, 0);
            cardDataContainer.SetActive(true);
            background.texture = cardFront;
            return FlipState.TurningFrontTowards;
        }
        rect.Rotate(new Vector3(0, degPerSecond * Time.deltaTime));
        return FlipState.TurningBackAway;
    }

    private FlipState TurnFrontTowards()
    {
        if (Mathf.Abs(rect.eulerAngles.y) < degThreshold)
        {
            rect.eulerAngles = Vector3.zero;
            return FlipState.FlipReadyFront;
        }
        rect.Rotate(new Vector3(0, degPerSecond * Time.deltaTime));
        return FlipState.TurningFrontTowards;
    }

    private FlipState TurnFrontAway()
    {
        if (Mathf.Abs(-90f - (rect.eulerAngles.y - 360)) < degThreshold)
        {
            rect.eulerAngles = new Vector3(0, 90, 0);
            cardDataContainer.SetActive(false);
            background.texture = cardBack;
            return FlipState.TurningBackTowards;
        }
        rect.Rotate(new Vector3(0, -degPerSecond * Time.deltaTime));
        return FlipState.TurningFrontAway;
    }

    private FlipState TurnBackTowards()
    {
        if (Mathf.Abs(rect.eulerAngles.y) < degThreshold)
        {
            rect.eulerAngles = Vector3.zero;
            return FlipState.FlipDone;
        }
        rect.Rotate(new Vector3(0, -degPerSecond * Time.deltaTime));
        return FlipState.TurningBackTowards;
    }
}
