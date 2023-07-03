using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class for all Objects that exist in the game.
/// </summary>
[Serializable]
public class Interactor : ThingsContainer<InteractionKey,InteractionBehaviour>
{
    public override Comparison<InteractionBehaviour> Comparer { get { return (a, b) => a.ToString().CompareTo(b.ToString()); } }

    public override Func<InteractionBehaviour, InteractionBehaviour, bool> Equator { get { return (a, b) => a.Equals(b); } }

    public virtual bool InteractWith(Interactor other, InteractionKey withBehaviour, InteractionKey toBehaviour)
    {
        if (Dictionary.ContainsKey(withBehaviour) && other.Dictionary.ContainsKey(toBehaviour))
        {
            return Dictionary[withBehaviour].Activate(other.Dictionary[toBehaviour]);
        }
        else return false;
    }
}
