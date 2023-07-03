using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all lower-level Triggers that exist in the game.
/// </summary>
public class DataTrigger<T> : Trigger<UnityEvent<T>>
{
    #region Instance Fields:

    [SerializeField] protected T m_DefaultArgument;

    private Dictionary<UnityAction, UnityAction<T>> m_VoidListeners = new();

    #endregion

    #region Instance Properties:

    public virtual T DefaultArgument { get { return m_DefaultArgument; } }
    public virtual Dictionary<UnityAction, UnityAction<T>> VoidListeners { get { return m_VoidListeners; } }

    #endregion

    #region Initializers:

    #endregion

    #region Instance Methods:

    public override void Invoke()
    {
        Event?.Invoke(DefaultArgument);
    }

    public virtual void Invoke(T arg)
    {
        Event?.Invoke(arg);
    }

    public override void AddListener(UnityAction listener)
    {
        if (Event != null)
        {
            UnityAction<T> voidListener = (arg) => listener.Invoke();
            VoidListeners.Add(listener, voidListener);
            Event.AddListener(voidListener);
        }
    }

    public virtual void AddListener(UnityAction<T> listener)
    {
        Event?.AddListener(listener);
    }

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

    public virtual void RemoveListener(UnityAction<T> listener)
    {
        Event?.RemoveListener(listener);
    }

    #endregion
}
