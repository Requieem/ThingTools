using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Displaceable))]
public class NavButton : MonoBehaviour
{
    [SerializeField] bool active = true;
    [SerializeField] bool displaceOnPointer = true;
    [SerializeField] Image icon;
    Displaceable displaceable;

    void OnEnable()
    {
        Enable();
    }

    public virtual void Enable()
    {
        displaceable = GetComponent<Displaceable>();
        SetActive(active);
    }

    protected void SetActive(bool active)
    {
        this.active = active;
        GetComponent<Button>().interactable = active;
        icon.color = active ? Color.white : new Color(1, 1, 1, 0.5f);
    }

    void OnMouseEnter()
    {
        if (active && displaceOnPointer)
        {
            displaceable?.Displace();
        }
    }

    void OnMouseExit()
    {
        if (active && displaceOnPointer)
        {
            displaceable?.ResetPosition();
        }
    }
}
