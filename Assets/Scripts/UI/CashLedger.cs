using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashLedger : MonoBehaviour
{

    public RectTransform rect;
    public GameObject contentHolder;
    public Text startingBalance;
    public LedgerEntry currentBalance;
    public MainGameManager mgm;
    public GameObject ledgerEntryPrefab;
    public GameObject dividerPrefab;
    public ScrollRect scrollRect;

    private readonly List<LedgerEntry> ledgerEntries = new List<LedgerEntry>();
    private readonly List<GameObject> dividers = new List<GameObject>();
    private bool updatingToPlayer = false;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(updatingToPlayer)
        {
            AddMissingEntries();
        }   
    }

    public void BeginUpdatingToPlayer()
    {
        foreach (LedgerEntry entry in ledgerEntries)
        {
            Destroy(entry.gameObject);
        }
        ledgerEntries.Clear();
        foreach (GameObject divider in dividers)
        {
            Destroy(divider);
        }
        dividers.Clear();
        player = mgm.GetPlayer(mgm.turnManager.GetCurrentPlayer());
        startingBalance.text = "Starting Balance: " + Utility.FormatMoney(player.ledger.startingBalance);
        currentBalance.amountText.text = Utility.FormatMoney(player.ledger.GetCurretBalance());
        AddMissingEntries();
        scrollRect.normalizedPosition = new Vector2(0, 0);
        updatingToPlayer = true;
    }

    public void StopUpdatingToPlayer()
    {
        updatingToPlayer = false;
    }

    private void AddMissingEntries()
    {
        int numEntries = player.ledger.NumEntries;
        if (numEntries <= ledgerEntries.Count) return;
        currentBalance.amountText.text = Utility.FormatMoney(player.ledger.GetCurretBalance());
        Ledger.Entry[] entries = player.ledger.Entries();
        for (int i = ledgerEntries.Count; i < numEntries; i++)
        {
            Ledger.Entry entry = entries[i];
            ledgerEntries.Add(AddEntry(entry.Add, entry.Amount));
        }
    }

    private LedgerEntry AddEntry(bool add, int amount)
    {
        GameObject entryObject = Instantiate(ledgerEntryPrefab, contentHolder.transform);
        entryObject.transform.SetSiblingIndex(currentBalance.transform.GetSiblingIndex());
        GameObject divider = Instantiate(dividerPrefab, contentHolder.transform);
        divider.transform.SetSiblingIndex(currentBalance.transform.GetSiblingIndex());
        dividers.Add(divider);
        LedgerEntry entry = entryObject.GetComponent<LedgerEntry>();
        entry.operatorText.text = add ? "+" : "-";
        entry.amountText.text = Utility.FormatMoney(amount);
        return entry;
    }

    public float GetWidth()
    {
        Vector3[] worlds = new Vector3[4];
        rect.GetWorldCorners(worlds);
        return worlds[2].x - worlds[0].x;
    }

}
