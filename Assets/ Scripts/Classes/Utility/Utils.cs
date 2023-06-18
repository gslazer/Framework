using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    public static void Append<K, V>(this Dictionary<K, V> first, Dictionary<K, V> second)
    {
        List<KeyValuePair<K, V>> pairs = second.ToList();
        pairs.ForEach(pair => first.Add(pair.Key, pair.Value));
    }
    public static Dictionary<TEnum, T> LoadPrefabsToDictionary<TEnum,T>(string path) 
        where TEnum : Enum
        where T : UnityEngine.Object
    {
        Dictionary<TEnum, T> prefabDict = Resources.LoadAll<T>(path)
            .ToDictionary(
                obj => EnumUtil<TEnum>.Parse(obj.name),
                obj => obj
            );
        return prefabDict;
    }
}

//gz : Util - String to Enum
public static class EnumUtil<T> {
    public static T Parse(string s) {
 	    return (T)Enum.Parse(typeof(T), s);
    }
}