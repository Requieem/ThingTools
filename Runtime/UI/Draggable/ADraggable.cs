using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ADraggable
{
    IDraggable target;
    Transform transform;
    Vector3 mousePositionOffset;

    [SerializeField] Transform dragSpace;
    [SerializeField] IDragContainer container;

    public Vector3 DragPosition { get { return MousePosition() + mousePositionOffset; } }

    public Transform Transform { get { return transform; } }
    public IDragContainer Container { get { return container; } }

    public bool IsTarget { get { return target.IsTarget; } }
    public bool IsLocked { get { return target.IsLocked; } }
    public bool IsDragged { get { return target.IsDragged; } }
    public bool IsContained { get { return target.IsContained; } }
    public bool IsPlaceHolder { get { return target.IsPlaceHolder; } }

    public void Initialize(Transform _transform, bool changeRegistration = true)
    {
        CheckPrevent(() => DoInitialize(_transform, changeRegistration));
    }

    public void DoInitialize(Transform _transform, bool changeRegistration = true)
    {
        if (!_transform.TryGetComponent(out target))
        {
            throw new Exception
                ("A Draggable behaviour should always be initialized by an Instance " +
                "of a class that implements the IDraggable interface. This draggable behaviour won't work.");
        }

        transform = _transform;
        dragSpace = transform.root;

        if (target.IsContained && changeRegistration)
        {
            target.OwnContainer.RemoveDraggable(target);
        }

        transform.parent.TryGetComponent(out container);

        if (target.IsContained && changeRegistration && !target.IsPlaceHolder && !target.IsEmpty)
        {
            target.OwnContainer.AddDraggable(target);
        }
    }

    void SetOffset()
    {
        mousePositionOffset = transform.position - MousePosition();
    }

    void DetachFromOrigin()
    {
        transform.SetParent(dragSpace);
    }

    void AttachToOrigin()
    {
        if (!IDraggable.HasOrigin) return;

        var origin = IDraggable.Origin;

        transform.SetParent(origin.parent);
        transform.SetSiblingIndex(origin.siblingIndex);
        transform.position = origin.position;
        ToggleContacts(true);
    }

    void ToggleContacts(bool toggle)
    {
        if (transform.TryGetComponent(out Collider2D collider))
        {
            collider.enabled = toggle;
        }

        if (transform.TryGetComponent(out Image image))
        {
            image.raycastTarget = toggle;
        }
    }

    Vector3 MousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Remember: when this is called, the hovered draggable is not the dragged one
    // Access the hovered one with this.draggable and the dragged one with IDraggable.Dragged
    public void OnHover()
    {
        var hasTarget = IDraggable.HasTarget;
        var lazySwapping = IDraggable.LazySwap;
        var shouldPreSwap = hasTarget && !lazySwapping && !IDraggable.Target.Equals(target) && IDraggable.Target.IsDisplaced;

        if (shouldPreSwap)
        {
            DoSwap(IDraggable.Target);
            IDraggable.Target.IsDisplaced = false;
        }

        if (!IDraggable.IsDragging || target.IsLocked || target.IsDragged || target.IsPlaceHolder)
        {
            IDraggable.Target = null;
            return;
        }

        IDraggable.Target = target;
        IDraggable.LazySwap = false;

        var dragged = IDraggable.Dragged;
        var isTargetContained = target.IsContained;
        var isDraggedContained = dragged.IsContained;
        var sameContainer = isTargetContained && isDraggedContained && target.OwnContainer.Equals(dragged.OwnContainer);

        var targetContainer = IDraggable.Target.OwnContainer;
        var shouldCrossSwap = !((isTargetContained && targetContainer.SlotMode) || (isDraggedContained && IDraggable.Container.SlotMode));

        if (sameContainer || shouldCrossSwap)
        {
            DoSwap(IDraggable.Target);
            IDraggable.Target.IsDisplaced = !IDraggable.target.IsDisplaced;
        }
        else
        {
            IDraggable.LazySwap = true;
        }
    }

    public void SyncContainer()
    {
        container = transform.parent.GetComponent<IDragContainer>();
    }

    void DoSwap(IDraggable other, bool destroy = false)
    {
        if (!IDraggable.HasPlaceholder || other == null) return;

        var placeholderTransform = IDraggable.Placeholder.Transform;
        IDraggable.Prevent = true;

        var otherTransform = other.Transform;
        var placeholderParent = placeholderTransform.parent;
        var targetParent = otherTransform.parent;
        var placeHolderIndex = placeholderTransform.GetSiblingIndex();
        var targetIndex = otherTransform.GetSiblingIndex();
        var placeholderPosition = placeholderTransform.position;
        var targetPosition = otherTransform.position;

        otherTransform.SetParent(placeholderParent);
        otherTransform.SetSiblingIndex(placeHolderIndex);
        otherTransform.position = placeholderPosition;
        other.Initialize(otherTransform);

        if (destroy)
        {
            IDraggable.RemovePlaceHolder();
        }
        else
        {
            placeholderTransform.SetParent(targetParent);
            placeholderTransform.SetSiblingIndex(targetIndex);
            placeholderTransform.position = targetPosition;
        }

        IDraggable.Prevent = false;
    }

    void EndDrag()
    {
        AttachToOrigin();
        IDraggable.ClearDrag();
        IDraggable.onDragEnd?.Invoke();
    }

    void StartDrag()
    {
        ToggleContacts(false);
        IDraggable.SetupDrag(target);
        SetOffset();
        DetachFromOrigin();
        IDraggable.onDragStart?.Invoke();
    }

    public void OnDragStart()
    {
        CheckPrevent(StartDrag);
    }

    public void OnDragCanceled()
    {
        CheckPrevent(EndDrag);
    }

    public void CheckPrevent(System.Action func)
    {
        if (IDraggable.Prevent) return;
        else func();
    }

    public void ClosingAdjustments()
    {
        if (!IDraggable.IsDragging) return;

        IDraggable.ChangeOrigin();

        if (IDraggable.LazySwap)
        {
            DoSwap(IDraggable.Target);
        }
    }

    public void OnDragEnd()
    {
        CheckPrevent(() =>
        {
            ClosingAdjustments();
            EndDrag();
        });
    }
}
