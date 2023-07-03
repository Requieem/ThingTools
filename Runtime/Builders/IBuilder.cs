public interface IBuilder<T>
{
    public T GetCopy();
    public T GetInstance();
    T Built { get; }
}








