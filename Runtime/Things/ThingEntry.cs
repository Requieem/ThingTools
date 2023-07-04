using System;
using UnityEngine;

/// <summary>
/// Represents an entry in a key-value pair collection for 'Thing' objects.
/// </summary>
/// <typeparam name="K">The type of the key in the entry.</typeparam>
/// <typeparam name="V">The type of the value in the entry.</typeparam>
[Serializable]
public class ThingEntry<K, V>
{
    #region Fields

    /// <summary>
    /// The key of the entry.
    /// </summary>
    [SerializeField]
    public K m_Key;

    /// <summary>
    /// The value of the entry.
    /// </summary>
    [SerializeField]
    public V m_Value;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ThingEntry{K, V}"/> class with the specified key and value.
    /// </summary>
    /// <param name="key">The key of the entry.</param>
    /// <param name="value">The value of the entry.</param>
    public ThingEntry(K key, V value)
    {
        this.m_Key = key;
        this.m_Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThingEntry{K, V}"/> class with default key and value.
    /// </summary>
    public ThingEntry()
    {
        this.m_Key = default;
        this.m_Value = default;
    }

    #endregion
}
