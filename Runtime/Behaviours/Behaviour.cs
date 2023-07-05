using System;
using UnityEngine;

/// <summary>
/// A base class for all lower-level Behaviours that exist in the game.
/// </summary>
public class Behaviour<A, O> : ScriptableObject // A = Activator, O = Other
{
    #region Instance Fields:

    [SerializeField]
    protected EventTrigger m_OnActivation;
    [SerializeField]
    protected Requirement m_Requirement;
    [NonSerialized]
    protected A m_Behaver;

    [SerializeField]
    protected readonly InteractionKey m_Key;

    #endregion

    #region Instance Properties:

    /// <summary>
    /// The EventTrigger to be invoked when this behaviour is activated.
    /// </summary>
    public EventTrigger OnActivation { get { return m_OnActivation; } }

    /// <summary>
    /// The requirement that must be met before this behaviour can be activated.
    /// </summary>
    public Requirement Requirement { get { return m_Requirement; } }

    /// <summary>
    /// The scriptable object that this behaviour is attached to.
    /// </summary>
    public A Behaver { get { return m_Behaver; } }

    /// <summary>
    /// The key of this behaviour.
    /// </summary>
    public InteractionKey Key { get { return m_Key; } }

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
        this.m_OnActivation = onActivation;
        this.m_Requirement = requirement;
        this.m_Behaver = behaver;
    }

    #endregion

    #region Instance Methods:

    /// <summary>
    /// Activates this behaviour.
    /// </summary>
    public bool CanActivate()
    {
        if (m_Requirement == null || m_Requirement.IsSatisfied)
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
        if (m_OnActivation != null)
        {
            m_OnActivation.Invoke();
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