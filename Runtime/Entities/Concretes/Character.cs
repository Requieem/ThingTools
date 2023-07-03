using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] UnityEvent m_OnLevelUp;

    public float Exp { get { return m_Exp; } set { m_Exp = value; } }
    public Level Level { get { return m_Level; } set { m_Level = value; } }
    public Class Class { get { return m_Class; } set { m_Class = value; } }
    public Faction Faction { get { return m_Faction; } set { m_Faction = value; } }
    public Vitality Vitality { get { return m_Vitality; } private set { m_Vitality = value; } }
    public Statistics Statistics { get { return m_Statistics; } private set { m_Statistics = value; } }
    public Inventory Inventory { get { return m_Inventory; } private set { m_Inventory = value; } }
    public Equipment Equipment { get { return m_Equipment; } private set { m_Equipment = value; } }
    public Activator Activator { get { return m_Activator; } private set { m_Activator = value; } }
    public Interactor Interactor { get { return m_Interactor; } private set { m_Interactor = value; } }
    public Actioneer Actioneer { get { return m_Actioneer; } private set { m_Actioneer = value; } }
    public UnityEvent OnLevelUp { get { return m_OnLevelUp; } private set { m_OnLevelUp = value; } }
    public override List<ISerializableThing> SerializableObjects { get { return new List<ISerializableThing>(base.SerializableObjects) { m_Level, m_Class, m_Faction }; } set { } }


    public virtual int AdvanceLevel()
    {
        if (!m_Level.IsMaxLevel)
        {
            m_Level = m_Level.m_NextLevel;
            m_OnLevelUp?.Invoke();
            return m_Level.m_LevelPoints;
        }
        else
        {
            return 0;
        }
    }

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

        while (!m_Level.IsMaxLevel && m_Exp >= m_Level.m_ExpToNextLevel)
        {
            m_Level = m_Level.m_NextLevel;
        }
    }

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
