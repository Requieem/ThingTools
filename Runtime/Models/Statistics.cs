using System;
using System.Collections.Generic;

/// <summary>
/// An abstract base class for all Containers that exist in the game.
/// </summary>
[Serializable]
public class Statistics : ThingsContainer<StatName, ShireBlock>
{
    /// <summary>
    /// A delegate representing a custom function that takes a stat and a value and returns a float.
    /// </summary>
    /// <param name="stat">The stat to apply the function to.</param>
    /// <param name="value">The value to pass to the function.</param>
    /// <returns>The result of applying the function to the stat and value.</returns>
    public delegate float StatFunc(StatName stat, float value);

    /// <summary>
    /// Overrides the Comparer property to compare ShireBlocks by their Value.
    /// </summary>
    public override Comparison<ShireBlock> Comparer { get { return (a, b) => a.Value.CompareTo(b.Value); } }

    /// <summary>
    /// Overrides the Equator property to equate ShireBlocks by their Value.
    /// </summary>
    public override Func<ShireBlock, ShireBlock, bool> Equator { get { return (a, b) => b.Value == a.Value; } }

    /// <summary>
    /// Initializes a new instance of the Statistics class with the given objects.
    /// </summary>
    /// <param name="objects">The list of ThingEntry objects.</param>
    public Statistics(List<ThingEntry> objects)
    {
        this.m_Entries = new();

        for (int i = 0; i < objects.Count; i++)
        {
            Add(objects[i].m_Key, objects[i].m_Value);
        }
    }

    /// <summary>
    /// Initializes a new instance of the Statistics class with the given stats.
    /// </summary>
    /// <param name="stats">The list of StatName objects.</param>
    public Statistics(List<StatName> stats)
    {
        this.m_Entries = new();

        for (int i = 0; i < stats.Count; i++)
        {
            Add(stats[i], new ShireBlock());
        }
    }

    /// <summary>
    /// Applies a stat change to the specified stat using the provided function.
    /// </summary>
    /// <param name="stat">The stat to apply the change to.</param>
    /// <param name="value">The value of the change.</param>
    /// <param name="func">The function to apply to the stat and value.</param>
    /// <returns>The result of applying the function to the stat and value.</returns>
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

    /// <summary>
    /// Changes the value of the specified stat by the given value.
    /// </summary>
    /// <param name="stat">The stat to change.</param>
    /// <param name="value">The value to change the stat by
    /// </param>
    /// <returns>The new value of the stat.</returns>
    public virtual float ChangeStat(StatName stat, float value)
    {
        return ApplyStatChange(stat, value, (StatName s, float v) =>
        {
            Dictionary[s].Value += v;
            return Dictionary[s].Value;
        });
    }

    /// <summary>
    /// Affects the temporary value of the specified stat by the given value.
    /// </summary>
    /// <param name="stat">The stat to affect.</param>
    /// <param name="value">The value to affect the stat by.</param>
    /// <returns>The new temporary value of the stat.</returns>
    public virtual float AffectStat(StatName stat, float value)
    {
        return ApplyStatChange(stat, value, (StatName s, float v) =>
        {
            Dictionary[s].Temp += v;
            return Dictionary[s].Temp;
        });
    }

    /// <summary>
    /// Modifies the modifier value of the specified stat by the given value.
    /// </summary>
    /// <param name="stat">The stat to modify.</param>
    /// <param name="value">The value to modify the stat's modifier by.</param>
    /// <returns>The new modifier value of the stat.</returns>
    public virtual float ModStat(StatName stat, float value)
    {
        return ApplyStatChange(stat, value, (StatName s, float v) =>
        {
            Dictionary[s].Mod += (int)v;
            return Dictionary[s].Mod;
        });
    }

    /// <summary>
    /// Modifies the temporary modifier value of the specified stat by the given value.
    /// </summary>
    /// <param name="stat">The stat to modify.</param>
    /// <param name="value">The value to modify the stat's temporary modifier by.</param>
    /// <returns>The new temporary modifier value of the stat.</returns>
    public virtual float ModTempStat(StatName stat, float value)
    {
        return ApplyStatChange(stat, value, (StatName s, float v) =>
        {
            Dictionary[s].ModTemp += v;
            return Dictionary[s].ModTemp;
        });
    }

    /// <summary>
    /// Appends the stats from another Statistics object to this Statistics object.
    /// </summary>
    /// <param name="other">The other Statistics object to append.</param>
    /// <param name="withMods">Specifies whether to include the modifiers in the append operation.</param>
    /// <param name="addNew">Specifies whether to add new stats if they don't exist in the current Statistics object.</param>
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

    /// <summary>
    /// Removes the stats from another Statistics object from this Statistics object.
    /// </summary>
    /// <param name="other">The other Statistics object to remove.</param>
    /// <param name="withMods">Specifies whether to include the modifiers in the removal operation.</param>
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
