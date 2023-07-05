using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all Satisfiers.
/// </summary>
/// <typeparam name="T">The type of objects this Satisfier can handle.</typeparam>
public abstract class Satisfier<T> : IComparer<Satisfier<T>.SatisfierBundle>
{
    /// <summary>
    /// Represents a bundle of data for a Satisfier, including watched object of generic type, event triggers and satisfaction counters.
    /// </summary>
    public class SatisfierBundle
    {
        /// <summary>
        /// Object being watched by the Satisfier.
        /// </summary>
        public T m_WatchedObject;

        /// <summary>
        /// Event invoked when the Satisfier is satisfied.
        /// </summary>
        public UnityEvent m_DoSatisfy;

        /// <summary>
        /// Event invoked when the Satisfier is unsatisfied.
        /// </summary>
        public UnityEvent m_UnSatisfy;

        /// <summary>
        /// Count of how many times the Satisfier has been satisfied.
        /// </summary>
        public int m_SatisfiedCount;

        /// <summary>
        /// Count of how many times the Satisfier has been unsatisfied.
        /// </summary>
        public int m_UnsatisfiedCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="SatisfierBundle{T}"/> class.
        /// </summary>
        /// <param name="watchedObject">The object being watched.</param>
        /// <param name="onSatisfyEvents">The event to trigger when the Satisfier is satisfied.</param>
        /// <param name="unSatisfyEvents">The event to trigger when the Satisfier is unsatisfied.</param>
        public SatisfierBundle(T watchedObject, UnityEvent onSatisfyEvents, UnityEvent unSatisfyEvents)
        {
            m_WatchedObject = watchedObject;
            m_DoSatisfy = onSatisfyEvents;
            m_UnSatisfy = unSatisfyEvents;
            m_SatisfiedCount = 0;
            m_UnsatisfiedCount = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SatisfierBundle{T}"/> class with predefined satisfaction counters.
        /// </summary>
        /// <param name="watchedObject">The object being watched.</param>
        /// <param name="onSatisfyEvents">The event to trigger when the Satisfier is satisfied.</param>
        /// <param name="unSatisfyEvents">The event to trigger when the Satisfier is unsatisfied.</param>
        /// <param name="satisfiedCount">Initial value for the satisfied count.</param>
        /// <param name="unsatisfiedCount">Initial value for the unsatisfied count.</param>
        public SatisfierBundle(T watchedObject, UnityEvent onSatisfyEvents, UnityEvent unSatisfyEvents, int satisfiedCount, int unsatisfiedCount)
        {
            m_WatchedObject = watchedObject;
            m_DoSatisfy = onSatisfyEvents;
            m_UnSatisfy = unSatisfyEvents;
            m_SatisfiedCount = satisfiedCount;
            m_UnsatisfiedCount = unsatisfiedCount;
        }

        /// <summary>
        /// Invokes the Satisfy event and increments the satisfied count.
        /// </summary>
        public void Satisfy()
        {
            m_DoSatisfy?.Invoke();
            m_SatisfiedCount++;
        }

        /// <summary>
        /// Invokes the Unsatisfy event and increments the unsatisfied count.
        /// </summary>
        public void Unsatisfy()
        {
            m_UnSatisfy?.Invoke();
            m_UnsatisfiedCount++;
        }

        /// <summary>
        /// Replaces the existing satisfy and unsatisfy events with new ones.
        /// </summary>
        /// <param name="onSatisfyEvents">The new event to trigger when the Satisfier is satisfied.</param>
        /// <param name="unSatisfyEvents">The new event to trigger when the Satisfier is unsatisfied.</param>
        public void ReplaceEvents(UnityEvent onSatisfyEvents, UnityEvent unSatisfyEvents)
        {
            m_DoSatisfy = onSatisfyEvents;
            m_UnSatisfy = unSatisfyEvents;
        }
    }

    #region Instance Fields:

    /// <summary>
    /// Comparison method for type <see cref="T"/>.
    /// </summary>
    protected Comparison<T> m_Comparer;

    /// <summary>
    /// Equality comparison method for type <see cref="T"/>.
    /// </summary>
    protected Func<T, T, bool> m_EqualityComparer;

    /// <summary>
    /// Provides a Comparer for the type <see cref="T"/> based on <see cref="m_Comparer"/>.
    /// </summary>
    protected Comparer<T> Comparer => Comparer<T>.Create(m_Comparer);

    #endregion

    #region Constructors:

    /// <summary>
    /// Constructs a new Satisfier.
    /// </summary>
    /// <param name="comparer">The comparison method for type <see cref="T"/>.</param>
    /// <param name="equalityComparer">The equality comparison method for type <see cref="T"/>.</param>
    public Satisfier(Comparison<T> comparer, Func<T, T, bool> equalityComparer)
    {
        this.m_Comparer = comparer;
        this.m_EqualityComparer = equalityComparer;
    }

    #endregion

    #region Methods:

