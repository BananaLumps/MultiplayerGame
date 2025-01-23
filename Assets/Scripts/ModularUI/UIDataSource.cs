using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ModularUI
{
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
                   AttributeTargets.Field | AttributeTargets.Class)
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
