using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Manger
{
    public partial class SettingsHolder
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public SettingsHolder(string ObjectName, object originalcolor)
        {
            Name = ObjectName;
            Value = originalcolor;
        }
    }
}
