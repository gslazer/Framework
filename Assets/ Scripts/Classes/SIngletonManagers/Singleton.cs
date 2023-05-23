using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Singleton<T> : IInitializable where T : Singleton<T>, new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
    public abstract void Initialize();
}
public abstract class MonoSingleton<T> : MonoBehaviour, IInitializable where T : MonoSingleton<T>
{
    private static MonoSingleton<T> instance;
    private bool instantiated = false;
    public bool Instantiated { get => instantiated; }

    public static MonoSingleton<T> Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent(typeof(T)) as T;
            }
            return instance;
        }
    }
    public virtual void Initialize()
    {
        if(instantiated)
        {
            Debug.LogWarning($"Singleton.Instantiate() : [{gameObject.name}] Is Already Instantiated!");
            return;
        }
        DontDestroyOnLoad(gameObject);
        instantiated = true;
    }
    void Awake() {
        if(!instantiated)
            Initialize();
    }
}