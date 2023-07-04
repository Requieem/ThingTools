using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a lister that contains and manages a list of scriptable objects of type <typeparamref name="D"/>.
/// </summary>
/// <remarks>
/// This class also provides functionality to add, remove, order, and toggle elements within the list.
/// </remarks>
/// <typeparam name="D">The type of the elements in the lister. Must inherit from <see cref="ScriptableObject"/> and implement the <see cref="ILister{D}"/> interface.</typeparam>
[Serializable]
public class Lister<D> where D : ScriptableObject, ILister<D>
{
    #region Instance Fields

    [SerializeField] protected List<D> m_Siblings = new List<D>();

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the list of sibling elements in the lister.
    /// </summary>
    public List<D> Siblings { get => m_Siblings; set => m_Siblings = value; }

    /// <summary>
    /// Gets or sets the action to perform when an element is added.
    /// </summary>
    public Action OnAdd { get => m_OnAdd; set => m_OnAdd = value; }

    /// <summary>
    /// Gets or sets the action to perform when an element is removed.
    /// </summary>
    public Action OnRemove { get => m_OnRemove; set => m_OnRemove = value; }

    #endregion

    #region Methods

    /// <summary>
    /// Initializes a new instance of the <see cref="Lister{D}"/> class.
    /// </summary>
    /// <param name="orderCriteria">The criteria to order the elements in the lister. If null, the default criteria will be used.</param>
    public Lister(Func<D, string> orderCriteria = null)
    {
        m_OrderCriteria = orderCriteria ?? DefaultCriteria;
    }

    /// <summary>
    /// Synchronizes the list of sibling elements with the main elements list.
    /// </summary>
    public void Sync()
    {
        OrderListables();
        m_Siblings.Clear();
        m_Siblings.AddRange(m_Elements);
    }

    /// <summary>
    /// Adds an element to the lister.
    /// </summary>
    /// <param name="item">The element to add.</param>
    public void AddListable(D item)
    {
        if (!m_Elements.Contains(item))
        {
            m_Elements.Add(item);
        }

        Sync();
        OnAdd?.Invoke();
    }

    /// <summary>
    /// Removes an element from the lister.
    /// </summary>
    /// <param name="item">The element to remove.</param>
    public void RemoveListable(D item)
    {
        m_Elements.Remove(item);
        Sync();
        OnRemove?.Invoke();
    }

    /// <summary>
    /// Orders the elements in the lister according to the current order criteria.
    /// </summary>
    public virtual void OrderListables()
    {
        m_Elements = m_Elements
                        .Where(x => x != null && x is not null)
                        .OrderBy(m_OrderCriteria)
                        .ToList();
    }

    /// <summary>
    /// Gets an ordered list of the elements in the lister.
    /// </summary>
    /// <returns>An ordered list of the elements in the lister.</returns>
    public static List<D> OrderedElements()
    {
        return m_Elements
                .Where(x => x != null && x is not null)
                .OrderBy(m_OrderCriteria)
                .ToList();
    }

    #endregion

    #region Static Fields

    // Represents the default criteria to order the elements in the lister.
    private static readonly Func<D, string> DefaultCriteria = x => "";

    // Represents the action to perform when an element is added.
    private static Action m_OnAdd;

    // Represents the action to perform when an element is removed.
    private static Action m_OnRemove;

    // The OrderCriteria used to order elements in the lister.
    private static Func<D, string> m_OrderCriteria = null;

    // The List of Elements in this lister.
    private static List<D> m_Elements = new();

    #endregion
}

