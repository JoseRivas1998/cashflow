using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerTab : MonoBehaviour
{
    public Text salaryText;
    public Text plusText;
    public Text passiveIncomeText;
    public Text totalIncomeText;
    public Text equals1;
    public Text minus;
    public Text totalExpensesText;
    public Text cashFlowText;
    public Text equals2;

    public Text playerNameText;
    public Text balanceText;
    public Text balanceLabel;
    public Image bg;

    [Range(0f, 1f)]
    public float luminanceThreshold = 0.4f;
    public float smoothing = 25f;

    public GameObject additionPrefab;

    private int playerBalance;
    private int balanceDisplay;
    private float pulledInTarget;
    private float targetY = 0;
    private Player player;

    private bool locked = false;
    private Queue<int> additions = new Queue<int>();

    public void Initialize(Player player)
    {
        this.player = player;
        playerNameText.text = player.name;
        playerBalance = player.ledger.GetCurretBalance();
        float luminance = player.color.r * 0.3f + player.color.g * 0.59f + player.color.b * 0.11f;
        UpdateValues();
        if (luminance < luminanceThreshold)
        {
            salaryText.color = Color.white;
            plusText.color = Color.white;
            passiveIncomeText.color = Color.white;
            totalIncomeText.color = Color.white;
            equals1.color = Color.white;
            minus.color = Color.white;
            totalExpensesText.color = Color.white;
            cashFlowText.color = Color.white;
            equals2.color = Color.white;
            playerNameText.color = Color.white;
            balanceText.color = Color.white;
            balanceLabel.color = Color.white;
        }
        bg.color = new Color(player.color.r, player.color.g, player.color.b, 0.75f);
        pulledInTarget = transform.localPosition.y;
        targetY = pulledInTarget;
        balanceDisplay = 0;
    }

    private void UpdateValues()
    {
        if (player == null) return;
        salaryText.text = Utility.FormatMoney(player.incomeStatement.salary);
        passiveIncomeText.text = Utility.FormatMoney(player.incomeStatement.PassiveIncome);
        totalIncomeText.text = Utility.FormatMoney(player.incomeStatement.TotalIncome);
        totalExpensesText.text = Utility.FormatMoney(player.incomeStatement.TotalExpenses);
        cashFlowText.text = Utility.FormatMoney(player.incomeStatement.MonthlyCashflow);
    }

    // Update is called once per frame
    void Update()
    {
        if (balanceDisplay < playerBalance)
        {
            int diff = playerBalance - balanceDisplay;
            if (diff > 5000)
            {
                balanceDisplay += 5000;
            }
            else if (diff > 1000)
            {
                balanceDisplay += 1000;
            }
            else if (diff > 100)
            {
                balanceDisplay += 100;
            }
            else if (diff > 10)
            {
                balanceDisplay += 10;
            }
            else
            {
                balanceDisplay++;
            }
            balanceText.text = Utility.FormatMoney(balanceDisplay);
        }
        else if (balanceDisplay > playerBalance)
        {
            int diff = balanceDisplay - playerBalance;
            if (diff > 5000)
            {
                balanceDisplay -= 5000;
            }
            else if (diff > 1000)
            {
                balanceDisplay -= 1000;
            }
            else if (diff > 100)
            {
                balanceDisplay -= 100;
            }
            else if (diff > 10)
            {
                balanceDisplay -= 10;
            }
            else
            {
                balanceDisplay--;
            }
            balanceText.text = Utility.FormatMoney(balanceDisplay);
        }
        if(Mathf.Abs(targetY - transform.localPosition.y) > 1)
        {
            float yOffset = (targetY - transform.localPosition.y) / smoothing;
            transform.localPosition += Vector3.up * yOffset;
        }
        UpdateValues();
        SpawnQueuedAddition();
    }

    public void UpdatePlayerBalance(Player player)
    {
        int newBalance = player.ledger.GetCurretBalance();
        if(newBalance != playerBalance)
        {
            EnqueAddition(newBalance - playerBalance);
            playerBalance = newBalance;
        }
    }

    private void EnqueAddition(int amount)
    {
        additions.Enqueue(amount);
    }

    private void SpawnQueuedAddition()
    {
        if(!locked)
        {
            if(additions.Count > 0)
            {
                locked = true;
                GameObject additionObject = Instantiate(additionPrefab, this.transform);
                additionObject.GetComponent<MoneyAddition>().Initialize(additions.Dequeue());
            }
        }
    }

    public void Unlock()
    {
        this.locked = false;
    }

    public void DropOut()
    {
        playerBalance = 0;
        this.bg.color = new Color(0, 0, 0, 0.25f);
        this.balanceText.color = Color.white;
        this.playerNameText.color = Color.white;
        this.playerNameText.text = $"{player.name} <color=#ff0000ff><i>BANKRUPT</i></color>";
    }

}
