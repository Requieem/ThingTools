using UnityEngine;

/// <summary>
/// An interface that combines IDisplayable and ISerializableThing interfaces.
/// </summary>
public interface IScriptableThing : IDisplayable, ISerializableThing { }

/// <summary>
/// An abstract class that defines scriptable things.
/// </summary>
/// <typeparam name="D">The type of the scriptable thing. Must inherit from <see cref="ScriptableThing{D}"/>.</typeparam>
public abstract class ScriptableThing<D> : ScriptableObject, IScriptableThing, ILister<D> where D : ScriptableThing<D>
{
    #region Serialized Fields

    [SerializeField] protected Displayable m_Displayable;
    [SerializeField] protected ThingID m_Identifier = new ThingID();
    [SerializeField] protected Lister<D> m_Lister = new Lister<D>();

    #endregion

    #region Properties

    /// <summary>
    /// The displayable object of the scriptable thing.
    /// </summary>
    public Displayable Displayable => m_Displayable;

    /// <summary>
    /// The identifier of the scriptable thing.
    /// </summary>
    public ThingID Identifier { get => m_Identifier; set => m_Identifier = value; }

    /// <summary>
    /// The lister for managing the scriptable thing.
    /// </summary>
    public Lister<D> Lister { get => m_Lister; set => m_Lister = value; }

    #endregion

    #region Methods

    /// <summary>
    /// Serializes the scriptable thing.
    /// </summary>
    public virtual void Serialize()
    {
        Lister.AddListable(this as D);
    }

    /// <summary>
    /// Called before the scriptable thing is serialized.
    /// </summary>
    public virtual void OnBeforeSerialize()
    {
        Serialize();
    }

    /// <summary>
    /// Called after the scriptable thing is deserialized.
    /// </summary>
    public virtual void OnAfterDeserialize()
    {
        Serialize();
    }

    private void OnDestroy()
    {
        Lister.RemoveListable(this as D);
    }

    private void OnEnable()
    {
        Enable();
    }

    /// <summary>
    /// Performs initialization tasks when the scriptable thing is enabled.
    /// </summary>
    public virtual void Enable()
    {
        m_Displayable ??= new Displayable();
        m_Lister ??= new Lister<D>();
        ThingSerializer.Register(this as D);
        Serialize();
    }

    /// <summary>
    /// Initializes the displayable object of the scriptable thing.
    /// </summary>
    /// <param name="displayable">The displayable object to set.</param>
    public void Initialize(Displayable displayable)
    {
        m_Displayable = displayable;
        Enable();
    }

    #endregion

}
