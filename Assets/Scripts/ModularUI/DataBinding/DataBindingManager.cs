using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base.ModularUI.DataBinding
{

    public static class DataBindManager
    {
        public static void Init()
        {
            DataBinds = new Dictionary<DataBindType, Dictionary<string, DataBindInfo>>();
            foreach (DataBindInfo dataBind in GetDataBinds())
            {
                if (!DataBinds.ContainsKey(dataBind.Type))
                {
                    DataBinds[dataBind.Type] = new Dictionary<string, DataBindInfo>();
                }
                DataBinds[dataBind.Type][dataBind.Name] = dataBind;
            }
        }
        public static DataBindInfo FindBind(DataBindType type, string name)
        {
            if (DataBinds.ContainsKey(type) && DataBinds[type].ContainsKey(name))
            {
                return DataBinds[type][name];
            }
            return null;
        }
        public static bool TriggerEvent(string name, object target)
        {
            DataBindInfo dataBind = FindBind(DataBindType.Action, name);
            if (dataBind != null)
            {
                MethodInfo method = (MethodInfo)dataBind.MemberInfo;
                method.Invoke(target, null);
                return true;
            }
            return false;
        }
        public static Dictionary<DataBindType, Dictionary<string, DataBindInfo>> DataBinds
        {
            get; private set;
        }
        static List<DataBindInfo> GetDataBinds()
        {
            List<DataBindInfo> dataBinds = new List<DataBindInfo>();
            Assembly assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes())
            {
                // Scan for DataBindAttribute on classes
                DataBindAttribute classAttribute = type.GetCustomAttribute<DataBindAttribute>();
                if (classAttribute != null)
                {
                    string name = classAttribute.Name ?? type.Name;
                    dataBinds.Add(new DataBindInfo(classAttribute.SubmenuPath, classAttribute.Type, name, type));
                }

                // Scan for DataBindAttribute on fields
                foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    DataBindAttribute attribute = field.GetCustomAttribute<DataBindAttribute>();
                    if (attribute != null)
                    {
                        string name = attribute.Name ?? field.Name;
                        dataBinds.Add(new DataBindInfo(attribute.SubmenuPath, attribute.Type, name, field));
                    }
                }

                // Scan for DataBindAttribute on properties
                foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    DataBindAttribute attribute = property.GetCustomAttribute<DataBindAttribute>();
                    if (attribute != null)
                    {
                        string name = attribute.Name ?? property.Name;
                        dataBinds.Add(new DataBindInfo(attribute.SubmenuPath, attribute.Type, name, property));
                    }
                }

                // Scan for DataBindAttribute on methods
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    DataBindAttribute attribute = method.GetCustomAttribute<DataBindAttribute>();
                    if (attribute != null)
                    {
                        string name = attribute.Name ?? method.Name;
                        dataBinds.Add(new DataBindInfo(attribute.SubmenuPath, attribute.Type, name, method));
                    }
                }
            }

            return dataBinds;
        }
        public static object GetValue(MemberInfo memberInfo, object target)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(target);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(target);
                case MemberTypes.Method:
                    return ((MethodInfo)memberInfo).Invoke(target, null);
                default:
                    throw new ArgumentException("Unsupported member type", nameof(memberInfo));
            }
        }
        public static T GetValue<T>(MemberInfo memberInfo, object target)
        {
            object value = GetValue(memberInfo, target);
            if (value is T typedValue)
            {
                return typedValue;
            }
            throw new InvalidCastException($"Cannot cast value to type {typeof(T)}");
        }
    }
}