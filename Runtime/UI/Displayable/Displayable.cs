using System;
using System.IO;
using UnityEngine;

/// <summary>
/// A base class for all ScriptableObjects that can be displayed in some sort of UI in the game.
/// </summary>
[Serializable]
public class Displayable
{
    #region Instance Fields:

    [SerializeField] Color color = Color.white;
    [SerializeField] Sprite sprite;
    [SerializeField] [TextAreaAttribute] string description;

    #endregion

    #region Instance Properties:

    /// <summary>
    /// Gets the description of the displayable object.
    /// </summary>
    public string Description { get { return description; } }

    /// <summary>
    /// Gets the sprite associated with the displayable object.
    /// </summary>
    public Sprite Sprite { get { return sprite; } }

    /// <summary>
    /// Gets the color of the displayable object.
    /// </summary>
    public Color Color { get { return color; } }

    #endregion

    #region Constructors:

    public Displayable() { }

    /// <summary>
    /// Initializes the displayable object with the specified description, sprite, and color.
    /// </summary>
    /// <param name="description">The description of the displayable object.</param>
    /// <param name="sprite">The sprite associated with the displayable object.</param>
    /// <param name="color">The color of the displayable object.</param>
    /// <param name="obj">The T object this displayable is attached to.</param>
    /// <param name="doDisplay">The event that is invoked when the displayable object is displayed.</param>
    /// <param name="doHide">The event that is invoked when the displayable object is hidden.</param>
    public Displayable(string description, Sprite sprite, Color color)
    {
        this.description = description;
        this.sprite = sprite;
        this.color = color;
    }

    #endregion
}
