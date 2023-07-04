using System;
using UnityEngine;

/// <summary>
/// Represents an entry that can be serialized and linked to a <see cref="ThingID"/>.
/// </summary>
/// <typeparam name="K">The type of the key, which should be a type derived from <see cref="ScriptableThing{K}"/>.</typeparam>
/// <typeparam name="V">The type of the value.</typeparam>
[Serializable]
public class SerializeableEntry<K, V> : ThingEntry<K, V>, ISerializationCallbackReceiver where K : ScriptableThing<K>
{
    [SerializeField] private ThingID identifier = null;

    /// <summary>
    /// Initializes a new instance of <see cref="SerializeableEntry{K, V}"/> with the provided key and value.
    /// </summary>
    /// <param name="key">The key of the entry.</param>
    /// <param name="value">The value of the entry.</param>
    public SerializeableEntry(K key, V value) : base(key, value)
    {
        this.identifier = key.Identifier;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SerializeableEntry{K, V}"/> with default key and value.
    /// </summary>
    public SerializeableEntry() : base()
    {
        this.identifier = null;
    }

    /// <summary>
    /// Indicates whether the key of the entry is valid.
    /// </summary>
    bool HasValidKey => m_Key != null;

    /// <summary>
    /// Indicates whether the identifier of the entry is valid.
    /// </summary>
    bool HasValidIdentifier => identifier != null && identifier != ThingID.m_EMPTY && ThingSerializer.Get(identifier) != null;

    /// <summary>
    /// Syncs the key and identifier of the entry.
    /// </summary>
    public void SyncKeyID()
    {
        if (!HasValidKey && !HasValidIdentifier) return;

        if (HasValidKey)
        {
            ThingSerializer.Register(m_Key);
            identifier = m_Key.Identifier;
        }
        else if (HasValidIdentifier)
        {
            m_Key = ThingSerializer.Get(identifier) as K;
        }
        else
        {
            m_Key = null;
            identifier = null;
        }
    }

    /// <summary>
    /// Performs operations that need to occur after deserialization of the entry.
    /// </summary>
    public void OnAfterDeserialize()
    {
        SyncKeyID();
    }

    /// <summary>
    /// Performs operations that need to occur before serialization of the entry.
    /// </summary>
    public void OnBeforeSerialize()
    {
        SyncKeyID();
    }
}
