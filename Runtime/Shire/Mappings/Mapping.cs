using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic mapping class that associates keys of type K with lists of values of type V.
/// </summary>
/// <typeparam name="K">The type of the keys.</typeparam>
/// <typeparam name="V">The type of the values.</typeparam>
public class Mapping<K, V> : Shire<Mapping<K, V>>
{
    [Serializable]
    public class ListWrapper
    {
        [SerializeField]
        private List<V> list = new List<V>();
        /// <summary>
        /// The list of values.
        /// </summary>
        public List<V> List { get { return list; } set { list = value; } }
    }

    [SerializeField]
    protected List<K> m_Keys = new List<K>();
    [SerializeField]
    protected List<ListWrapper> m_Values = new List<ListWrapper>();

    /// <summary>
    /// The list of keys.
    /// </summary>
    public List<K> Keys => m_Keys;

    /// <summary>
    /// The list of value wrappers.
    /// </summary>
    public List<ListWrapper> Values => m_Values;

    /// <summary>
    /// Gets a dictionary that maps keys to lists of values.
    /// </summary>
    public Dictionary<K, List<V>> Map
    {
        get
        {
            var dict = new Dictionary<K, List<V>>();
            for (int i = 0; i < m_Keys.Count; i++)
            {
                dict.Add(m_Keys[i], m_Values[i].List);
            }
            return dict;
        }
    }
}