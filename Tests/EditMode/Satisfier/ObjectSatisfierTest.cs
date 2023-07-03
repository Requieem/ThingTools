using NUnit.Framework;
using UnityEngine.Events;

public class ObjectSatisfierTest : SatisfierTest
{
    ObjectSatisfier<Dummy> satisfier;

    public override void InitSatisfier()
    {
        satisfier = new ObjectSatisfier<Dummy>((a, b) => a.CompareTo(b), (Dummy x, Dummy y) => x == y);
    }

    [Test, Order(4)]
    public void Watch_DosntAddNulls()
    {
        InitSatisfier();

        var x_count = 0;
        var ret = satisfier.Watch(null, null, null);

        Assert.That(ret, Is.EqualTo(0));
        Assert.IsTrue(satisfier.Bundles.Count == x_count);
    }

    [Test, Order(5)]
    public void Watch_AddsNewKey()
    {
        m_Obj = new Dummy();
        var x_count = satisfier.Bundles.Count + 1;
        var ret = satisfier.Watch(m_Obj, null, null);

        Assert.That(ret, Is.EqualTo(0));
        Assert.IsTrue(satisfier.Bundles.Count == x_count);
        Assert.IsTrue(satisfier.Bundles.ContainsKey(m_Obj));

        var bundle = satisfier.Bundles[m_Obj];

        AssertBundle(m_Obj, 0, 0, null, null, bundle);
    }

    [Test, Order(6)]
    public void Watch_ReplacesEvents()
    {
        var x_count = satisfier.Bundles.Count;
        var satEvent = new UnityEvent();
        var unsatEvent = new UnityEvent();

        satisfier.Watch(m_Obj, satEvent, unsatEvent);

        Assert.AreEqual(x_count, satisfier.Bundles.Count);
        Assert.IsTrue(satisfier.Bundles.ContainsKey(m_Obj));

        var bundle = satisfier.Bundles[m_Obj];

        AssertBundle(m_Obj, 0, 0, satEvent, unsatEvent, bundle);
    }

    [Test, Order(7)]
    public void Satisfy_ReturnOnNull()
    {
        var ret = satisfier.Satisfy(null);
        Assert.IsFalse(ret);
    }

    [Test, Order(8)]
    public void Satisfy_DoOnPresent()
    {
        var satEvent = satisfier.Bundles[m_Obj].m_DoSatisfy;

        var x_count = satisfier.Bundles[m_Obj].m_SatisfiedCount + 1;
        var satCount = 0;
        satEvent.AddListener(() => satCount++);

        var ret = satisfier.Satisfy(m_Obj);

        Assert.IsTrue(ret);
        Assert.IsTrue(satisfier.Bundles[m_Obj].m_SatisfiedCount == x_count);
        Assert.IsTrue(satCount == 1);
    }

    [Test, Order(9)]
    public void Satisfy_DoOnEqualValues()
    {
        var satEvent = satisfier.Bundles[m_Obj].m_DoSatisfy;
        var x_count = satisfier.Bundles[m_Obj].m_SatisfiedCount + 1;
        var satCount = 0;
        satEvent.AddListener(() => satCount++);

        var simObj = new Dummy();
        // According to the == Dummy operation
        // Watch out for other individual comparisons and equators.
        simObj.value = m_Obj.value;

        var ret = satisfier.Satisfy(simObj);

        Assert.IsTrue(ret);
        Assert.IsTrue(satisfier.Bundles[m_Obj].m_SatisfiedCount == x_count);
        Assert.IsTrue(satCount == 1);
    }

