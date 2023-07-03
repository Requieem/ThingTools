
using UnityEngine;
using UnityEngine.Events;

public struct DraggableOrigin
{
    public Transform parent;
    public int siblingIndex;
    public Vector3 position;
    public IDragContainer container;

    public DraggableOrigin(IDraggable draggable)
    {
        parent = draggable.Transform.parent;
        siblingIndex = draggable.Transform.GetSiblingIndex();
        position = draggable.Transform.position;
        container = draggable.OwnContainer;
    }
}

public interface IDraggable : IInitializable
{
    public static bool prevent = false;
    public static bool lazySwap = false;
    public static IDraggable dragged = null;
    public static IDraggable target = null;
    public static IDraggable placeHolder = null;
    public static DraggableOrigin origin = default;
    public static readonly string layerName = "Draggable";
    public static UnityEvent onDragStart = new UnityEvent();
    public static UnityEvent onDragEnd = new UnityEvent();

    public static IDraggable Dragged { get { return dragged; } set { dragged = value; } }
    public static IDraggable Target { get { return IsDragging ? target : null; } set { target = IsDragging ? value : null; } }
    public static IDragContainer Container { get { return IsDragging && Dragged.IsContained ? Dragged.OwnContainer : null; } }
    public static DraggableOrigin Origin { get { return IsDragging ? origin : default; } set { origin = IsDragging ? value : default; } }
    public static IDraggable Placeholder { get { return IsDragging ? placeHolder : null; } set { placeHolder = IsDragging ? value : null; } }

    public static string LayerName { get { return layerName; } }
    public static bool Prevent { get { return EditorPrevent || prevent; } set { prevent = value; } }
    public static bool EditorPrevent { get { return !Application.isPlaying; } }
    public static bool LazySwap { get { return lazySwap; } set { lazySwap = value; } }
    public static bool IsDragging { get { return !Prevent && dragged != null; } }
    public static bool HasTarget { get { return IsDragging && target != null; } }
    public static bool HasOrigin { get { return IsDragging; } }
    public static bool HasContainer { get { return IsDragging && Dragged.IsContained; } }
    public static bool HasPlaceholder { get { return IsDragging && placeHolder != null; } }

    public bool IsLocked { get; set; }
    public bool IsDisplaced { get; set; }
    public Transform Transform { get; }
    public IDraggable AsDraggable { get; }
    public IDragContainer OwnContainer { get; }
    public ADraggable DragBehaviour { get; set; }

    public bool IsDragged { get { return IsDragging && dragged.Equals(this); } }
    public bool IsTarget { get { return HasTarget && target.Equals(this); } }
    public bool IsPlaceHolder { get { return HasPlaceholder && placeHolder.Equals(this); } }
    public bool IsContained { get { return OwnContainer != null && OwnContainer is not null; } }
    public bool IsEmptySlot { get { return IsContained && IsEmpty && OwnContainer.HasEmpty && OwnContainer.EmptySlot.Equals(this); } }
    public bool IsEmpty { get; set; }

    public void EmptyInitialize(bool add);

    public static void ClearDrag()
    {
        Dragged = null;
        Target = null;
        Origin = default;
        RemovePlaceHolder();
    }

    public static void SetupDrag(IDraggable _dragged)
    {
        dragged = _dragged;
        origin = new DraggableOrigin(dragged);
        InstantiatePlaceHolder(origin);
        lazySwap = false;
    }

    public static void ChangeOrigin()
    {
        if(!IsDragging) return;
        DraggableOrigin newOrigin;

        if(HasTarget && lazySwap)
            newOrigin = new DraggableOrigin(Target);
        else
            newOrigin = new DraggableOrigin(Placeholder);

        Origin = newOrigin;
    }

    public static void RemovePlaceHolder()
    {
        Object.Destroy(placeHolder.Transform.gameObject);
        placeHolder = null;
    }

    public static void InstantiatePlaceHolder(DraggableOrigin origin)
    {
        if (!IsDragging) return;
        if(!HasPlaceholder)
        {
            placeHolder = new GameObject("PlaceHolder", typeof(RectTransform), typeof(Draggable), typeof(BoxCollider2D)).GetComponent<IDraggable>();
        }
        placeHolder.Transform.SetParent(origin.parent);
        placeHolder.Transform.SetSiblingIndex(origin.siblingIndex);
        placeHolder.Transform.position = origin.position;
        placeHolder.Initialize(placeHolder.Transform);
    }
}
