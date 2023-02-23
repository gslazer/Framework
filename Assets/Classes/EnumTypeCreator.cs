using System;
using System.Collections.Generic;
/// <summary>
/// Enum과 1대1로 매칭되는 특정 자식 클래스의 인스턴스를 생성하기 위한 클래스 
/// 
/// 1.생성자 EnumTypeCreator(string prefix)에서 TBaseObject를 상속받고, 이름이 prefix + TEnum.name 로 정의된 클래스를 찾아
/// 그 Type을 멤버 Dictionary<TEnum,TYpe> mType으로 보관한다.
/// 
/// 2. 필요한 곳에서 Create(TEnum e)를 사용. TEnum을 인자로 해당 enum에 매칭되는 클래스 Type을 mType에서 찾아, 인스턴스를 생성하여 리턴한다.
/// 
/// </summary>
/// <typeparam name="TEnum"></typeparam>
/// <typeparam name="TBaseObject"></typeparam>
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