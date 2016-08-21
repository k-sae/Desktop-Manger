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
        public static string loc = StartUp.Location() + "file.txt";
        private static List<PowerPlan> CurrentPowerPlanes = new List<PowerPlan>();
        public power()
        {
            InitializeComponent();
            GETplans();
            divdeplans();
        }
       

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
            for (int i = 0; i < lines.Count(); i++)
            {
                PowerPlan bed = new PowerPlan();
                bed.Name = GetStrBetweenTags(lines[4], "(", ")");
                bed.Id = GetStrBetweenTags(lines[4], "GUID: ", "  (");
                CurrentPowerPlanes.Add(bed);
            }
            
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
