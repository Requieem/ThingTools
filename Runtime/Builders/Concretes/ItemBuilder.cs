using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Builder for creating instances of type <see cref="Item"/>.
/// </summary>
[CreateAssetMenu(fileName = "ItemBuilder", menuName = "ShireSoft/Item")]
public class ItemBuilder : EntityBuilder<ItemBuilder, Item>
{
    [SerializeField] protected float m_Value;
    [SerializeField] protected ItemLevel m_ItemLevel;
    [SerializeField] protected SlotType[] m_Usages;
    [SerializeField] protected SlotType m_CurrentUsage;
    [SerializeField] protected ActivatableBuilder m_Activatable;

    /// <summary>
    /// The builder for the item's statistic.
    /// </summary>
    [SerializeField] protected StatisticsBuilder m_Statistics;


    [SerializeField] UnityEvent m_OnAcquire;
    [SerializeField] UnityEvent m_OnLose;
    [SerializeField] UnityEvent m_OnUse;

    /// <summary>
    /// Gets or sets the value of the item.
    /// </summary>
    public float Value
    {
        get { return m_Value; }
        set { m_Value = value; }
    }

    /// <summary>
    /// Gets a copy of the item's statistics.
    /// </summary>
    public Statistics Statistics
    {
        get { return GetSafeCopy<Statistics, StatisticsBuilder>(m_Statistics); }
    }

    /// <summary>
    /// Gets the item level.
    /// </summary>
    public ItemLevel ItemLevel
    {
        get { return m_ItemLevel; }
        private set { m_ItemLevel = value; }
    }

    /// <summary>
    /// Gets the possible usages of the item.
    /// </summary>
    public SlotType[] Usages
    {
        get { return m_Usages; }
        private set { m_Usages = value; }
    }

    /// <summary>
    /// Gets the current usage of the item.
    /// </summary>
    public SlotType CurrentUsage
    {
        get { return m_CurrentUsage; }
        private set { m_CurrentUsage = value; }
    }

    /// <summary>
    /// Gets a copy of the item's activatable.
    /// </summary>
    public Activatable Activatable
    {
        get { return GetSafeCopy<Activatable, ActivatableBuilder>(m_Activatable); }
    }

    /// <summary>
    /// Gets the event that triggers when the item is acquired.
    /// </summary>
    public UnityEvent OnAcquire
    {
        get { return m_OnAcquire; }
        private set { m_OnAcquire = value; }
    }

    /// <summary>
    /// Gets the event that triggers when the item is lost.
    /// </summary>
    public UnityEvent OnLose
    {
        get { return m_OnLose; }
        private set { m_OnLose = value; }
    }

    /// <summary>
    /// Gets the event that triggers when the item is used.
    /// </summary>
    public UnityEvent OnUse
    {
        get { return m_OnUse; }
        private set { m_OnUse = value; }
    }

    /// <summary>
    /// Gets a copy of the item.
    /// </summary>
    /// <returns>A copy of the item.</returns>
    public override Item GetCopy()
    {
        return new Item(this);
    }
}
