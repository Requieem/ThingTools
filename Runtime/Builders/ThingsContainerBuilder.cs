using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract builder for creating instances of type <typeparamref name="T"/>, which are containers for Things.
/// </summary>
/// <typeparam name="D">The type of the builder itself, must inherit from <see cref="ThingsContainerBuilder{D,T,K,V,B}"/>.</typeparam>
/// <typeparam name="T">The type of the ThingsContainer that this builder can build.</typeparam>
/// <typeparam name="K">The type of the ScriptableThing used as a key in the container.</typeparam>
/// <typeparam name="V">The type of the builder used to build the objects in the container.</typeparam>
/// <typeparam name="B">The type of the built objects in the container.</typeparam>
[Serializable]
public abstract class ThingsContainerBuilder<D, T, K, V, B> : ThingBuilder<D, T> where D : ThingsContainerBuilder<D, T, K, V, B> where T : ThingsContainer<K, B> where K : ScriptableThing<K> where V : IBuilder<B>
{
    /// <summary>
    /// Represents an entry in a ThingContainer, with a key of type <typeparamref name="K"/> and a value of type <typeparamref name="V"/>.
    /// </summary>
    [Serializable]
    public class TemplateObject : ThingEntry<K, V> { }

    /// <summary>
    /// The list of TemplateObjects that will be built into the ThingsContainer.
    /// </summary>
    [SerializeField] protected List<TemplateObject> m_Objects = new();

    /// <summary>
    /// Gets the list of built objects in the ThingsContainer.
    /// </summary>
    /// <value>A list of ThingEntry with key of type <typeparamref name="K"/> and value of type <typeparamref name="B"/>.</value>
    public List<ThingEntry<K, B>> BuiltObjects => m_Objects.ConvertAll(x => new ThingEntry<K, B>(x.m_Key, x.m_Value.GetCopy()));

    /// <summary>
    /// Gets the list of TemplateObjects that will be built into the ThingsContainer.
    /// </summary>
    /// <value>A list of TemplateObjects.</value>
    public List<TemplateObject> Objects { get { return m_Objects; } }
}
