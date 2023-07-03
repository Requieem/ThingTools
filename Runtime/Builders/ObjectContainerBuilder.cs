using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class ObjectContainerBuilder<D, T, V, B> : ThingBuilder<D, T> where T : ObjectContainer<B> where V : IBuilder<B> where D : ObjectContainerBuilder<D, T, V, B>
{
    [SerializeField] List<V> m_Objects = new ();
    
    public List<V> Objects => m_Objects;
    public List<B> BuiltObjects => m_Objects.ConvertAll(x => x.GetCopy());
}