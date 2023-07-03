using System;
using UnityEngine;

public interface ISerializableThing
{
    [SerializeField] public ThingID Identifier { get; set; }

    public virtual void Persist()
    {
        Identifier ??= new();
    }

    public virtual void OnDuplicate()
    {
        Identifier = new();
    }
}
