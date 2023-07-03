using System.Collections.Generic;

public class ThingSerializer
{
    public static Dictionary<string, ISerializableThing> m_Map = new();

    public static ISerializableThing Get(ThingID id)
    {
        if (id == null) return null;
        else if (id == ThingID.EMPTY) return null;
        else if (!m_Map.ContainsKey(id.Id)) return null;
        else return m_Map[id.Id];
    }

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

    public static void Unregister(ISerializableThing serializable)
    {
        var id = serializable.Identifier;
        var keyPresent = m_Map.ContainsKey(id.Id);
        var keyEquals = keyPresent && m_Map[id.Id].Equals(serializable);

        if(keyPresent && keyEquals)
        {
            m_Map.Remove(id.Id);
        }
    }
}