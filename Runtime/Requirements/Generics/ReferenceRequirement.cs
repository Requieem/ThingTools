using Unity.VisualScripting;
using UnityEngine;

public abstract class ReferenceRequirement<D, S> : Requirement where D : IObjectSatisfier<S> {
    #region Instance Fields:
    [SerializeReference] protected D m_Satisfier;
    [SerializeField] protected S m_Item;
    #endregion

    #region Instance Properties:
    public D Collection { get { return m_Satisfier; } }
    public S Item { get { return m_Item; } }
    #endregion

    #region Initializers:
    public virtual void Initialize(bool isSatisfied, DataTrigger<Requirement> onSatisfied, DataTrigger<Requirement> onUnsatisfied, D collection, S item)
    {
        base.Initialize(isSatisfied, onSatisfied, onUnsatisfied);
        this.m_Satisfier = collection;
        this.m_Item = item;
    }
    #endregion

    #region Instance Methods:
    
    public override void Enable()
    {
        base.Enable();
        if (m_Item != null)
            if (m_Satisfier?.Watch(m_Item, m_DoSatisfy, m_UnSatisfy) != 0) {
                Satisfy();
            }
    }

    public override void Disable()
    {
        base.Disable();
        if (m_Item != null)
            m_Satisfier?.Unwatch(m_Item);
    }

    #endregion
}