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
        public ThingEntry() : base() { }
        public ThingEntry(K key, V value) : base(key, value) { }
    }

    #region Instance Fields:

    [SerializeField]
    protected List<ThingEntry> m_Entries = new() { new ThingEntry() };
    Dictionary<K, V> m_Dictionary;

    #endregion
    #region Instance Properties:

    /// <summary>
    /// The Values in this Container.
    /// </summary>
    public List<ThingEntry> Entries { get { return m_Entries; } }

    /// <summary>
    /// The Values in this Container as a Dictionary of K and V.
    /// </summary>
    public Dictionary<K, V> Dictionary
    {
        get
        {
            if (m_Dictionary != null)
                return m_Dictionary;

            m_Dictionary = new Dictionary<K, V>();
            for (int i = 0; i < m_Entries.Count; i++)
            {
                m_Dictionary.Add(m_Entries[i].m_Key, m_Entries[i].m_Value);
            }
            return m_Dictionary;
        }

        set { m_Dictionary = value; }
    }
    #endregion

    #region Methods:

    public void EraseValues()
    {
        foreach (var obj in m_Entries)
        {
            obj.m_Value = default;
        }
    }

    public override void EnableSatisfier()
    {
        base.EnableSatisfier();

        Dictionary = null;
        m_Entries ??= new List<ThingEntry>();

        foreach (ThingEntry obj in Entries)
        {
            CheckSatisfyOnChange(obj.m_Key, obj.m_Value);
        }
    }

    /// <summary>
    /// Adds the given object to this Container.
    /// </summary>

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
    public virtual bool Remove(K key, V value)
    {
        Dictionary = null;
        var found = false;
        if (Dictionary.ContainsKey(key))
        {
            if (Equator.Invoke(Dictionary[key], value))
            {
                // remove all values and keys where the key is equal to the given key
                int index = Entries.FindIndex((ThingEntry obj) => obj.m_Key.Equals(key));
                while (index != -1)
                {
                    CheckUnsatisfyOnChange(Entries[index].m_Key, Entries[index].m_Value);
                    Entries.RemoveAt(index);
                    index = Entries.FindIndex((ThingEntry obj) => obj.m_Key.Equals(key));
                    found = true;
                }
            }
        }

        return found;
    }

    public virtual bool Nullify(K key, V value)
    {
        Dictionary = null;
        var found = false;
        if (Dictionary.ContainsKey(key))
        {
            if (Equator.Invoke(Dictionary[key], value))
            {
                // remove all values and keys where the key is equal to the given key
                int index = Entries.FindIndex((ThingEntry obj) => obj.m_Key.Equals(key));
                while (index != -1)
                {
                    CheckUnsatisfyOnChange(Entries[index].m_Key, Entries[index].m_Value);
                    Entries[index] = null;
                    index = Entries.FindIndex((ThingEntry obj) => obj.m_Key.Equals(key) && obj.m_Value != null);
                    found = true;
                }
            }
        }

        return found;
    }

    /// <summary>
    /// Removes the given object from the given key in this Container.
    /// </summary>
    public virtual bool Remove(K key)
    {
        Dictionary = null;
        var found = false;
        if (Dictionary.ContainsKey(key))
        {
            // remove all values and keys where the key is equal to the given key
            int index = Entries.FindIndex((ThingEntry obj) => obj.m_Key.Equals(key));
            while (index != -1)
            {
                CheckUnsatisfyOnChange(Entries[index].m_Key, Entries[index].m_Value);
                Entries.RemoveAt(index);
                index = Entries.FindIndex((ThingEntry obj) => obj.m_Key.Equals(key));
                found = true;
            }
        }

        return found;
    }

    public virtual bool Nullify(K key)
    {
        Dictionary = null;
        var found = false;
        if (Dictionary.ContainsKey(key))
        {
            // remove all values and keys where the key is equal to the given key
            int index = Entries.FindIndex((ThingEntry obj) => obj.m_Key.Equals(key));
            while (index != -1)
            {
                CheckUnsatisfyOnChange(Entries[index].m_Key, Entries[index].m_Value);
                Entries[index] = null;
                index = Entries.FindIndex((ThingEntry obj) => obj.m_Key.Equals(key) && obj.m_Value != null);
                found = true;
            }
        }

        return found;
    }

    public override float WatchFeedback(K _key, V _value, UnityEvent onSatisfy, UnityEvent onUnsatisfy)
    {
        var watch = Satisfier.Watch(_key, _value, onSatisfy, onUnsatisfy);

        if (Dictionary.ContainsKey(_key))
        {
            var value = Dictionary[_key];
            if (value != null)
            {
                return Comparer.Invoke(value, _value);
            }
        }

        return -1;
    }

    #endregion
}

