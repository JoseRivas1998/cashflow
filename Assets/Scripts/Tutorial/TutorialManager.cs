using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
struct TutorialContent
{
    public string intro;
}

public class TutorialManager : MonoBehaviour
{

    public GameObject dialogueBox;
    public Text dialogueBoxContent;

    public float openDialogueBoxTime = 0.75f;
    public float textAppearTime = 5f;

    private TutorialContent content;
    private Coroutine setTextRoutine;

    void Start()
    {
        StartCoroutine(LoadTutorialContent("JSON/tutorial_en"));
        setTextRoutine = null;
    }

    IEnumerator LoadTutorialContent(string filePath)
    {
        ResourceRequest requsest = Resources.LoadAsync<TextAsset>(filePath);

        while(!requsest.isDone)
        {
            yield return null;
        }
        TextAsset textAsset = (TextAsset) requsest.asset;
        string jsonText = textAsset.text;
        content  = JsonUtility.FromJson<TutorialContent>(jsonText);
        yield return StartCoroutine(OpenDialogueBox());
    }

    IEnumerator OpenDialogueBox()
    {
        var openTween = LeanTween.scale(dialogueBox, Vector3.one, openDialogueBoxTime).setEase(LeanTweenType.easeOutElastic);
        while(LeanTween.isTweening(openTween.id))
        {
            yield return null;
        }
        setTextRoutine = StartCoroutine(SetDialogueText(content.intro));
    }

    private void StopSetText()
    {
        if(setTextRoutine != null)
        {
            StopCoroutine(setTextRoutine);
            setTextRoutine = null;
        }
    }

    IEnumerator SetDialogueText(string text)
    {
        dialogueBoxContent.text = "";
        float delay = textAppearTime / (float)text.Length;

        char[] charArr = text.ToCharArray();
        foreach (char letter in charArr)
        {
            dialogueBoxContent.text += letter;
            yield return new WaitForSeconds(delay);
        }
        setTextRoutine = null;
    }

}
