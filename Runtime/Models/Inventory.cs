using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : ObjectContainer<Item>
{
    #region Instance Fields:
    [SerializeField] Numismatic m_Wallet;
    #endregion

    public Numismatic Wallet { get { return m_Wallet; } }

    /// <summary>
    /// Buys the given object with this Wallet from the given seller (AInventory), with the given price.
    /// </summary>
    /// <param name="obj">The object to buy.</param>
    /// <param name="seller">The seller to buy from.</param>
    public virtual bool Buy(Item obj)
    {
        if (obj == null || obj.Inventory == null)
        {
            Log.Msg("Could not buy " + obj?.Name + " because the seller or object was null.");
            return false;
        }


        var price = obj.Price.ApplyRate(m_Wallet.PersonalBuyRate);
        if (m_Wallet.CanAfford(price) && obj.Inventory.Sell(obj, price))
        {
            m_Wallet.Spend(price.Value);
            Add(obj);
            return true;
        }
        else
        {
            Log.Msg("Could not buy " + obj?.Name + " because you cannot afford it, or there was a problem with the transaction.");
            return false;
        }
    }

    /// <summary>
    /// Buys the given object with this Wallet from the given seller (AInventory), with the given price.
    /// </summary>
    /// <param name="obj">The object to buy.</param>
    /// <param name="seller">The seller to buy from.</param>
    public virtual bool Gain(Item obj)
    {
        if (obj == null)
        {
            Log.Msg("Could not gain " + obj?.Name + " because the seller, object, or price was null.");
            return false;
        }
        else
        {
            Add(obj);
            return true;
        }
    }

    /// <summary>
    /// Sells the given object with this Wallet to the given buyer (AWallet).
    /// </summary>
    /// <param name="obj">The object to sell.</param>
    /// <param name="buyer">The buyer to sell to.</param>
    public virtual bool Sell(Item obj, Numismatic price)
    {
        if (price == null || obj == null)
            return false;
        else if (price != null && Remove(obj))
        {
            m_Wallet.Gain(price.Value);
            return true;
        }
        else
        {
            Log.Msg("Could not sell " + obj?.Name + " because it is not in this inventory, or the price is invalid.");
            return false;
        }
    }

    /// <summary>
    /// Loses the given object from the inventory;
    /// </summary>
    /// <param name="obj">The object to sell.</param>
    /// <param name="buyer">The buyer to sell to.</param>
    public virtual bool Lose(Item obj)
    {
        if (obj == null)
            return false;
        else if (Remove(obj))
        {
            return true;
        }
        else
        {
            Log.Msg("Could not loose " + obj?.Name + " because it is not in this inventory.");
            return false;
        }
    }

    public Inventory(InventoryBuilder builder)
    {
        var actualItems = builder.BuiltObjects;
        base.Initialize(actualItems);
        this.m_Wallet = builder.Wallet;
    }

    public Inventory(List<Item> objects, Numismatic _money)
    {
        base.Initialize(objects);
        this.m_Wallet = _money;
    }

    /// <summary>
    /// A custom Equality function for this Inventory.
    /// </summary>
    public override Func<Item, Item, bool> Equator { get { return (a, b) => a == b; } }

    /// <summary>
    /// A custom Comparision function for this Inventory.
    /// </summary>
    public override Comparison<Item> Comparer { get { return (a, b) => a.Name.CompareTo(b.Name); } }
}