    /// <summary>
    /// Adds the given object to the list of watched objects.
    /// Also adds the given events to the list of events to be triggered when the object is acquired or removed.
    /// </summary>
    /// <remarks>
    /// If the object is already being watched (based on <see cref="m_EqualityComparer"/>), this method replaces the events.
    /// </remarks>
    /// <param name="obj">The object to be watched.</param>
    /// <param name="doSatisfy">The event to be triggered when the object is acquired.</param>
    /// <param name="unSatisfy">The event to be triggered when the object is removed.</param>
    /// <param name="_bundles">The dictionary of current bundles being watched.</param>
    /// <returns>An integer value representing how many times this object's requirement has already been satisfied.</returns>
    /// <exception cref="ArgumentNullException">Thrown when obj is null.</exception>
    public virtual int Watch(T obj, UnityEvent doSatisfy, UnityEvent unSatisfy, Dictionary<T, SatisfierBundle> _bundles)
    {
        // check for null objects
        if (obj == null || obj.Equals(null))
        {
            Log.Wng($"Cannot watch a null object, check {typeof(T)} requirements for null object references.");
            return 0;
        }

        // if object is already being watched
        if (IsObjectBeingWatched(obj, _bundles))
        {
            _bundles[obj].ReplaceEvents(doSatisfy, unSatisfy);
            return _bundles[obj].m_SatisfiedCount;
        }

        // if object is not yet being watched
        IncludeBundleInWatchlist(CreateEventBundle(obj, doSatisfy, unSatisfy), _bundles);
        return 0;
    }

    /// <summary>
    /// Removes the given object from the list of watched objects.
    /// </summary>
    /// <param name="obj">The object to be unwatched.</param>
    /// <param name="_bundles">The dictionary of current bundles being watched.</param>
    /// <returns>Returns true if the object was successfully removed, false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when obj is null.</exception>
    public virtual bool Unwatch(T obj, Dictionary<T, SatisfierBundle> _bundles)
    {
        // check for null objects
        if (obj == null || obj.Equals(null))
        {
            Log.Wng("Cannot unwatch a null object, you can ignore this warning if you are trying to unwatch a null object.");
            return false;
        }

        // if object is being watched
        if (IsObjectBeingWatched(obj, _bundles))
        {
            ExcludeBundleFromWatchlist(_bundles[obj], _bundles);
            return true;
        }

        // if object is not being watched
        Log.Wng("Cannot unwatch an object that is not being watched, you can ignore this warning if you are trying to unwatch an object that is not in the list.");
        return false;
    }

    /// <summary>
    /// Creates a new SatisfierBundle with the given object and null events.
    /// </summary>
    public virtual SatisfierBundle CreateNullEventBundle(T obj, int satisfiedCount, int unsatisfiedCount)
    {
        // Create and return a new SatisfierBundle with the given object and null events
        return new SatisfierBundle(obj, null, null, satisfiedCount, unsatisfiedCount);
    }

    /// <summary>
    /// Creates a new SatisfierBundle with the given object and the given events.
    /// </summary>
    public virtual SatisfierBundle CreateEventBundle(T obj, UnityEvent doSatisfy, UnityEvent unSatisfy)
    {
        // Create and return a new SatisfierBundle with the given object and the given events
        return new SatisfierBundle(obj, doSatisfy, unSatisfy);
    }

    /// <summary>
    /// Adds a given SatisfierBundle to the list of watched objects.
    /// </summary>
    public virtual void IncludeBundleInWatchlist(SatisfierBundle bundle, Dictionary<T, SatisfierBundle> bundles)
    {
        // Add the bundle to the dictionary and sort the dictionary
        bundles.Add(bundle.m_WatchedObject, bundle);
        bundles = bundles.OrderBy(x => x.Key, Comparer).ToDictionary(x => x.Key, x => x.Value);
    }

    /// <summary>
    /// Removes a given SatisfierBundle from the list of watched objects.
    /// </summary>
    public virtual void ExcludeBundleFromWatchlist(SatisfierBundle bundle, Dictionary<T, SatisfierBundle> bundles)
    {
        if (IsObjectBeingWatched(bundle.m_WatchedObject, bundles))
        {
            // Remove the bundle from the dictionary if the watched object is being watched
            bundles.Remove(bundle.m_WatchedObject);
        }
        else
        {
            // Log a warning message if the watched object is not being watched
            Log.Wng("Cannot remove a bundle whose watched object is not a key in the dictionary. You can ignore this warning if you are trying to remove a bundle that is not in the list.");
        }
    }

    /// <summary>
    /// Satisfies the bundle that contains the given object.
    /// </summary>
    public virtual bool SatisfyBundle(T obj, Dictionary<T, SatisfierBundle> bundles)
    {
        if (obj == null || obj.Equals(null))
        {
            Log.Wng("Cannot satisfy a bundle with null key. You can ignore this warning if you are explicitly doing that.");
            return false;
        }
        else if (IsObjectBeingWatched(obj, bundles))
        {
            // Satisfy the bundle if the object is being watched
            bundles[obj].Satisfy();
            return true;
        }
        else
        {
            // Check if one of the keys is equal to the object based on the equality comparer
            var keys = bundles.Keys;
            var index = keys.ToList().FindIndex((key) => m_EqualityComparer(obj, key));

            if (index != -1)
            {
                var key = keys.ToArray()[index];
                bundles[key].Satisfy();
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
    /// Unsatisfies the bundle that contains the given object.
    /// </summary>
    public virtual bool UnsatisfyBundle(T obj, Dictionary<T, SatisfierBundle> bundles)
    {
        if (obj == null)
        {
            Log.Wng("Cannot unsatisfy a null object.");
            return false;
        }
        else if (IsObjectBeingWatched(obj, bundles))
        {
            bundles[obj].Unsatisfy();
            return true;
        }
        else
        {
            // if one of the keys is equal to the object based on the equality comparer
            var keys = bundles.Keys;
            var index = keys.ToList().FindIndex((key) => m_EqualityComparer(obj, key));

            if (index != -1)
            {
                var key = keys.ToArray()[index];
                bundles[key].Unsatisfy();
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
    public virtual bool IsObjectBeingWatched(T obj, Dictionary<T, SatisfierBundle> _bundles)
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

