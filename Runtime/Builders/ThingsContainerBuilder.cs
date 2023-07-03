using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class ThingsContainerBuilder<D, T, K, V, B> : ThingBuilder<D, T> where D : ThingsContainerBuilder<D, T, K, V, B> where T : ThingsContainer<K, B> where K : ScriptableThing<K> where V : IBuilder<B>
{
    [Serializable]
    public class TemplateObject : ThingEntry<K, V> { }

    [SerializeField] protected List<TemplateObject> m_Objects = new();
    public List<ThingEntry<K, B>> BuiltObjects => m_Objects.ConvertAll(x => new ThingEntry<K, B>(x.m_Key, x.m_Value.GetCopy()));

    public List<TemplateObject> Objects { get { return m_Objects; } }
}