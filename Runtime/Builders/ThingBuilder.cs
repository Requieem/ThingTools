using UnityEngine;

/// <summary>
/// Implementation of a builder for creating instances of type <typeparamref name="T"/>, known as "Thing".
/// </summary>
/// <typeparam name="D">The type of the builder itself, must inherit from <see cref="ThingBuilder{D,T}"/>.</typeparam>
/// <typeparam name="T">The type of the Thing that this builder can build. Must be serializable by <see cref="JsonUtility"/>.</typeparam>
public class ThingBuilder<D, T> : Builder<D, T> where D : ThingBuilder<D, T>
{
    /// <summary>
    /// Gets a copy of the built Thing. The copy is created by serializing and deserializing the existing Thing.
    /// </summary>
    /// <returns>A copy of the built Thing of type <typeparamref name="T"/>.</returns>
    /// <remarks>Uses JSON serialization to ensure a deep copy. This implies <typeparamref name="T"/> must be serializable by <see cref="JsonUtility"/>.</remarks>
    public override T GetCopy()
    {
        var json = JsonUtility.ToJson(m_Built);
        var copy = JsonUtility.FromJson<T>(json);
        return copy;
    }
}