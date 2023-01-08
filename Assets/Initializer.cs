using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Awake()
    {
        AppManager.Instance.Initialize();
        DataManager.Instance.Initialize();
        GameManager.Instance.Initialize();
        UIManager.Instance.Initialize();
    }
}