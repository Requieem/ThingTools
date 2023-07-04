using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all lower-level Events that exist in the game.
/// </summary>
[CreateAssetMenu(fileName = "EventTrigger", menuName = "ShireSoft/Events/EventTrigger", order = 0)]
public class EventTrigger : Trigger<UnityEvent>
{
    /// <summary>
    /// Invokes the event associated with the trigger.
    /// </summary>
    public override void Invoke()
    {
        Event?.Invoke();
    }

    /// <summary>
    /// Adds a listener to the event associated with the trigger.
    /// </summary>
    /// <param name="listener">The listener to add.</param>
    public override void AddListener(UnityAction listener)
    {
        Event?.AddListener(listener);
    }

    /// <summary>
    /// Removes a listener from the event associated with the trigger.
    /// </summary>
    /// <param name="listener">The listener to remove.</param>
    public override void RemoveListener(UnityAction listener)
    {
        Event?.RemoveListener(listener);
    }

}