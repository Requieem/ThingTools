using UnityEngine;

/// <summary>
/// Interface for the serialization callbacks of the lister of scriptable objects of type <typeparamref name="D"/>.
/// </summary>
/// <typeparam name="D">The type of the elements in the lister. Must inherit from <see cref="ScriptableObject"/> and implement the <see cref="ILister{D}"/> interface.</typeparam>
public interface ILister<D> : ISerializationCallbackReceiver where D : ScriptableObject, ILister<D>
{
    /// <summary>
    /// Gets or sets the lister of the scriptable object.
    /// </summary>
    Lister<D> Lister { get; set; }

    /// <summary>
    /// Serialize method to be implemented by classes that implement this interface.
    /// </summary>
    void Serialize();
}