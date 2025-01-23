using System;

namespace Base.ModularUI.DataBinding
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class DataBindAttribute : Attribute
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

        public DataBindAttribute(string submenuPath, DataBindType type, string name)
        {
            SubmenuPath = submenuPath;
            Type = type;
            Name = name;
        }
    }
}

// [DataBind("Player\\Inventory\\Item0\\Name", DataBindType.Text, "CustomExampleFieldName")]