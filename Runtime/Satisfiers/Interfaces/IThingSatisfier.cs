using UnityEngine.Events;

public interface IThingSatisfier<K, V> : ISatisfier<ThingSatisfier<K, V>, V>
{
    public bool CheckSatisfyOnChange(K _key, V _value);
    public bool CheckUnsatisfyOnChange(K _key, V _value);
    public float Watch(K _key, V _value, UnityEvent onSatisfy, UnityEvent onUnsatisfy);
    public void Unwatch(K _key, V _value);
    public float WatchFeedback(K _key, V _value, UnityEvent onSatisfy, UnityEvent onUnsatisfy);
    public bool IsWatching(K _key);
    public bool IsWatching(K _key, V _value);
}