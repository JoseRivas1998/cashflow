using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    /*
     * case TutorialState.TakeLoans:
     * case TutorialState.PayDebt:
     * case TutorialState.PayLoans:
     * case TutorialState.FastTrackCondition:
     * case TutorialState.EnterFastTrack:
     * case TutorialState.EndTutorial:
     */

    [System.Serializable]
    struct TutorialContent
    {
        public string intro;
        public string intro2;
        public string gamePiece;
        public string profession;
        public string startingBalance;
        public string financialStatement;
        public string cashLedger;
        public string startOfTurn;
        public string showDie;
        public string rollDie;
        public string spaces;
        public string payday;
        public string income;
        public string expenses;
        public string lose;
        public string deal;
        public string dealTypes;
        public string doodad;
        public string market;
        public string martketTypes;
        public string baby;
        public string charity;
        public string downsized;
        public string takeLoans;
        public string payDebt;
        public string payLoans;
        public string fastTrackCondition;
        public string enterFastTrack;
        public string endTutorial;
    }

    [System.Serializable]
    public struct CameraPositions
    {
        public Transform startingPosition;
        public Transform gamePiece;
        public Transform profession;
        public Transform payday;
        public Transform deal;
        public Transform doodad;
        public Transform market;
        public Transform baby;
        public Transform charity;
        public Transform downsized;
    }

    enum TutorialState
    {
        Loading, Open, Intro1, Intro2, GamePiece,
        Profession, StartingBalance, FinancialStatement, CashLedger, StartOfTurn,
        ShowDie, RollDie, Spaces, PayDay, Income, Expenses, 
        Lose, Deal, DealTypes, Doodad, 
        Market, MarketTypes,
        Baby, Charity, Downsize, TakeLoans, PayDebt, 
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

    public PlayerTab tab;
    public FinancialStatementToggle financialStatementToggle;
    public FinancialStatement financialStatement;
    public CashLedgerToggle cashLedgerToggle;
    public CashLedger ledger;

    public DiceContainer die;

    public Image fadeOut;

    public GameObject cancelContainer;
    public GameObject cancelDialog;

    private TutorialContent content;
    private Coroutine setTextRoutine;

    private TutorialState state;

    private Player player;
    private bool fadingOut = false;

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
        this.dialogueBox.transform.localScale = new Vector3(0, 1, 1);
        fadeOut.gameObject.SetActive(false);
        cancelContainer.SetActive(false);
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
        die.roller.ResetRoll();
        die.gameObject.SetActive(false);
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
        setTextRoutine = StartCoroutine(SetDialogueText(content.intro2));
        yield return setTextRoutine;
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
        setTextRoutine = StartCoroutine(SetDialogueText(content.gamePiece));
        yield return setTextRoutine;
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
        setTextRoutine = StartCoroutine(SetDialogueText(content.profession));
        yield return setTextRoutine;
    }

    public IEnumerator StartingBalance()
    {
        this.state = TutorialState.StartingBalance;
        tab.Initialize(player);
        tab.gameObject.SetActive(true);
        StopSetText();
        setTextRoutine = StartCoroutine(SetDialogueText(content.startingBalance));
        yield return setTextRoutine;
    }

    public IEnumerator FinalncialStatement()
    {
        this.state = TutorialState.FinancialStatement;
        this.financialStatementToggle.gameObject.SetActive(true);
        this.professionCard.gameObject.SetActive(false);
        StopSetText();
        setTextRoutine = StartCoroutine(SetDialogueText(content.financialStatement));
        yield return setTextRoutine;
    }

    public IEnumerator CashLedger()
    {
        this.state = TutorialState.CashLedger;
        this.cashLedgerToggle.gameObject.SetActive(true);
        StopSetText();
        setTextRoutine = StartCoroutine(SetDialogueText(content.cashLedger));
        yield return setTextRoutine;
    }

    public IEnumerator ShowDie()
    {
        this.state = TutorialState.ShowDie;
        this.nextBtn.interactable = false;
        StopSetText();
        setTextRoutine = StartCoroutine(SetDialogueText(content.showDie));
        this.die.gameObject.SetActive(true);
        this.die.roller.ResetRoll();
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        this.die.roller.Shake();
        yield return RollDie();
    }

    public IEnumerator RollDie()
    {
        this.state = TutorialState.RollDie;
        StopSetText();
        setTextRoutine = StartCoroutine(SetDialogueText(content.rollDie));
        while (!Input.GetMouseButtonUp(0))
        {
            yield return null;
        }
        this.die.roller.Roll();
        int dieValue = -1;
        while (dieValue == -1)
        {
            if (this.die.roller.RollComplete())
            {
                dieValue = this.die.dir.DieValue();
                if(dieValue == -1)
                {
                    this.die.roller.ResetRoll();
                    this.die.roller.Roll();
                }
            }
            else
            {
                yield return null;
            }
        }
        Destroy(this.die.gameObject);
        yield return StartCoroutine(Spaces());
    }

    public IEnumerator Spaces()
    {
        this.state = TutorialState.Spaces;
        this.nextBtn.interactable = true;
        StopSetText();
        setTextRoutine = StartCoroutine(SetDialogueText(content.spaces));
        yield return setTextRoutine;
    }

    IEnumerator SimpleText(TutorialState targetState, string text)
    {
        this.state = targetState;
        StopSetText();
        setTextRoutine = StartCoroutine(SetDialogueText(text));
        yield return setTextRoutine;
    }

    IEnumerator MoveToPosition(TutorialState targetState, Transform targetTransform, string text)
    {
        this.state = targetState;
        this.nextBtn.interactable = false;
        var move = LeanTween.move(this.cam.gameObject, targetTransform.position, cameraMoveTime).setEase(LeanTweenType.easeInOutSine);
        var rotate = LeanTween.rotate(this.cam.gameObject, targetTransform.eulerAngles, cameraMoveTime).setEase(LeanTweenType.easeInOutSine);
        while (LeanTween.isTweening(move.id) || LeanTween.isTweening(rotate.id))
        {
            yield return null;
        }
        StopSetText();
        setTextRoutine = StartCoroutine(SetDialogueText(text));
        this.nextBtn.interactable = true;
        yield return setTextRoutine;
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
                StartCoroutine(StartingBalance());
                break;
            case TutorialState.StartingBalance:
                StartCoroutine(FinalncialStatement());
                break;
            case TutorialState.FinancialStatement:
                StartCoroutine(CashLedger());
                break;
            case TutorialState.CashLedger:
                StartCoroutine(SimpleText(TutorialState.StartOfTurn, content.startOfTurn));
                return;
            case TutorialState.StartOfTurn:
                StartCoroutine(ShowDie());
                break;
            case TutorialState.Spaces:
                StartCoroutine(MoveToPosition(TutorialState.PayDay, cameraPositions.payday, content.payday));
                break;
            case TutorialState.PayDay:
                StartCoroutine(SimpleText(TutorialState.Income, content.income));
                break;
            case TutorialState.Income:
                StartCoroutine(SimpleText(TutorialState.Expenses, content.expenses));
                break;
            case TutorialState.Expenses:
                StartCoroutine(SimpleText(TutorialState.Lose, content.lose));
                break;
            case TutorialState.Lose:
                StartCoroutine(MoveToPosition(TutorialState.Deal, cameraPositions.deal, content.deal));
                break;
            case TutorialState.Deal:
                StartCoroutine(SimpleText(TutorialState.DealTypes, content.dealTypes));
                break;
            case TutorialState.DealTypes:
                StartCoroutine(MoveToPosition(TutorialState.Doodad, cameraPositions.doodad, content.doodad));
                break;
            case TutorialState.Doodad:
                StartCoroutine(MoveToPosition(TutorialState.Market, cameraPositions.market, content.market));
                break;
            case TutorialState.Market:
                StartCoroutine(SimpleText(TutorialState.MarketTypes, content.martketTypes));
                break;
            case TutorialState.MarketTypes:
                StartCoroutine(MoveToPosition(TutorialState.Baby, cameraPositions.baby, content.baby));
                break;
            case TutorialState.Baby:
                StartCoroutine(MoveToPosition(TutorialState.Charity, cameraPositions.charity, content.charity));
                break;
            case TutorialState.Charity:
                StartCoroutine(MoveToPosition(TutorialState.Downsize, cameraPositions.downsized, content.downsized));
                break;
            case TutorialState.Downsize:
                StartCoroutine(MoveToPosition(TutorialState.TakeLoans, cameraPositions.startingPosition, content.takeLoans));
                break;
            case TutorialState.TakeLoans:
                StartCoroutine(SimpleText(TutorialState.PayDebt, content.payDebt));
                break;
            case TutorialState.PayDebt:
                StartCoroutine(SimpleText(TutorialState.PayLoans, content.payLoans));
                break;
            case TutorialState.PayLoans:
                StartCoroutine(SimpleText(TutorialState.FastTrackCondition, content.fastTrackCondition));
                break;
            case TutorialState.FastTrackCondition:
                StartCoroutine(SimpleText(TutorialState.EnterFastTrack, content.enterFastTrack));
                break;
            case TutorialState.EnterFastTrack:
                StartCoroutine(SimpleText(TutorialState.EndTutorial, content.endTutorial));
                break;
            case TutorialState.EndTutorial:
                ReturnToTitleScreen();
                break;
            default:
                break;
        }
    }

    public void ReturnToTitleScreen()
    {
        if (fadingOut) return;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        fadingOut = true;
        fadeOut.color = new Color(0f, 0f, 0f, 0f);
        fadeOut.gameObject.SetActive(true);
        var fade = LeanTween.value(0f, 1f, cameraMoveTime)
            .setOnUpdate(alpha =>
            {
                Color c = fadeOut.color;
                c.a = alpha;
                fadeOut.color = c;
            })
            .setEase(LeanTweenType.easeInOutSine);
        while (LeanTween.isTweening(fade.id))
        {
            yield return null;
        }
        SceneManager.LoadScene("TitleScreen");
    }

    public void OpenCancelDialog()
    {
        StartCoroutine(OpenCancel());
    }

    public void CloseCancelDialog()
    {
        StartCoroutine(CloseCancel());
    }

    IEnumerator OpenCancel()
    {
        cancelDialog.transform.localScale = new Vector3(0, 0f, 1f);
        cancelContainer.SetActive(true);
        var open = LeanTween.scale(cancelDialog.gameObject, Vector3.one, openDialogueBoxTime).setEase(LeanTweenType.easeOutElastic);
        while (LeanTween.isTweening(open.id))
        {
            yield return null;
        }
    }
    
    IEnumerator CloseCancel()
    {
        var open = LeanTween.scale(cancelDialog.gameObject, new Vector3(0f, 0f, 1f), openDialogueBoxTime).setEase(LeanTweenType.easeInBack);
        while (LeanTween.isTweening(open.id))
        {
            yield return null;
        }
        cancelContainer.SetActive(false);
    }

    public void SetFinanicalStateMentToCurrentPlayer()
    {
        this.financialStatement.UpdateToCurrentPlayer(player);
    }

    public void SetCashLedgerToCurrentPlayer()
    {
        this.ledger.BeginUpdatingToPlayer(this.player);
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
