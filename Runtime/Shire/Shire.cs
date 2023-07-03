
using System;
using UnityEngine;

[Serializable]
public abstract class Shire<T> : ScriptableThing<T>, IShire where T : Shire<T>
{
    [SerializeField] bool m_Used = true;
    [SerializeField] protected int m_Order = -1;

    public bool Used { get => m_Used; set => m_Used = value; }
    public int Order { get => m_Order; set => m_Order = value; }

    public override void ListerSerialization()
    {
        if (!m_Used)
        {
            Lister.RemoveListable(this as T);
        }
        else if (m_Used)
        {
            Lister.AddListable(this as T);
        }
    }
    private void OnDestroy()
    {
        m_Used = false;
        Lister.RemoveListable(this as T);
    }
    public virtual string OrderCriteria(T thing)
    {
        return thing.m_Order.ToString();
    }
    public override void Enable()
    {
        var orderCriteria = new Func<T, string>(x => OrderCriteria(x));
        m_Lister = new Lister<T>(orderCriteria);
        
        base.Enable();

    }
}
