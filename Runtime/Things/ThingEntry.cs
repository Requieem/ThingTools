using System;
using UnityEngine;

[Serializable]
public class ThingEntry<K, V>
{
    [SerializeField]
    public K m_Key;
    [SerializeField]
    public V m_Value;

    public ThingEntry(K key, V value)
    {
        this.m_Key = key;
        this.m_Value = value;
    }

    public ThingEntry()
    {
        this.m_Key = default;
        this.m_Value = default;
    }
}