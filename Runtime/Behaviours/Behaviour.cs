using System;
using UnityEngine;

/// <summary>
/// A base class for all lower-level Behaviours that exist in the game.
/// </summary>
public class Behaviour<A, O> : ScriptableObject // A = Activator, O = Other
{
    #region Instance Fields:

    [SerializeField]
    EventTrigger onActivation;
    [SerializeField]
    Requirement requirement;
    [NonSerialized]
    protected A behaver;

    [SerializeField]
    protected readonly InteractionKey key;

    #endregion

    #region Instance Properties:

    /// <summary>
    /// The AEvent to be invoked when this behaviour is activated.
    /// </summary>
    public EventTrigger OnActivation { get { return onActivation; } }

    /// <summary>
    /// The requirement that must be met before this behaviour can be activated.
    /// </summary>
    public Requirement Requirement { get { return requirement; } }

    /// <summary>
    /// The scriptable object that this behaviour is attached to.
    /// </summary>
    public A Behaver { get { return behaver; } }

    /// <summary>
    /// The key of this behaviour.
    /// </summary>
    public InteractionKey Key { get { return key; } }

    #endregion

    #region Initializers:

    /// <summary>
    /// Initializes the ABehaviour instance with the given AEvent and requirement.
    /// </summary>
    /// <param name="onActivation">The AEvent to be invoked when this behaviour is activated.</param>
    /// <param name="requirement">The requirement that must be met before this behaviour can be activated.</param>
    /// <param name="activator">The scriptable object that this behaviour is attached to, responsible for activating it.</param>
    public void Initialize(EventTrigger onActivation, Requirement requirement, A behaver)
    {
        this.onActivation = onActivation;
        this.requirement = requirement;
        this.behaver = behaver;
    }

    #endregion

    #region Instance Methods:

    /// <summary>
    /// Activates this behaviour.
    /// </summary>
    public bool CanActivate()
    {
        if (requirement == null || requirement.IsSatisfied)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// BroadCast the activation of this behaviour
    /// </summary>
    public void InvokeActivation()
    {
        if (onActivation != null)
        {
            onActivation.Invoke();
        }
    }

    /// <summary>
    /// Activates this behaviour.
    /// </summary>
    public bool Activate(Behaviour<O, A> _param)
    {
        if (CanActivate() && _param.CanActivate() && DoActivation(_param.Behaver))
        {
            InvokeActivation();
            return true;
        }
        else { return false; }
    }

    /// <summary>
    /// DoActivation serves as an entry point for classes of type P that want to activate this behaviour as a consequence of their own activation (if they are "Behaviour"s)
    /// </summary>
    public virtual bool DoActivation(O param)
    {
        return false;
    }

    #endregion
}