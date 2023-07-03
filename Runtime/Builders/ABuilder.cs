using UnityEngine;

public interface IBuilder : IScriptableThing { }

public abstract class ABuilder<D, T> : ScriptableThing<D>, IBuilder, IBuilder<T> where D : ABuilder<D, T>
{
    [SerializeField] protected T m_Built;

    public T Built { get { return m_Built; } }

    public abstract T GetCopy();
    public V GetSafeCopy<V,K>(K builder) where K : ABuilder<K, V>
    {
        if (builder is not null)
        {
            return builder.GetCopy();
        }
        else return default;
    }
    public virtual T GetInstance()
    {
        if(m_Built == null)
        {
            m_Built = GetCopy();
        }

        return m_Built;
    }
}








