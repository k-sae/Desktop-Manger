using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace Desktop_Manger
{
    /// <summary>
    /// Interaction logic for power.xaml
    /// </summary>
    public partial class power : Page
    {
        public power()
        {
            InitializeComponent();
            GETplans();
            divdeplans();
        }
        public static string loc = StartUp.Location() + "file.txt";

        public class plans{
            public string planname { get; set; }
            public string planschyema { get; set; }
        };
        private static void GETplans()
        {
 
            
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();
            process.StandardInput.WriteLine(@"powercfg -LIST >> " + "\"" + loc + "\"");
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
        }
        private static void divdeplans()
        {
            string[] lines = File.ReadAllLines(loc);
            int cnt = lines.Count();
            plans[] bed = new plans[cnt-4];


            MessageBox.Show(lines[4]);   
               bed[0].planname = GetStrBetweenTags(lines[4],"(",")");
             bed[0].planschyema = GetStrBetweenTags(lines[4], "GUID: ", "  (");
            
   

        }
        public static string GetStrBetweenTags(string value,
                                       string startTag,
                                       string endTag)
        {
            
            if (value.Contains(startTag) && value.Contains(endTag))
            {
                int index = value.IndexOf(startTag) + startTag.Length;
                return value.Substring(index, value.IndexOf(endTag) - index);
            }
            else
                return null;
        }
    }

 
}
