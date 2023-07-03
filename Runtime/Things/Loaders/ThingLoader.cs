using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
[ExecuteInEditMode]
public abstract class ThingLoader<T> : MonoBehaviour where T : IScriptableThing
{
    public Object[] m_Things;

    void OnEnable()
    {
        m_Things = Resources.LoadAll("", typeof(T));
    }

    void OnDisable()
    {
        Array.Clear(m_Things, 0, m_Things.Length);
        m_Things = null;
    }
}