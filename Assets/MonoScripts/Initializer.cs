using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Initializer : MonoSingleton<Initializer>, IInitializable
{
    public void InitManagers()
    {
        AppManager.Instance.Initialize();
        DataManager.Instance.Initialize();
        GameManager.Instance.Initialize();
        UIManager.Instance.Initialize();
    }

    void Awake()
    {
        InitManagers();
    }
}