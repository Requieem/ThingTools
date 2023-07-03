using System;
using UnityEngine.Events;

public interface IObjectSatisfier<T> : ISatisfier<ObjectSatisfier<T>, T> {
    public bool CheckSatisfyOnChange(T obj);
    public bool CheckUnsatisfyOnChange(T obj);
    public float Watch(T obj, UnityEvent onSatisfy, UnityEvent onUnsatisfy);
    public float WatchFeedback(T obj, UnityEvent onSatisfy, UnityEvent onUnsatisfy);
    public void Unwatch(T obj);
    public bool IsWatching(T obj);
}