using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// An base class for all Satisfiers that exist in the process.
/// </summary>
public abstract class Satisfier<T> : IComparer<Satisfier<T>.SatisfierBundle>
{
    /// <summary>
    /// This class is used to reprensent ASatisfier bundle of data.
    /// </summary>
    public class SatisfierBundle
    {
        public T m_WatchedObject;
        public UnityEvent m_DoSatisfy;
        public UnityEvent m_UnSatisfy;
        public int m_SatisfiedCount;
        public int m_UnsatisfiedCount;

        public SatisfierBundle(T watchedObject, UnityEvent onSatisfyEvents, UnityEvent unSatisfyEvents)
        {
            m_WatchedObject = watchedObject;
            m_DoSatisfy = onSatisfyEvents;
            m_UnSatisfy = unSatisfyEvents;
            m_SatisfiedCount = 0;
            m_UnsatisfiedCount = 0;
        }

        public SatisfierBundle(T watchedObject, UnityEvent onSatisfyEvents, UnityEvent unSatisfyEvents, int satisfiedCount, int unsatisfiedCount)
        {
            m_WatchedObject = watchedObject;
            m_DoSatisfy = onSatisfyEvents;
            m_UnSatisfy = unSatisfyEvents;
            m_SatisfiedCount = satisfiedCount;
            m_UnsatisfiedCount = unsatisfiedCount;
        }

        public void Satisfy()
        {
            m_DoSatisfy?.Invoke();
            m_SatisfiedCount++;
        }

        public void Unsatisfy()
        {
            m_UnSatisfy?.Invoke();
            m_UnsatisfiedCount++;
        }

        public void ReplaceEvents(UnityEvent onSatisfyEvents, UnityEvent unSatisfyEvents)
        {
            m_DoSatisfy = onSatisfyEvents;
            m_UnSatisfy = unSatisfyEvents;
        }
    }

    #region Instance Fields:

    protected Comparison<T> m_Comparer;
    protected Func<T, T, bool> m_EqualityComparer;

    protected Comparer<T> Comparer { get { return Comparer<T>.Create(m_Comparer); } }

    #endregion

    #region Constructors:

    /// <summary>
    /// Constructor for the Satisfier class.
    /// </summary>
    public Satisfier(Comparison<T> comparer, Func<T, T, bool> equalityComparer)
    {
        this.m_Comparer = comparer;
        this.m_EqualityComparer = equalityComparer;
    }

    #endregion

    #region Methods:

    /// <summary>
    /// Add the given object the the given list watched objects.
    /// also add the given events to the list of events to be triggered when the object is acquired or removed.
    /// </summary>
    /// <remarks>
    /// Replaces the events if the object is already being watched (based on <see cref="m_EqualityComparer"/>).
    /// </remarks>
    /// <param name="obj"></param>
    /// <param name="doSatisfy"></param>
    /// <param name="unSatisfy"></param>
    /// <param name="_bundles"></param>
    /// <returns>An integer value representing how many times this object's requirement has already been satisfied</returns>
    public virtual int Watch(T obj, UnityEvent doSatisfy, UnityEvent unSatisfy, Dictionary<T, SatisfierBundle> _bundles)
    {
        if (obj == null || obj.Equals(null))
        {
            Log.Wng("Cannot watch a null object, check " + typeof(T) + " requirements for null object references.");
            return 0;
        }
        else if (IsWatching(obj, _bundles))
        {
            _bundles[obj].ReplaceEvents(doSatisfy, unSatisfy);
            return _bundles[obj].m_SatisfiedCount;
        }
        else
        {
            AddBundle(CreateBundle(obj, doSatisfy, unSatisfy), _bundles);
            return 0;
        }
    }

    /// <summary>
    /// Remove the given object from the list of watched objects.
    /// </summary>
    public virtual bool Unwatch(T obj, Dictionary<T, SatisfierBundle> _bundles)
    {
        if (obj == null || obj.Equals(null))
        {
            Log.Wng("Cannot unwatch a null object, you can ignore this warning if you are trying to unwatch a null object.");
            return false;
        }
        else if (IsWatching(obj, _bundles))
        {
            RemoveBundle(_bundles[obj], _bundles);
            return true;
        }
        else
        {
            Log.Wng("Cannot unwatch an object that is not being watched, you can ignore this warning if you are trying to unwatch an object that is not in the list.");
            return false;
        }
    }

    /// <summary>
    /// a function that given a T object, creates a new SatisfierBundle with the given object, and null events
    /// </summary>
    public virtual SatisfierBundle CreateBundle(T obj, int satisfiedCount, int unsatisfiedCount)
    {
        return new SatisfierBundle(obj, null, null, satisfiedCount, unsatisfiedCount);
    }

