public class ResourceManager : MonoSingleton<ResourceManager>
{
    ObjectPool<ePrefab> prefabObjectPool = new ObjectPool<ePrefab>();
}

public enum ePrefab
{
    
}