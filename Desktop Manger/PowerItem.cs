using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Desktop_Manger
{
    class PowerItem : Grid
    {
        private bool IsActive { get; set; }
        private PowerPlan Plan { get; set; }
        public PowerItem()
        {
            string color = AppTheme.GetAnotherColor(AppTheme.Background);
            Background  = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(color)));
            MouseLeftButtonDown += PowerItem_MouseLeftButtonDown;
            MouseEnter += PowerItem_MouseEnter;
            MouseLeave += PowerItem_MouseLeave;
        }

        private void PowerItem_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //TODO
        }

        private void PowerItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //TODO

        }

        private void PowerItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
            process.StandardInput.WriteLine(@"powercfg /S " + Plan.Id);
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
        }

        public PowerItem(PowerPlan MyPlan, bool IsActive = false) : this()
        {
            this.Plan = MyPlan;
            Children.Add(CreateTextBlock());
            //IsActive == true
            //TODO
            this.IsActive = IsActive;
        }
        private TextBlock CreateTextBlock()
        {
            TextBlock Tb = new TextBlock();
            Tb.FontSize = 22;
            Tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
            Tb.HorizontalAlignment = HorizontalAlignment.Center;
            Tb.VerticalAlignment = VerticalAlignment.Center;
            //Tb.Margin = new Thickness(0, 25, 0, 0);
            Tb.Text = Plan.Name;
            return Tb;
        }
    }
}
