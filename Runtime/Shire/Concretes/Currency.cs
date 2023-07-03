using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// An abstract base class for all Currencies that exist in the game.
/// </summary>
[CreateAssetMenu(fileName = "Currency", menuName = "ShireSoft/Numismatic/Currency", order = 0)]
public class Currency : Shire<Currency>
{
    #region Instance Fields:

    [SerializeField]
    List<Money> m_MoneyPieces = new();
    [SerializeField]
    float fixedRate;

    public static Currency UNIT
    {
        get
        {
            var unit = ScriptableObject.CreateInstance<Currency>();
            var piece = ScriptableObject.CreateInstance<Money>();
            piece.Initialize(1);
            unit.Initialize(new List<Money>() { piece }, 1);
            return unit;
        }
    }

    #endregion
    #region Instance Properties:

    /// <summary>
    /// The given fixedRate for this piece of Currency.
    /// This is relative to the unit value of 1 which is the base value of all currencies.
    /// </summary>
    public virtual float FixedRate { get { return fixedRate; } }

    /// <summary>
    /// The list of Money pieces that are part of this Currency.
    /// </summary>
    public virtual List<Money> MoneyPieces { get { return m_MoneyPieces; } }

    /// <summary>
    /// The count of Money pieces that are part of this Currency.
    /// </summary>
    public virtual int MoneyCount { get { return MoneyPieces.Count; } }

    #endregion

    #region Initializers:

    /// <summary>
    /// Initializes the Currency with the given description, sprite, color, known Currencys, and relations.
    /// </summary>
    /// <param name="fixedRate">The fixedRate of this piece of Currency.</param>
    public void Initialize(float fixedRate)
    {
        this.m_Displayable = new();
        this.fixedRate = fixedRate;
        this.m_MoneyPieces = new List<Money>();
    }

    /// <summary>
    /// Initialized the Currency with the given fixedRate and moneyPieces list
    /// </summary>
    /// <param name="moneyPieces">The list of money pieces that make up this currency</param>
    /// <param name="fixedRate">The fixedRate of this currency against the unitary rate of 1</param>
    public void Initialize(List<Money> moneyPieces, float fixedRate)
    {
        this.m_Displayable = new();
        this.m_MoneyPieces.AddRange(moneyPieces);
        this.fixedRate = fixedRate;
    }

    #endregion

    #region Methods:

    /// <summary>
    /// Adds the given piece of Money to this Currency.
    /// </summary>
    /// <param name="money">The piece of Money to add to this Currency.</param>
    public virtual bool AddMoney(Money money)
    {
        if (money != null)
        {
            if (!m_MoneyPieces.Contains(money) && m_MoneyPieces.Where((a) => a.Rate == money.Rate).Count() == 0)
            {
                m_MoneyPieces.Add(money);
                m_MoneyPieces.Sort((a, b) => a.Rate.CompareTo(b.Rate));
                return true;
            }
            else { return false; }
        }
        else { return false; }
    }

    /// <summary>
    /// Removes the given piece of Money from this Currency.
    /// </summary>
    /// <param name="money">The piece of Money to remove from this Currency.</param>
    public virtual bool RemoveMoney(Money money)
    {
        if (money != null)
        {
            return m_MoneyPieces.Remove(money);
        }
        else { return false; }
    }

    #endregion
}

