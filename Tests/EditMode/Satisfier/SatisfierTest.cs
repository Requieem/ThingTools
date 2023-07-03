using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public abstract class SatisfierTest
{
    protected Dummy m_Obj;

    public abstract void InitSatisfier();

    public void AssertBundle(Dummy obj, int satCount, int unsatCount, UnityEvent satEvent, UnityEvent unsatEvent, Satisfier<Dummy>.SatisfierBundle bundle)
    {
        Assert.AreEqual(obj, bundle.m_WatchedObject);
        Assert.AreEqual(satCount, bundle.m_SatisfiedCount);
        Assert.AreEqual(unsatCount, bundle.m_UnsatisfiedCount);
        Assert.AreEqual(satEvent, bundle.m_DoSatisfy);
        Assert.AreEqual(unsatEvent, bundle.m_UnSatisfy);
    }

    [Test, Order(1)]
    public void Bundle_Satisfy()
    {
        var satEvent = new UnityEvent();
        var eventCount = 0;
        satEvent.AddListener(() => eventCount++);

        var bundle = new ObjectSatisfier<Dummy>.SatisfierBundle(m_Obj, satEvent, null);
        bundle.Satisfy();

        Assert.IsTrue(eventCount == 1);
        Assert.IsTrue(bundle.m_SatisfiedCount == 1);
        Assert.IsTrue(bundle.m_UnsatisfiedCount == 0);
    }

    [Test, Order(2)]
    public void Bundle_Unsatisfy()
    {
        var unsatEvent = new UnityEvent();
        var eventCount = 0;
        unsatEvent.AddListener(() => eventCount++);

        var bundle = new ObjectSatisfier<Dummy>.SatisfierBundle(m_Obj, null, unsatEvent);
        bundle.Unsatisfy();

        Assert.IsTrue(eventCount == 1);
        Assert.IsTrue(bundle.m_UnsatisfiedCount == 1);
        Assert.IsTrue(bundle.m_SatisfiedCount == 0);
    }

    [Test, Order(3)]
    public void Bundle_SwapEvents()
    {
        var bundle = new ObjectSatisfier<Dummy>.SatisfierBundle(null, null, null);
        var satEvent = new UnityEvent();
        var satCount = 0;
        satEvent.AddListener(() => satCount++);

        var unsatEvent = new UnityEvent();
        var unsatCount = 0;
        unsatEvent.AddListener(() => unsatCount++);

        bundle.ReplaceEvents(satEvent, unsatEvent);

        bundle.Satisfy();
        bundle.Unsatisfy();

        Assert.AreEqual(satEvent, bundle.m_DoSatisfy);
        Assert.AreEqual(unsatEvent, bundle.m_UnSatisfy);
        Assert.IsTrue(satCount == 1);
        Assert.IsTrue(unsatCount == 1);
        Assert.IsTrue(bundle.m_SatisfiedCount == 1);
        Assert.IsTrue(bundle.m_UnsatisfiedCount == 1);
    }
}
