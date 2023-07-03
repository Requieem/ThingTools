using NUnit.Framework;
using UnityEngine.Events;

public class ContextSatisfierTest : SatisfierTest
{
    ThingSatisfier<Dummy, Dummy> satisfier;

    public override void InitSatisfier()
    {
        satisfier = new ThingSatisfier<Dummy, Dummy>((a, b) => a.CompareTo(b), (Dummy x, Dummy y) => x == y);
    }

    [Test, Order(4)]
    public void Watch_DosntAddNullKeys()
    {
        InitSatisfier();

        var x_count = 0;
        var ret = satisfier.Watch(null, new Dummy(), null, null);

        Assert.That(ret, Is.EqualTo(0));
        Assert.IsTrue(satisfier.Bundles.Count == x_count);
    }

    [Test, Order(5)]
    public void Watch_DosntAddNullValues()
    {
        var x_count = 0;
        var ret = satisfier.Watch(new Dummy(), null, null, null);

        Assert.That(ret, Is.EqualTo(0));
        Assert.IsTrue(satisfier.Bundles.Count == x_count);
    }

    [Test, Order(6)]
    public void Watch_AddsNewKeyAndValue()
    {
        m_Obj = new Dummy();
        var x_count = satisfier.Bundles.Count + 1;
        var ret = satisfier.Watch(m_Obj, m_Obj, null, null);

        Assert.That(ret, Is.EqualTo(0));
        Assert.IsTrue(satisfier.Bundles.Count == x_count);
        Assert.IsTrue(satisfier.Bundles.ContainsKey(m_Obj));

        var bundle = satisfier.Bundles[m_Obj];

        Assert.IsTrue(bundle.ContainsKey(m_Obj));

        AssertBundle(m_Obj, 0, 0, null, null, bundle[m_Obj]);
    }

    /// <remark>
    /// I should probably check for the whole Bundles collection to not be modified
    /// except for what is expected. TODO for later.
    /// This goes for <see cref="ObjectSatisfierTest.Watch_ReplacesEvents"/> too.
    /// </remark>
    [Test, Order(7)]
    public void Watch_ReplacesEvents()
    {
        var k_count = satisfier.Bundles.Count;
        var v_count = satisfier.Bundles[m_Obj].Count;
        var satEvent = new UnityEvent();
        var unsatEvent = new UnityEvent();

        satisfier.Watch(m_Obj, m_Obj, satEvent, unsatEvent);

        Assert.AreEqual(k_count, satisfier.Bundles.Count);
        Assert.IsTrue(satisfier.Bundles.ContainsKey(m_Obj));
        Assert.IsTrue(satisfier.Bundles[m_Obj].Count == v_count);

        var bundle = satisfier.Bundles[m_Obj];

        Assert.IsTrue(bundle.ContainsKey(m_Obj));

        AssertBundle(m_Obj, 0, 0, satEvent, unsatEvent, bundle[m_Obj]);
    }

    [Test, Order(8)]
    public void Watch_AddsNewValueForKey()
    {
        var k_count = satisfier.Bundles.Count;
        var v_count = satisfier.Bundles[m_Obj].Count + 1;
        var newObj = new Dummy();

        satisfier.Watch(m_Obj, newObj, null, null);

        Assert.AreEqual(k_count, satisfier.Bundles.Count);
        Assert.AreEqual(v_count, satisfier.Bundles[m_Obj].Count);
        Assert.IsTrue(satisfier.Bundles.ContainsKey(m_Obj));

        var bundle = satisfier.Bundles[m_Obj];

        Assert.IsTrue(bundle.ContainsKey(newObj));

        AssertBundle(newObj, 0, 0, null, null, bundle[newObj]);
    }

    [Test, Order(9)]
    public void Satisfy_ReturnOnNullKey()
    {
        var ret = satisfier.Satisfy(null, m_Obj);
        Assert.IsFalse(ret);
    }

    [Test, Order(10)]
    public void Satisfy_ReturnOnNullValue()
    {
        var ret = satisfier.Satisfy(m_Obj, null);
        Assert.IsFalse(ret);
    }

    [Test, Order(11)]
    public void Satisfy_DoOnPresent()
    {
        var bundle = satisfier.Bundles[m_Obj][m_Obj];
        var satEvent = bundle.m_DoSatisfy;

        var x_count = bundle.m_SatisfiedCount + 1;
        var satCount = 0;
        satEvent.AddListener(() => satCount++);

        var ret = satisfier.Satisfy(m_Obj, m_Obj);

        Assert.IsTrue(ret);
        Assert.IsTrue(bundle.m_SatisfiedCount == x_count);
        Assert.IsTrue(satCount == 1);
    }

    [Test, Order(12)]
    public void Satisfy_DoOnEqualValues()
    {
        var bundle = satisfier.Bundles[m_Obj][m_Obj];
        var satEvent = bundle.m_DoSatisfy;
        var x_count = bundle.m_SatisfiedCount + 1;
        var satCount = 0;
        satEvent.AddListener(() => satCount++);

        var simObj = new Dummy();
        // According to the == Dummy operation
        // Watch out for other individual comparisons and equators.
        simObj.value = m_Obj.value;

        var ret = satisfier.Satisfy(m_Obj, simObj);

        Assert.IsTrue(ret);
        Assert.IsTrue(bundle.m_SatisfiedCount == x_count);
        Assert.IsTrue(satCount == 1);
    }

