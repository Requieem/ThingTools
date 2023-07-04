using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract base class for entities in the game.
/// </summary>
/// <typeparam name="D">The derived entity type.</typeparam>
/// <typeparam name="B">The entity builder type.</typeparam>
[Serializable]
public abstract class Entity<D, B> : ISerializationCallbackReceiver, IThingSerializer where D : Entity<D, B> where B : EntityBuilder<B, D>
{
    #region Instance Fields

    [SerializeField] protected string m_Name = "Entity";
    [SerializeField] protected List<ThingID> m_ThingIDs;
    [SerializeField] protected EntityBuilder<B, D> m_Builder;

    #endregion

    #region Instance Properties

    /// <summary>
    /// The name of the entity.
    /// </summary>
    public string Name { get { return m_Name; } private set { m_Name = value; } }

    /// <summary>
    /// The displayable object of the entity.
    /// </summary>
    public Displayable Displayable { get { return m_Builder?.Displayable ?? new Displayable(); } }

    /// <summary>
    /// The list of serializable objects associated with the entity.
    /// </summary>
    public virtual List<ISerializableThing> SerializableObjects { get { return new List<ISerializableThing>() { m_Builder }; } set { } }

    /// <summary>
    /// The list of ThingIDs associated with the entity.
    /// </summary>
    public List<ThingID> ThingIDs { get { return m_ThingIDs; } set { m_ThingIDs = value; } }

    /// <summary>
    /// The serializer for the entity.
    /// </summary>
    public IThingSerializer Serializer { get { return this; } set { } }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor for the entity.
    /// </summary>
    public Entity()
    {
        m_Builder = null;
    }

    /// <summary>
    /// Constructor for the entity that takes an entity builder.
    /// </summary>
    /// <param name="builder">The entity builder.</param>
    public Entity(EntityBuilder<B, D> builder)
    {
        m_Name = builder.AssignedName;
        m_Builder = builder;
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Method called before serialization.
    /// </summary>
    public void OnBeforeSerialize()
    {
        Serializer.Sync();
    }

    /// <summary>
    /// Method called after deserialization.
    /// </summary>
    public void OnAfterDeserialize()
    {
        GetSerializedProperties();
    }

    /// <summary>
    /// Gets the serialized properties of the entity.
    /// </summary>
    /// <returns>A tuple containing the serialized objects and the index.</returns>
    public virtual Tuple<ISerializableThing[], int> GetSerializedProperties()
    {
        var arr = Serializer.GetAll();
        var index = 0;
        m_Builder = (EntityBuilder<B, D>)arr[index++];

        return new Tuple<ISerializableThing[], int>(arr, index);
    }

    #endregion

}