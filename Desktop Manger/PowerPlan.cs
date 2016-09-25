using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Desktop_Manger
{
    class PowerPlan
    {
        public string Name { get; set; }
        public string Id { get; set; }
        //contstactor 
        //see divideplanes funtion to see how to use it
        public PowerPlan(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;



            //Here create stackpanel 
            // create the textblock 

        }
        public StackPanel CreateElement()
        {
            StackPanel St = new StackPanel();
            string color = AppTheme.GetAnotherColor(AppTheme.Background);
            St.Width = 150;
            St.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(color)));
            TextBlock Tb = new TextBlock();
            Tb.FontSize = 22;
            Tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
            Tb.HorizontalAlignment = HorizontalAlignment.Center;
            Tb.VerticalAlignment = VerticalAlignment.Center;
            Tb.Margin = new Thickness(0,25,0,0);
            Tb.Text = this.Name;
            St.AddHandler(Control.MouseLeftButtonDownEvent, new MouseButtonEventHandler(click), true);
            St.Children.Add(Tb);
            return St;
        }
       private void click(object sender, MouseEventArgs e)
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
            process.StandardInput.WriteLine(@"powercfg /S "+this.Id);
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
        }
    }
       
}
