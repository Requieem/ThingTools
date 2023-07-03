using System;
using UnityEngine.Events;

/// <summary>
/// A base class for all Dictionary Satisfiers that exist in the game.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <typeparamref name="K"/> is the type of the key in the dictionary.
/// </list>
/// <list type="bullet">
/// <typeparamref name="V"/> is the type of the value in the dictionary.
/// </list>
/// </remarks>
public abstract class TSatisfier<K, V> : IThingSatisfier<K, V>
{
    #region Backing Fields:

    ThingSatisfier<K, V> m_Satisfier;

    #endregion

    #region Instance Properties

    /// <summary>
    /// Gets the comparison function used to compare elements in the collection.
    /// </summary>
    /// <value>
    /// The comparison function used to compare elements in the collection.
    /// </value>
    /// <remarks>
    /// This property must be overridden by subclasses for the rest of the class to work properly.
    /// </remarks>
    public abstract Comparison<V> Comparer { get; }

    /// <summary>
    /// Gets the equality testing function used to determine if two elements in the collection are equal.
    /// </summary>
    /// <value>
    /// The equality testing function used to determine if two <typeparamref name="V"/> elements in the collection are equal.
    /// </value>
    /// <remarks>
    /// This property must be overridden by subclasses for the rest of the class to work properly.
    /// </remarks>
    /// <returns>
    /// A Bool representing equality.
    /// </returns>
    public abstract Func<V, V, bool> Equator { get; }

    /// <summary>
    /// Gets the satisfier that supplies the Satisfier Behaviour to this entity.
    /// </summary>
    /// <value>
    /// The instance of the <see cref="ThingSatisfier{K, V}"/> satisfier currently in use. 
    /// </value>
    public ThingSatisfier<K, V> Satisfier
    {
        get
        {
            if (m_Satisfier == null)
            {
                m_Satisfier = new ThingSatisfier<K, V>(Comparer, Equator);
            }
            return m_Satisfier;
        }
        private set { m_Satisfier = value; }
    }

    #endregion

    #region SO Methods:

    /// <summary>
    /// Scriptable Object OnEnable method.
    /// </summary>
    /// <remarks>
    /// and it is required to preserve that functionalty.
    /// </remarks>
    public virtual void OnEnable()
    {
        EnableSatisfier();
    }
    #endregion

    #region IVSatisfier Realization:

    /// <summary>
    /// Virtual Function used to Enable the current <see cref="ThingSatisfier{K, V}"/>
    /// </summary>
    /// <remarks>
    /// This becomes useful further down in the hierarchy when ensuring and enabling a 
    /// satisfier are done are different times and require separate operations.
    /// </remarks>
    public virtual void EnableSatisfier()
    {
        EnsureSatisfier();
    }


    /// <summary>
    /// Function used to ensure an attached instance of the given Satisfier
    /// </summary>
    /// <returns>
    /// A bool value, mostly used to indicate that no satisfier was attached.
    /// </returns>
    public bool EnsureSatisfier()
    {
        if (m_Satisfier == null)
        {
            Satisfier = new ThingSatisfier<K, V>(Comparer, Equator);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Function that takes a <typeparamref name="K"/>,<typeparamref name="V"/> pair to register a new <see cref="Satisfier{T}.SatisfierBundle"/>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    /// <param name="onSatisfy"></param>
    /// <param name="onUnsatisfy"></param>
    /// <returns>
    /// A float value, which is usally computed using the <see cref="WatchFeedback(K, V, UnityEvent, UnityEvent)"/> function.
    /// </returns>
    public float Watch(K _key, V _value, UnityEvent onSatisfy, UnityEvent onUnsatisfy)
    {
        EnsureSatisfier();
        return WatchFeedback(_key, _value, onSatisfy, onUnsatisfy);
    }

    /// <summary>
    /// A function Used to actually send the data to the satisfier while also providing feedback for the <see cref="Watch(K, V, UnityEvent, UnityEvent)"/> method.
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    /// <param name="onSatisfy"></param>
    /// <param name="onUnsatisfy"></param>
    /// <returns>
    /// A float, which by defaults equals the return of the <see cref="Satisfier"/> <see cref="ThingSatisfier{K, V}.Watch(K, V, UnityEvent, UnityEvent)"/> method.
    /// </returns>
    /// <remarks>
    /// This function can be overwritten to provide different kinds of feedback to Objects trying to register a new Satisfier Bundle <seealso cref="Requirement"/> sub-classes are a good example of where this could matter.
    /// </remarks>
    public virtual float WatchFeedback(K _key, V _value, UnityEvent onSatisfy, UnityEvent onUnsatisfy)
    {
        return Satisfier.Watch(_key, _value, onSatisfy, onUnsatisfy);
    }

    /// <summary>
    /// /// Function that takes a <typeparamref name="K"/> key and a 
    /// <typeparamref name="V"/> value to unwatch, if present
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public void Unwatch(K _key, V _value)
    {
        if (EnsureSatisfier())
            return;
        Satisfier.Unwatch(_key, _value);
    }

    /// <summary>
    /// Function that takes a <typeparamref name="K"/> key to check wheter or not
    /// the satisfier currently has a list of values at the current key.
    /// </summary>
    /// <param name="_key"></param>
    /// <returns>
    /// A boolean value, indicating watch status of the given key
    /// </returns>
    public bool IsWatching(K _key)
    {
        if (EnsureSatisfier())
            return false;

        return Satisfier.IsWatching(_key);
    }

    /// <summary>
    /// Function that takes a <typeparamref name="K"/> key to check wheter or not
    /// the satisfier currently has the given <typeparamref name="V"/> value in the list at that key.
    /// </summary>
    /// <param name="_key"></param>
    /// <returns>
    /// A boolean value, indicating watch status of the given key - value pair
    /// </returns>
    public bool IsWatching(K _key, V _value)
    {
        if (EnsureSatisfier())
            return false;

        return Satisfier.IsWatching(_key, _value);
    }

    /// <summary>
    /// Used when the collection is affected by a change that could potentially satisfy a condition.
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public bool CheckSatisfyOnChange(K _key, V _value)
    {
        if (EnsureSatisfier() || !IsWatching(_key, _value))
        {
            Satisfier.AddBundle(_key, _value);
            return false;
        }
        else if (IsWatching(_key, _value))
        {
            Satisfier.Satisfy(_key, _value);
            return true;
        }
        else return false;
    }

    /// <summary>
    /// Used when the collection is affected by a change that could potentially unsatisfy a condition.
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public bool CheckUnsatisfyOnChange(K _key, V _value)
    {
        if (EnsureSatisfier() || !IsWatching(_key, _value))
            return false;
        else if (IsWatching(_key, _value))
        {
            return Satisfier.Unsatisfy(_key, _value);
        }
        else return false;
    }

    #endregion
}
