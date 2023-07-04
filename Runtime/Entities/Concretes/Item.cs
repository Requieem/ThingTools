using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Represents an item in the game.
/// </summary>
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

    /// <summary>
    /// The value of the item.
    /// </summary>
    public float Value { get { return m_Value; } set { this.m_Value = value; } }

    /// <summary>
    /// The inventory that holds this item.
    /// </summary>
    public Inventory Inventory { get { return m_Inventory; } private set { m_Inventory = value; } }

    /// <summary>
    /// The equipment that this item is equipped to.
    /// </summary>
    public Equipment Equipment { get { return m_Equipment; } private set { m_Equipment = value; } }

    /// <summary>
    /// The price of the item in the inventory's currency.
    /// </summary>
    public Numismatic Price { get { return Inventory.Wallet.SoldPrice(Value); } }

    /// <summary>
    /// The level of the item.
    /// </summary>
    public ItemLevel ItemLevel { get { return m_ItemLevel; } private set { m_ItemLevel = value; } }

    /// <summary>
    /// The list of slot types that this item can be used in.
    /// </summary>
    public SlotType[] Usages { get { return m_Usages; } private set { m_Usages = value; } }

    /// <summary>
    /// The current slot type that the item is equipped in.
    /// </summary>
    public SlotType CurrentUsage { get { return m_CurrentUsage; } private set { m_CurrentUsage = value; } }

    /// <summary>
    /// The statistics provided by the item.
    /// </summary>
    public Statistics Statistics { get { return m_Statistics; } private set { m_Statistics = value; } }

    /// <summary>
    /// The activatable behavior of the item.
    /// </summary>
    public Activatable Activatable { get { return m_Activatable; } private set { m_Activatable = value; } }

    /// <summary>
    /// The event invoked when the item is acquired.
    /// </summary>
    public UnityEvent OnAcquire { get { return onAcquire; } private set { onAcquire = value; } }

    /// <summary>
    /// The event invoked when the item is lost.
    /// </summary>
    public UnityEvent OnLose { get { return onLose; } private set { onLose = value; } }

    /// <summary>
    /// The event invoked when the item is used.
    /// </summary>
    public UnityEvent OnUse { get { return onUse; } private set { onUse = value; } }

    /// <summary>
    /// The icon sprite of the item.
    /// </summary>
    public Sprite Icon { get { return Displayable?.Sprite; } }

    /// <summary>
    /// Gets the list of serializable objects associated with the item.
    /// </summary>
    public override List<ISerializableThing> SerializableObjects
    {
        get
        {
            return new List<ISerializableThing>(base.SerializableObjects)
            {
                m_ItemLevel,
                m_CurrentUsage
            };
        }
        set { }
    }

    /// <summary>
    /// Gets the serialized properties of the item.
    /// </summary>
    /// <returns>A tuple containing the serialized properties and the index of the next serialized property.</returns>
    public override Tuple<ISerializableThing[], int> GetSerializedProperties()
    {
        var arr = Serializer.GetAll();
        var index = 0;
        m_Builder = (ItemBuilder)arr[index++];
        m_ItemLevel = (ItemLevel)arr[index++];
        m_CurrentUsage = (SlotType)arr[index++];

        return new Tuple<ISerializableThing[], int>(arr, index);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Item"/> class.
    /// </summary>
    public Item() : base()
    {
        m_Value = 0;
        m_Statistics = null;
        m_ItemLevel = null;
        m_Activatable = null;
        m_CurrentUsage = null;
        m_Usages = new SlotType[0];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Item"/> class with the specified builder.
    /// </summary>
    /// <param name="builder">The item builder.</param>
    public Item(ItemBuilder builder) : base(builder)
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

