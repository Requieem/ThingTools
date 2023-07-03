using System;

/// <summary>
/// A base class for all Characters that exist in the game.
/// </summary>
[Serializable]
public class Activator : ThingsContainer<ActivationKey, ActivatorBehaviour>
{
    public override Comparison<ActivatorBehaviour> Comparer { get { return (a, b) => a.ToString().CompareTo(b.ToString()); } }
    public override Func<ActivatorBehaviour, ActivatorBehaviour, bool> Equator { get { return (a, b) => b == a; } }

    #region Instance Methods:

    public virtual bool Activate(Activatable other, ActivationKey withBehaviour, ActivationKey toBehaviour)
    {
        if (Dictionary.ContainsKey(withBehaviour) && other.Dictionary.ContainsKey(toBehaviour))
        {
            return Dictionary[withBehaviour].Activate(other.Dictionary[toBehaviour]);
        }
        else return false;
    }

    #endregion
}
