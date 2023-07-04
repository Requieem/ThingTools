using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// An abstract base class for all Currencies that exist in the game.
/// </summary>
[CreateAssetMenu(fileName = "Currency", menuName = "ShireSoft/Numismatic/Currency", order = 0)]
public class Currency : Shire<Currency>
{
    #region Instance Fields
    [SerializeField]
    private List<Money> moneyPieces = new List<Money>();
    [SerializeField]
    private float fixedRate;

    #endregion

    public static Currency UNIT
    {
        get
        {
            var unit = CreateInstance<Currency>();
            var piece = CreateInstance<Money>();
            piece.Initialize(1);
            unit.Initialize(new List<Money>() { piece }, 1);
            return unit;
        }
    }

    #region Instance Properties

    /// <summary>
    /// The fixed rate for this piece of Currency.
    /// This is relative to the unit value of 1, which is the base value of all currencies.
    /// </summary>
    public float FixedRate => fixedRate;

    /// <summary>
    /// The list of Money pieces that are part of this Currency.
    /// </summary>
    public List<Money> MoneyPieces => moneyPieces;

    /// <summary>
    /// The count of Money pieces that are part of this Currency.
    /// </summary>
    public int MoneyCount => moneyPieces.Count;

    #endregion

    #region Initializers

    /// <summary>
    /// Initializes the Currency with the given fixed rate.
    /// </summary>
    /// <param name="fixedRate">The fixed rate of this piece of Currency.</param>
    public void Initialize(float fixedRate)
    {
        m_Displayable = new Displayable();
        this.fixedRate = fixedRate;
        moneyPieces = new List<Money>();
    }

    /// <summary>
    /// Initializes the Currency with the given fixed rate and money pieces list.
    /// </summary>
    /// <param name="moneyPieces">The list of money pieces that make up this Currency.</param>
    /// <param name="fixedRate">The fixed rate of this Currency against the unitary rate of 1.</param>
    public void Initialize(List<Money> moneyPieces, float fixedRate)
    {
        m_Displayable = new Displayable();
        this.moneyPieces.AddRange(moneyPieces);
        this.fixedRate = fixedRate;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds the given piece of Money to this Currency.
    /// </summary>
    /// <param name="money">The piece of Money to add to this Currency.</param>
    /// <returns>Returns true if the Money was successfully added, false otherwise.</returns>
    public bool AddMoney(Money money)
    {
        if (money != null)
        {
            if (!moneyPieces.Contains(money) && moneyPieces.Where(a => a.Rate == money.Rate).Count() == 0)
            {
                moneyPieces.Add(money);
                moneyPieces.Sort((a, b) => a.Rate.CompareTo(b.Rate));
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Removes the given piece of Money from this Currency.
    /// </summary>
    /// <param name="money">The piece of Money to remove from this Currency.</param>
    /// <returns>Returns true if the Money was successfully removed, false otherwise.</returns>
    public bool RemoveMoney(Money money)
    {
        if (money != null)
        {
            return moneyPieces.Remove(money);
        }
        else
        {
            return false;
        }
    }

    #endregion

}