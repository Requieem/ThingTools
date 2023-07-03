using System;
using UnityEngine;

/// <summary>
/// A base class for all Characters that exist in the game.
/// </summary>
[Serializable]
public class Subject : ThingsContainer<ActionKey, ActionResponseBehaviour>
{
    [SerializeField]
    ActionResponseBehaviour defaultResponse;

    public override Comparison<ActionResponseBehaviour> Comparer { get { return (a, b) => a.ToString().CompareTo(b.ToString()); } }
    public override Func<ActionResponseBehaviour, ActionResponseBehaviour, bool> Equator { get { return (a, b) => b == a; } }

    #region Instance Methods:

    public virtual ActionResponseBehaviour Response(ActionKey action)
    {
        if (Dictionary.ContainsKey(action))
        {
            return Dictionary[action];
        }
        else return defaultResponse;
    }

    #endregion
}
