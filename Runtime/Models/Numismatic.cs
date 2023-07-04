using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all numismatics in the game.
/// </summary>
/// <remarks>
/// This could potentially be implemented as an SDictionary&lt;ANumismatic, AMoney, int&gt;.
/// </remarks>
[Serializable]
public class Numismatic : OSatisfier<Numismatic>, ISerializationCallbackReceiver
{
    #region Instance Fields

    [SerializeField] Currency m_Currency;
    [SerializeField] List<int> quantities = new List<int>();
    [SerializeField] float remainder = 0f;
    [SerializeField] float personalBuyRate = 1f;
    [SerializeField] float personalSellRate = 1f;
    [SerializeField] float value = 0f;

    public static float m_EPSILON = 0.01f;

    public static Numismatic UNIT
    {
        get
        {
            var currency = Currency.UNIT;
            var quantities = new List<int>() { 0 };
            var remainder = 0f;
            var tuple = new Tuple<List<int>, float>(quantities, remainder);
            var unit = new Numismatic(currency, tuple);
            return unit;
        }
    }

    #endregion

    #region Instance Properties

    /// <summary>
    /// The currency used for this numismatic.
    /// </summary>
    public virtual Currency Currency { get { return m_Currency; } }

    /// <summary>
    /// The quantities of each AMoney in the currency for this numismatic.
    /// </summary>
    public virtual List<int> Quantities { get { return quantities; } }

    /// <summary>
    /// The number of money pieces in this numismatic.
    /// </summary>
    public virtual int MoneyCount { get { return Currency.MoneyCount; } }

    /// <summary>
    /// The money pieces in this numismatic.
    /// </summary>
    public virtual List<Money> MoneyPieces { get { return Currency.MoneyPieces; } }

    /// <summary>
    /// The fixed rate of this numismatic.
    /// </summary>
    public virtual float FixedRate { get { return Currency.FixedRate; } }

    /// <summary>
    /// The personal buy rate of this numismatic.
    /// </summary>
    public virtual float PersonalBuyRate { get { return personalBuyRate; } }

    /// <summary>
    /// The personal sell rate of this numismatic.
    /// </summary>
    public virtual float PersonalSellRate { get { return personalSellRate; } }

    /// <summary>
    /// The price of this numismatic based on its value and the fixed rate.
    /// </summary>
    public virtual float Price { get { return Value * FixedRate; } }

    /// <summary>
    /// The remainder of the numismatic.
    /// </summary>
    public virtual float Remainder { get { return remainder; } }

    /// <summary>
    /// A custom comparison function for this numismatic.
    /// </summary>
    public override Comparison<Numismatic> Comparer { get { return (a, b) => b.Value.CompareTo(a.Value); } }

    /// <summary>
    /// A custom equality function for this numismatic.
    /// </summary>
    public override Func<Numismatic, Numismatic, bool> Equator { get { return (a, b) => a.Value == b.Value || Math.Abs(a.Value - b.Value) < m_EPSILON; } }

    #endregion

    #region Initializers

