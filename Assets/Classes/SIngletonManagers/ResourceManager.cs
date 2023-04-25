using System;
using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    ObjectPool<ePrefab> prefabObjectPool = new ObjectPool<ePrefab>();
    

    //Static Utility Methods
    //현재 : Resources.Load 기반으로 리소스 로드
    //개선 여지 : Addressable Asset Bundle 로 로드할수 있도록 추후 개선할 여지가 있음

    //클라이언트에서 Resources, Addressable 등 리소스 관리 방식에 의존성을 갖지 않도록 랩핑하여 사용.
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