    [Test, Order(13)]
    public void Satisfy_ReturnOnAbsent()
    {
        var bundle = satisfier.Bundles[m_Obj][m_Obj];
        var satEvent = bundle.m_DoSatisfy;
        var x_count = bundle.m_SatisfiedCount;
        var satCount = 0;
        satEvent.AddListener(() => satCount++);

        var simObj = new Dummy();
        // According to the != Dummy operation
        // Watch out for other individual comparisons and equators.
        simObj.value = m_Obj.value + 100;

        var ret = satisfier.Satisfy(m_Obj, simObj);

        Assert.IsFalse(ret);
        Assert.IsTrue(bundle.m_SatisfiedCount == x_count);
        Assert.IsTrue(satCount == 0);
    }

    [Test, Order(14)]
    public void Unsatisfy_ReturnOnNullKey()
    {
        var ret = satisfier.Unsatisfy(null, m_Obj);
        Assert.IsFalse(ret);
    }

    [Test, Order(15)]
    public void Unsatisfy_ReturnOnNullValue()
    {
        var ret = satisfier.Unsatisfy(m_Obj, null);
        Assert.IsFalse(ret);
    }

    [Test, Order(16)]
    public void Unsatisfy_DoOnPresent()
    {
        var bundle = satisfier.Bundles[m_Obj][m_Obj];
        var unsatEvent = bundle.m_UnSatisfy;

        var x_count = bundle.m_UnsatisfiedCount + 1;
        var unsatCount = 0;
        unsatEvent.AddListener(() => unsatCount++);

        var ret = satisfier.Unsatisfy(m_Obj, m_Obj);

        Assert.IsTrue(ret);
        Assert.IsTrue(bundle.m_UnsatisfiedCount == x_count);
        Assert.IsTrue(unsatCount == 1);
    }

    [Test, Order(17)]
    public void Unsatisfy_DoOnEqualValues()
    {
        var bundle = satisfier.Bundles[m_Obj][m_Obj];
        var unsatEvent = bundle.m_UnSatisfy;
        var x_count = bundle.m_UnsatisfiedCount + 1;
        var unsatCount = 0;
        unsatEvent.AddListener(() => unsatCount++);

        var simObj = new Dummy();
        // According to the == Dummy operation
        // Watch out for other individual comparisons and equators.
        simObj.value = m_Obj.value;

        var ret = satisfier.Unsatisfy(m_Obj, simObj);

        Assert.IsTrue(ret);
        Assert.IsTrue(bundle.m_UnsatisfiedCount == x_count);
        Assert.IsTrue(unsatCount == 1);
    }

    [Test, Order(18)]
    public void Unatisfy_ReturnOnAbsent()
    {
        var bundle = satisfier.Bundles[m_Obj][m_Obj];
        var unsatEvent = bundle.m_UnSatisfy;
        var x_count = bundle.m_UnsatisfiedCount;
        var unsatCount = 0;
        unsatEvent.AddListener(() => unsatCount++);

        var simObj = new Dummy();
        // According to the != Dummy operation
        // Watch out for other individual comparisons and equators.
        simObj.value = m_Obj.value + 100;

        var ret = satisfier.Unsatisfy(m_Obj, simObj);

        Assert.IsFalse(ret);
        Assert.IsTrue(bundle.m_UnsatisfiedCount == x_count);
        Assert.IsTrue(unsatCount == 0);
    }

    [Test, Order(19)]
    public void Unwatch_ReturnOnNullKey()
    {
        var x_count = satisfier.Bundles.Count;
        var ret = satisfier.Unwatch(null, m_Obj);

        Assert.IsFalse(ret);
        Assert.IsTrue(satisfier.Bundles.Count == x_count);
    }

    [Test, Order(20)]
    public void Unwatch_ReturnOnNullValue()
    {
        var x_count = satisfier.Bundles.Count;
        var ret = satisfier.Unwatch(m_Obj, null);

        Assert.IsFalse(ret);
        Assert.IsTrue(satisfier.Bundles.Count == x_count);
    }

    [Test, Order(21)]
    public void Unwatch_RemovesOnPresent()
    {
        var k_count = satisfier.Bundles.Count;
        var v_count = satisfier.Bundles[m_Obj].Count - 1;
        var ret = satisfier.Unwatch(m_Obj, m_Obj);

        Assert.IsTrue(ret);
        Assert.IsTrue(satisfier.Bundles.Count == k_count);
        Assert.IsTrue(satisfier.Bundles.ContainsKey(m_Obj));
        Assert.IsFalse(satisfier.Bundles[m_Obj].ContainsKey(m_Obj));
        Assert.IsTrue(satisfier.Bundles[m_Obj].Count == v_count);
    }

    [Test, Order(22)]
    public void Unwatch_ReturnsOnAbsent()
    {
        var k_count = satisfier.Bundles.Count;
        var v_count = satisfier.Bundles[m_Obj].Count;
        var ret = satisfier.Unwatch(m_Obj, m_Obj);

        Assert.IsFalse(ret);
        Assert.IsTrue(satisfier.Bundles.Count == k_count);
        Assert.IsTrue(satisfier.Bundles.ContainsKey(m_Obj));
        Assert.IsTrue(satisfier.Bundles[m_Obj].Count == v_count);
        Assert.IsFalse(satisfier.Bundles[m_Obj].ContainsKey(m_Obj));
    }
}
