using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Item : Entity<Item, ItemBuilder>
{
    [SerializeField] protected float m_Value;
    [SerializeField] protected ItemLevel m_ItemLevel;
    [SerializeField] protected SlotType[] m_Usages;
    [SerializeField] protected SlotType m_CurrentUsage;
    [SerializeField] protected Statistics m_Statistics;
    [NonSerialized] protected Inventory m_Inventory;
    [NonSerialized] protected Equipment m_Equipment;
    [SerializeField] protected Activatable m_Activatable;

    [SerializeField] UnityEvent onAcquire;
    [SerializeField] UnityEvent onLose;
    [SerializeField] UnityEvent onUse;

    // public get-set properties for all of the above, not sure if these are needed so they might be removed later
    // for now, they are here to allow for easy access to the data while ensuring no direct access to field modifications where set is private

    public float Value { get { return m_Value; } set { this.m_Value = value; } }
    public Inventory Inventory { get { return m_Inventory; } private set { m_Inventory = value; } }
    public Equipment Equipment { get { return m_Equipment; } private set { m_Equipment = value; } }
    public Numismatic Price { get { return Inventory.Wallet.SellValue(Value); } }
    public ItemLevel ItemLevel { get { return m_ItemLevel; } private set { m_ItemLevel = value; } }
    public SlotType[] Usages { get { return m_Usages; } private set { m_Usages = value; } }
    public SlotType CurrentUsage { get { return m_CurrentUsage; } private set { m_CurrentUsage = value; } }
    public Statistics Statistics { get { return m_Statistics; } private set { m_Statistics = value; } }
    public Activatable Activatable { get { return m_Activatable; } private set { m_Activatable = value; } }
    public UnityEvent OnAcquire { get { return onAcquire; } private set { onAcquire = value; } }
    public UnityEvent OnLose { get { return onLose; } private set { onLose = value; } }
    public UnityEvent OnUse { get { return onUse; } private set { onUse = value; } }
    public Sprite Icon { get { return Displayable?.Sprite; } }
    public override List<ISerializableThing> SerializableObjects { get { return  new List<ISerializableThing>(base.SerializableObjects) { m_ItemLevel, m_CurrentUsage }; } set { } }

    public override Tuple<ISerializableThing[], int> GetSerializedProperties()
    {
        var arr = Serializer.GetAll();
        var index = 0;
        m_Builder = (ItemBuilder)arr[index++];
        m_ItemLevel = (ItemLevel)arr[index++];
        m_CurrentUsage = (SlotType)arr[index++];

        return new Tuple<ISerializableThing[], int>(arr, index);
    }

    public Item() : base() {
        m_Value = 0;
        m_Statistics = null;
        m_ItemLevel = null;
        m_Activatable = null;
        m_CurrentUsage = null;
        m_Usages = new SlotType[0];
    }
    public Item(ItemBuilder builder) : base (builder)
    {
        m_ItemLevel = builder.ItemLevel;
        m_Usages = builder.Usages;
        m_Statistics = builder.Statistics;
        onAcquire = builder.OnAcquire;
        onLose = builder.OnLose;
        onUse = builder.OnUse;
        m_Activatable = builder.Activatable;

        GetSerializedProperties();
    }
}
