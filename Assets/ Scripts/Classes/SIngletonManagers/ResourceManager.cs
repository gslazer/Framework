using System;
using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    ObjectPool<ePrefab> prefabObjectPool = new ObjectPool<ePrefab>();
    

    //Static Utility Methods
    //���� : Resources.Load ������� ���ҽ� �ε�
    //���� ���� : Addressable Asset Bundle �� �ε��Ҽ� �ֵ��� ���� ������ ������ ����

    //Ŭ���̾�Ʈ���� Resources, Addressable �� ���ҽ� ���� ��Ŀ� �������� ���� �ʵ��� �����Ͽ� ���.
    public static UnityEngine.Object Load(string path)
    {
        return Resources.Load(path, typeof(UnityEngine.Object));
    }

    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        return (T)Resources.Load(path, typeof(T));
    }
}

public enum ePrefab
{
}