using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Desktop_Manger
{
    class PowerTimer : StackPanel
    {
        public TextBlock Timer = null;
        public PowerTimer()
        {
            Orientation = Orientation.Horizontal;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#60000000"));
            TextBlock Timer = new TextBlock();
            Timer.FontSize = 40;
            this.Timer = Timer;
            Border br = new Border();
            br.BorderThickness = new System.Windows.Thickness(1, 0, 0, 0);
            br.BorderBrush = Brushes.LightGray;
            br.Margin = new System.Windows.Thickness(10, 0, 10, 0);
            Timer.Foreground = Brushes.White;
            TextBlock CloseButton = Createtb("\xE10A");
            CloseButton.Foreground = Brushes.Red;
            TextBlock MinimizeButton = Createtb("\xE108");
            MinimizeButton.Foreground = Brushes.Yellow;
            Children.Add(Timer);
            Children.Add(br);
            Children.Add(MinimizeButton);
            Children.Add(CloseButton); 
        }
        private TextBlock Createtb(string Text)
        {
            TextBlock tb = new TextBlock();
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            tb.FontFamily = new FontFamily("Segoe MDL2 Assets");
            tb.FontSize = 20;
            tb.Text = Text;
            tb.Margin = new System.Windows.Thickness(5, 0, 5, 0);
            return tb;
        }
    }
}
