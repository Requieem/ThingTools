using System;
using UnityEngine;

/// <summary>
/// A base class for all ScriptableObjects that can be displayed in some sort of UI in the game.
/// </summary>
[Serializable]
public class Displayable
{
    #region Instance Fields

    [SerializeField] private Color color = Color.white;
    [SerializeField] private Sprite sprite;
    [SerializeField][TextAreaAttribute] private string description;

    #endregion

    #region Instance Properties

    /// <summary>
    /// Gets the description of the displayable object.
    /// </summary>
    public string Description => description;

    /// <summary>
    /// Gets the sprite associated with the displayable object.
    /// </summary>
    public Sprite Sprite => sprite;

    /// <summary>
    /// Gets the color of the displayable object.
    /// </summary>
    public Color Color => color;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Displayable"/> class.
    /// </summary>
    public Displayable() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Displayable"/> class with the specified description, sprite, and color.
    /// </summary>
    /// <param name="description">The description of the displayable object.</param>
    /// <param name="sprite">The sprite associated with the displayable object.</param>
    /// <param name="color">The color of the displayable object.</param>
    public Displayable(string description, Sprite sprite, Color color)
    {
        this.description = description;
        this.sprite = sprite;
        this.color = color;
    }

    #endregion
}
