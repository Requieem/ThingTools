using System;
using System.Collections.Generic;

/// <summary>
/// An abstract base class for all Containers that exist in the game.
/// </summary>
[Serializable]
public class Statistics : ThingsContainer<StatName, ShireBlock>
{
    /// declare a custom function that takes a stat and a value and returns a float
    public delegate float StatFunc(StatName stat, float value);

    // override Comparer to compare AStatBundles by their Value
    public override Comparison<ShireBlock> Comparer { get { return (a, b) => a.Value.CompareTo(b.Value); } }

    // override Equator to equate AStatBundles by their Value
    public override Func<ShireBlock, ShireBlock, bool> Equator { get { return (a, b) => b.Value == a.Value; } }

    public Statistics(List<ThingEntry> objects)
    {
        this.m_Entries = new();

        for (int i = 0; i < objects.Count; i++)
        {
            Add(objects[i].m_Key, objects[i].m_Value);
        }
    }

    public Statistics(List<StatName> stats)
    {
        this.m_Entries = new();

        for (int i = 0; i < stats.Count; i++)
        {
            Add(stats[i], new ShireBlock());
        }
    }

    /// a function that takes any float func(AStat, float) and applies it to all stats in the statblock
    public virtual float ApplyStatChange(StatName stat, float value, StatFunc func)
    {
        Dictionary = null;
        if (Dictionary.ContainsKey(stat))
        {
            var res = func(stat, value);
            if (value > 0)
                CheckSatisfyOnChange(stat, Dictionary[stat]);
            else
                CheckUnsatisfyOnChange(stat, Dictionary[stat]);
            return res;
        }
        else
        {
            return -1;
        }
    }

    public virtual float ChangeStat(StatName stat, float value)
    {
        return ApplyStatChange(stat, value, (StatName stat, float value) =>
        {
            Dictionary[stat].Value += value;
            return Dictionary[stat].Value;
        });
    }

    public virtual float AffectStat(StatName stat, float value)
    {
        return ApplyStatChange(stat, value, (StatName stat, float value) =>
        {
            Dictionary[stat].Temp += value;
            return Dictionary[stat].Temp;
        });
    }

    public virtual float ModStat(StatName stat, float value)
    {
        return ApplyStatChange(stat, value, (StatName stat, float value) =>
        {
            Dictionary[stat].Mod += (int)value;
            return Dictionary[stat].Mod;
        });
    }

    public virtual float ModTempStat(StatName stat, float value)
    {
        return ApplyStatChange(stat, value, (StatName stat, float value) =>
        {
            Dictionary[stat].ModTemp += value;
            return Dictionary[stat].ModTemp;
        });
    }

    public virtual void AppendBlock(Statistics other, bool withMods = false, bool addNew = false)
    {
        foreach (var stat in other.Dictionary.Keys)
        {
            if (Dictionary.ContainsKey(stat))
            {
                ChangeStat(stat, other.Dictionary[stat].Value);
                AffectStat(stat, other.Dictionary[stat].Temp);
                if (withMods)
                {
                    ModStat(stat, other.Dictionary[stat].Mod);
                    ModTempStat(stat, other.Dictionary[stat].ModTemp);
                }
            }
            else if (addNew)
            {
                Add(stat, other.Dictionary[stat]);
            }
        }
    }

    public virtual void RemoveBlock(Statistics other, bool withMods = false)
    {
        foreach (var stat in other.Dictionary.Keys)
        {
            if (Dictionary.ContainsKey(stat))
            {
                ChangeStat(stat, -other.Dictionary[stat].Value);
                AffectStat(stat, -other.Dictionary[stat].Temp);
                if (withMods)
                {
                    ModStat(stat, -other.Dictionary[stat].Mod);
                    ModTempStat(stat, -other.Dictionary[stat].ModTemp);
                }
            }
        }
    }
}



