using System;
using System.Collections.Generic;

public class EnumTypeCreator<TEnum, TBaseObject> where TEnum : Enum where TBaseObject : class
{
    IDictionary<TEnum, Type> mTypes;

    public EnumTypeCreator(string prefix)
    {
        BuildDictionary(prefix, out mTypes);
    }

    public TBaseObject Create(TEnum e)
    {
        if (!mTypes.TryGetValue(e, out var type))
        {
            return null;
        }
        if (null == type)
        {
            return null;
        }
        var instance = Activator.CreateInstance(type);
        if (null == instance)
        {
            return null;
        }
        return instance as TBaseObject;
    }

    void BuildDictionary(string prefix, out IDictionary<TEnum, Type> types)
    {
        var enumType = typeof(TEnum);
        var baseType = typeof(TBaseObject);
        var values = Enum.GetValues(enumType);
        var namespacePath = enumType.Namespace;
        if (null == namespacePath)
        {
            namespacePath = string.Empty;
        }
        types = new Dictionary<TEnum, Type>(values.Length);
        foreach (object value in values)
        {
            var name = Enum.GetName(enumType, value);
            if (null == name)
            {
                continue;
            }
            var type = CreateType(namespacePath, prefix, name);
            if (null == type)
            {
                continue;
            }
            if (!type.IsSubclassOf(baseType))
            {
                continue;
            }
            types.Add((TEnum)value, type);
        }
    }

    Type CreateType(string namespacePath, string prefix, string name)
    {
        if (null == name)
        {
            return null;
        }
        if (null == prefix)
        {
            prefix = string.Empty;
        }
        if (string.Empty != namespacePath)
        {
            namespacePath += ".";
        }
        var path = string.Concat(namespacePath, prefix, name);
        var type = Type.GetType(path);
        return type;
    }
}