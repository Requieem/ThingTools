using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract base class for all Containers that exist in the game.
/// </summary>
public abstract class ObjectContainer<S> : OSatisfier<S>
{
    #region Instance Fields:

    [SerializeField]
    protected List<S> m_Elements = new List<S>();

    #endregion

    /// <summary>
    /// Initializes the container with the specified list of objects.
    /// </summary>
    /// <param name="objects">The list of objects to initialize the container with.</param>
    public virtual void Initialize(List<S> objects)
    {
        m_Elements = objects;
        EnableSatisfier();
    }

    #region Instance Properties:

    /// <summary>
    /// Gets the objects in this Container.
    /// </summary>
    public virtual List<S> Elements
    {
        get { return m_Elements; }
    }

    #endregion

    #region Methods:

    /// <summary>
    /// Enables the container's satisfier and starts watching all the objects in the container.
    /// </summary>
    public override void EnableSatisfier()
    {
        base.EnableSatisfier();

        // Watch all the objects in the container
        foreach (S obj in Elements)
        {
            CheckSatisfyOnChange(obj);
        }
    }

    /// <summary>
    /// Adds the given object to this Container.
    /// </summary>
    /// <param name="obj">The object to add.</param>
    public virtual void Add(S obj)
    {
        if (obj == null)
        {
            throw new Exception("Cannot add a null object to a Container.");
        }

        m_Elements.Add(obj);
        CheckSatisfyOnChange(obj);
    }

    /// <summary>
    /// Removes the given object from this Container.
    /// </summary>
    /// <param name="obj">The object to remove.</param>
    /// <returns>True if the object was successfully removed, false otherwise.</returns>
    public virtual bool Remove(S obj)
    {
        if (obj == null)
        {
            Log.Msg("Cannot remove a null object from a Container.");
            return false;
        }
        else if (m_Elements.Remove(obj))
        {
            CheckUnsatisfyOnChange(obj);
            return true;
        }
        else
        {
            Log.Msg("Cannot remove an object that is not in this Container.");
            return false;
        }
    }

    #endregion
}