    /// <summary>
    /// Initializes the numismatic with the given currency and quantities.
    /// </summary>
    /// <param name="currency">The currency used for this numismatic.</param>
    /// <param name="quantities">The quantities of each AMoney in the currency for this numismatic.</param>
    public Numismatic(Currency currency, Tuple<List<int>, float> _quantities = null)
    {
        this.m_Currency = currency;
        var quantities = _quantities?.Item1;
        var remainder = _quantities?.Item2;

        this.remainder = remainder ?? 0f;

        if (MoneyCount != quantities?.Count)
        {
            /* 
             throw new ArgumentException("The Currency Money Count must be equal to the Count of Quantities"); 
             throwing is kind of useless here, I will never want to catch and handle this
            */

            var actualQuantities = new List<int>(MoneyCount);
            for (int i = 0; i < actualQuantities.Count; i++)
            {
                if (quantities != null && i < quantities.Count)
                {
                    actualQuantities[i] = quantities[i];
                }
                else
                {
                    actualQuantities[i] = 0;
                }
            }

            this.quantities.AddRange(actualQuantities);
            Log.Wng("Tried to initialize a numismatic with an incoherent currency and quantity values. This is taken care of internally but you may want to check this Initialization call since this is not supposed to happen.");
        }
        else
        {
            this.quantities = quantities;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Returns the total value of this numismatic.
    /// </summary>
    /// <returns>The total value of this numismatic.</returns>
    public virtual float Value
    {
        get
        {
            if (quantities == null || quantities.Count == 0)
            {
                var tuple = Convert(0);
                quantities = tuple.Item1;
                remainder = tuple.Item2;
                return 0;
            }

            float value = 0;

            for (int i = 0; i < MoneyCount; i++)
            {
                value += MoneyPieces[i].Rate * quantities[i];
            }
            return (value + remainder) * FixedRate;
        }
        set
        {
            var tuple = Convert(value);
            quantities = tuple.Item1;
            remainder = tuple.Item2;
        }
    }

    /// <summary>
    /// Converts a given amount of money into pieces of money from this numismatic's currency.
    /// </summary>
    /// <param name="amount">The amount of money to convert.</param>
    /// <returns>A tuple containing the list of quantities of each money piece and the remaining amount.</returns>
    public virtual Tuple<List<int>, float> Convert(float amount)
    {
        List<int> _quantities = new(new int[MoneyCount]);
        amount /= FixedRate;
        int i = _quantities.Count;
        for (i = _quantities.Count - 1; i >= 0; i--)
        {
            _quantities[i] = Mathf.FloorToInt(amount / MoneyPieces[i].Rate);
            amount -= _quantities[i] * MoneyPieces[i].Rate;
        }

        return new Tuple<List<int>, float>(_quantities, amount);
    }

    /// <summary>
    /// Checks whether this numismatic can afford a given numismatic.
    /// </summary>
    /// <param name="numismatic">The numismatic to check.</param>
    /// <returns>True if this numismatic can afford the given numismatic, false otherwise.</returns>
    public virtual bool CanAfford(Numismatic numismatic)
    {
        return Value >= numismatic.Value || Math.Abs(Value - numismatic.Value) <= m_EPSILON;
    }

    /// <summary>
    /// Increases the quantity of money in this numismatic's currency by the specified amount.
    /// </summary>
    /// <param name="amount">The amount of money to gain.</param>
    public virtual void Gain(float amount)
    {
        Tuple<List<int>, float> quantities = Convert(amount);
        for (int i = 0; i < MoneyCount; i++)
        {
            this.quantities[i] += quantities.Item1[i];
        }

        this.remainder += quantities.Item2;

        CheckSatisfyOnChange(this);
    }

    /// <summary>
    /// Decreases the quantity of money in this numismatic's currency by the specified amount.
    /// </summary>
    /// <param name="amount">The amount of money to spend.</param>
    public virtual void Spend(float amount)
    {
        Tuple<List<int>, float> quantities = Convert(amount);
        for (int i = 0; i < MoneyCount; i++)
        {
            this.quantities[i] -= quantities.Item1[i];
        }

        this.remainder -= quantities.Item2;

        CheckUnsatisfyOnChange(this);
    }

    /// <summary>
    /// Converts the value of this numismatic multiplied by the specified rate into pieces of money from this numismatic's currency.
    /// </summary>
    /// <param name="rate">The rate to convert the value by.</param>
    /// <returns>A tuple containing the list of quantities of each money piece and the remaining amount.</returns>
    public virtual Tuple<List<int>, float> ConvertRate(float rate)
    {
        return Convert(Value * rate);
    }

    /// <summary>
    /// Creates a new numismatic with the same currency as this numismatic, but with a value equal to the specified value multiplied by the personal sell rate.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A new numismatic with the same currency as this numismatic, but with a value equal to the specified value multiplied by the personal sell rate.</returns>
    public virtual Numismatic SoldPrice(float value)
    {
        return new Numismatic(m_Currency, ConvertRate(value * personalSellRate));
    }

    /// <summary>
    /// Creates a new numismatic with the same currency as this numismatic, but with a value equal to the specified value multiplied by the personal buy rate.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A new numismatic with the same currency as this numismatic, but with a value equal to the specified value multiplied by the personal buy rate.</returns>
    public virtual Numismatic BoughtPrice(float value)
    {
        return new Numismatic(m_Currency, ConvertRate(value * personalBuyRate));
    }

    /// <summary>
    /// Creates a new numismatic with the same currency as this numismatic, but with the value multiplied by the specified rate.
    /// </summary>
    /// <param name="rate">The rate to convert the value by.</param>
    /// <returns>A new numismatic with the same currency as this numismatic, but with the value multiplied by the specified rate.</returns>
    public virtual Numismatic ApplyRate(float rate)
    {
        return new Numismatic(m_Currency, ConvertRate(rate));
    }

    public override bool CheckSatisfyOnChange(Numismatic obj)
    {
        EnsureSatisfier();
        var res = false;
        // get the satisfier enumerator
        var bundles = Satisfier.GetEnumerator();
        // while there are satisfier bundles to check
        while (bundles.MoveNext())
        {
            // if the current satisfier bundle can be satisfied
            if (obj.CanAfford(bundles.Current.m_WatchedObject))
            {
                // satisfy the current satisfier bundle
                bundles.Current.Satisfy();
                res = true;
            }
            // else if the current satisfier bundle cannot be satisfied
            else
            {
                // break out of the loop
                break;
            }
        }
        return res;
    }

    public override bool CheckUnsatisfyOnChange(Numismatic obj)
    {
        EnsureSatisfier();
        var res = false;
        // get the satisfier reverse enumerator 
        var bundles = Satisfier.GetEnumerator();

        // while there are satisfier bundles to check
        while (bundles.MoveNext())
        {
            // if the current satisfier bundle cannot be satisfied
            if (!obj.CanAfford(bundles.Current.m_WatchedObject))
            {
                // satisfy the current satisfier bundle
                bundles.Current.Unsatisfy();
                res = true;
            }
            // else if the current satisfier bundle can be satisfied
            else
            {
                // break out of the loop
                break;
            }
        }
        return res;
    }

    public override float WatchFeedback(Numismatic obj, UnityEvent onSatisfy, UnityEvent onUnsatisfy)
    {
        Satisfier.Watch(obj, onSatisfy, onUnsatisfy);
        return CanAfford(obj) ? 1 : -1;
    }

    void SyncValue()
    {
        if (m_Currency == null)
        {
            quantities = new List<int>();
        }
        else
        {
            (quantities, remainder) = Convert(value);
        }
    }

    public void OnBeforeSerialize()
    {
        SyncValue();
    }

    public void OnAfterDeserialize()
    {
        SyncValue();
    }

    #endregion

}

