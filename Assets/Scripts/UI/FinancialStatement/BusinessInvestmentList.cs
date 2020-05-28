using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessInvestmentList : MonoBehaviour
{

    public GameObject businessInvestmentRowPrefab;
    public Transform content;

    private readonly List<GameObject> listItems = new List<GameObject>();

    public void ResetPlayer(Player player)
    {
        listItems.ForEach(listItem => Destroy(listItem));
        listItems.Clear();
        int startingCashflow = player.fastTrackIncomeStatement.StartingCashFlowDayIncome;
        var businessInvestments = player.fastTrackIncomeStatement.GetEntries();
        foreach (var businessInvestment in businessInvestments)
        {
            startingCashflow += businessInvestment.MonthlyCashFlow;

            GameObject rowObject = Instantiate(businessInvestmentRowPrefab, content);
            BusinessInvestmentRow row = rowObject.GetComponent<BusinessInvestmentRow>();

            row.businessName.text = businessInvestment.Business;
            row.cashFlow.text = $"+{Utility.FormatMoney(businessInvestment.MonthlyCashFlow)}";
            row.totalCashflow.text = $"={Utility.FormatMoney(startingCashflow)}";

            listItems.Add(rowObject);

        }
    }

}
