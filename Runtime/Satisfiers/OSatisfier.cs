using System;
using UnityEngine.Events;

/// <summary>
/// A base class for all List Satisfiers that exist in the game.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <typeparamref name="T"/> is the type of the values in the list.
/// </list>
/// </remarks>
public abstract class OSatisfier<T> : IObjectSatisfier<T>
{

    #region Backing Fields:

    protected ObjectSatisfier<T> m_Satisfier;

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
    public virtual Comparison<T> Comparer { get; }

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
    public virtual Func<T, T, bool> Equator { get; private set; }

    /// <summary>
    /// Gets the satisfier that supplies the Satisfier Behaviour to this entity.
    /// </summary>
    /// <value>
    /// The instance of the <see cref="ObjectSatisfier{T}"/> satisfier currently in use. 
    /// </value>
    public ObjectSatisfier<T> Satisfier
    {
        get
        {
            if (m_Satisfier == null)
            {
                m_Satisfier = new ObjectSatisfier<T>(Comparer, Equator);
            }
            return m_Satisfier;
        }
        private set
        { m_Satisfier = value; }
    }

    #endregion

    #region SO Methods:

    /// <summary>
    /// Scriptable Object OnEnable method.
    /// </summary>
    /// <remarks>
    /// <see cref="Thing{D}"/> already implements <see cref="Thing{D}.OnEnable"/>
    /// and it is required to preserve that functionalty.
    /// </remarks>
    public virtual void OnEnable()
    {
        EnableSatisfier();
    }

    #endregion

    #region IRSatisfier Realization:

    /// <summary>
    /// Virtual Function used to Enable the current <see cref="ObjectSatisfier{T}"/>
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
            m_Satisfier = new ObjectSatisfier<T>(Comparer, Equator);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Function that takes a <typeparamref name="T"/> object to register a new <see cref="Satisfier{T}.SatisfierBundle"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="onSatisfy"></param>
    /// <param name="onUnsatisfy"></param>
    /// <returns>
    /// A float value, which is usally computed using the <see cref="WatchFeedback(T, UnityEvent, UnityEvent)"/> function.
    /// </returns>
    public float Watch(T obj, UnityEvent onSatisfy, UnityEvent onUnsatisfy)
    {
        EnsureSatisfier();
        return WatchFeedback(obj, onSatisfy, onUnsatisfy);
    }

    /// <summary>
    /// A function Used to actually send the data to the satisfier while also providing feedback for the <see cref="Watch(T, UnityEvent, UnityEvent)"/> method.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="onSatisfy"></param>
    /// <param name="onUnsatisfy"></param>
    /// <returns>
    /// A float, which by defaults equals the return of the <see cref="Satisfier"/> <see cref="ThingSatisfier{K, V}.Watch(K, V, UnityEvent, UnityEvent)"/> method.
    /// </returns>
    /// <remarks>
    /// This function can be overwritten to provide different kinds of feedback to Objects trying to register a new Satisfier Bundle <seealso cref="Requirement"/> sub-classes are a good example of where this could matter.
    /// </remarks>
    public virtual float WatchFeedback(T obj, UnityEvent onSatisfy, UnityEvent onUnsatisfy)
    {
        return Satisfier.Watch(obj, onSatisfy, onUnsatisfy);
    }

    /// <summary>
    /// /// Function that takes a <typeparamref name="T"/> obj to unwatch, if present
    /// </summary>
    /// <param name="obj"></param>
    public void Unwatch(T obj)
    {
        if (EnsureSatisfier())
            return;
        Satisfier.Unwatch(obj);
    }

    /// <summary>
    /// Function that takes a <typeparamref name="T"/> obj to check wheter or not
    /// the satisfier is currently watching that object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>
    /// A boolean value, indicating watch status of the given object
    /// </returns>
    public bool IsWatching(T obj)
    {
        return Satisfier.IsWatching(obj);
    }

    /// <summary>
    /// Used when the collection is affected by a change that could potentially satisfy a condition.
    /// </summary>
    /// <param name="obj>
    public virtual bool CheckSatisfyOnChange(T obj)
    {
        if (EnsureSatisfier() || !IsWatching(obj))
        {
            Satisfier.AddBundle(Satisfier.CreateNullEventBundle(obj, 1, 0));
            return false;
        }
        else if (IsWatching(obj))
        {
            return Satisfier.Satisfy(obj);
        }
        else return false;
    }

    /// <summary>
    /// Used when the collection is affected by a change that could potentially unsatisfy a condition.
    /// </summary>
    /// <param name="obj"></param>
    public virtual bool CheckUnsatisfyOnChange(T obj)
    {
        if (EnsureSatisfier() || !IsWatching(obj))
            return false;
        else if (IsWatching(obj))
        {
            return Satisfier.Unsatisfy(obj);
        }
        else return false;
    }

    #endregion
}
