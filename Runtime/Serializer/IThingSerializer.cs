using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a serializer for objects that implement the <see cref="ISerializableThing"/> interface.
/// </summary>
public interface IThingSerializer : ISerializationCallbackReceiver
{
    /// <summary>
    /// Gets or sets the list of objects that implement the <see cref="ISerializableThing"/> interface.
    /// </summary>
    List<ISerializableThing> SerializableObjects { get; set; }

    /// <summary>
    /// Gets or sets the list of <see cref="ThingID"/> of the objects that implement the <see cref="ISerializableThing"/> interface.
    /// </summary>
    List<ThingID> ThingIDs { get; set; }

    /// <summary>
    /// Retrieves the properties of the serialized objects and their count.
    /// </summary>
    /// <returns>A tuple that contains the properties of the serialized objects and their count.</returns>
    public Tuple<ISerializableThing[], int> GetSerializedProperties();

    /// <summary>
    /// Registers all objects in the <see cref="SerializableObjects"/> list with the <see cref="ThingSerializer"/>.
    /// </summary>
    public virtual void Register()
    {
        foreach (ISerializableThing obj in SerializableObjects)
        {
            ThingSerializer.Register(obj);
        }
    }

    /// <summary>
    /// Synchronizes the <see cref="SerializableObjects"/> and the <see cref="ThingIDs"/> lists.
    /// </summary>
    public virtual void Sync()
    {
        Register();

        var count = SerializableObjects.Count;

        ThingIDs ??= new(count);
        ThingIDs.Clear();

        for (int i = 0; i < count; i++)
        {
            if (SerializableObjects[i] != null)
            {
                SerializableObjects[i].Persist();
                ThingIDs.Add(new(SerializableObjects[i].Identifier));
            }
            else
            {
                ThingIDs.Add(ThingID.m_EMPTY);
            }
        }
    }

    /// <summary>
    /// Retrieves all the objects that implement the <see cref="ISerializableThing"/> interface.
    /// </summary>
    /// <returns>An array of all the objects that implement the <see cref="ISerializableThing"/> interface.</returns>
    public virtual ISerializableThing[] GetAll()
    {
        var count = SerializableObjects.Count;
        if (ThingIDs is null || ThingIDs.Count != count)
            Sync();

        var arr = new ISerializableThing[ThingIDs.Count];
        foreach (var id in ThingIDs)
        {
            arr[ThingIDs.IndexOf(id)] = ThingSerializer.Get(id);
        }

        return arr;
    }
}
