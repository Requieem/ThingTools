using System;
using UnityEngine;

/// <summary>
/// Represents a unique identifier for a Thing.
/// </summary>
[Serializable]
public class ThingID
{
    #region Fields

    private readonly Guid guid;
    [SerializeField] private string id;

    #endregion

    #region Properties

    /// <summary>
    /// Represents an empty ThingID.
    /// </summary>
    public static readonly ThingID m_EMPTY = new(Guid.Empty);

    /// <summary>
    /// Gets the string representation of the ThingID.
    /// </summary>
    public string Id => id;

    /// <summary>
    /// Gets the GUID (Globally Unique Identifier) of the ThingID.
    /// </summary>
    public Guid Guid => guid;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new ThingID based on an existing ThingID.
    /// </summary>
    /// <param name="thingID">The existing ThingID.</param>
    public ThingID(ThingID thingID)
    {
        guid = thingID.Guid;
        id = thingID.Id;
    }

    /// <summary>
    /// Creates a new ThingID from a given GUID.
    /// </summary>
    /// <param name="_guid">The GUID to base the ThingID on.</param>
    public ThingID(Guid _guid)
    {
        guid = _guid;
        id = guid.ToString();
    }

    /// <summary>
    /// Creates a new ThingID with a new GUID.
    /// </summary>
    public ThingID()
    {
        guid = Guid.NewGuid();
        id = guid.ToString();
    }

    #endregion
}
