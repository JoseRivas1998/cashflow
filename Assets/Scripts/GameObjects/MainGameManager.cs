using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainGameManager : MonoBehaviour
{

    public Camera mainCam;
    public CameraTrack mainCamTracker;
    public PlayerCount playerCount;
    public PlayerNameDreamColor playerNameDreamColor;
    public PlayerProfessionReveal playerProfessionReveal;
    public GameStateDisplay gameStateDisplay;
    public TurnOrder turnOrder;
    public FinancialStatementToggle financialStatementToggle;
    public CashLedgerToggle cashLedgerToggle;
    public PayDayAnimation payDayAnimation;
    public PayDayAnimation babyAnimation;
    public PlayerTabContainer playerTabs;
    public Canvas mainUICanvas;
    public BoardManager board;
    public AnimationCurve playerMovementCurve;

    public GameObject diePrefab;
    public float diceSpawnDistance = 1f;
    public float diceSpawnOffset = 2f;

    public GameObject gamePiecePrefab;

    public GameObject charityOptionPrefab;
    public GameObject dieAmountSelectPrefab;
    public GameObject doodadDisplayPrefab;
    public GameObject loanDisplayPrefab;
    public GameObject preTurnChoicesPrefab;
    public GameObject postTurnChoicesPrefab;

    public GameObject dealCardPrefab;
    public GameObject dealTypeChoicesPrefab;
    public GameObject buyStockOptionsPrefab;
    public GameObject numberStocksInputPrefab;
    public GameObject yesNoOptionsPrefab;

    public GameObject buyPropertyOptionsPrefab;
    public GameObject realEstateBuyOnlyOptionPrefab;

    public GameObject sellCardOptionsPrefab;
    public GameObject cardOfferPrefab;

    public GameObject gambleOptionsPrefab;

    public GameObject buyGoldOptionsPrefab;
    public GameObject goldBuyOnlyOptionPrefab;

    public GameObject marketCardPrefab;
    public GameObject damageOptionsPrefab;
    public GameObject numberCoinsInput;

    public GameObject realEstateSellPrefab;
    public GameObject realEstateButtonPrefab;
    public GameObject realEstateRowPrefab;

    public GameObject payDebtScreenPrefab;

    public GameObject bankruptOptionsPrefab;

    private GameState currentState;
    private Player[] players;
    private Stack<Professions.Profession> professions;
    private CardStack<DoodadCard> doodadCards;
    private CardStack<DealCard> smallDeals;
    private CardStack<DealCard> bigDeals;
    private CardStack<MarketCard> marketCards;
    public TurnManager turnManager;
    public int NumPlayers { get { return players != null ? players.Length : 0; } }

    // Start is called before the first frame update
    void Start()
    {
        LoadProfessions();
        LoadDoodads();
        LoadSmallDeals();
        LoadBigDeals();
        LoadMarkets();
        mainCamTracker.origin = board.transform.position + board.ratRaceCenterOffset;
        currentState = new PlayerCountSelectState(this);
    }

    private void LoadProfessions()
    {
        List<Professions.Profession> professionList = new List<Professions.Profession>();
        professionList.AddRange(GameData.Instance.GetProfessions().professions);
        Utility.ShuffleList(professionList);
        professions = new Stack<Professions.Profession>();
        foreach (Professions.Profession profession in professionList)
        {
            professions.Push(profession);
        }
    }

    private void LoadDoodads()
    {
        doodadCards = new CardStack<DoodadCard>(DoodadCard.LoadDoodadCards());
    }

    private void LoadSmallDeals()
    {
        smallDeals = new CardStack<DealCard>(DealCard.SmallDeals());
    }

    private void LoadBigDeals()
    {
        bigDeals = new CardStack<DealCard>(DealCard.BigDeals());
    }

    private void LoadMarkets()
    {
        marketCards = new CardStack<MarketCard>(MarketCard.LoadMarketCards());
    }

    public void SetNumPlayers(int numPlayers)
    {
        if(players == null)
        {
            players = new Player[numPlayers];
            turnManager = new TurnManager(numPlayers);
        } 
        else
        {
            throw new System.Exception("The player count was already set!");
        }
    }

    public void RegisterPlayer(int index, string name, string dream, Color color)
    {
        if(professions.Count == 0)
        {
            throw new System.Exception("No more professions!");
        }
        players[index] = new Player(index, name, dream, color, professions.Pop());
    }

    public void RegisterPlayerTurnRoll(int player, int rollValue)
    {
        turnManager.RegisterRoll(player, rollValue);
    }

    public void CreatePlayerTabs()
    {
        int[] playerOrder = turnManager.TurnOrder();
        foreach (int pIndex in playerOrder)
        {
            players[pIndex].SetTab(playerTabs.AddTab(players[pIndex]));
        }
    }

    public void SpawnGamePiece(int player)
    {
        GameObject gamePiece = Instantiate(gamePiecePrefab);
        players[player].SetGamePiece(gamePiece.GetComponent<GamePiece>());
    }

    public Player GetPlayer(int i)
    {
        return players[i];
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Update(this);
    }

    public DiceContainer[] SpawnDice(int n)
    {
        DiceContainer[] dice = new DiceContainer[n];
        Transform camTransform = mainCam.transform;
        float mid = (n - 1) / 2f;
        Vector3 center = camTransform.position + camTransform.forward * diceSpawnDistance;
        for (int i = 0; i < n; i++)
        {
            float distToMid = i - mid;
            Vector3 spawnPoint = center + Vector3.right * distToMid * diceSpawnOffset;
            GameObject diceObject = Instantiate(diePrefab, spawnPoint, camTransform.rotation);
            diceObject.transform.Rotate(Random.onUnitSphere * 360);
            dice[i] = diceObject.GetComponent<DiceContainer>();
        }
        return dice;
    }

    public DoodadCard PullDoodadCard()
    {
        return doodadCards.Pop();
    }

    public DealCard PullSmallDealCard()
    {
        return smallDeals.Pop();
    }

    public DealCard PullBigDeal()
    {
        return bigDeals.Pop();
    }

    public MarketCard PullMarketCard()
    {
        return marketCards.Pop();
    }

    public void DropOutPlayer(int playerIndex)
    {
        GetPlayer(playerIndex).DropOut();
        turnManager.DropOutPlayer(playerIndex);
    }

}
