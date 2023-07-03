
using UnityEngine;

public interface IDragContainer : IInitializable
{
    public bool SlotMode { get; set; }
    public bool AllowsAdding { get; set; }
    public bool HasEmpty { get; }
    public bool IsEmpty { get; }
    public IDraggable EmptySlot { get; }
    public Transform Transform { get; }
    public ADragContainer DragContainerBehaviour { get; set; }
    public void AdjustEmptySlot();
    public void ToggleDraggables(bool toggle = true);
    public void AddDraggable(IDraggable draggable);
    public void RemoveDraggable(IDraggable draggable);
}
