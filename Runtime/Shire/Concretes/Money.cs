using UnityEngine;

/// <summary>
/// An abstract base class for all Moneys that exist in the game.
/// </summary>
[CreateAssetMenu(fileName = "Money", menuName = "ShireSoft/Numismatic/Money", order = 0)]
public class Money : Shire<Money>
{
    #region Instance Fields:

    [SerializeField]
    float rate;

    #endregion
    #region Instance Properties:

    /// <summary>
    /// The given rate for this piece of Money.
    /// This is relative to the relative rate of the Currency this piece of money is part of.
    /// </summary>
    public virtual float Rate { get { return rate; } }

    #endregion
    #region Initializers:

    /// <summary>
    /// Initializes the Money with the given description, sprite, color, known Moneys, and relations.
    /// </summary>
    /// <param name="description">The description of the Money.</param>
    /// <param name="sprite">The sprite representing the Money.</param>
    /// <param name="color">The color associated with the Money.</param>
    /// <param name="rate">The rate of this piece of Money.</param>
    public virtual void Initialize(Displayable displayable, float rate)
    {
        base.Initialize(displayable);
        this.rate = rate;
    }

    /// <summary>
    /// Initializes the Money with a rate only, setting all other values to default.
    /// </summary>
    /// <param name="rate">The rate of this piece of Money.</param>
    public virtual void Initialize(float rate)
    {
        base.Initialize(new());
        this.rate = rate;
    }

    #endregion
}

