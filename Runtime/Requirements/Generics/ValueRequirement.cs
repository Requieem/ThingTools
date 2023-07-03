using Unity.VisualScripting;
using UnityEngine;

public abstract class ValueRequirement<D, K, V> : Requirement where D : ThingsContainer<K,V> where K : ScriptableThing<K> {
    #region Instance Fields:
    [SerializeField] protected D m_Dict;
    [SerializeField] protected K m_Key;
    [SerializeField] protected V m_Value;
    #endregion

    #region Instance Properties:
    public ThingsContainer<K,V> Dict { get { return m_Dict; } }
    public K Key { get { return m_Key; } }
    #endregion

    #region Initializers:
    public virtual void Initialize(bool isSatisfied, DataTrigger<Requirement> onSatisfied, DataTrigger<Requirement> onUnsatisfied, D _dict, K _key, V _value)
    {
        base.Initialize(isSatisfied, onSatisfied, onUnsatisfied);
        this.m_Dict = _dict;
        this.m_Key = _key;
        this.m_Value = _value;
    }

    #endregion

    #region Instance Methods:
    
    public override void Enable()
    {
        base.Enable();
        if (m_Key != null)
            if (m_Dict?.Watch(m_Key, m_Value, m_DoSatisfy, m_UnSatisfy) >= 0) {
                Satisfy();
            }
    }

    public override void Disable()
    {
        base.Disable();
        if (m_Key != null)
        {
            m_Dict?.Unwatch(m_Key, m_Value);
        }
    }

    #endregion
}