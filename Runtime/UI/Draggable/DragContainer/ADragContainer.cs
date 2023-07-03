using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using UnityEngine.UI;

[Serializable]
public class ADragContainer
{
    [SerializeField] bool slotMode = false;
    [SerializeField] bool allowsAdding = true;
    [SerializeField] List<IDraggable> draggables = new();
    [SerializeField] GameObject emptyPrefab;
    [SerializeField] Vector2 emptySize = new(100, 100);

    IDraggable emptySlot = null;

    Transform transform;

    public Transform Transform { get { return transform; } }
    public IDraggable EmptySlot { get { return emptySlot; } set { emptySlot = value; } }
    public bool SlotMode { get { return slotMode; } set { slotMode = value; } }
    public bool AllowsAdding { get { return allowsAdding; } set { allowsAdding = value; } }
    public bool IsEmpty { get { return draggables == null || draggables.Count == 0; } }
    public bool HasEmpty { get { return emptySlot != null; } }

    public void Initialize(Transform _transform)
    {
        if (_transform.TryGetComponent<IDragContainer>(out _))
        {
            transform = _transform;
            var shouldAddEmpty = allowsAdding && (IsEmpty || !SlotMode);

            if (shouldAddEmpty)
            {
                AdjustEmptySlot();
            }
        }
        else
        {
            throw new Exception
                ("A DragContainer behaviour should always be initialized by an Instance " +
                "of a class that implements the IDragContainer interface. This dragContainer behaviour won't work.");
        }
    }

    public void UpdateDraggables()
    {
        draggables = transform.GetComponentsInChildren<IDraggable>().ToList();

        for (int i = draggables.Count - 1; i >= 0; i--)
        {
            if (draggables[i].IsEmpty && !draggables[i].IsEmptySlot)
            {
                Object.Destroy(draggables[i].Transform.gameObject);
                draggables.RemoveAt(i);
            }
        }

        draggables = draggables.Where(d => d != null && d is not null && !d.IsEmpty).ToList();
    }

    public void ToggleDraggables(bool toggle=true)
    {
        foreach (var draggable in draggables)
        {
            draggable.IsLocked = !toggle;
        }

        if(HasEmpty)
        {
            EmptySlot.IsLocked = !toggle;
        }
    }

    public void AdjustEmptySlot()
    {
        UpdateDraggables();
        var shouldAdjustSlot = allowsAdding && (IsEmpty || !SlotMode);

        if(shouldAdjustSlot)
        {
            InstantiateEmptySlot();
        }
        else
        {
            RemoveEmptySlot();
        }
    }

    public void RemoveEmptySlot()
    {
        if (emptySlot != null && emptySlot is not null)
        {
            Object.Destroy(emptySlot.Transform.gameObject);
            emptySlot = null;
        }
    }

    public void InstantiateEmptySlot()
    {
        if (emptyPrefab != null)
        {
            emptySlot ??= Object.Instantiate(emptyPrefab, transform).GetComponent<IDraggable>(); 
        }
        else
        {
            emptySlot ??= new GameObject("EmptySlot", typeof(RectTransform), typeof(Draggable), typeof(BoxCollider2D)).GetComponent<IDraggable>();
        }
        
        emptySlot.Transform.SetParent(transform);
        emptySlot.Transform.SetSiblingIndex(transform.childCount - 1);
        emptySlot.Transform.position = transform.position;

        var rectTransform = emptySlot.Transform as RectTransform;

        if(emptySlot.Transform.TryGetComponent<Collider2D>(out var collider))
        {
            collider.layerOverridePriority = 9;
        }

        if(emptySlot.Transform.TryGetComponent<Image>(out var image))
        {
            image.color = new Color(0, 0, 0, 0.01f);
        }

        emptySlot.Transform.localScale = Vector3.one;
        rectTransform.sizeDelta = emptySize;
        emptySlot.EmptyInitialize(false);
    }

    public void AddDraggable(IDraggable draggable)
    {
        UpdateDraggables();
        var wasEmpty = false;

        if(IsEmpty)
        {
            draggables = new();
            wasEmpty = true;
        }

        if(!draggables.Contains(draggable) && !draggable.IsEmpty)
        {
            draggables.Add(draggable);

            if(wasEmpty)
            {
                RemoveEmptySlot();
            }
        }
    }

    public void RemoveDraggable(IDraggable draggable)
    {
        UpdateDraggables();
        if (IsEmpty)
        {
            draggables = new();
            return;
        }

        draggables ??= new();
        draggables.Remove(draggable);

        if(allowsAdding && (IsEmpty || !SlotMode))
        {
            InstantiateEmptySlot();
        }
    }
}
