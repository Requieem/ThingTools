using System;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// An base class for all Satisfiers that exist in the game.
/// </summary>
public class ObjectSatisfier<T> : Satisfier<T>
{
    #region Instance Fields

    protected Dictionary<T, SatisfierBundle> m_Bundles;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the ObjectSatisfier class.
    /// </summary>
    /// <param name="comparer">The comparison function used to compare objects of type T.</param>
    /// <param name="equalityComparer">The equality comparison function used to determine equality between objects of type T.</param>
    public ObjectSatisfier(Comparison<T> comparer, Func<T, T, bool> equalityComparer) : base(comparer, equalityComparer)
    {
        m_Bundles = new Dictionary<T, SatisfierBundle>();
    }

    #endregion

    #region Instance Properties

    /// <summary>
    /// Gets the dictionary of bundles.
    /// </summary>
    public Dictionary<T, SatisfierBundle> Bundles => m_Bundles;

    #endregion

    #region Methods

    /// <summary>
    /// Adds the given object to the list of watched objects and associates it with the specified events.
    /// </summary>
    /// <param name="objectToWatch">The object to watch.</param>
    /// <param name="doSatisfy">The event to trigger when the object is acquired.</param>
    /// <param name="unSatisfy">The event to trigger when the object is removed.</param>
    /// <returns>The number of times the object's requirement has already been satisfied.</returns>
    public virtual int Watch(T objectToWatch, UnityEvent doSatisfy, UnityEvent unSatisfy)
    {
        return base.Watch(objectToWatch, doSatisfy, unSatisfy, m_Bundles);
    }

    /// <summary>
    /// Removes the given object from the list of watched objects.
    /// </summary>
    /// <param name="obj">The object to unwatch.</param>
    /// <returns>True if the object was successfully unwatched; otherwise, false.</returns>
    public virtual bool Unwatch(T obj)
    {
        return base.Unwatch(obj, m_Bundles);
    }

    /// <summary>
    /// Satisfies the given object.
    /// </summary>
    /// <param name="obj">The object to satisfy.</param>
    /// <returns>True if the object was successfully satisfied; otherwise, false.</returns>
    public virtual bool Satisfy(T obj)
    {
        return base.SatisfyBundle(obj, m_Bundles);
    }

    /// <summary>
    /// Unsatisfies the given object.
    /// </summary>
    /// <param name="obj">The object to unsatisfy.</param>
    /// <returns>True if the object was successfully unsatisfied; otherwise, false.</returns>
    public virtual bool Unsatisfy(T obj)
    {
        return base.UnsatisfyBundle(obj, m_Bundles);
    }

    /// <summary>
    /// Determines whether the given object is being watched.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns>True if the object is being watched; otherwise, false.</returns>
    public virtual bool IsWatching(T obj)
    {
        return base.IsObjectBeingWatched(obj, m_Bundles);
    }

    /// <summary>
    /// Gets an enumerator to iterate through the list of bundles.
    /// </summary>
    /// <returns>An enumerator to iterate through the list of bundles.</returns>
    public virtual IEnumerator<SatisfierBundle> GetEnumerator()
    {
        return base.GetEnumerator(m_Bundles);
    }

    /// <summary>
    /// Gets a reverse enumerator to iterate through the list of bundles in reverse order.
    /// </summary>
    /// <returns>A reverse enumerator to iterate through the list of bundles.</returns>
    public virtual IEnumerator<SatisfierBundle> GetReverseEnumerator()
    {
        return base.GetReverseEnumerator(m_Bundles);
    }

    /// <summary>
    /// Clears all the bundles from the satisfier.
    /// </summary>
    public virtual void ClearBundles()
    {
        base.ClearBundles(m_Bundles);
    }

    /// <summary>
    /// Adds a bundle to the list of bundles.
    /// </summary>
    /// <param name="bundle">The bundle to add.</param>
    public virtual void AddBundle(SatisfierBundle bundle)
    {
        base.IncludeBundleInWatchlist(bundle, m_Bundles);
    }

    /// <summary>
    /// Removes a bundle from the list of bundles.
    /// </summary>
    /// <param name="bundle">The bundle to remove.</param>
    public virtual void RemoveBundle(SatisfierBundle bundle)
    {
        base.ExcludeBundleFromWatchlist(bundle, m_Bundles);
    }

    #endregion
}

