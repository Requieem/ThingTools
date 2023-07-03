using UnityEngine;

#if UNITY_EDITOR
#endif

public abstract class EntityBuilder<D, T> : ABuilder<D, T> where D : EntityBuilder<D, T>
{
    #region Instance Fields:

    [SerializeField] protected Level m_Level;
    [SerializeReference] protected Class m_Class;
    [SerializeField] protected Faction m_Faction;
    [SerializeField] protected VitalityBuilder m_Vitality;
    [SerializeField] protected StatisticsBuilder m_Statistics;

    #endregion
    #region Instance Properties:

    public string AssignedName { get { return name; } private set { name = value; } }
    public Level Level { get { return m_Level; } set { m_Level = value; } }
    public Class Class { get { return m_Class; } set { m_Class = value; } }
    public Faction Faction { get { return m_Faction; } set { m_Faction = value; } }
    public Vitality Vitality { get { return m_Vitality is not null ? m_Vitality.GetCopy() : null; } }
    public Statistics Statistics { get { return m_Statistics is not null ? m_Statistics.GetCopy() : null; } }

    #endregion
}








