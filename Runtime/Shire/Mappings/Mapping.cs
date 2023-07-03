using System;
using System.Collections.Generic;
using UnityEngine;

public class Mapping<K, V> : Shire<Mapping<K, V>>
{
    [Serializable]
    public class ListWrapper
    {
        [SerializeField]
        List<V> list = new();

        public List<V> List { get { return list; } set { list = value; } }
    }

    [SerializeField]
    protected List<K> keys = new();
    [SerializeField]
    protected List<ListWrapper> values = new();

    public List<K> Keys { get { return keys; } set { keys = value; } }
    public List<ListWrapper> Values { get { return values; } set { values = value; } }
    public Dictionary<K, List<V>> Map { 
        get 
        {
            var dict = new Dictionary<K, List<V>>();
            for (int i = 0; i < keys.Count; i++)
            {
                dict.Add(keys[i], values[i].List);
            }
            return dict;
        } 
    }
}
