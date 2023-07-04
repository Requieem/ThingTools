using UnityEngine;

/// <summary>
/// An interface for objects that require initialization with a Transform.
/// </summary>
public interface IInitializable
{
    /// <summary>
    /// Initializes the object with the given Transform.
    /// </summary>
    /// <param name="_transform">The Transform to use for initialization.</param>
    void Initialize(Transform _transform);
}
