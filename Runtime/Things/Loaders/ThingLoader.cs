using System;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Abstract class that provides functionality to load and manage 'Thing' objects that implement the <see cref="IScriptableThing"/> interface.
/// </summary>
/// <typeparam name="T">The type of the 'Thing' objects. Must implement <see cref="IScriptableThing"/>.</typeparam>
[Serializable]
[ExecuteInEditMode]
public abstract class ThingLoader<T> : MonoBehaviour where T : IScriptableThing
{
    #region Fields

    /// <summary>
    /// Holds references to the loaded 'Thing' objects.
    /// </summary>
    public Object[] m_Things;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Loads all 'Thing' objects of type <typeparamref name="T"/> from the Resources folder.
    /// </summary>
    void OnEnable()
    {
        m_Things = Resources.LoadAll("", typeof(T));
    }

    /// <summary>
    /// Called when the behaviour becomes disabled or inactive.
    /// Clears all references to the loaded 'Thing' objects.
    /// </summary>
    void OnDisable()
    {
        Array.Clear(m_Things, 0, m_Things.Length);
        m_Things = null;
    }

    #endregion
}