    [Test, Order(10)]
    public void Satisfy_ReturnOnAbsent()
    {
        var satEvent = satisfier.Bundles[m_Obj].m_DoSatisfy;
        var x_count = satisfier.Bundles[m_Obj].m_SatisfiedCount;
        var satCount = 0;
        satEvent.AddListener(() => satCount++);

        var simObj = new Dummy();
        // According to the != Dummy operation
        // Watch out for other individual comparisons and equators.
        simObj.value = m_Obj.value + 100;

        var ret = satisfier.Satisfy(simObj);

        Assert.IsFalse(ret);
        Assert.IsTrue(satisfier.Bundles[m_Obj].m_SatisfiedCount == x_count);
        Assert.IsTrue(satCount == 0);
    }

    [Test, Order(11)]
    public void Unsatisfy_ReturnOnNull()
    {
        var ret = satisfier.Unsatisfy(null);
        Assert.IsFalse(ret);
    }

    [Test, Order(12)]
    public void Unsatisfy_DoOnPresent()
    {
        var unsatEvent = satisfier.Bundles[m_Obj].m_UnSatisfy;
        var x_count = satisfier.Bundles[m_Obj].m_UnsatisfiedCount + 1;
        var unsatCount = 0;
        unsatEvent.AddListener(() => unsatCount++);

        var ret = satisfier.Unsatisfy(m_Obj);

        Assert.IsTrue(ret);
        Assert.IsTrue(satisfier.Bundles[m_Obj].m_UnsatisfiedCount == x_count);
        Assert.IsTrue(unsatCount == 1);
    }

    [Test, Order(13)]
    public void Unsatisfy_DoOnEqualValues()
    {
        var unsatEvent = satisfier.Bundles[m_Obj].m_UnSatisfy;
        var x_count = satisfier.Bundles[m_Obj].m_UnsatisfiedCount + 1;
        var unsatCount = 0;
        unsatEvent.AddListener(() => unsatCount++);

        var simObj = new Dummy();
        // According to the == Dummy operation
        // Watch out for other individual comparisons and equators.
        simObj.value = m_Obj.value;

        var ret = satisfier.Unsatisfy(simObj);

        Assert.IsTrue(ret);
        Assert.IsTrue(satisfier.Bundles[m_Obj].m_UnsatisfiedCount == x_count);
        Assert.IsTrue(unsatCount == 1);
    }

    [Test, Order(14)]
    public void Unatisfy_ReturnOnAbsent()
    {
        var unsatEvent = satisfier.Bundles[m_Obj].m_UnSatisfy;
        var x_count = satisfier.Bundles[m_Obj].m_UnsatisfiedCount;
        var unsatCount = 0;
        unsatEvent.AddListener(() => unsatCount++);

        var simObj = new Dummy();
        // According to the != Dummy operation
        // Watch out for other individual comparisons and equators.
        simObj.value = m_Obj.value + 100;

        var ret = satisfier.Unsatisfy(simObj);

        Assert.IsFalse(ret);
        Assert.IsTrue(satisfier.Bundles[m_Obj].m_UnsatisfiedCount == x_count);
        Assert.IsTrue(unsatCount == 0);
    }

    [Test, Order(15)]
    public void Unwatch_ReturnOnNull()
    {
        var x_count = satisfier.Bundles.Count;
        var ret = satisfier.Unwatch(null);

        Assert.IsFalse(ret);
        Assert.IsTrue(satisfier.Bundles.Count == x_count);
    }

    [Test, Order(16)]
    public void Unwatch_RemovesOnPresent()
    {
        var x_count = satisfier.Bundles.Count - 1;
        var ret = satisfier.Unwatch(m_Obj);

        Assert.IsTrue(ret);
        Assert.IsTrue(satisfier.Bundles.Count == x_count);
        Assert.IsFalse(satisfier.Bundles.ContainsKey(m_Obj));
    }

    [Test, Order(17)]
    public void Unwatch_ReturnsOnAbsent()
    {
        var x_count = satisfier.Bundles.Count;
        var ret = satisfier.Unwatch(m_Obj);

        Assert.IsFalse(ret);
        Assert.IsTrue(satisfier.Bundles.Count == x_count);
        Assert.IsFalse(satisfier.Bundles.ContainsKey(m_Obj));
    }
}
