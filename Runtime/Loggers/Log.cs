using UnityEngine;

public class Log
{
    public static void Msg(string message)
    {
        Debug.Log(message);
    }

    public static void Wng(string message)
    {
        Debug.LogWarning(message);
    }

    public static void Err(string message)
    {
        Debug.LogError(message);
    }

    public static void Exc(System.Exception ex, string alt = null)
    {
        if (alt != null)
            Err(alt);
        Debug.LogException(ex);
    }

    public static void Obj(object obj, string alt = null)
    {
        if (alt != null)
            Msg(alt);
        Debug.Log(obj);
    }
}
