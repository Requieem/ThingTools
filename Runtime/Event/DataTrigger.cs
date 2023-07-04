using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all lower-level Triggers that exist in the game.
/// </summary>
public class DataTrigger<T> : Trigger<UnityEvent<T>>
{
    #region Instance Fields

    [SerializeField] protected T m_DefaultArgument;
    private Dictionary<UnityAction, UnityAction<T>> m_VoidListeners = new Dictionary<UnityAction, UnityAction<T>>();

    #endregion

    #region Instance Properties

    /// <summary>
    /// The default argument value to be used when invoking the event.
    /// </summary>
    public virtual T DefaultArgument { get { return m_DefaultArgument; } }

    /// <summary>
    /// Dictionary of void listeners associated with their corresponding typed listeners.
    /// </summary>
    public virtual Dictionary<UnityAction, UnityAction<T>> VoidListeners { get { return m_VoidListeners; } }

    #endregion

    #region Instance Methods

    /// <summary>
    /// Invokes the event with the default argument.
    /// </summary>
    public override void Invoke()
    {
        Event?.Invoke(DefaultArgument);
    }

    /// <summary>
    /// Invokes the event with the specified argument.
    /// </summary>
    /// <param name="arg">The argument to pass to the event.</param>
    public virtual void Invoke(T arg)
    {
        Event?.Invoke(arg);
    }

    /// <summary>
    /// Adds a listener to the event without an argument.
    /// </summary>
    /// <param name="listener">The listener to add.</param>
    public override void AddListener(UnityAction listener)
    {
        if (Event != null)
        {
            UnityAction<T> voidListener = (arg) => listener.Invoke();
            VoidListeners.Add(listener, voidListener);
            Event.AddListener(voidListener);
        }
    }

    /// <summary>
    /// Adds a listener to the event with the specified argument.
    /// </summary>
    /// <param name="listener">The listener to add.</param>
    public virtual void AddListener(UnityAction<T> listener)
    {
        Event?.AddListener(listener);
    }

    /// <summary>
    /// Removes a listener from the event without an argument.
    /// </summary>
    /// <param name="listener">The listener to remove.</param>
    public override void RemoveListener(UnityAction listener)
    {
        if (Event != null)
        {
            var hasListener = VoidListeners.ContainsKey(listener);
            if (hasListener)
            {
                var voidListener = VoidListeners[listener];
                VoidListeners.Remove(listener);
                Event.RemoveListener(voidListener);
            }
        }
    }

    /// <summary>
    /// Removes a listener from the event with the specified argument.
    /// </summary>
    /// <param name="listener">The listener to remove.</param>
    public virtual void RemoveListener(UnityAction<T> listener)
    {
        Event?.RemoveListener(listener);
    }

    #endregion

}