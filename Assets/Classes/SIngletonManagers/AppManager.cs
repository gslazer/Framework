#define Debug

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AppManager : MonoSingleton<AppManager>
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

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("Escape Pushed!");
            UIManager.Instance.OnEscape();
        }
    }
}