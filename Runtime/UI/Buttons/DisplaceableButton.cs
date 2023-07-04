using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a navigation button with displacement behavior.
/// </summary>
[RequireComponent(typeof(Displaceable))]
public class DisplaceableButton : MonoBehaviour
{
    [SerializeField] private bool active = true;
    [SerializeField] private bool displaceOnPointer = true;
    [SerializeField] private Image icon;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = new Color(1f, 1f, 1f, 0.5f);
    private Displaceable displaceable;

    /// <summary>
    /// Called when the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        Enable();
    }

    /// <summary>
    /// Enables the navigation button.
    /// </summary>
    public virtual void Enable()
    {
        displaceable = GetComponent<Displaceable>();
        SetActive(active);
    }

    /// <summary>
    /// Sets the active state of the navigation button.
    /// </summary>
    /// <param name="active">The active state to set.</param>
    protected void SetActive(bool active)
    {
        this.active = active;
        GetComponent<Button>().interactable = active;
        icon.color = active ? activeColor : inactiveColor;
    }

    /// <summary>
    /// Called when the mouse enters the object's boundaries.
    /// </summary>
    private void OnMouseEnter()
    {
        if (active && displaceOnPointer)
        {
            displaceable?.Displace();
        }
    }

    /// <summary>
    /// Called when the mouse exits the object's boundaries.
    /// </summary>
    private void OnMouseExit()
    {
        if (active && displaceOnPointer)
        {
            displaceable?.ResetPosition();
        }
    }
}
