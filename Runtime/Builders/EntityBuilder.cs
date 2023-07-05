using UnityEngine;

/// <summary>
/// An abstract base class for building entities.
/// </summary>
/// <typeparam name="D">The derived type of the entity builder.</typeparam>
/// <typeparam name="T">The type of the entity being built.</typeparam>
public abstract class EntityBuilder<D, T> : Builder<D, T> where D : EntityBuilder<D, T>
{
    [SerializeField] protected string m_AssignedName = string.Empty;

    /// <summary>
    /// The assigned name for the entity being built.
    /// </summary>
    public string AssignedName => m_AssignedName;
}
