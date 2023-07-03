using System;

[Serializable]
public class Damage : ThingsContainer<DamageType, ShireBlock>
{
    /// declare a custom function that takes a stat and a value and returns a float
    public delegate float StatFunc(DamageType stat, float value);

    // override Comparer to compare ASt atBundles by their Value
    public override Comparison<ShireBlock> Comparer { get { return (a, b) => a.Value.CompareTo(b.Value); } }

    // override Equator to equate AStatBundles by their Value
    public override Func<ShireBlock, ShireBlock, bool> Equator { get { return (a, b) => b.Value == a.Value; } }
}



