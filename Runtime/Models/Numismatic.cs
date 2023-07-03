using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all Numismatics that exist in the game.
/// </summary>
/// <remarks>
/// This could be probably implemented as an SDictionary<ANumismatic, AMoney, int>
/// </remarks>
[Serializable]
public class Numismatic : OSatisfier<Numismatic>, ISerializationCallbackReceiver
{
    #region Instance Fields:

    [SerializeField] Currency m_Currency;
    [SerializeField] List<int> quantities = new List<int>();
    [SerializeField] float remainder = 0f;
    [SerializeField] float personalBuyRate = 1f;
    [SerializeField] float personalSellRate = 1f;
    [SerializeField] float value = 0f;

    public static float EPSILON = 0.01f;

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
    #region Instance Properties:

    /// <summary>
    /// The given currency used for this Numismatic.
    /// </summary>
    public virtual Currency Currency { get { return m_Currency; } }

    /// <summary>
    /// The given quantities of each AMoney in the currency for this Numismatic.
    /// </summary>
    public virtual List<int> Quantities { get { return quantities; } }

    /// <summary>
    /// The number of pieces of money in this Numismatic.
    /// </summary>
    public virtual int MoneyCount { get { return Currency.MoneyCount; } }

    /// <summary>
    /// The money pieces in this Numismatic.
    /// </summary>
    public virtual List<Money> MoneyPieces { get { return Currency.MoneyPieces; } }

    /// <summary>
    /// The fixed rate of this Numismatic.
    /// </summary>
    public virtual float FixedRate { get { return Currency.FixedRate; } }
    public virtual float PersonalBuyRate { get { return personalBuyRate; } }
    public virtual float PersonalSellRate { get { return personalSellRate; } }
    public virtual float Price { get { return Value * FixedRate; } }
    public virtual float Remainder { get { return remainder; } }

    /// <summary>
    /// A custom Comparision function for this Numismatic.
    /// </summary>
    public override Comparison<Numismatic> Comparer { get { return (a, b) => b.Value.CompareTo(a.Value); } }

    /// <summary>
    /// A custom Equality function for this Numismatic.
    /// </summary>
    public override Func<Numismatic, Numismatic, bool> Equator { get { return (a, b) => a.Value == b.Value || Math.Abs(a.Value - b.Value) < EPSILON; } }

    #endregion
    #region Initializers:

    /// <summary>
    /// Initializes the Numismatic with the given Currency and Quantities.
    /// </summary>
    /// <param name="currency">The Currency used for this Numismatic.</param>
    /// <param name="quantities">The Quantities of each AMoney in the currency for this Numismatic.</param>
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
    #region Methods:

    /// <summary>
    /// returns the total value of this Numismatic.
    /// </summary>
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
    /// Convert a given float amount of money into pieces of money from this Numismatic's currency.
    /// </summary>
    /// <param name="amount">The amount of money to convert.</param>
    /// <returns>A list of pieces of money from this Numismatic's currency.</returns>
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
    /// Function that given a numismatic with a certain unknown currency, returns wheter or not this numismatic can afford the given numismatic.
    /// </summary>
    /// <param name="numismatic">The numismatic to check if this numismatic can afford.</param>
    /// <returns>True if this numismatic can afford the given numismatic, false otherwise.</returns>
    public virtual bool CanAfford(Numismatic numismatic)
    {
        return Value >= numismatic.Value || Math.Abs(Value - numismatic.Value) <= EPSILON;
    }

    /// <summary>
    /// Gain a specified amount of money in this Numismatic's currency.
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
    /// Spend a specified amount of money in this Numismatic's currency.
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
    /// given a float rate, converts the value * rate into pieces of money from this Numismatic's currency.
    /// </summary>
    /// <param name="rate">The rate to convert the value by.</param>
    /// <returns>A list of pieces of money from this Numismatic's currency.</returns>
    public virtual Tuple<List<int>, float> ConvertRate(float rate)
    {
        return Convert(Value * rate);
    }

    public virtual Numismatic SellValue(float value)
    {
        return new Numismatic(m_Currency, ConvertRate(value * personalSellRate));
    }

    public virtual Numismatic BuyValue(float value)
    {
        return new Numismatic(m_Currency, ConvertRate(value * personalBuyRate));
    }

    /// <summary>
    /// Given an external rate, return a new Numismatic with the same currency as this Numismatic, but with the value * rate.
    /// </summary>
    /// <param name="rate">The rate to convert the value by.</param>
    /// <returns>A new Numismatic with the same currency as this Numismatic, but with the value * rate.</returns>
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

