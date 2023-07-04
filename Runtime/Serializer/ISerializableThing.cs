using UnityEngine;

/// <summary>
/// Defines an interface for things that can be serialized.
/// </summary>
public interface ISerializableThing
{
    /// <summary>
    /// Gets or sets the identifier of the thing.
    /// </summary>
    [SerializeField]
    public ThingID Identifier { get; set; }

    /// <summary>
    /// Ensures the thing has an identifier for persistence.
    /// </summary>
    public virtual void Persist()
    {
        Identifier ??= new();
    }

    /// <summary>
    /// Changes the identifier of the thing to a new identifier. This is generally called when duplicating the thing.
    /// </summary>
    public virtual void OnDuplicate()
    {
        Identifier = new();
    }
}
