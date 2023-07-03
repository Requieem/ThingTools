using System;
public interface ISatisfier<S, T> where S : Satisfier<T> {
    S Satisfier { get; }
    public Comparison<T> Comparer { get; }
    public Func<T, T, bool> Equator { get; }
    public bool EnsureSatisfier();
    public void EnableSatisfier();
}