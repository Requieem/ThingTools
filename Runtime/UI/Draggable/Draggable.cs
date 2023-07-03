using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class Draggable : MonoBehaviour, IDraggable, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    [SerializeField] protected bool isLocked = false;
    [SerializeField] protected ADraggable dragBehaviour = new();
    [SerializeField] protected bool isEmpty = false;
    protected bool isDisplaced = false;

    public IDraggable AsDraggable { get { return this; } }
    public IDragContainer OwnContainer { get { return DragBehaviour.Container; } }
    public Transform Transform { get { return transform; } }
    public bool IsEmpty { get { return isEmpty; } set { isEmpty = value; } }
    public bool IsLocked { get { return isLocked; } set { isLocked = value; } }
    public bool IsDisplaced { get { return isDisplaced; } set { isDisplaced = value; } }
    public ADraggable DragBehaviour { get { return dragBehaviour; } set { dragBehaviour = value; } }

    void Awake()
    {
        if (transform.parent != null)
            Initialize(transform);
    }

    void OnEnable()
    {
        SetLayer();
        IDraggable.onDragStart.AddListener(dragBehaviour.SyncContainer);
        IDraggable.onDragEnd.AddListener(dragBehaviour.SyncContainer);
    }

    void OnDisable()
    {
        IDraggable.onDragStart.RemoveListener(dragBehaviour.SyncContainer);
        IDraggable.onDragEnd.RemoveListener(dragBehaviour.SyncContainer);
    }

#if UNITY_EDITOR 
    public void SetLayer()
    {
        if (!LayerManager.AddSetLayer(gameObject, IDraggable.LayerName))
        {
            throw new System.Exception("Draggable: LayerManager.AddSetLayer failed");
        }
    }
#else
    public void SetLayer()
    {
        if (!LayerManager.SetLayer(gameObject, IDraggable.LayerName))
        {
            throw new System.Exception("Draggable: LayerManager.SetLayer failed");
        }
    }
#endif

    public void Initialize(Transform _transform)
    {
        dragBehaviour.Initialize(_transform);
    }

    public void EmptyInitialize(bool add)
    {
        isEmpty = true;
        dragBehaviour.Initialize(transform, add);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (AsDraggable.IsDragged)
        {
            transform.position = dragBehaviour.DragPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!AsDraggable.IsEmptySlot && !IsEmpty)
            dragBehaviour.OnDragStart();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!AsDraggable.IsEmptySlot && !IsEmpty)
            dragBehaviour.OnDragEnd();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!AsDraggable.IsDragged)
            dragBehaviour.OnHover();
    }
}
