using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class InventoryTest
{

    readonly Numismatic firstNumismatic = Numismatic.UNIT;
    readonly Numismatic secondNumismatic = Numismatic.UNIT;

    readonly Item firstObject = new Item();
    readonly Item secondObject = new Item();

    Inventory firstInventory;
    Inventory secondInventory;

    public void InitializeInventories()
    {
        var objects = new List<Item>(2) { firstObject, secondObject };

        firstNumismatic.Value += 100;

        firstInventory = new Inventory(objects, firstNumismatic);
        secondInventory = new Inventory(objects, secondNumismatic);
    }

    [Test, Order(1)]
    public void Sell_DontOnAnyNull()
    {
        InitializeInventories();

        var x_money = firstNumismatic.Value;
        var res1 = firstInventory.Sell(null, firstNumismatic);
        var res2 = firstInventory.Sell(firstObject, null);

        Assert.IsFalse(res1);
        Assert.IsFalse(res2);

        CollectionAssert.Contains(firstInventory.Elements, firstObject);
        Assert.AreEqual(x_money, firstNumismatic.Value);
    }

    [Test, Order(2)]
    public void Sell_DoOnObjectPresent()
    {
        Assert.Contains(firstObject, firstInventory.Elements);

        var x_money = firstNumismatic.Value + firstNumismatic.Value;
        var res = firstInventory.Sell(firstObject, firstNumismatic);

        Assert.IsTrue(res);
        CollectionAssert.DoesNotContain(firstInventory.Elements, firstObject);
        Assert.IsTrue(firstNumismatic.Value == x_money || Math.Abs(firstNumismatic.Value - x_money) <= Numismatic.EPSILON);
    }

    [Test, Order(3)]
    public void Sell_DoNotOnObjectAbsent()
    {
        CollectionAssert.DoesNotContain(firstInventory.Elements, firstObject);

        var x_money = firstNumismatic.Value;
        var res = firstInventory.Sell(firstObject, firstNumismatic);

        Assert.IsFalse(res);
        CollectionAssert.DoesNotContain(firstInventory.Elements, firstObject);
        Assert.IsTrue(firstNumismatic.Value == x_money);
    }

    [Test, Order(4)]
    public void Buy_DontOnAnyNull()
    {
        var x_money = firstNumismatic.Value;
        var y_money = secondNumismatic.Value;
        var res1 = firstInventory.Buy(null);

        Assert.IsFalse(res1);

        CollectionAssert.DoesNotContain(firstInventory.Elements, firstObject);
        Assert.AreEqual(x_money, firstNumismatic.Value);
        Assert.AreEqual(y_money, secondNumismatic.Value);
    }

    [Test, Order(5)]
    public void WatchFeedback_ZeroOnAbsent()
    {
        var thirdObject = new Item();
        var satEvent = new UnityEvent();
        var unsatEvent = new UnityEvent();
        var satCount = 0;
        var unsatCount = 0;

        satEvent.AddListener(() => satCount++);
        unsatEvent.AddListener(() => unsatCount++);

        var res = firstInventory.Watch(thirdObject, satEvent, unsatEvent);

        Assert.AreEqual(0, res);
        Assert.AreEqual(0, satCount);
        Assert.AreEqual(0, unsatCount);
    }

    [Test, Order(6)]
    public void WatchFeedback_OneOnPresent()
    {
        var res = firstInventory.Watch(secondObject, null, null);
        Assert.AreEqual(1, res);
    }

    [Test, Order(7)]
    public void WatchFeedback_NOnMultiples()
    {
        var i = 0;
        for(i = 0; i < Random.Range(1f, 100f); i++)
            firstInventory.Add(secondObject);

        var res = firstInventory.Watch(secondObject, null, null);

        // was object was already present at the beginning of the test
        Assert.AreEqual(i + 1, res);
    }

    [Test, Order(8)]
    public void CheckSatisfyOnChange_SatisfiesWatched()
    {
        var satEvent = new UnityEvent();
        var satCount = 0;
        satEvent.AddListener(() => satCount++);

        firstInventory.Watch(secondObject, satEvent, null);

        var res = firstInventory.CheckSatisfyOnChange(secondObject);

        Assert.IsTrue(res);
        Assert.AreEqual(1, satCount);
    }

    [Test, Order(9)]
    public void CheckSatisfyOnChange_AddsBundleOnAbsent()
    {
        var thirdObject = new Item();
        Assert.False(firstInventory.Satisfier.Bundles.ContainsKey(thirdObject));

        var res = firstInventory.CheckSatisfyOnChange(thirdObject);

        Assert.True(firstInventory.Satisfier.Bundles.ContainsKey(thirdObject));

        Assert.IsFalse(res);
        Assert.AreEqual(1, firstInventory.Satisfier.Bundles[thirdObject].m_SatisfiedCount);
    }

    [Test, Order(10)]
    public void CheckUnsatisfyOnChange_UnsatisfiesWatched()
    {
        var unsatEvent = new UnityEvent();
        var unsatCount = 0;
        unsatEvent.AddListener(() => unsatCount++);

        firstInventory.Watch(secondObject, null, unsatEvent);

        var res = firstInventory.CheckUnsatisfyOnChange(secondObject);

        Assert.IsTrue(res);
        Assert.AreEqual(1, unsatCount);
    }

    [Test, Order(11)]
    public void CheckUnsatisfyOnChange_DoesntOnAbsent()
    {
        var thirdObject = new Item();
        Assert.False(firstInventory.Satisfier.Bundles.ContainsKey(thirdObject));

        var res = firstInventory.CheckUnsatisfyOnChange(thirdObject);

        Assert.False(firstInventory.Satisfier.Bundles.ContainsKey(thirdObject));

        Assert.IsFalse(res);
    }
}
