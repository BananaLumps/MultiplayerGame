using System;
using System.Reflection;

namespace Base.ModularUI.DataBinding
{
    public class DataBindInfo
    {
        public string SubmenuPath
        {
            get; private set;
        }
        public DataBindType Type
        {
            get; private set;
        }
        public string Name
        {
            get; private set;
        }
        public MemberInfo MemberInfo
        {
            get; private set;
        }
        public Type ClassType
        {
            get; private set;
        }

        public DataBindInfo(string submenuPath, DataBindType type, string name, MemberInfo memberInfo)
        {
            SubmenuPath = submenuPath;
            Type = type;
            Name = name;
            MemberInfo = memberInfo;
        }

        public DataBindInfo(string submenuPath, DataBindType type, string name, Type classType)
        {
            SubmenuPath = submenuPath;
            Type = type;
            Name = name;
            ClassType = classType;
        }
    }
}

// [DataBind("Player\\Inventory\\Item0\\Name", DataBindType.Text, "CustomExampleFieldName")]