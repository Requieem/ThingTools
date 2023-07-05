using UnityEngine;

/// <summary>
/// Interface for a builder that is also a scriptable thing.
/// </summary>
public interface IBuilder : IScriptableThing { }

/// <summary>
/// A builder that can generate instances of type <typeparamref name="T"/> and is itself a scriptable thing of type <typeparamref name="D"/>.
/// </summary>
/// <typeparam name="D">The type of the builder itself, must inherit from <see cref="Builder{D,T}"/>.</typeparam>
/// <typeparam name="T">The type of the instance that this builder can build.</typeparam>
public abstract class Builder<D, T> : ScriptableThing<D>, IBuilder, IBuilder<T> where D : Builder<D, T>
{
    [SerializeField]
    /// <summary>
    /// The built instance of type <typeparamref name="T"/>.
    /// </summary>
    protected T m_Built;

    /// <summary>
    /// Gets the built instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <value>The built instance of type <typeparamref name="T"/>.</value>
    public T Built { get { return m_Built; } }

    /// <summary>
    /// Gets a copy of the built instance.
    /// </summary>
    /// <returns>A copy of the built instance of type <typeparamref name="T"/>.</returns>
    public abstract T GetCopy();

    /// <summary>
    /// Gets a safe copy of an instance from the specified builder.
    /// </summary>
    /// <typeparam name="V">The type of the instance to copy.</typeparam>
    /// <typeparam name="K">The type of the builder to use for copying.</typeparam>
    /// <param name="builder">The builder to use for copying.</param>
    /// <returns>A safe copy of the instance if the builder is not null; otherwise, the default value of type <typeparamref name="V"/>.</returns>
    public V GetSafeCopy<V, K>(K builder) where K : Builder<K, V>
    {
        if (builder is not null)
        {
            return builder.GetCopy();
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Gets an instance of type <typeparamref name="T"/>. If no instance is currently built, a new one is built and stored.
    /// </summary>
    /// <returns>An instance of type <typeparamref name="T"/>.</returns>
    public virtual T GetInstance()
    {
        if (m_Built == null)
        {
            m_Built = GetCopy();
        }

        return m_Built;
    }
}