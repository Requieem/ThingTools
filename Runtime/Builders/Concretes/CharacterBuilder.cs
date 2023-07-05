using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Builder for creating instances of type <see cref="Character"/>.
/// </summary>
[CreateAssetMenu(fileName = "CharacterBuilder", menuName = "ShireSoft/Character")]
public class CharacterBuilder : EntityBuilder<CharacterBuilder, Character>
{
    /// <summary>
    /// The starting experience for the character.
    /// </summary>
    [SerializeField] protected float m_Exp;

    /// <summary>
    /// The Class that the built character will have.
    /// </summary>
    [SerializeField] protected Class m_Class;

    /// <summary>
    /// The Faction that the built character will be part of.
    /// </summary>
    [SerializeField] protected Faction m_Faction;

    /// <summary>
    /// The Level that the built character will be at.
    /// </summary>
    [SerializeField] protected Level m_Level;

    /// <summary>
    /// The builder for the character's statistic.
    /// </summary>
    [SerializeField] protected StatisticsBuilder m_Statistics;

    /// <summary>
    /// The builder for the character's vitality.
    /// </summary>
    [SerializeField] protected VitalityBuilder m_Vitality;

    /// <summary>
    /// The builder for the character's inventory.
    /// </summary>
    [SerializeField] protected InventoryBuilder m_Inventory;

    /// <summary>
    /// The builder for the character's equipment.
    /// </summary>
    [SerializeField] protected EquipmentBuilder m_Equipment;

    /// <summary>
    /// The builder for the character's activator.
    /// </summary>
    [SerializeField] protected ActivatorBuilder m_Activator;

    /// <summary>
    /// The builder for the character's interactor.
    /// </summary>
    [SerializeField] protected InteractorBuilder m_Interactor;

    /// <summary>
    /// The builder for the character's actioneer.
    /// </summary>
    [SerializeField] protected ActioneerBuilder m_Actioneer;

    /// <summary>
    /// Event that triggers when the character levels up.
    /// </summary>
    [SerializeField] UnityEvent m_OnLevelUp;

    /// <summary>
    /// Gets or sets the starting experience for the character.
    /// </summary>
    public float StartingExp
    {
        get { return m_Exp; }
        set { m_Exp = value; }
    }

    /// <summary>
    /// Gets the builder's class.
    /// </summary>
    public Class Class
    {
        get { return m_Class; }
    }

    /// <summary>
    /// Gets the builder's faction.
    /// </summary>
    public Faction Faction
    {
        get { return m_Faction; }
    }

    /// <summary>
    /// Gets the builder's level.
    /// </summary>
    public Level Level
    {
        get { return m_Level; }
    }

    /// <summary>
    /// Gets a copy of the character's statistics.
    /// </summary>
    public Statistics Statistics
    {
        get { return GetSafeCopy<Statistics, StatisticsBuilder>(m_Statistics); }
    }

    /// <summary>
    /// Gets a copy of the character's vitality.
    /// </summary>
    public Vitality Vitality
    {
        get { return GetSafeCopy<Vitality, VitalityBuilder>(m_Vitality); }
    }

    /// <summary>
    /// Gets a copy of the character's equipment.
    /// </summary>
    public Equipment Equipment
    {
        get { return GetSafeCopy<Equipment, EquipmentBuilder>(m_Equipment); }
    }

    /// <summary>
    /// Gets a copy of the character's inventory.
    /// </summary>
    public Inventory Inventory
    {
        get { return GetSafeCopy<Inventory, InventoryBuilder>(m_Inventory); }
    }

    /// <summary>
    /// Gets a copy of the character's activator.
    /// </summary>
    public Activator Activator
    {
        get { return GetSafeCopy<Activator, ActivatorBuilder>(m_Activator); }
    }

    /// <summary>
    /// Gets a copy of the character's interactor.
    /// </summary>
    public Interactor Interactor
    {
        get { return GetSafeCopy<Interactor, InteractorBuilder>(m_Interactor); }
    }

    /// <summary>
    /// Gets a copy of the character's actioneer.
    /// </summary>
    public Actioneer Actioneer
    {
        get { return GetSafeCopy<Actioneer, ActioneerBuilder>(m_Actioneer); }
    }

    /// <summary>
    /// Gets the event that triggers when the character levels up.
    /// </summary>
    public UnityEvent OnLevelUp
    {
        get { return m_OnLevelUp; }
    }

    /// <summary>
    /// Gets a copy of the character.
    /// </summary>
    /// <returns>A copy of the character.</returns>
    public override Character GetCopy()
    {
        return new Character(this);
    }
}