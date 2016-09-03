using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Manger
{
    public partial class ThemeChanger
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public ThemeChanger(string ObjectName, object originalcolor)
        {
            Name = ObjectName;
            Value = originalcolor;
        }
    }
}
