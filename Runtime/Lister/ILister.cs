using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ILister<D> : ISerializationCallbackReceiver where D : ScriptableObject, ILister<D>
{
    public Lister<D> Lister { get; set; }
    public void ListerSerialization();
}
