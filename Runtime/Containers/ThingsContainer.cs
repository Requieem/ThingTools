using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all Containers that exist in the game.
/// </summary>
public abstract class ThingsContainer<K, V> : TSatisfier<K, V> where K : ScriptableThing<K>
{
    [Serializable]
    public class ThingEntry : SerializeableEntry<K, V>
    {
        /// <summary>
        /// Default constructor for the ThingEntry class.
        /// </summary>
        public ThingEntry() : base() { }

        /// <summary>
        /// Constructor for the ThingEntry class.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <param name="value">The value of the entry.</param>
        public ThingEntry(K key, V value) : base(key, value) { }
    }

    #region Instance Fields:

    [SerializeField]
    protected List<ThingEntry> m_Entries = new List<ThingEntry> { new ThingEntry() };
    private Dictionary<K, V> m_Dictionary;

    #endregion

    #region Instance Properties:

    /// <summary>
    /// The entries in this Container.
    /// </summary>
    public List<ThingEntry> Entries => m_Entries;

    /// <summary>
    /// The entries in this Container as a Dictionary of K and V.
    /// </summary>
    public Dictionary<K, V> Dictionary
    {
        get
        {
            if (m_Dictionary != null)
                return m_Dictionary;

            m_Dictionary = new Dictionary<K, V>();
            foreach (ThingEntry entry in m_Entries)
            {
                m_Dictionary.Add(entry.m_Key, entry.m_Value);
            }
            return m_Dictionary;
        }

        set { m_Dictionary = value; }
    }

    #endregion

    #region Methods:

    /// <summary>
    /// Sets the value of all entries to default.
    /// </summary>
    public void EraseValues()
    {
        foreach (var entry in m_Entries)
        {
            entry.m_Value = default;
        }
    }

    /// <summary>
    /// Enables the satisfier and checks for satisfaction status of each entry.
    /// </summary>
    public override void EnableSatisfier()
    {
        base.EnableSatisfier();

        Dictionary = null;
        m_Entries ??= new List<ThingEntry>();

        foreach (ThingEntry entry in Entries)
        {
            CheckSatisfyOnChange(entry.m_Key, entry.m_Value);
        }
    }

    /// <summary>
    /// Adds the given object to this Container.
    /// </summary>
    /// <param name="key">The key of the object.</param>
    /// <param name="value">The value of the object.</param>
    public virtual void Add(K key, V value)
    {
        if (key == null)
        {
            throw new Exception("Cannot add a null value or key to a Container.");
        }

        Entries.Add(new ThingEntry(key, value));

        if (value != null)
        {
            CheckSatisfyOnChange(key, value);
        }
    }

    /// <summary>
    /// Removes the given object from the given key in this Container.
    /// </summary>
    /// <param name="key">The key of the object to remove.</param>
    /// <param name="value">The value of the object to remove.</param>
    /// <returns>True if the object was removed successfully, false otherwise.</returns>
    public virtual bool Remove(K key, V value)
    {
        Dictionary = null;
        var found = false;

        if (Dictionary.ContainsKey(key))
        {
            if (Equator.Invoke(Dictionary[key], value))
            {
                // Remove all values and keys where the key is equal to the given key
                int index = Entries.FindIndex(entry => entry.m_Key.Equals(key));
                while (index != -1)
                {
                    CheckUnsatisfyOnChange(Entries[index].m_Key, Entries[index].m_Value);
                    Entries.RemoveAt(index);
                    index = Entries.FindIndex(entry => entry.m_Key.Equals(key));
                    found = true;
                }
            }
        }

        return found;
    }

    /// <summary>
    /// Removes all entries with the given key from this Container.
    /// </summary>
    /// <param name="key">The key of the entries to remove.</param>
    /// <returns>True if entries were removed successfully, false otherwise.</returns>
    public virtual bool Remove(K key)
    {
        Dictionary = null;
        var found = false;

        if (Dictionary.ContainsKey(key))
        {
            // Remove all values and keys where the key is equal to the given key
            int index = Entries.FindIndex(entry => entry.m_Key.Equals(key));
            while (index != -1)
            {
                CheckUnsatisfyOnChange(Entries[index].m_Key, Entries[index].m_Value);
                Entries.RemoveAt(index);
                index = Entries.FindIndex(entry => entry.m_Key.Equals(key));
                found = true;
            }
        }

        return found;
    }

    /// <summary>
    /// Sets the value of the entries with the given key to null.
    /// </summary>
    /// <param name="key">The key of the entries to nullify.</param>
    /// <returns>True if entries were nullified successfully, false otherwise.</returns>
    public virtual bool Nullify(K key)
    {
        Dictionary = null;
        var found = false;

        if (Dictionary.ContainsKey(key))
        {
            // Set the value of entries with the given key to null
            int index = Entries.FindIndex(entry => entry.m_Key.Equals(key));
            while (index != -1)
            {
                CheckUnsatisfyOnChange(Entries[index].m_Key, Entries[index].m_Value);
                Entries[index].m_Value = default;
                index = Entries.FindIndex(entry => entry.m_Key.Equals(key) && entry.m_Value != null);
                found = true;
            }
        }

        return found;
    }

    /// <summary>
    /// Removes the given object from the given key in this Container.
    /// </summary>
    /// <param name="key">The key of the object to remove.</param>
    /// <param name="value">The value of the object to remove.</param>
    /// <returns>True if the object was nullified successfully, false otherwise.</returns>
    public virtual bool Nullify(K key, V value)
    {
        Dictionary = null;
        var found = false;

        if (Dictionary.ContainsKey(key))
        {
            if (Equator.Invoke(Dictionary[key], value))
            {
                // Remove all values and keys where the key is equal to the given key
                int index = Entries.FindIndex(entry => entry.m_Key.Equals(key));
                while (index != -1)
                {
                    CheckUnsatisfyOnChange(Entries[index].m_Key, Entries[index].m_Value);
                    Entries[index].m_Value = default;
                    index = Entries.FindIndex(entry => entry.m_Key.Equals(key) && entry.m_Value != null);
                    found = true;
                }
            }
        }

        return found;
    }

    /// <summary>
    /// Calculates the feedback value for watching the object with the given key and value.
    /// </summary>
    /// <param name="key">The key of the object to watch.</param>
    /// <param name="value">The value of the object to watch.</param>
    /// <param name="onSatisfy">The UnityEvent to invoke when the object is satisfied.</param>
    /// <param name="onUnsatisfy">The UnityEvent to invoke when the object is unsatisfied.</param>
    /// <returns>The feedback value calculated based on the object's satisfaction status.</returns>
    public override float WatchFeedback(K key, V value, UnityEvent onSatisfy, UnityEvent onUnsatisfy)
    {
        var watch = Satisfier.Watch(key, value, onSatisfy, onUnsatisfy);

        if (Dictionary.ContainsKey(key))
        {
            var entryValue = Dictionary[key];
            if (entryValue != null)
            {
                return Comparer.Invoke(entryValue, value);
            }
        }

        return -1;
    }

    #endregion
}