    /// <summary>
    /// a function that given a T object, creates a new SatisfierBundle with the given object, and the given events
    /// </summary>
    public virtual SatisfierBundle CreateBundle(T obj, UnityEvent doSatisfy, UnityEvent unSatisfy)
    {
        return new SatisfierBundle(obj, doSatisfy, unSatisfy);
    }

    /// <summary>
    /// a function that given a SatifiserBundle, adds it to the list of watched objects
    /// </summary>
    public virtual void AddBundle(SatisfierBundle bundle, Dictionary<T, SatisfierBundle> _bundles)
    {
        _bundles.Add(bundle.m_WatchedObject, bundle);
        _bundles = _bundles.OrderBy(x => x.Key, Comparer).ToDictionary(x => x.Key, x => x.Value);
    }

    /// <summary>
    /// a function that given a SatifiserBundle, removes it from the list of watched objects
    /// </summary>
    public virtual void RemoveBundle(SatisfierBundle bundle, Dictionary<T, SatisfierBundle> _bundles)
    {
        if (IsWatching(bundle.m_WatchedObject, _bundles))
        {
            _bundles.Remove(bundle.m_WatchedObject);
        }
        else
        {
            Log.Wng("Cannot remove a bundle whose whatched object is not a key in the dictionary, you can ignore this warning if you are trying to remove a bundle that is not in the list.");
        }
    }

    /// <summary>
    /// a function that given a T object, satisfies the bundle that contains the object
    /// </summary>
    public virtual bool Satisfy(T obj, Dictionary<T, SatisfierBundle> _bundles)
    {
        if (obj == null || obj.Equals(null))
        {
            Log.Wng("Cannot satisfy a bundle with null key. You can ignore this warning if you are explicitly doing that.");
            return false;
        }
        else if (IsWatching(obj, _bundles))
        {
            _bundles[obj].Satisfy();
            return true;
        }
        else
        {
            // if one of the keys is equal to the object based on the equality comparer
            var keys = _bundles.Keys;
            var index = keys.ToList().FindIndex((key) => m_EqualityComparer(obj, key));

            if (index != -1)
            {
                var key = keys.ToArray()[index];
                _bundles[key].Satisfy();
                return true;
            }
            else
            {
                Log.Wng("Tried to Satisfy a requirement that is not being watched");
                return false;
            }
        }
    }

    /// <summary>
    /// a function that given a T object, unsatisfies the bundle that contains the object
    /// </summary>
    public virtual bool Unsatisfy(T obj, Dictionary<T, SatisfierBundle> _bundles)
    {
        if (obj == null)
        {
            Log.Wng("Cannot unsatisfy a null object.");
            return false;
        }
        else if (IsWatching(obj, _bundles))
        {
            _bundles[obj].Unsatisfy();
            return true;
        }
        else
        {
            // if one of the keys is equal to the object based on the equality comparer
            var keys = _bundles.Keys;
            var index = keys.ToList().FindIndex((key) => m_EqualityComparer(obj, key));

            if (index != -1)
            {
                var key = keys.ToArray()[index];
                _bundles[key].Unsatisfy();
                return true;
            }
            else
            {
                Log.Wng("Tried to Satisfy a requirement that is not being watched");
                return false;
            }
        }
    }

    /// <summary>
    /// Given a T object, returns wheter or not the object is being watched.
    /// </summary>
    public virtual bool IsWatching(T obj, Dictionary<T, SatisfierBundle> _bundles)
    {
        return _bundles.ContainsKey(obj);
    }

    /// <summary>
    /// A function to iterate through the list of _bundles
    /// </summary>
    public virtual IEnumerator<SatisfierBundle> GetEnumerator(Dictionary<T, SatisfierBundle> _bundles)
    {
        return _bundles.Values.OrderBy(x => x.m_WatchedObject, Comparer).GetEnumerator();
    }

    /// <summary>
    /// A function to iterate through the list of _bundles the opposite way
    /// </summary>
    public virtual IEnumerator<SatisfierBundle> GetReverseEnumerator(Dictionary<T, SatisfierBundle> _bundles)
    {
        return _bundles.Values.OrderBy(x => x.m_WatchedObject, Comparer).Reverse().GetEnumerator();
    }

    /// <summary>
    /// A method to clean the Satifiser of all data
    /// </summary>
    public virtual void ClearBundles(Dictionary<T, SatisfierBundle> _bundles)
    {
        _bundles.Clear();
    }

    public int Compare(SatisfierBundle x, SatisfierBundle y)
    {
        return m_Comparer(x.m_WatchedObject, y.m_WatchedObject);
    }

    #endregion
}

