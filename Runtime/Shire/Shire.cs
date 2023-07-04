using System;
using UnityEngine;

/// <summary>
/// Abstract base class for Shire objects.
/// </summary>
/// <typeparam name="T">The derived type of the Shire.</typeparam>
[Serializable]
public abstract class Shire<T> : ScriptableThing<T>, IShire where T : Shire<T>
{
    [SerializeField] private bool m_Used = true;
    [SerializeField] protected int m_Order = -1;

    /// <summary>
    /// Gets or sets a value indicating whether the Shire is used.
    /// </summary>
    public bool Used { get => m_Used; set => m_Used = value; }

    /// <summary>
    /// Gets or sets the order of the Shire.
    /// </summary>
    public int Order { get => m_Order; set => m_Order = value; }

    /// <summary>
    /// Serializes the Shire object.
    /// </summary>
    public override void Serialize()
    {
        if (!m_Used)
        {
            Lister.RemoveListable(this as T);
        }
        else if (m_Used)
        {
            Lister.AddListable(this as T);
        }
    }

    private void OnDestroy()
    {
        m_Used = false;
        Lister.RemoveListable(this as T);
    }

    /// <summary>
    /// Determines the order criteria for the given Shire object.
    /// </summary>
    /// <param name="thing">The Shire object.</param>
    /// <returns>The order criteria as a string.</returns>
    public virtual string OrderCriteria(T thing)
    {
        return thing.m_Order.ToString();
    }

    /// <summary>
    /// Enables the Shire object.
    /// </summary>
    public override void Enable()
    {
        var orderCriteria = new Func<T, string>(x => OrderCriteria(x));
        m_Lister = new Lister<T>(orderCriteria);

        base.Enable();
    }
}
