using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatBlockTest
{
    Statistics statistics_0;
    Statistics statistics_1;

    readonly StatName firstStat = ScriptableObject.CreateInstance<StatName>();
    readonly StatName secondStat = ScriptableObject.CreateInstance<StatName>();

    ShireBlock statBlock_0;
    ShireBlock statBlock_1;

    public void InitializeStatBlocks()
    {
        statBlock_0 = new ShireBlock();
        statBlock_1 = new ShireBlock();
        
        var bundles = new List<Statistics.ThingEntry>() { new Statistics.ThingEntry(firstStat, statBlock_0), new Statistics.ThingEntry(secondStat, statBlock_1) };

        statistics_0 = new Statistics(bundles);
        statistics_1 = new Statistics(bundles);
    }

    [Test, Order(1)]
    public void ChangeStat_ChangesOnPresent()
    {
        InitializeStatBlocks();

        var x_value = statBlock_0.Value + 5;

        var res = statistics_0.ChangeStat(firstStat, 5);

        Assert.AreEqual(x_value, statBlock_0.Value);
        Assert.AreEqual(res, statBlock_0.Value);
    }

    [Test, Order(2)]
    public void ChangeStat_DoesntOnAbsent()
    {
        InitializeStatBlocks();

        var x_value = statBlock_0.Value;

        var res = statistics_0.ChangeStat(ScriptableObject.CreateInstance<StatName>(), 5);

        Assert.AreEqual(x_value, statBlock_0.Value);
        Assert.AreEqual(-1, res);
    }

    [Test, Order(3)]
    public void AffectStat_ChangesOnPresent()
    {
        InitializeStatBlocks();

        var x_value = statBlock_0.Temp + 5;

        var res = statistics_0.AffectStat(firstStat, 5);

        Assert.AreEqual(x_value, statBlock_0.Temp);
        Assert.AreEqual(res, statBlock_0.Temp);
    }

    [Test, Order(4)]
    public void AffectStat_DoesntOnAbsent()
    {
        InitializeStatBlocks();

        var x_value = statBlock_0.Temp;

        var res = statistics_0.AffectStat(ScriptableObject.CreateInstance<StatName>(), 5);

        Assert.AreEqual(x_value, statBlock_0.Temp);
        Assert.AreEqual(-1, res);
    }

    [Test, Order(5)]
    public void ModStat_ChangesOnPresent()
    {
        InitializeStatBlocks();

        var x_value = statBlock_0.Mod + 5;

        var res = statistics_0.ModStat(firstStat, 5);

        Assert.AreEqual(x_value, statBlock_0.Mod);
        Assert.AreEqual(res, statBlock_0.Mod);
    }

    [Test, Order(6)]
    public void ModStat_DoesntOnAbsent()
    {
        InitializeStatBlocks();

        var x_value = statBlock_0.Mod;

        var res = statistics_0.ModStat(ScriptableObject.CreateInstance<StatName>(), 5);

        Assert.AreEqual(x_value, statBlock_0.Mod);
        Assert.AreEqual(-1, res);
    }

    [Test, Order(7)]
    public void ModTempStat_ChangesOnPresent()
    {
        InitializeStatBlocks();

        var x_value = statBlock_0.ModTemp + 5;

        var res = statistics_0.ModTempStat(firstStat, 5);

        Assert.AreEqual(x_value, statBlock_0.ModTemp);
        Assert.AreEqual(res, statBlock_0.ModTemp);
    }

    [Test, Order(8)]
    public void ModTempStat_DoesntOnAbsent()
    {
        InitializeStatBlocks();

        var x_value = statBlock_0.ModTemp;

        var res = statistics_0.ModTempStat(ScriptableObject.CreateInstance<StatName>(), 5);

        Assert.AreEqual(x_value, statBlock_0.ModTemp);
        Assert.AreEqual(-1, res);
    }

    [Test, Order(9)]
    public void WatchFeedback_NegativeOnAbsent()
    {
        var thirdStat = ScriptableObject.CreateInstance<StatName>();
        var thirdBundle = new ShireBlock();
        var satEvent = new UnityEvent();
        var unsatEvent = new UnityEvent();
        var satCount = 0;
        var unsatCount = 0;

        satEvent.AddListener(() => satCount++);
        unsatEvent.AddListener(() => unsatCount++);

        var res = statistics_0.Watch(thirdStat, thirdBundle, satEvent, unsatEvent);

        Assert.AreEqual(-1, res);
        Assert.AreEqual(0, satCount);
        Assert.AreEqual(0, unsatCount);
    }

    [Test, Order(10)]
    public void WatchFeedback_ZeroOnEqual()
    {
        var thirdBundle = new ShireBlock();
        thirdBundle.Copy(statBlock_0);

        var satEvent = new UnityEvent();
        var unsatEvent = new UnityEvent();
        var satCount = 0;
        var unsatCount = 0;


        satEvent.AddListener(() => satCount++);
        unsatEvent.AddListener(() => unsatCount++);

        var res = statistics_0.Watch(firstStat, thirdBundle, satEvent, unsatEvent);

        Assert.AreEqual(0, res);
        Assert.AreEqual(0, satCount);
        Assert.AreEqual(0, unsatCount);
    }

    [Test, Order(11)]
    public void WatchFeedback_OneOnMore()
    {
        var thirdBundle = new ShireBlock();
        thirdBundle.Copy(statBlock_0);
        var satEvent = new UnityEvent();
        var unsatEvent = new UnityEvent();
        var satCount = 0;
        var unsatCount = 0;

        thirdBundle.Value -= 5;

        satEvent.AddListener(() => satCount++);
        unsatEvent.AddListener(() => unsatCount++);

        var res = statistics_0.Watch(firstStat, thirdBundle, satEvent, unsatEvent);

        Assert.AreEqual(1, res);
        Assert.AreEqual(0, satCount);
        Assert.AreEqual(0, unsatCount);
    }

    [Test, Order(12)]
    public void WatchFeedback_NegativeOnNotEnough()
    {
        var thirdBundle = new ShireBlock();
        thirdBundle.Copy(statBlock_0);
        var satEvent = new UnityEvent();
        var unsatEvent = new UnityEvent();
        var satCount = 0;
        var unsatCount = 0;

        thirdBundle.Value += 50;

        satEvent.AddListener(() => satCount++);
        unsatEvent.AddListener(() => unsatCount++);

        var res = statistics_0.Watch(ScriptableObject.CreateInstance<StatName>(), thirdBundle, satEvent, unsatEvent);

        Assert.AreEqual(-1, res);
        Assert.AreEqual(0, satCount);
        Assert.AreEqual(0, unsatCount);
    }

    [Test, Order(13)]
    public void CheckSatisfyOnChange_SatisfiesWatched()
    {
        var satEvent = new UnityEvent();
        var satCount = 0;
        satEvent.AddListener(() => satCount++);

        statistics_0.Watch(firstStat, statBlock_0, satEvent, null);

        var res = statistics_0.CheckSatisfyOnChange(firstStat, statBlock_0);

        Assert.IsTrue(res);
        Assert.AreEqual(1, satCount);
    }

    [Test, Order(14)]
    public void CheckSatisfyOnChange_AddsBundleOnNotEnough()
    {
        var thirdStat = ScriptableObject.CreateInstance<StatName>();
        var thirdBundle = new ShireBlock();
        thirdBundle.Copy(statBlock_0);

        Assert.False(statistics_0.Satisfier.Bundles.ContainsKey(thirdStat));

        var res = statistics_0.CheckSatisfyOnChange(thirdStat, thirdBundle);

        Assert.True(statistics_0.Satisfier.Bundles.ContainsKey(thirdStat));

        Assert.IsFalse(res);
        Assert.AreEqual(1, statistics_0.Satisfier.Bundles[thirdStat][thirdBundle].m_SatisfiedCount);
    }

    [Test, Order(15)]
    public void CheckUnsatisfyOnChange_UnsatisfiesWatched()
    {
        var unsatEvent = new UnityEvent();
        var unsatCount = 0;
        unsatEvent.AddListener(() => unsatCount++);

        statistics_0.Watch(firstStat, statBlock_0, null, unsatEvent);

        var res = statistics_0.CheckUnsatisfyOnChange(firstStat, statBlock_0);

        Assert.IsTrue(res);
        Assert.AreEqual(1, unsatCount);
    }

    [Test, Order(16)]
    public void CheckUnsatisfyOnChange_DoesntOnAbsent()
    {
        var thirdStat = ScriptableObject.CreateInstance<StatName>();
        var thirdBundle = new ShireBlock();
        thirdBundle.Copy(statBlock_0);

        Assert.False(statistics_0.Satisfier.Bundles.ContainsKey(thirdStat));

        var res = statistics_0.CheckUnsatisfyOnChange(thirdStat, thirdBundle);

        Assert.False(statistics_0.Satisfier.Bundles.ContainsKey(thirdStat));

        Assert.IsFalse(res);
    }
}
