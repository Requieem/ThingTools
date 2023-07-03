using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// An base class for all Satisfiers that exist in the game.
/// </summary>
public class ThingSatisfier<K, V> : Satisfier<V>
{
  #region Instance Fields:

  protected Dictionary<K, Dictionary<V, SatisfierBundle>> m_Bundles;

  #endregion

  #region Instance Properties:

  public Dictionary<K, Dictionary<V, SatisfierBundle>> Bundles
  {
    get { return m_Bundles; }
  }

  #endregion

  #region Constructors:

  public ThingSatisfier(Comparison<V> comparer, Func<V, V, bool> equalityComparer) : base(comparer, equalityComparer)
  {
    m_Bundles = new Dictionary<K, Dictionary<V, SatisfierBundle>>();
  }

  #endregion

  #region Methods:

  /// <summary>
  /// Add the given object to the list of watched objects.
  /// also add the given events to the list of events to be triggered when the object is acquired or removed.
  /// </summary>
  public virtual int Watch(K _key, V _value, UnityEvent doSatisfy, UnityEvent unSatisfy)
  {
    if (_key == null || _value == null)
    {
      Log.Wng("Cannot watch a null object nor a null key, check " + typeof(V) + " requirements for null object references.");
      return 0;
    }

    if (!IsWatching(_key))
    {
      m_Bundles.Add(_key, new Dictionary<V, SatisfierBundle>());
    }

    return base.Watch(_value, doSatisfy, unSatisfy, m_Bundles[_key]);
  }

  /// <summary>
  /// Remove the given object from the list of watched objects.
  /// </summary>
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
  /// Returns wheter or not the given key is being watched
  /// </summary>
  public virtual bool IsWatching(K _key)
  {
    if (_key == null) { return false; }
    return m_Bundles.ContainsKey(_key);
  }

  /// <summary>
  /// Returns wheter or not the current value at the given key is being watched
  /// </summary>
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
  /// Adds the given value to the given key
  /// </summary>
  public virtual void AddBundle(K _key, V _value)
  {
    if (_key == null || _value == null)
    {
      Log.Wng("Cannot add a null object nor a null key, check " + typeof(V) + " requirements for null object references.");
      return;
    }

    if (!IsWatching(_key))
    {
      m_Bundles.Add(_key, new Dictionary<V, SatisfierBundle>());
    }

    if (!IsWatching(_key, _value))
    {
      m_Bundles[_key].Add(_value, CreateBundle(_value, 1, 0));
    }
  }

  /// <summary>
  /// Removes the given value from the given key
  /// </summary>
  public virtual void RemoveBundle(K _key, V _value)
  {
    if (IsWatching(_key, _value))
    {
      m_Bundles[_key].Remove(_value);
    }
  }

  /// <summary>
  /// Satisfies the given object.
  /// </summary>
  public virtual bool Satisfy(K _key, V _value)
  {
    if (IsWatching(_key, _value))
    {
      return base.Satisfy(_value, m_Bundles[_key]);
    }
    else
    {
      return false;
    }
  }

  /// <summary>
  /// Unsatisfies the given object.
  /// </summary>
  public virtual bool Unsatisfy(K _key, V _value)
  {
    if (IsWatching(_key, _value))
    {
      return base.Unsatisfy(_value, m_Bundles[_key]);
    }
    else
    {
      return false;
    }
  }


  #endregion
}

