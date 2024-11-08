using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ModularUI
{
    //public abstract class UIDataSource
    //{
    //    public abstract string Name
    //    {
    //        get;
    //    }
    //    public abstract Dictionary<string, object> DataBinds
    //    {
    //        get;
    //    }
    //}
    [AttributeUsage(AttributeTargets.Property |
                       AttributeTargets.Field)
]
    public class UIDataBind : Attribute
    {
        private string Name;
        private string DataSource;

        public UIDataBind(string name, string dataSource)
        {
            Name = name;
            DataSource = dataSource;
        }
    }
    [AttributeUsage(AttributeTargets.Property |
                   AttributeTargets.Field)
]
    public class UIDataSource : Attribute
    {
        private string Name;
        private string Parent;

        public UIDataSource(string name, string parent = "")
        {
            Name = name;
            Parent = parent;
        }
    }
}
