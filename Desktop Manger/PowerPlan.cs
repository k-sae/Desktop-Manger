using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Desktop_Manger
{
    class PowerPlan
    {
        public string Name { get; set; }
        public string Id { get; set; }
        //contstactor 
        //see divideplanes funtion to see how to use it
        public PowerPlan(object Parent, string Id, string Name, int Row, int Column )
        {
            this.Id = Id;
            this.Name = Name;
            StackPanel stp = CreateStackPanel();
          (Parent as StackPanel).Children.Add(stp);

            //Here create stackpanel 
            // create the textblock 

        }
        private StackPanel CreateStackPanel()
        {
            //create stp
            StackPanel stp = new StackPanel();
            stp.Height = 60;
            stp.Width = 120;
            stp.Margin = new Thickness(5,10, 5, 0);
            stp.VerticalAlignment = VerticalAlignment.Top;
            stp.MouseEnter += Stp_MouseEnter;
            stp.Background = Brushes.Red;
            stp.MouseLeftButtonUp += Stp_MouseLeftButtonUp;
            return stp;
        }

        private void Stp_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            MessageBox.Show(this.Name);
        }

        private void Stp_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999"));
        }
    }
}
