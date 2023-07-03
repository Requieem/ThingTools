using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all lower-level Requirements that exist in the game.
/// </summary>
public abstract class Requirement : ScriptableObject
{
    #region Instance Fields:

    [SerializeField]
    bool isSatisfied;

    protected UnityEvent m_DoSatisfy;
    protected UnityEvent m_UnSatisfy;

    [SerializeField]
    DataTrigger<Requirement> onSatisfied;
    [SerializeField]
    DataTrigger<Requirement> onUnsatisfied;

    #endregion

    #region Instance Properties:

    /// <summary>
    /// Gets a value indicating whether the requirement is met.
    /// </summary>
    public virtual bool IsSatisfied { get { return isSatisfied; } }

    /// <summary>
    /// Gets the Unity event that is invoked by satisfier to satisfy this requirement.
    /// </summary>
    public virtual UnityEvent DoSatisfy { get { return m_DoSatisfy; } }

    /// <summary>
    /// Gets the Unity event that is invoked by satisfier to unsatisfy this requirement.
    /// </summary>
    public virtual UnityEvent UnSatisfy { get { return m_UnSatisfy; } }

    /// <summary>
    /// Gets the Unity event that is invoked when the requirement is met.
    /// </summary>
    public virtual DataTrigger<Requirement> OnSatisfied { get { onSatisfied ??= new();  return onSatisfied; } }

    /// <summary>
    /// Gets the Unity event that is invoked when the requirement is met.
    /// </summary>
    public virtual DataTrigger<Requirement> OnUnsatisfied { get { onUnsatisfied ??= new();  return onUnsatisfied; } }

    #endregion

    #region Initializers:

    /// <summary>
    /// Initializes the requirement with the specified parameters.
    /// </summary>
    /// <param name="isSatisfied">A boolean indicating whether the requirement is met.</param>
    /// <param name="onSatisfied">AEvent that is invoked when the requirement is met.</param>
    public virtual void Initialize(bool isSatisfied, DataTrigger<Requirement> onSatisfied, DataTrigger<Requirement> onUnsatisfied)
    {
        this.isSatisfied = isSatisfied;
        this.onSatisfied = onSatisfied;
        this.onUnsatisfied = onUnsatisfied;
        this.m_DoSatisfy = new UnityEvent();
        this.m_UnSatisfy = new UnityEvent();
    }

    /// <summary>
    /// Initializes the requirement with the specified boolean value, and a new Unity event.
    /// </summary>
    /// <param name="isSatisfied">A boolean indicating whether the requirement is met.</param>
    public virtual void Initialize(bool isSatisfied)
    {
        this.isSatisfied = isSatisfied;
        this.onSatisfied = null;
        this.m_DoSatisfy = null;
    }

    #endregion

    #region Instance Methods:

    public virtual void Enable()
    {
        if (m_DoSatisfy != null)
        {
            m_DoSatisfy.AddListener(Satisfy);
        }
        else
        {
            m_DoSatisfy = new UnityEvent();
            m_DoSatisfy.AddListener(Satisfy);
        }

        if (m_UnSatisfy != null)
        {
            m_UnSatisfy.AddListener(Unsatisfy);
        }
        else
        {
            m_UnSatisfy = new UnityEvent();
            m_UnSatisfy.AddListener(Unsatisfy);
        }
    }

    public virtual void Disable()
    {
        if (m_DoSatisfy != null)
        {
            m_DoSatisfy.RemoveListener(Satisfy);
        }
        if (m_UnSatisfy != null)
        {
            m_UnSatisfy.RemoveListener(Unsatisfy);
        }
    }

    void OnEnable()
    {
        Enable();
    }

    void OnDisable()
    {
        Disable();
    }

    /// <summary>
    /// Invokes the onSatisfied event, while setting the isSatisfied boolean to true.
    /// </summary>
    /// <remarks>
    /// This method is called when the doSatisfy event is invoked. It also sets the isSatisfied boolean to true.
    /// </remarks>
    public virtual void Satisfy()
    {
        isSatisfied = true;
        onSatisfied?.Invoke(this);
    }

    /// <summary>
    /// Invokes the onUnsatisfied event, while setting the isSatisfied boolean to false.
    /// </summary>
    /// <remarks>
    /// This method is called when the unSatisfy event is invoked. It also sets the isSatisfied boolean to false.
    /// </remarks>
    public virtual void Unsatisfy()
    {
        isSatisfied = false;
        onUnsatisfied?.Invoke(this);
    }

    #endregion
}
