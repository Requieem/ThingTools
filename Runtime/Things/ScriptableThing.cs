using UnityEngine;

#if UNITY_EDITOR
#endif

public interface IScriptableThing : IDisplayable, ISerializableThing { }

public abstract class ScriptableThing<D> : ScriptableObject, IScriptableThing, ILister<D> where D : ScriptableThing<D>
{
    #region Instance Fields:

    [SerializeField] protected Displayable m_Displayable;
    [SerializeField] protected ThingID m_Identifier = new();
    [SerializeField] protected Lister<D> m_Lister = new();

    #endregion

    #region Instance Properties:

    public Displayable Displayable { get { return m_Displayable; } }
    public ThingID Identifier { get { return m_Identifier; } set { m_Identifier = value; } }
    public Lister<D> Lister { get { return m_Lister; } set { m_Lister = value; } }

    #endregion

    #region Instance Methods:

    public virtual void ListerSerialization()
    {
        Lister.AddListable(this as D);
    }

    public virtual void OnBeforeSerialize()
    {
        ListerSerialization();
    }

    public virtual void OnAfterDeserialize()
    {
        ListerSerialization();
    }

    private void OnDestroy()
    {
        Lister.RemoveListable(this as D);
    }
    private void OnEnable()
    {
        Enable();
    }

    public virtual void Enable()
    {
        m_Displayable ??= new();
        m_Lister ??= new();
        ThingSerializer.Register(this as D);
        ListerSerialization();

        /*#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
        #endif*/

    }

    public void Initialize(Displayable displayable)
    {
        this.m_Displayable = displayable;
    }

    #endregion
}
