using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Manger
{
    class Extension
    {
        public string Extensions { get; set; }
        public string URL { get; set; }

        public Extension(string Extensions, string URL)
        {
            this.Extensions = Extensions;
            this.URL = URL;
        }

        public void AddExtension(string extension)
        {
            if(extension.StartsWith("."))
                Extensions += " " + extension;
            else
                Extensions += " ." + extension;
        }
    }
}
