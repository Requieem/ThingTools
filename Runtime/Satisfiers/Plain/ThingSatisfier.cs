using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// Base class for all ThingSatisfiers in the game. ThingSatisfiers are responsible for tracking the satisfaction of various in-game objects.
/// </summary>
/// <typeparam name="K">The type of keys in the dictionary.</typeparam>
/// <typeparam name="V">The type of values in the dictionary.</typeparam>
public class ThingSatisfier<K, V> : Satisfier<V>
{
    #region Instance Fields
    /// <summary>
    /// A dictionary of bundles, organized by keys and values.
    /// </summary>
    protected Dictionary<K, Dictionary<V, SatisfierBundle>> m_Bundles;
    #endregion

    #region Constructors
    /// <summary>
    /// Constructs a new ThingSatisfier.
    /// </summary>
    /// <param name="comparer">A comparison function for values.</param>
    /// <param name="equalityComparer">An equality comparison function for values.</param>
    public ThingSatisfier(Comparison<V> comparer, Func<V, V, bool> equalityComparer) : base(comparer, equalityComparer)
    {
        m_Bundles = new Dictionary<K, Dictionary<V, SatisfierBundle>>();
    }
    #endregion

    #region Instance Properties
    /// <summary>
    /// Returns the dictionary of bundles.
    /// </summary>
    /// <value>The dictionary of bundles.</value>
    public Dictionary<K, Dictionary<V, SatisfierBundle>> Bundles
    {
        get { return m_Bundles; }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Adds the given object and its corresponding events to the list of watched objects.
    /// </summary>
    /// <param name="_key">The key of the object.</param>
    /// <param name="_value">The object to watch.</param>
    /// <param name="doSatisfy">Event to be triggered when the object is satisfied.</param>
    /// <param name="unSatisfy">Event to be triggered when the object is unsatisfied.</param>
    /// <returns>The number of items currently being watched by the Satisfier, after the new item has been added.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either _key or _value is null.</exception>
    /// <remarks>If the key is not being watched, a new dictionary of SatisfierBundles will be added to the main dictionary under this key.</remarks>
    public virtual int Watch(K _key, V _value, UnityEvent doSatisfy, UnityEvent unSatisfy)
    {
        // check for null objects
        if (_key is null || _value is null)
        {
            Log.Wng($"Cannot watch a null object or a null key, check {typeof(K)} and {typeof(V)} parameters for null object references.");
            return 0;
        }

        if (!IsWatching(_key))
        {
            m_Bundles.Add(_key, new Dictionary<V, SatisfierBundle>());
        }

        return base.Watch(_value, doSatisfy, unSatisfy, m_Bundles[_key]);
    }


    /// <summary>
    /// Removes the given object from the list of watched objects.
    /// </summary>
    /// <param name="_key">The key of the object.</param>
    /// <param name="_value">The object to unwatch.</param>
    /// <returns>True if the object was being watched and has now been removed; false otherwise.</returns>
    /// <remarks>This method does nothing if the object is not being watched/// </summary>
    /// <param name="_key">The key of the object.</param>
    /// <param name="_value">The object to unwatch.</param>
    /// <returns>True if the object was being watched and has now been removed; false otherwise.</returns>
    /// <remarks>This method does nothing if the object is not being watched.</remarks>
    public virtual bool Unwatch(K _key, V _value)
    {
        if (IsWatching(_key, _value))
        {
            return base.Unwatch(_value, m_Bundles[_key]);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if the given key is being watched.
    /// </summary>
    /// <param name="_key">The key to check.</param>
    /// <returns>True if the key is being watched; false otherwise.</returns>
    public virtual bool IsWatching(K _key)
    {
        if (_key == null) { return false; }
        return m_Bundles.ContainsKey(_key);
    }

    /// <summary>
    /// Checks if the given object at the specified key is being watched.
    /// </summary>
    /// <param name="_key">The key at which to check the object.</param>
    /// <param name="_value">The object to check.</param>
    /// <returns>True if the object at the specified key is being watched; false otherwise.</returns>
    public virtual bool IsWatching(K _key, V _value)
    {
        if (_value == null || _key == null) { return false; }

        if (IsWatching(_key))
        {
            return m_Bundles[_key].Any(b => m_EqualityComparer(b.Key, _value));
        }

        return false;
    }

    /// <summary>
    /// Adds a bundle with the given key and value to the list of bundles.
    /// </summary>
    /// <param name="_key">The key at which to add the bundle.</param>
    /// <param name="_value">The value of the bundle to add.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when either _key or _value is null.</exception>
    /// <remarks>If the key is not being watched, a new dictionary of SatisfierBundles will be added to the main dictionary under this key. If the object at the specified key is not being watched, a new SatisfierBundle will be added to the dictionary at this key.</remarks>
    public virtual void AddBundle(K _key, V _value)
    {
        if (_key == null || _value == null)
        {
            throw new ArgumentNullException("Cannot add a null object nor a null key, check " + typeof(V) + " requirements for null object references.");
        }

        if (!IsWatching(_key))
        {
            m_Bundles.Add(_key, new Dictionary<V, SatisfierBundle>());
        }

        if (!IsWatching(_key, _value))
        {
            m_Bundles[_key].Add(_value, CreateNullEventBundle(_value, 1, 0));
        }
    }

    /// <summary>
    /// Removes a bundle with the given key and value from the list of bundles.
    /// </summary>
    /// <param name="_key">The key at which to remove the bundle.</param>
    /// <param name="_value">The value of the bundle to remove.</param>
    /// <remarks>This method does nothing if the object at the specified key is not being watched.</remarks>
    public virtual void RemoveBundle(K _key, V _value)
    {
        if (IsWatching(_key, _value))
        {
            m_Bundles[_key].Remove(_value);
        }
    }

    /// <summary>
    /// Satisfies the object at the specified key and value.
    /// </summary>
    /// <param name="_key">The key of the object to satisfy.</param>
    /// <param name="_value">The object to satisfy.</param>
    /// <returns>True if the object was being watched and has been satisfied; false otherwise.</returns>
    public virtual bool Satisfy(K _key, V _value)
    {
        if (IsWatching(_key, _value))
        {
            return base.SatisfyBundle(_value, m_Bundles[_key]);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Unsatisfies the object at the specified key and value.
    /// </summary>
    /// <param name="_key">The key of the object to unsatisfy.</param>
    /// <param name="_value">The object to unsatisfy.</param>
    /// <returns>True if the object was being watched and has been unsatisfied; false otherwise.</returns>
    public virtual bool Unsatisfy(K _key, V _value)
    {
        if (IsWatching(_key, _value))
        {
            return base.UnsatisfyBundle(_value, m_Bundles[_key]);
        }
        else
        {
            return false;
        }
    }

    #endregion
}