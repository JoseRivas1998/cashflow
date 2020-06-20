using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace Tests
{
    public class IncomeStatementTest
    {
        const string TEST_STOCK = "TEST";
        Professions.Profession profession;
        IncomeStatement incomeStatement;

        [SetUp]
        public void CreateProfession()
        {
            var professionsJSON = Resources.Load<TextAsset>("JSON/professions");
            Professions professions = Professions.CreateFromJSON(professionsJSON.ToString());
            profession = professions.professions[0];
            incomeStatement = new IncomeStatement(profession);
        }

        [Test]
        public void FirstProfessionIsSecretary()
        {
            Assert.AreEqual("Secretary", profession.name);
        }

        [Test]
        public void SecretaryTotalStartingIncome2500()
        {
            Assert.AreEqual(incomeStatement.TotalIncome, 2500);
        }

        [Test]
        public void SecretaryTotalExpenses1700()
        {
            Assert.AreEqual(incomeStatement.TotalExpenses, 1700);
        }

        [Test]
        public void SecretaryMontlyCashFlow800()
        {
            Assert.AreEqual(incomeStatement.MonthlyCashflow, 800);
        }

        [Test]
        public void NewIncomeStatementShouldHaveNoStock()
        {
            Assert.IsEmpty(incomeStatement.StockEntries());
        }

        [Test]
        public void IncomeStatementShouldHaveStocksAfterAddingOneStock()
        {
            incomeStatement.AddStock(TEST_STOCK, 40, 1);
            Assert.IsNotEmpty(incomeStatement.StockEntries());
        }

        [Test]
        public void IncomeStatementShouldHaveCorrectSharesAfterAddingStockAtSamePrice()
        {
            incomeStatement.AddStock(TEST_STOCK, 40, 5);
            Assert.AreEqual(incomeStatement.NumShares(TEST_STOCK), 5);
        }

        [Test]
        public void IncomeStatementShouldHaveCorrectSharesAfterAddingMultipleStocksAtSamePrice()
        {
            incomeStatement.AddStock(TEST_STOCK, 40, 5);
            incomeStatement.AddStock(TEST_STOCK, 40, 5);
            Assert.AreEqual(incomeStatement.NumShares(TEST_STOCK), 10);
        }

        [Test]
        public void IncomeStatementShouldHaveCorrectSharesAfterAddingMultipleStocksAtDifferentPrice()
        {
            incomeStatement.AddStock(TEST_STOCK, 40, 5);
            incomeStatement.AddStock(TEST_STOCK, 20, 2);
            Assert.AreEqual(incomeStatement.NumShares(TEST_STOCK), 7);
        }

        [Test]
        public void IncomeStatementAfterAddingAndRemovingStock()
        {
            incomeStatement.AddStock(TEST_STOCK, 40, 4);
            incomeStatement.SellStock(TEST_STOCK, 4);
            Assert.AreEqual(incomeStatement.NumShares(TEST_STOCK), 0);
        }

        [Test]
        public void IncomeStatementAfterAddingAndRemovingStockAtDifferentPrices()
        {
            incomeStatement.AddStock(TEST_STOCK, 40, 5);
            incomeStatement.AddStock(TEST_STOCK, 20, 5);
            incomeStatement.SellStock(TEST_STOCK, 10);
            Assert.AreEqual(incomeStatement.NumShares(TEST_STOCK), 0);
        }

        [Test]
        public void IncomeStatementAfterSellingSomeStock()
        {
            incomeStatement.AddStock(TEST_STOCK, 40, 10);
            incomeStatement.SellStock(TEST_STOCK, 2);
            Assert.AreEqual(incomeStatement.NumShares(TEST_STOCK), 8);
        }

        [Test]
        public void IncomeStatementAfterSellingSomeStockDifferentPrices()
        {
            incomeStatement.AddStock(TEST_STOCK, 40, 10);
            incomeStatement.AddStock(TEST_STOCK, 20, 10);
            incomeStatement.SellStock(TEST_STOCK, 11);
            Assert.AreEqual(incomeStatement.NumShares(TEST_STOCK), 9);
        }

    }
}
