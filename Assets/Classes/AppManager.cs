#define Debug

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AppManager : Singleton<AppManager>
{
    public override void Initialize()
    {
    }

    public void Pause()
    {
        //todo later
    }
    public void OnQuit()
    {
        Application.Quit();
    }
}

public class Debug
{

    //[Conditional("Debug")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }
    //[Conditional("Debug")]
    public static void Log(object message, Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }
}