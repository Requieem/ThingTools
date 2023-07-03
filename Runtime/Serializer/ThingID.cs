using System;
using UnityEngine;

[Serializable]
public class ThingID
{
    private readonly Guid guid;
    [SerializeField] private string id;

    public static readonly ThingID EMPTY = new(Guid.Empty);

    public string Id { get { return id; } }
    public Guid Guid { get { return guid; } }

    public ThingID(ThingID shireID)
    {
        guid = shireID.Guid;
        id = shireID.Id;
    }

    public ThingID(Guid _guid)
    {
        guid = _guid;
        id = guid.ToString();
    }

    public ThingID()
    {
        var guid = Guid.NewGuid();
        id = guid.ToString();
    }
}