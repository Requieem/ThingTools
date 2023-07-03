using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all lower-level Triggers that exist in the game.
/// </summary>
public abstract class Trigger<T> : ScriptableObject where T : UnityEventBase
{
    #region Instance Fields:

    [SerializeField] protected T m_Event;

    #endregion

    #region Instance Properties:

    public virtual T Event { get { return m_Event; } }

    #endregion

    #region Instance Methods:

    public abstract void Invoke();

    public abstract void AddListener(UnityAction listener);

    public abstract void RemoveListener(UnityAction listener);

    #endregion
}
