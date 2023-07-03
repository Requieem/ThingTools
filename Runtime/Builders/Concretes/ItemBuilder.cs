using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemBuilder", menuName = "ShireSoft/Item")]
public class ItemBuilder : EntityBuilder<ItemBuilder, Item>
{
    [SerializeField] protected float m_Value;
    [SerializeField] protected ItemLevel m_ItemLevel;
    [SerializeField] protected SlotType[] m_Usages;
    [SerializeField] protected SlotType m_CurrentUsage;
    [SerializeField] protected ActivatableBuilder m_Activatable;

    [SerializeField] UnityEvent m_OnAcquire;
    [SerializeField] UnityEvent m_OnLose;
    [SerializeField] UnityEvent m_OnUse;

    public float Value { get { return m_Value; } set { this.m_Value = value; } }
    public ItemLevel ItemLevel { get { return m_ItemLevel; } private set { m_ItemLevel = value; } }
    public SlotType[] Usages { get { return m_Usages; } private set { m_Usages = value; } }
    public SlotType CurrentUsage { get { return m_CurrentUsage; } private set { m_CurrentUsage = value; } }
    public Activatable Activatable
    { get { return GetSafeCopy<Activatable, ActivatableBuilder>(m_Activatable); } }
    public UnityEvent OnAcquire { get { return m_OnAcquire; } private set { m_OnAcquire = value; } }
    public UnityEvent OnLose { get { return m_OnLose; } private set { m_OnLose = value; } }
    public UnityEvent OnUse { get { return m_OnUse; } private set { m_OnUse = value; } }

    public override Item GetCopy()
    {
        Item item = new (this);
        return item;
    }
}
