using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DefaultBehaviour<A,O> : IBehaviour<O>, IPassiveBehaviour<A,O> where O : IPassiveBehaviour<O,A>
{
    [SerializeField] Behaviour<A, O> behaviour;
    public Behaviour<A, O> Behaviour { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool Activate(O _param)
    {
        if (CanActivate())
        {
            Behaviour.Activate(_param.Behaviour);
            return true;
        }
        else { return false; }
    }

    public bool CanActivate()
    {
        return Behaviour.CanActivate();
    }
}
