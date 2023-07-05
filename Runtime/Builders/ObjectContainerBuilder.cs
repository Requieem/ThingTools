using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Builder for creating instances of type <see cref="ObjectContainer{B}"/>. This is an abstract class to be inherited by specific container builders.
/// </summary>
/// <typeparam name="D">The specific type of builder being implemented.</typeparam>
/// <typeparam name="T">The specific type of container being built.</typeparam>
/// <typeparam name="V">The builder for the objects contained.</typeparam>
/// <typeparam name="B">The type of the objects being built.</typeparam>
[Serializable]
public abstract class ObjectContainerBuilder<D, T, V, B> : ThingBuilder<D, T>
    where T : ObjectContainer<B>
    where V : IBuilder<B>
    where D : ObjectContainerBuilder<D, T, V, B>
{
    /// <summary>
    /// The list of builders for the objects contained in the container.
    /// </summary>
    [SerializeField] List<V> m_Objects = new();

    /// <summary>
    /// Gets the list of builders for the objects.
    /// </summary>
    public List<V> Objects => m_Objects;

    /// <summary>
    /// Gets the list of built objects contained in the container.
    /// </summary>
    public List<B> BuiltObjects => m_Objects.ConvertAll(x => x.GetCopy());
}
