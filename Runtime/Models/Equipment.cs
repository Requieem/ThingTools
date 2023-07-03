using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Equipment : ThingsContainer<SlotType, Item>
{
    [SerializeField] protected Statistics m_Statistics;

    /// <summary>
    /// A custom Equality function for this Inventory.
    /// </summary>
    public override Func<Item, Item, bool> Equator { get { return (a, b) => a == b; } }

    /// <summary>
    /// A custom Comparision function for this Inventory.
    /// </summary>
    public override Comparison<Item> Comparer { get { return (a, b) => a.Name.CompareTo(b.Name); } }

    /// <summary>
    /// Equips or unequips a specific piece of gear to this Equipment.
    /// </summary>
    public virtual bool Toggle(Item item)
    {
        Dictionary = null;

        if (item == null) return false;

        if (item.CurrentUsage != null && Dictionary.ContainsKey(item.CurrentUsage) && Dictionary[item.CurrentUsage] == item)
        {
            return Unequip(item);
        }
        else if (item.CurrentUsage != null)
        {
            return Equip(item);
        }
        else return false;
    }

    /// <summary>
    /// Equips a specific piece of gear to this Equipment.
    /// </summary>
    public virtual bool Equip(Item item)
    {
        Add(item.CurrentUsage, item);
        m_Statistics.AppendBlock(item.Statistics);
        return true;
    }

    /// <summary>
    /// Unequips a specific piece of gear from this Equipment.
    /// </summary>
    public virtual bool Unequip(Item item)
    {
        if (Remove(item.CurrentUsage, item))
        {
            m_Statistics.RemoveBlock(item.Statistics);
            return true;
        }
        return false;
    }

    public Equipment(List<SlotType> slots)
    {
        if (slots == null) return;

        foreach (SlotType slot in slots)
        {
            Add(slot, null);
        }
    }

    public Equipment(EquipmentBuilder builder)
    {
        this.m_Entries = new();
        var builderObjects = builder.BuiltObjects;
        foreach (var obj in builderObjects)
        {
            Add(obj.m_Key, obj.m_Value);
        }
    }
}