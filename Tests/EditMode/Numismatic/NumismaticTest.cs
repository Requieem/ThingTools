using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class NumismaticTest
{
    Numismatic firstNumismatic;
    Numismatic secondNumismatic;

    readonly Currency firstCurrency = ScriptableObject.CreateInstance<Currency>();
    readonly Currency secondCurrency = ScriptableObject.CreateInstance<Currency>();

    public void InitializeNumismatics()
    {
        var currencies = new List<Currency>(2) { firstCurrency, secondCurrency };
        for (int i = 0; i < 2; i++)
        {
            var randomRate = Random.Range(0.1f, 10f);
            currencies[i].Initialize(randomRate);
            var randomMoneyCount = Random.Range(2, 11);
            for (int j = 0; j < randomMoneyCount; j++)
            {
                randomRate = Random.Range(0.1f, 10f);
                var moneyPiece = ScriptableObject.CreateInstance<Money>();

                moneyPiece.Initialize(randomRate);
                currencies[i].AddMoney(moneyPiece);
            }
        }

        firstNumismatic = new Numismatic(firstCurrency);
        secondNumismatic = new Numismatic(secondCurrency);
    }

    [Test, Order(1)]
    public void Convert_Correctness()
    {
        InitializeNumismatics();

        var originalValue = Random.Range(1f, 1000f);

        var converted = firstNumismatic.Convert(originalValue);
        var quantities = converted.Item1;

        Assert.AreEqual(quantities.Count, firstNumismatic.MoneyCount);

        var value = originalValue;
        value /= firstNumismatic.FixedRate;

        // This is mimicking method behaviour, is there a better way to do it?
        for (int i = quantities.Count - 1; i > 0; i--)
        {
            var pieceCount = Mathf.FloorToInt(value / firstNumismatic.MoneyPieces[i].Rate);
            value -= pieceCount * firstNumismatic.MoneyPieces[i].Rate;
            Assert.IsTrue(quantities[i] == pieceCount);
        }

        firstNumismatic.Value = originalValue;

        Debug.Log("Numismatic Value: " + firstNumismatic.Value);
        Debug.Log("Original Value: " + originalValue);

        Assert.IsTrue(Math.Abs(firstNumismatic.Value - originalValue) <= Numismatic.EPSILON);
    }

    [Test, Order(2)]
    public void CanAfford_Correctness()
    {
        secondNumismatic.Value = firstNumismatic.Value;

        Debug.Log("I Numismatic Value: " + firstNumismatic.Value);
        Debug.Log("II Numismatic Value: " + secondNumismatic.Value);
        // right now, both numismatics have equal value. let's assert that.
        /*        Assert.IsTrue(firstNumismatic.Value == secondNumismatic.Value);*/
        // Precision is not guaranteed, so we use an epsilon... this will do for now
        Assert.IsTrue(Math.Abs(firstNumismatic.Value - secondNumismatic.Value) <= Numismatic.EPSILON);

        // given the above, both should be able to afford the other.
        Assert.IsTrue(secondNumismatic.CanAfford(firstNumismatic));
        Assert.IsTrue(secondNumismatic.CanAfford(firstNumismatic));

        firstNumismatic.Value = Random.Range(1f, 100f);
        secondNumismatic.Value = firstNumismatic.Value - 1f;

        // given that the above worked as expected we should now assert the new conditions
        Assert.IsTrue(firstNumismatic.CanAfford(secondNumismatic));
        Assert.IsFalse(secondNumismatic.CanAfford(firstNumismatic));
        Assert.IsTrue(firstNumismatic.CanAfford(firstNumismatic));
        Assert.IsTrue(secondNumismatic.CanAfford(secondNumismatic));
    }

    [Test, Order(3)]
    public void WatchFeedback_PositiveOnAffordable()
    {
        secondNumismatic.Value = firstNumismatic.Value - 1f;

        Assert.IsTrue(firstNumismatic.CanAfford(secondNumismatic));
        Assert.IsTrue(!secondNumismatic.CanAfford(firstNumismatic));
        // I am testing the WatchFeedback method this way because of the implicit
        // dependency of the Watch method on the WatchFeedback method.
        // Moreover, the watch method is not tested because it is not virtual and it only
        // calls the WatchFeedback method.
        // other than activating the satisfier if needed.
        var res = firstNumismatic.Watch(secondNumismatic, null, null);
        Assert.IsTrue(res == 1);
    }

    [Test, Order(4)]
    public void WatchFeedback_NegativeOnUnaffordable()
    {
        secondNumismatic.Value -= 100f;
        var res = secondNumismatic.Watch(firstNumismatic, null, null);
        Assert.IsTrue(res == -1);
    }

    [Test, Order(5)]
    public void CheckSatisfyOnChange_SatisfiesOne()
    {
        var satEvent = new UnityEvent();
        var satCount = 0;
        satEvent.AddListener(() => satCount++);

        var res = secondNumismatic.Watch(firstNumismatic, satEvent, null);
        secondNumismatic.Value = firstNumismatic.Value + 1;

        secondNumismatic.CheckSatisfyOnChange(secondNumismatic);

        Assert.IsTrue(secondNumismatic.CanAfford(firstNumismatic));
        Assert.AreEqual(-1, res);
        Assert.AreEqual(1, satCount);
    }

    [Test, Order(6)]
    public void CheckSatisfyOnChange_SatisfiesMultiple()
    {
        var satEvent = new UnityEvent();
        var satCount = 0;
        satEvent.AddListener(() => satCount++);

        var newNumismatic = new Numismatic(firstNumismatic.Currency);

        newNumismatic.Value = firstNumismatic.Value + 100;
        secondNumismatic.Value = 0;

        var res1 = secondNumismatic.Watch(firstNumismatic, satEvent, null);
        var res2 = secondNumismatic.Watch(newNumismatic, satEvent, null);

        secondNumismatic.Value = newNumismatic.Value + 1;

        secondNumismatic.CheckSatisfyOnChange(secondNumismatic);

        Assert.AreEqual(secondNumismatic.Satisfier.Bundles.Count, 2);
        Assert.IsTrue(secondNumismatic.CanAfford(firstNumismatic));
        Assert.IsTrue(secondNumismatic.CanAfford(newNumismatic));
        Assert.AreEqual(-1, res1);
        Assert.AreEqual(-1, res2);
        Assert.AreEqual(2, satCount);
    }

    [Test, Order(7)]
    public void CheckUnsatisfyOnChange_UnsatisfiesMultiple()
    {
        var unsatEvent = new UnityEvent();
        var unsatCount = 0;
        unsatEvent.AddListener(() => unsatCount++);

        var newNumismatic = new Numismatic(firstNumismatic.Currency);
        newNumismatic.Value = firstNumismatic.Value;

        secondNumismatic.Value = firstNumismatic.Value + 1;

        var res1 = secondNumismatic.Watch(firstNumismatic, null, unsatEvent);
        var res2 = secondNumismatic.Watch(newNumismatic, null, unsatEvent);

        secondNumismatic.Value = firstNumismatic.Value - 1;

        secondNumismatic.CheckUnsatisfyOnChange(secondNumismatic);

        Assert.IsTrue(!secondNumismatic.CanAfford(firstNumismatic));
        Assert.IsTrue(!secondNumismatic.CanAfford(newNumismatic));
        Assert.AreEqual(1, res1);
        Assert.AreEqual(1, res2);
        Assert.AreEqual(2, unsatCount);
    }

    [Test, Order(8)]
    public void CheckUnsatisfyOnChange_UnsatisfiesOne()
    {
        var unsatEvent = new UnityEvent();
        var unsatCount = 0;
        unsatEvent.AddListener(() => unsatCount++);

        var newNumismatic = new Numismatic(firstNumismatic.Currency);
        newNumismatic.Value = firstNumismatic.Value - 2;

        secondNumismatic.Value = firstNumismatic.Value + 1;

        var res2 = secondNumismatic.Watch(newNumismatic, null, unsatEvent);
        var res1 = secondNumismatic.Watch(firstNumismatic, null, unsatEvent);

        secondNumismatic.Value = firstNumismatic.Value - 1;

        secondNumismatic.CheckUnsatisfyOnChange(secondNumismatic);

        Assert.IsTrue(!secondNumismatic.CanAfford(firstNumismatic));
        Assert.IsTrue(secondNumismatic.CanAfford(newNumismatic));
        Assert.AreEqual(1, res1);
        Assert.AreEqual(1, res2);
        Assert.AreEqual(1, unsatCount);
    }

    [Test, Order(9)]
    public void Gain_EqualOnZero()
    {
        var x_value = firstNumismatic.Value;

        firstNumismatic.Gain(0);

        Assert.AreEqual(x_value, firstNumismatic.Value);
    }

    [Test, Order(10)]
    public void Gain_MoreOnPositive()
    {
        var x_value = firstNumismatic.Value + firstNumismatic.Value;

        firstNumismatic.Gain(firstNumismatic.Value);

        Assert.IsTrue(Math.Abs(firstNumismatic.Value - x_value) <= Numismatic.EPSILON);
    }

    [Test, Order(11)]
    public void Gain_LessOnNegative()
    {
        var x_value = firstNumismatic.Value - firstNumismatic.Value;

        firstNumismatic.Gain(-firstNumismatic.Value);

        Assert.IsTrue(Math.Abs(firstNumismatic.Value - x_value) <= Numismatic.EPSILON);
    }

    [Test, Order(12)]
    public void Spend_EqualOnZero()
    {
        var x_value = firstNumismatic.Value;

        firstNumismatic.Spend(0);

        Assert.AreEqual(x_value, firstNumismatic.Value);
    }

    [Test, Order(13)]
    public void Spend_LessOnPositive()
    {
        var x_value = firstNumismatic.Value - firstNumismatic.Value;

        firstNumismatic.Spend(firstNumismatic.Value);

        Assert.IsTrue(Math.Abs(firstNumismatic.Value - x_value) <= Numismatic.EPSILON);
    }

    [Test, Order(14)]
    public void Spend_MoreOnNegative()
    {
        var x_value = firstNumismatic.Value + firstNumismatic.Value;

        firstNumismatic.Gain(-firstNumismatic.Value);

        Assert.IsTrue(Math.Abs(firstNumismatic.Value - x_value) <= Numismatic.EPSILON);
    }

}
