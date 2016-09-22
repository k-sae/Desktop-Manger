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
        public PowerPlan(object Parent, string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;

            (Parent as StackPanel).Children.Add(Createtitle());

            //Here create stackpanel 
            // create the textblock 

        }
        private TileLayout.Tile Createtitle()
        {

            TileLayout.Tile ti = new TileLayout.Tile();
            ti.ChildMinWidth = 100;
            ti.Height = 50;
            ti.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(AppTheme.Background)));
            ti.Margin = new Thickness(5);
            ti.AllowAnimation = true;
            return ti;

        }

    }
}
