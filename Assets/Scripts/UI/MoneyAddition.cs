using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyAddition : MonoBehaviour
{
    public RectTransform rect;
    public Text text;
    public Color additionColor;
    public Color subtractionColor;
    public AnimationCurve interpolation;

    public float enterTime = 1f;
    public float sustainTime = 0.5f;
    public float exitTime = 0.2f;

    private float stateTime;
    private float targetY;
    private float startY;
    private Color startColor;
    private Color targetColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(int amount)
    {
        if(amount < 0)
        {
            text.text = "-" + Utility.FormatMoney(Mathf.Abs(amount));
            text.color = new Color(subtractionColor.r, subtractionColor.g, subtractionColor.b, 0f);
            targetColor = subtractionColor;
        }
        else
        {
            text.text = "+" + Utility.FormatMoney(Mathf.Abs(amount));
            text.color = new Color(additionColor.r, additionColor.g, additionColor.b, 0f);
            targetColor = additionColor;
        }
        startColor = text.color;
        stateTime = 0;
        startY = transform.localPosition.y;
        Vector3[] worlds = new Vector3[4];
        rect.GetLocalCorners(worlds);
        targetY = transform.localPosition.y - Mathf.Abs(worlds[0].y - worlds[1].y);
    }

    // Update is called once per frame
    void Update()
    {
        if (stateTime > enterTime + sustainTime + exitTime)
        {
            GetComponentInParent<PlayerTab>().Unlock();
            Destroy(this.gameObject);
        } 
        else if (stateTime > enterTime + sustainTime)
        {
            float timeLeft = (enterTime + sustainTime + exitTime) - stateTime;
            float y = Mathf.Lerp(targetY, startY, interpolation.Evaluate(1 - timeLeft / exitTime));
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            text.color = Color.Lerp(targetColor, startColor, interpolation.Evaluate(1 - timeLeft / exitTime));
        }
        else if (stateTime < enterTime)
        {
            float y = Mathf.Lerp(startY, targetY, interpolation.Evaluate(stateTime / enterTime));
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            text.color = Color.Lerp(startColor, targetColor, interpolation.Evaluate(stateTime / enterTime));
        }
        stateTime += Time.deltaTime;
    }
}
