#define Debug

using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Wrap UnityEngine.Debug.Log Methods
/// </summary>
public class Debug
{
    [Conditional("Debug")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }
    [Conditional("Debug")]
    public static void Log(object message, Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }
    [Conditional("Debug")]
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }
    [Conditional("Debug")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }
}