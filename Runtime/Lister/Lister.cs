using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Lister<D> where D : ScriptableObject, ILister<D>
{
    public static Func<D, string> m_OrderCriteria = null;
    public static List<D> m_Elements = new();
    [SerializeField] List<D> siblings = new List<D>();
    static readonly Func<D, string> DefaultCriteria = x => "";
    public static Action m_OnAdd;
    public static Action m_OnRemove;

    public List<D> Siblings { get => siblings; set => siblings = value; }
    public Action OnAdd { get => m_OnAdd; set => m_OnAdd = value; }
    public Action OnRemove { get => m_OnRemove; set => m_OnRemove = value; }

    public Lister(Func<D, string> orderCriteria = null)
    {
        if(orderCriteria != null)
            m_OrderCriteria = orderCriteria;
        else
            m_OrderCriteria = DefaultCriteria;
    }

    public void Sync()
    {
        OrderListables();
        
        siblings.Clear();

        foreach (D item in m_Elements)
        {
            siblings.Add(item);
        }
    }

    public void AddListable(D item)
    {
        if (!m_Elements.Contains(item))
        {
            m_Elements.Add(item);
        }

        Sync();

        OnAdd?.Invoke();
    }

    public void RemoveListable(D item)
    {
        m_Elements.Remove(item);
        Sync();

        OnRemove?.Invoke();
    }

    public void ToggleListable(D item, bool toggle)
    {
        if(toggle)
        {
            AddListable(item);
        }    
        else
        {
            RemoveListable(item);
        }
    }

    public bool ToggleListable(D item)
    {
        if(m_Elements.Contains(item))
        {
            RemoveListable(item);
            return false;
        }
        else
        {
            AddListable(item);
            return true;
        }
    }

    public virtual void OrderListables()
    {
        m_Elements = m_Elements
                        .Where(x => x != null && x is not null)
                        .OrderBy(m_OrderCriteria)
                        .ToList();
    }

    public static List<D> OrderedElements()
    {
        return m_Elements
                .Where(x => x != null && x is not null)
                .OrderBy(m_OrderCriteria)
                .ToList();
    }
}
