using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a character in the game.
/// </summary>
[Serializable]
public class Character : Entity<Character, CharacterBuilder>
{
    [SerializeField] protected float m_Exp;
    [SerializeField] protected Level m_Level;
    [SerializeField] protected Class m_Class;
    [SerializeField] protected Faction m_Faction;
    [SerializeField] protected Vitality m_Vitality;
    [SerializeField] protected Statistics m_Statistics;
    [SerializeField] protected Inventory m_Inventory;
    [SerializeField] protected Equipment m_Equipment;
    [SerializeField] protected Activator m_Activator;
    [SerializeField] protected Interactor m_Interactor;
    [SerializeField] protected Actioneer m_Actioneer;
    [SerializeField] protected UnityEvent m_OnLevelUp;

    /// <summary>
    /// The experience points of the character.
    /// </summary>
    public float Exp { get { return m_Exp; } set { m_Exp = value; } }

    /// <summary>
    /// The level of the character.
    /// </summary>
    public Level Level { get { return m_Level; } set { m_Level = value; } }

    /// <summary>
    /// The class of the character.
    /// </summary>
    public Class Class { get { return m_Class; } set { m_Class = value; } }

    /// <summary>
    /// The faction of the character.
    /// </summary>
    public Faction Faction { get { return m_Faction; } set { m_Faction = value; } }

    /// <summary>
    /// The vitality of the character.
    /// </summary>
    public Vitality Vitality { get { return m_Vitality; } private set { m_Vitality = value; } }

    /// <summary>
    /// The statistics of the character.
    /// </summary>
    public Statistics Statistics { get { return m_Statistics; } private set { m_Statistics = value; } }

    /// <summary>
    /// The inventory of the character.
    /// </summary>
    public Inventory Inventory { get { return m_Inventory; } private set { m_Inventory = value; } }

    /// <summary>
    /// The equipment of the character.
    /// </summary>
    public Equipment Equipment { get { return m_Equipment; } private set { m_Equipment = value; } }

    /// <summary>
    /// The activator of the character.
    /// </summary>
    public Activator Activator { get { return m_Activator; } private set { m_Activator = value; } }

    /// <summary>
    /// The interactor of the character.
    /// </summary>
    public Interactor Interactor { get { return m_Interactor; } private set { m_Interactor = value; } }

    /// <summary>
    /// The actioneer of the character.
    /// </summary>
    public Actioneer Actioneer { get { return m_Actioneer; } private set { m_Actioneer = value; } }

    /// <summary>
    /// The event invoked when the character levels up.
    /// </summary>
    public UnityEvent OnLevelUp { get { return m_OnLevelUp; } private set { m_OnLevelUp = value; } }

    /// <summary>
    /// The list of serializable objects associated with the character.
    /// </summary>
    public override List<ISerializableThing> SerializableObjects { get { return new List<ISerializableThing>(base.SerializableObjects) { m_Level, m_Class, m_Faction }; } set { } }

    /// <summary>
    /// Advances the character to the next level and returns the level points gained.
    /// </summary>
    /// <returns>The level points gained from advancing to the next level.</returns>
    public virtual int AdvanceLevel()
    {
        if (!m_Level.IsMaxLevel)
        {
            m_Level = m_Level.NextLevel;
            m_OnLevelUp?.Invoke();
            return m_Level.LevelPoints;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Character"/> class.
    /// </summary>
    public Character() : base()
    {
        m_Exp = 0;
        m_Level = null;
        m_Class = null;
        m_Faction = null;
        m_Vitality = null;
        m_Statistics = null;
        m_Inventory = null;
        m_Equipment = null;
        m_Activator = null;
        m_Interactor = null;
        m_Actioneer = null;
        m_OnLevelUp = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Character"/> class with the specified builder, name, and class.
    /// </summary>
    /// <param name="builder">The builder used to construct the character.</param>
    /// <param name="chosenName">The chosen name for the character.</param>
    /// <param name="chosenClass">The chosen class for the character.</param>
    public Character(CharacterBuilder builder, string chosenName = null, Class chosenClass = null) : base(builder)
    {
        if (chosenName != null)
        {
            m_Name = chosenName;
        }

        if (chosenClass != null)
        {
            m_Class = chosenClass;
        }
        else
        {
            m_Class = builder.Class;
        }

        m_Exp = builder.StartingExp;
        m_Faction = builder.Faction;
        m_Level = builder.Level;
        m_Statistics = builder.Statistics;
        m_Vitality = builder.Vitality;
        m_Inventory = builder.Inventory;
        m_Equipment = builder.Equipment;
        m_OnLevelUp = builder.OnLevelUp;
        m_Activator = builder.Activator;
        m_Interactor = builder.Interactor;
        m_Actioneer = builder.Actioneer;

        if (m_Class != null)
            Statistics.AppendBlock(m_Class.Statistics);

        while (!m_Level.IsMaxLevel && m_Exp >= m_Level.ExpToNextLevel)
        {
            m_Level = m_Level.NextLevel;
        }
    }

    /// <summary>
    /// Retrieves the serialized properties for the character and updates the object's state.
    /// </summary>
    /// <returns>A tuple containing the serialized objects and the index of the next serialized property.</returns>
    public override Tuple<ISerializableThing[], int> GetSerializedProperties()
    {
        var arr = Serializer.GetAll();
        var index = 0;
        m_Builder = (CharacterBuilder)arr[index++];
        m_Level = (Level)arr[index++];
        m_Class = (Class)arr[index++];
        m_Faction = (Faction)arr[index++];

        return new Tuple<ISerializableThing[], int>(arr, index);
    }
}
