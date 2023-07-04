using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : ObjectContainer<Item>
{
    [SerializeField] private Numismatic m_Wallet;

    public Numismatic Wallet { get { return m_Wallet; } }

    /// <summary>
    /// Buys the given object using the wallet from the seller's inventory with the specified price.
    /// </summary>
    /// <param name="obj">The object to buy.</param>
    /// <returns>True if the object was successfully bought, false otherwise.</returns>
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
    /// Adds the given object to the inventory.
    /// </summary>
    /// <param name="obj">The object to add.</param>
    /// <returns>True if the object was successfully added, false otherwise.</returns>
    public virtual bool Gain(Item obj)
    {
        if (obj == null)
        {
            Log.Msg("Could not gain " + obj?.Name + " because the object was null.");
            return false;
        }
        else
        {
            Add(obj);
            return true;
        }
    }

    /// <summary>
    /// Sells the given object from the inventory and adds the price to the wallet.
    /// </summary>
    /// <param name="obj">The object to sell.</param>
    /// <param name="price">The price of the object.</param>
    /// <returns>True if the object was successfully sold, false otherwise.</returns>
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
    /// Removes the given object from the inventory.
    /// </summary>
    /// <param name="obj">The object to remove.</param>
    /// <returns>True if the object was successfully removed, false otherwise.</returns>
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
            Log.Msg("Could not lose " + obj?.Name + " because it is not in this inventory.");
            return false;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Inventory"/> class using the specified builder.
    /// </summary>
    /// <param name="builder">The builder containing the inventory data.</param>
    public Inventory(InventoryBuilder builder)
    {
        var actualItems = builder.BuiltObjects;
        base.Initialize(actualItems);
        this.m_Wallet = builder.Wallet;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Inventory"/> class using the specified list of items and wallet.
    /// </summary>
    /// <param name="objects">The list of items in the inventory.</param>
    /// <param name="_money">The wallet associated with the inventory.</param>
    public Inventory(List<Item> objects, Numismatic _money)
    {
        base.Initialize(objects);
        this.m_Wallet = _money;
    }

    /// <summary>
    /// A custom equality function for comparing items in the inventory.
    /// </summary>
    public override Func<Item, Item, bool> Equator { get { return (a, b) => a == b; } }

    /// <summary>
    /// A custom comparison function for sorting items in the inventory.
    /// </summary>
    public override Comparison<Item> Comparer { get { return (a, b) => a.Name.CompareTo(b.Name); } }

}