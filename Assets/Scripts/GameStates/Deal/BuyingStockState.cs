using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyingStockState : GameState
{
    private enum Choice { Cancel, Buy, Loan }

    private readonly DealCard dealCard;
    private readonly DealCardGameObject dealCardObject;
    private readonly BuyStockOptions stockOptions;

    private bool selected;
    private Choice choice;
    private bool takingLoan;

    public BuyingStockState(MainGameManager mgm, DealCard dealCard, DealCardGameObject dealCardObject)
    {
        this.dealCard = dealCard;
        this.dealCardObject = dealCardObject;
        GameObject options = Object.Instantiate(mgm.buyStockOptionsPrefab, mgm.mainUICanvas.transform);
        options.transform.SetSiblingIndex(dealCardObject.transform.GetSiblingIndex() + 1);
        stockOptions = options.GetComponent<BuyStockOptions>();
        this.selected = false;
        this.takingLoan = false;
        stockOptions.loanBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            choice = Choice.Loan;
        });
        stockOptions.buyBtn.onClick.AddListener(() =>
        {
            if (selected) return;
            selected = true;
            choice = Choice.Buy;
        });
        stockOptions.cancelBtn.onClick.AddListener(() => 
        {
            if (selected) return;
            selected = true;
            choice = Choice.Cancel;
        });
    }

    public override GameState Update(MainGameManager mgm)
    {
        if (takingLoan)
        {
            dealCardObject.gameObject.SetActive(true);
            stockOptions.gameObject.SetActive(true);
            selected = false;
            takingLoan = false; 
        }
        if (selected)
        {
            switch (choice)
            {
                case Choice.Cancel:
                    Object.Destroy(stockOptions.gameObject);
                    return new SellingStockState(mgm, (StockCard)dealCard, dealCardObject);
                case Choice.Buy:
                    Object.Destroy(stockOptions.gameObject);
                    return new EnteringNumberStocksToBuyState(mgm, dealCard, dealCardObject);
                case Choice.Loan:
                    dealCardObject.gameObject.SetActive(false);
                    stockOptions.gameObject.SetActive(false);
                    takingLoan = true;
                    return new LoanState(mgm, this);
                default:
                    break;
            }
        }
        return this;
    }

}
