using NUnit.Framework;
using UnityEngine;

public class CurrencyTest
{
    readonly Currency currency = ScriptableObject.CreateInstance<Currency>();
    Money firstMoney;

    [Test, Order(1)]
    public void AddMoney_ReturnOnNull()
    {
        var x_count = currency.MoneyCount;
        var res = currency.AddMoney(null);

        Assert.IsFalse(res);
        Assert.AreEqual(x_count, currency.MoneyCount);
    }

    [Test, Order(2)]
    public void AddMoney_AddsOnNew()
    {
        var x_count = currency.MoneyCount + 1;

        // istantiate a new Money object
        firstMoney = ScriptableObject.CreateInstance<Money>();
        firstMoney.Initialize(1);

        var res = currency.AddMoney(firstMoney);

        Assert.IsTrue(res);
        Assert.AreEqual(x_count, currency.MoneyCount);
        Assert.Contains(firstMoney, currency.MoneyPieces);
    }

    [Test, Order(3)]
    public void AddMoney_ReturnOnDuplicate()
    {
        var x_count = currency.MoneyCount;
        var res = currency.AddMoney(firstMoney);

        Assert.IsFalse(res);
        Assert.AreEqual(x_count, currency.MoneyCount);
    }

    [Test, Order(4)]
    public void AddMoney_ReturnOnDuplicateRate()
    {
        var newMoney = ScriptableObject.CreateInstance<Money>();
        newMoney.Initialize(firstMoney.Rate);

        var x_count = currency.MoneyCount;
        var res = currency.AddMoney(newMoney);

        Assert.IsFalse(res);
        Assert.AreEqual(x_count, currency.MoneyCount);
    }

    [Test, Order(5)]
    public void AddMoney_SortedOnNew() 
    {
        var x_count = currency.MoneyCount + 1;

        // istantiate a new Money object
        var secondMoney = ScriptableObject.CreateInstance<Money>();
        secondMoney.Initialize(0.1f);

        var res = currency.AddMoney(secondMoney);

        Assert.IsTrue(res);
        Assert.AreEqual(x_count, currency.MoneyCount);
        Assert.Contains(secondMoney, currency.MoneyPieces);
        Assert.AreEqual(secondMoney, currency.MoneyPieces[0]);
        Assert.AreEqual(firstMoney, currency.MoneyPieces[1]);
    }

    [Test, Order(6)]
    public void RemoveMoney_ReturnOnNull()
    {
        var x_count = currency.MoneyCount;
        var res = currency.RemoveMoney(null);

        Assert.IsFalse(res);
        Assert.AreEqual(x_count, currency.MoneyCount);
    }

    [Test, Order(7)]
    public void RemoveMoney_DoOnPresent()
    {
        var x_count = currency.MoneyCount - 1;
        var res = currency.RemoveMoney(firstMoney);

        Assert.IsTrue(res);
        CollectionAssert.DoesNotContain(currency.MoneyPieces, firstMoney);
        Assert.AreEqual(x_count, currency.MoneyCount);
    }


    [Test, Order(8)]
    public void RemoveMoney_ReturnOnAbsent()
    {
        var x_count = currency.MoneyCount;
        var res = currency.RemoveMoney(firstMoney);

        Assert.IsFalse(res);
        Assert.AreEqual(x_count, currency.MoneyCount);
    }
}
