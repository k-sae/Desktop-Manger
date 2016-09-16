using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Desktop_Manger
{
    class ShortcutItem : Canvas
    {
        public string ShortCutLocation { get; set; }
        public ShortcutItem(string file)
        {
            //To get the original exe file instead of shortcut
            ShortCutLocation = Path.GetExtension(file).ToLower() == ".lnk" ? LayoutObjects.GetOriginalFileURL(file) : file;
            
        }
    }
}
