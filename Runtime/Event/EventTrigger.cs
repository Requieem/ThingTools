using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all lower-level Events that exist in the game.
/// </summary>
[CreateAssetMenu(fileName = "EventTrigger", menuName = "ShireSoft/Events/EventTrigger", order = 0)]
public class EventTrigger : Trigger<UnityEvent>
{
    public override void Invoke()
    {
        Event?.Invoke();
    }

    public override void AddListener(UnityAction listener)
    {
        Event?.AddListener(listener);
    }

    public override void RemoveListener(UnityAction listener)
    {
        Event?.RemoveListener(listener);
    }
}
