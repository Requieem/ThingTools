using UnityEngine;

public interface IPassiveBehaviour<A, O>
{
    [SerializeField]
    public Behaviour<A, O> Behaviour { get; set; }
}
