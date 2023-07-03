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
    protected List<S> m_Elements = new();

    #endregion

    public virtual void Initialize(List<S> objects)
    {
        this.m_Elements = objects;
        EnableSatisfier();
    }

    #region Instance Properties:

    /// <summary>
    /// The objects in this Container.
    /// </summary>
    public virtual List<S> Elements { get { return m_Elements; } }
    #endregion

    #region Methods:

    public override void EnableSatisfier()
    {
        base.EnableSatisfier();
        // watch all the objects in the inventory
        foreach (S obj in Elements)
        {
            CheckSatisfyOnChange(obj);
        }
    }

    /// <summary>
    /// Adds the given object to this Container.
    /// </summary>

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

