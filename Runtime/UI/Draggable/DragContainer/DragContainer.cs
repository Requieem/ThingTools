using UnityEngine;

public class DragContainer : MonoBehaviour, IDragContainer
{
    [SerializeField] protected ADragContainer m_DragContainerBehaviour;

    public ADragContainer DragContainerBehaviour { get { return m_DragContainerBehaviour; } set { m_DragContainerBehaviour = value; } }
    public Transform Transform { get { return m_DragContainerBehaviour.Transform; } }
    public bool SlotMode { get { return m_DragContainerBehaviour.SlotMode; } set { m_DragContainerBehaviour.SlotMode = value; } }
    public bool AllowsAdding { get { return m_DragContainerBehaviour.AllowsAdding; } set { m_DragContainerBehaviour.AllowsAdding = value; } }
    public bool IsEmpty { get { return m_DragContainerBehaviour.IsEmpty; } }
    public bool HasEmpty { get { return m_DragContainerBehaviour.HasEmpty; } }
    public IDraggable EmptySlot { get { return m_DragContainerBehaviour.EmptySlot; } }

    void Awake()
    {
        DoAwake();
    }

    public virtual void DoAwake()
    {
        if (Transform == null)
            Initialize(transform);
    }

    public virtual void DoEnable()
    { 
        IDraggable.onDragEnd.AddListener(AdjustEmptySlot);
        IDraggable.onDragStart.AddListener(AdjustEmptySlot);
    }

    public virtual void DoDisable()
    {
        IDraggable.onDragEnd.RemoveListener(AdjustEmptySlot);
        IDraggable.onDragStart.RemoveListener(AdjustEmptySlot);
    }

    void OnEnable()
    {
        DoEnable();
    }

    void OnDisable()
    {
        DoDisable();
    }

    public void Initialize(Transform _transform)
    {
        m_DragContainerBehaviour.Initialize(_transform);
    }

    public void AddDraggable(IDraggable draggable)
    {
        if(Transform == null)
        {
            Initialize(transform);
        }

        m_DragContainerBehaviour.AddDraggable(draggable);
    }

    public void RemoveDraggable(IDraggable draggable)
    {
        if (Transform == null)
        {
            Initialize(transform);
        }

        m_DragContainerBehaviour.RemoveDraggable(draggable);
    }

    public void AdjustEmptySlot()
    {
        m_DragContainerBehaviour.AdjustEmptySlot();
    }

    public void ToggleDraggables(bool toggle=true)
    {
        m_DragContainerBehaviour.ToggleDraggables(toggle);
    }
}
