using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class for all Objects that exist in the game.
/// </summary>
[Serializable]
public class Activatable : ThingsContainer<ActivationKey,ActivatableBehaviour>
{
  public override Comparison<ActivatableBehaviour> Comparer { get { return (a, b) => a.ToString().CompareTo(b.ToString()); } }

  public override Func<ActivatableBehaviour, ActivatableBehaviour, bool> Equator { get { return (a, b) => b == a; } }
}
