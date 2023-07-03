using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IThingSerializer : ISerializationCallbackReceiver
{
    List<ISerializableThing> SerializableObjects { get; set; }
    List<ThingID> ThingIDs { get; set; }

    public Tuple<ISerializableThing[], int> GetSerializedProperties();

    public virtual void Register()
    {
        foreach (ISerializableThing obj in SerializableObjects)
        {
            ThingSerializer.Register(obj);
        }
    }

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
                ThingIDs.Add(ThingID.EMPTY);
            }
        }
    }

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
