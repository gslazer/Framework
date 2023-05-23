using System;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool<TEnum> : MonoBehaviour
{
    //change SerializeField to ShowInInspector after import OdinInspector to Check objects at Inspector
    [SerializeField] private Dictionary <TEnum, Stack<GameObject>> dictOjectPool = new Dictionary<TEnum, Stack<GameObject>>();
    public GameObject PopObject(TEnum type)
    {
        if(!dictOjectPool.TryGetValue(type, out var stackObj))
        {
            //object pool do not have gameobject of 'type'.
            //instantiate that object    
        }
        if(stackObj.Count == 0)
        {
            //object pool do not have gameobject of 'type'.
            //instantiate that object
        }
        return stackObj.Pop();
    }
    public void PushObject(TEnum type, GameObject gameObj)
    {
        if(!dictOjectPool.TryGetValue(type, out var stackObj))
        {    
            stackObj = new Stack<GameObject>();
            dictOjectPool.Add(type, stackObj);
        }
        stackObj.Push(gameObj);
        gameObj.SetActive(false);
    }
}