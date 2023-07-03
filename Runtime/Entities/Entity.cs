using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Entity<D, B> : ISerializationCallbackReceiver, IThingSerializer where D : Entity<D, B> where B : EntityBuilder<B, D>
{
    #region Instance Fields:

    [SerializeField] protected string m_Name = "Entity";
    [SerializeField] protected List<ThingID> m_ThingIDs;

    // I chose to enforce the existence of B because choosing to declare the builder on lower level would lead to more code duplication.
    // Adding a new builder if there is none only requires one line.
    // Rewriting the constructors each time requires 10 lines
    [SerializeField] protected EntityBuilder<B, D> m_Builder;

    #endregion

    #region Instance Properties:
    public string Name { get { return m_Name; } private set { m_Name = value; } }
    public Displayable Displayable { get { return m_Builder?.Displayable ?? new Displayable(); } }

    public virtual List<ISerializableThing> SerializableObjects { get { return new List<ISerializableThing>() { m_Builder }; } set { } }
    public List<ThingID> ThingIDs { get { return m_ThingIDs; } set { m_ThingIDs = value; } }
    public IThingSerializer Serializer { get { return this; } set { } }
    #endregion

    #region Constructors
    public Entity()
    {
        m_Builder = null;
    }

    public Entity(EntityBuilder<B, D> builder)
    {
        m_Name = builder.AssignedName;
        m_Builder = builder;
    }

    public void OnBeforeSerialize()
    {
        Serializer.Sync();
    }

    public void OnAfterDeserialize()
    {
        GetSerializedProperties();
    }

    public virtual Tuple<ISerializableThing[], int> GetSerializedProperties()
    {
        var arr = Serializer.GetAll();
        var index = 0;
        m_Builder = (EntityBuilder<B, D>)arr[index++];

        return new Tuple<ISerializableThing[], int>(arr, index);
    }

    #endregion
}

