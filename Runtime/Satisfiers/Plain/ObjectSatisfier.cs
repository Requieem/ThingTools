using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An base class for all Satisfiers that exist in the game.
/// </summary>
public class ObjectSatisfier<T> : Satisfier<T>
{
    #region Instance Fields:

    protected Dictionary<T, SatisfierBundle> m_Bundles;

    #endregion

    #region Constructors:

    /// <summary>
    /// Constructor for the Satisfier class.
    /// </summary>

    public ObjectSatisfier(Comparison<T> comparer, Func<T, T, bool> equalityComparer) : base(comparer, equalityComparer)
    {
        m_Bundles = new Dictionary<T, SatisfierBundle>();
    }

    #endregion

    #region Instance Properties:
    
    public Dictionary<T, SatisfierBundle> Bundles
    {
        get { return m_Bundles; }
    }

    #endregion

    #region Methods:

    /// <summary>
    /// Add the given object to the list of watched objects.
    /// also add the given events to the list of events to be triggered when the object is acquired or removed.
    /// </summary>
    public virtual int Watch(T objectToWatch, UnityEvent doSatisfy, UnityEvent unSatisfy)
    {
        return base.Watch(objectToWatch, doSatisfy, unSatisfy, m_Bundles);
    }

    /// <summary>
    /// Remove the given object from the list of watched objects.
    /// </summary>
    public virtual bool Unwatch(T obj)
    {
        return base.Unwatch(obj, m_Bundles);
    }

    /// <summary>
    /// Satisties the given object.
    /// </summary>
    public virtual bool Satisfy(T obj)
    {
        return base.Satisfy(obj, m_Bundles);
    }

    /// <summary>
    /// Unsatisfies the given object.
    /// </summary>
    public virtual bool Unsatisfy(T obj)
    {
        return base.Unsatisfy(obj, m_Bundles);
    }

    /// <summary>
    /// Returns true if the given object is being watched.
    /// </summary>
    public virtual bool IsWatching(T obj)
    {
        return base.IsWatching(obj, m_Bundles);
    }

    /// <summary>
    /// A function to iterate through the list of bundles
    /// </summary>
    public virtual IEnumerator<SatisfierBundle> GetEnumerator()
    {
        return base.GetEnumerator(m_Bundles);
    }

    /// <summary>
    /// A function to iterate through the list of bundles the opposite way
    /// </summary>
    public virtual IEnumerator<SatisfierBundle> GetReverseEnumerator()
    {
        return base.GetReverseEnumerator(m_Bundles);
    }

    /// <summary>
    /// A function to clear the list of bundles
    /// </summary>
    public virtual void ClearBundles()
    {
        base.ClearBundles(m_Bundles);
    }

    /// <summary>
    /// A function to add an object to the list of bundles
    /// </summary>
    public virtual void AddBundle(SatisfierBundle bundle)
    {
        base.AddBundle(bundle, m_Bundles);
    }

    /// <summary>
    /// A function to remove an object from the list of bundles
    /// </summary>
    public virtual void RemoveBundle(SatisfierBundle bundle)
    {
        base.RemoveBundle(bundle, m_Bundles);
    }

    #endregion
}

