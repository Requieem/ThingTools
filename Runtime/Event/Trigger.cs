using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all lower-level Triggers that exist in the game.
/// </summary>
/// <typeparam name="T">The type of UnityEvent used by the trigger.</typeparam>
public abstract class Trigger<T> : ScriptableObject where T : UnityEventBase
{
    #region Instance Fields

    [SerializeField]
    protected T m_Event;

    #endregion

    #region Instance Properties

    /// <summary>
    /// The event associated with the trigger.
    /// </summary>
    public virtual T Event => m_Event;

    #endregion

    #region Instance Methods

    /// <summary>
    /// Invokes the trigger's event.
    /// </summary>
    public abstract void Invoke();

    /// <summary>
    /// Adds a listener to the trigger's event.
    /// </summary>
    /// <param name="listener">The listener to add.</param>
    public abstract void AddListener(UnityAction listener);

    /// <summary>
    /// Removes a listener from the trigger's event.
    /// </summary>
    /// <param name="listener">The listener to remove.</param>
    public abstract void RemoveListener(UnityAction listener);

    #endregion

}