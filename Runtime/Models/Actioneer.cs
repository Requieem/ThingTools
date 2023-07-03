using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A base class for all Characters that exist in the game.
/// </summary>
[Serializable]
public class Actioneer : ThingsContainer<ActionKey,List<ActionBehaviour>>
{
    public override Comparison<List<ActionBehaviour>> Comparer { get { return (a, b) => a.ToString().CompareTo(b.ToString()); } }
    public override Func<List<ActionBehaviour>, List<ActionBehaviour>, bool> Equator { get { return (a, b) => b == a; } }

    #region Instance Methods:

    public virtual bool ActOn(Subject other, ActionKey withBehaviour, int actionIndex) {
        if(Dictionary.ContainsKey(withBehaviour) && Dictionary[withBehaviour].Count > actionIndex)
        {
            var actionList = Dictionary[withBehaviour];
            var action = actionList[actionIndex];
            var response = other.Response(withBehaviour);

            return action.Activate(response);
        }
        else return false;
    }

    #endregion
}
