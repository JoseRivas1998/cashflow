using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{

    public GameObject colorButtonPrefab;
    public RectTransform bounds;
    public Color[] colors;
    public float margin;

    public ColorButton selectedButton { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        selectedButton = null;
    }

    public void Initialize()
    {
        float totalMargin = margin * colors.Length + margin;
        float buttonWidth = (bounds.rect.width - totalMargin) / colors.Length;
        for (int i = 0; i < colors.Length; i++)
        {
            GameObject gameObject = Instantiate(colorButtonPrefab);
            gameObject.transform.parent = this.gameObject.transform;
            ColorButton colorButton = gameObject.GetComponent<ColorButton>();
            colorButton.rect.localScale = Vector3.one;
            float leftOffset = margin + ((margin + buttonWidth) * i);
            float bottomOffset = margin;
            float rightOffset = bounds.rect.width - leftOffset - buttonWidth;
            float topOffset = margin;
            colorButton.rect.offsetMin = new Vector3(leftOffset, bottomOffset);
            colorButton.rect.offsetMax = new Vector3(-rightOffset, -topOffset);
            colorButton.image.color = colors[i];
            colorButton.button.onClick.AddListener(() => {
                if(selectedButton != null)
                {
                    selectedButton.Deselect();
                    selectedButton.button.interactable = true;
                }
                colorButton.Select();
                colorButton.button.interactable = false;
                selectedButton = colorButton;
            });
        }
    }

    public void ResetSelected()
    {
        if(selectedButton != null)
        {
            selectedButton.Deselect();
            selectedButton = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
