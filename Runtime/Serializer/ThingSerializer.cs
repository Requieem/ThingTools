using System.Collections.Generic;

/// <summary>
/// A class to manage serializable things and provide lookups by identifier.
/// </summary>
public class ThingSerializer
{
    /// <summary>
    /// Map to hold serializable things, keyed by their string identifier.
    /// </summary>
    public static Dictionary<string, ISerializableThing> m_Map = new();

    /// <summary>
    /// Retrieves the serializable thing associated with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the serializable thing.</param>
    /// <returns>The serializable thing if it exists, otherwise null.</returns>
    public static ISerializableThing Get(ThingID id)
    {
        if (id == null || id == ThingID.m_EMPTY || !m_Map.ContainsKey(id.Id))
            return null;

        return m_Map[id.Id];
    }

    /// <summary>
    /// Registers a serializable thing. If the thing's identifier is already in use, it will be given a new one.
    /// </summary>
    /// <param name="serializable">The serializable thing to register.</param>
    /// <returns>True if the serializable thing was successfully registered, otherwise false.</returns>
    public static bool Register(ISerializableThing serializable)
    {
        if (serializable == null) return false;

        serializable.Persist();

        var id = serializable.Identifier;
        var keyPresent = m_Map.ContainsKey(id.Id);
        var keyEquals = keyPresent && m_Map[id.Id].Equals(serializable);

        if (!keyEquals)
        {
            if (keyPresent)
            {
                serializable.OnDuplicate();
                id = serializable.Identifier;
            }

            m_Map.Add(id.Id, serializable);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Unregisters a serializable thing.
    /// </summary>
    /// <param name="serializable">The serializable thing to unregister.</param>
    public static void Unregister(ISerializableThing serializable)
    {
        var id = serializable.Identifier;
        var keyPresent = m_Map.ContainsKey(id.Id);
        var keyEquals = keyPresent && m_Map[id.Id].Equals(serializable);

        if (keyPresent && keyEquals)
        {
            m_Map.Remove(id.Id);
        }
    }
}