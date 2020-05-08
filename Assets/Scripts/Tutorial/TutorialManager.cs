using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{


    [System.Serializable]
    struct TutorialContent
    {
        public string intro;
        public string intro2;
        public string gamePiece;
        public string profession;
    }

    [System.Serializable]
    public struct CameraPositions
    {
        public Transform startingPosition;
        public Transform gamePiece;
        public Transform profession;
    }

    enum TutorialState
    {
        Loading, Open, Intro1, Intro2, GamePiece,
        Profession, StartingBalance, FinancialStatement, CashLedger, StartOfTurn,
        ShowDie, RollDie, Spaces, PayDay, Income, Expenses, 
        Lose, Deal, DealTypes, RealEstate, SellCard, AddProperty, Stock,
        AddStock, SellStock, Coins, AddCoins, Gamble, Doodad, 
        Market, MarketTypes, RealEstateMarket, SellProperty, StockSplit, Damage, GoldBuyer,
        Charity, Baby, Downsize, TakeLoans, PayDabt, 
        PayLoans, FastTrackCondition, EnterFastTrack, EndTutorial
    }

    public GameObject dialogueBox;
    public Text dialogueBoxContent;

    public float openDialogueBoxTime = 0.75f;
    public float textDelay = 0.01f;
    public float cameraMoveTime = 1f;

    public Button nextBtn;

    public Camera cam;
    public CameraPositions cameraPositions;

    public GamePiece gamePiece;
    public ProfessionCard professionCard;

    private TutorialContent content;
    private Coroutine setTextRoutine;

    private TutorialState state;

    private Player player;

    void Start()
    {
        StartCoroutine(Initialize());
        StartCoroutine(LoadTutorialContent("JSON/tutorial_en"));
        setTextRoutine = null;
    }

    IEnumerator Initialize()
    {
        cam.transform.position = cameraPositions.startingPosition.position;
        cam.transform.rotation = cameraPositions.startingPosition.rotation;
        Professions.Profession[] professions = GameData.Instance.GetProfessions().professions;
        Professions.Profession profession = null;
        bool found = false;
        for (int i = 0; i < professions.Length && !found; i++)
        {
            if(professions[i].name.Equals("Airline Pilot"))
            {
                profession = professions[i];
                found = true;
            }
        }
        professionCard.SetProfession(profession);
        player = new Player(0, "Bob", "Start Alpaca Farm", new Color(4f / 255f, 0, 180f / 255f, 1f), profession);
        player.SetGamePiece(gamePiece);
        yield return null;
    }

    IEnumerator LoadTutorialContent(string filePath)
    {
        this.state = TutorialState.Loading;
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
        this.state = TutorialState.Open;
        var openTween = LeanTween.scale(dialogueBox, Vector3.one, openDialogueBoxTime).setEase(LeanTweenType.easeOutElastic);
        while(LeanTween.isTweening(openTween.id))
        {
            yield return null;
        }
        this.state = TutorialState.Intro1;
        setTextRoutine = StartCoroutine(SetDialogueText(content.intro));
    }

    public IEnumerator Intro2()
    {
        this.state = TutorialState.Intro2;
        StopSetText();
        yield return StartCoroutine(SetDialogueText(content.intro2));
    }

    public IEnumerator GamePiece()
    {
        this.state = TutorialState.GamePiece;
        gamePiece.gameObject.SetActive(true);
        var moveTween = LeanTween.move(cam.gameObject, cameraPositions.gamePiece.position, cameraMoveTime).setEase(LeanTweenType.easeInOutSine);
        var rotateTween = LeanTween.rotate(cam.gameObject, cameraPositions.gamePiece.rotation.eulerAngles, cameraMoveTime).setEase(LeanTweenType.easeInOutSine);
        nextBtn.interactable = false;
        while(LeanTween.isTweening(moveTween.id) || LeanTween.isTweening(rotateTween.id))
        {
            yield return null;
        }
        StopSetText();
        nextBtn.interactable = true;
        yield return StartCoroutine(SetDialogueText(content.gamePiece));
    }

    public IEnumerator Profession()
    {
        this.state = TutorialState.Profession;
        var moveTween = LeanTween.move(cam.gameObject, cameraPositions.profession.position, cameraMoveTime).setEase(LeanTweenType.easeInOutSine);
        var rotateTween = LeanTween.rotate(cam.gameObject, cameraPositions.profession.rotation.eulerAngles, cameraMoveTime).setEase(LeanTweenType.easeInOutSine);
        nextBtn.interactable = false;
        while (LeanTween.isTweening(moveTween.id) || LeanTween.isTweening(rotateTween.id))
        {
            yield return null;
        }
        professionCard.gameObject.SetActive(true);
        yield return null; // skip a frame
        professionCard.cardFlip.background.texture = professionCard.cardFlip.cardFront;
        professionCard.cardFlip.cardDataContainer.SetActive(true);
        StopSetText();
        nextBtn.interactable = true;
        yield return StartCoroutine(SetDialogueText(content.profession));
    }

    public void Next()
    {
        switch (this.state)
        {
            case TutorialState.Intro1:
                StartCoroutine(Intro2());
                break;
            case TutorialState.Intro2:
                StartCoroutine(GamePiece());
                break;
            case TutorialState.GamePiece:
                StartCoroutine(Profession());
                break;
            case TutorialState.Profession:
                break;
            case TutorialState.StartingBalance:
                break;
            case TutorialState.FinancialStatement:
                break;
            case TutorialState.CashLedger:
                break;
            case TutorialState.StartOfTurn:
                break;
            case TutorialState.ShowDie:
                break;
            case TutorialState.RollDie:
                break;
            case TutorialState.Spaces:
                break;
            case TutorialState.PayDay:
                break;
            case TutorialState.Income:
                break;
            case TutorialState.Expenses:
                break;
            case TutorialState.Lose:
                break;
            case TutorialState.Deal:
                break;
            case TutorialState.DealTypes:
                break;
            case TutorialState.RealEstate:
                break;
            case TutorialState.SellCard:
                break;
            case TutorialState.AddProperty:
                break;
            case TutorialState.Stock:
                break;
            case TutorialState.AddStock:
                break;
            case TutorialState.SellStock:
                break;
            case TutorialState.Coins:
                break;
            case TutorialState.AddCoins:
                break;
            case TutorialState.Gamble:
                break;
            case TutorialState.Doodad:
                break;
            case TutorialState.Market:
                break;
            case TutorialState.MarketTypes:
                break;
            case TutorialState.RealEstateMarket:
                break;
            case TutorialState.SellProperty:
                break;
            case TutorialState.StockSplit:
                break;
            case TutorialState.Damage:
                break;
            case TutorialState.GoldBuyer:
                break;
            case TutorialState.Charity:
                break;
            case TutorialState.Baby:
                break;
            case TutorialState.Downsize:
                break;
            case TutorialState.TakeLoans:
                break;
            case TutorialState.PayDabt:
                break;
            case TutorialState.PayLoans:
                break;
            case TutorialState.FastTrackCondition:
                break;
            case TutorialState.EnterFastTrack:
                break;
            case TutorialState.EndTutorial:
                break;
            default:
                break;
        }
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

        char[] charArr = text.ToCharArray();
        foreach (char letter in charArr)
        {
            dialogueBoxContent.text += letter;
            yield return new WaitForSeconds(textDelay);
        }
        setTextRoutine = null;
    }

}
