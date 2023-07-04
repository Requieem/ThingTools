using UnityEngine;

/// <summary>
/// Utility class for logging messages, warnings, errors, exceptions, and objects.
/// </summary>
public class Log
{
    /// <summary>
    /// Logs a message to the console.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Msg(string message)
    {
        Debug.Log(message);
    }
    /// <summary>
    /// Logs a warning message to the console.
    /// </summary>
    /// <param name="message">The warning message to log.</param>
    public static void Wng(string message)
    {
        Debug.LogWarning(message);
    }

    /// <summary>
    /// Logs an error message to the console.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    public static void Err(string message)
    {
        Debug.LogError(message);
    }

    /// <summary>
    /// Logs an exception to the console.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    /// <param name="alt">An alternate error message (optional).</param>
    public static void Exc(System.Exception ex, string alt = null)
    {
        if (alt != null)
            Err(alt);
        Debug.LogException(ex);
    }

    /// <summary>
    /// Logs an object to the console.
    /// </summary>
    /// <param name="obj">The object to log.</param>
    /// <param name="alt">An alternate message (optional).</param>
    public static void Obj(object obj, string alt = null)
    {
        if (alt != null)
            Msg(alt);
        Debug.Log(obj);
    }
}