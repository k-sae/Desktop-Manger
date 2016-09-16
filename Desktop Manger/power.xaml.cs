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
        public static string loc = SaveFiles.Location() + "file.txt";
        private static List<PowerPlan> CurrentPowerPlanes = new List<PowerPlan>();
        public power()
        {
            InitializeComponent();
            GETplans();
            divdeplans();
            SetTheme();

        }


        private static void GETplans()
        {

            if (File.Exists(loc))
            {
                File.Delete(loc);
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
            else
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
        }
        private void divdeplans()
        {
            string[] lines = File.ReadAllLines(loc);


            for (int i = 0; i < lines.Count() - 3; i++)
            {
                //Constractor
                PowerPlan bed = new PowerPlan(St1, GetStrBetweenTags(lines[i + 3], "GUID: ", "  ("), GetStrBetweenTags(lines[i + 3], "(", ")"), i + 1, 1);
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
        private void SetTheme()
        {
            Grid1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
            St.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(Grid1.Background.ToString())));
            St_text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
            power_txt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
            St_text1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
            foreach (object obj1 in St1.Children)
            {
                if(obj1 is StackPanel)
                {
                    (obj1 as StackPanel).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
                }
            }

        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(AppTheme.Background)));
        }
        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString((AppTheme.Background)));
        }

        private void Shutdown_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
            process.StandardInput.WriteLine(@"shutdown /s /f /t 600");
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();

        }
       

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
         
        }
    }
    }

    

 
