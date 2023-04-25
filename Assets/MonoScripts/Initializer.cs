using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Initializer : MonoBehaviour, IInitializable
{
    public void Initialize()
    {
        AppManager.Instance.Initialize();
        DataManager.Instance.Initialize();
        GameManager.Instance.Initialize();
        UIManager.Instance.Initialize();
    }

    void Awake()
    {
        Initialize();
    }
}