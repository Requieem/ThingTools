using UnityEngine;

public class ThingBuilder<D, T> : ABuilder<D, T> where D : ThingBuilder<D, T>
{
    public override T GetCopy()
    {
        var json = JsonUtility.ToJson(m_Built);
        var copy = JsonUtility.FromJson<T>(json);
        return copy;
    }
}






