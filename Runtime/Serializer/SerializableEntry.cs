using System;
using UnityEngine;

[Serializable]
public class SerializeableEntry<K, V> : ThingEntry<K, V>, ISerializationCallbackReceiver where K : ScriptableThing<K>
{
    [SerializeField] private ThingID identifier = null;

    public SerializeableEntry(K key, V value) : base(key, value)
    {
        this.identifier = key.Identifier;
    }

    public SerializeableEntry() : base()
    {
        this.identifier = null;
    }

    bool HasValidKey => m_Key != null;
    bool HasValidIdentifier => identifier != null && identifier != ThingID.EMPTY && ThingSerializer.Get(identifier) != null;

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

    public void OnAfterDeserialize()
    {
        SyncKeyID();
    }

    public void OnBeforeSerialize()
    {
        SyncKeyID();
    }
}