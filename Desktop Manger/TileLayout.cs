using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Desktop_Manger
{
    class TileLayout : Canvas
    {
        int ColNo = 0;
        double ChildMinWidth = 300;
        double ChildWidth = 300;
        double Childheight = 130;
        double MarginLeft = 5;
        double MarginTop = 5;
        //Constructor
        public TileLayout()
        {
            this.SizeChanged += TileLayout_SizeChanged;
        }
 
        private void TileLayout_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ColNoChanged())
            {
                //TODO
            }
            if (ColNo != 0)
            {
                ChildWidth = ActualWidth / ColNo;
            }
            ColNo = FindColNo();
            ChangeChildWidth();

        }
        private void ChangeChildWidth()
        {
            foreach(FrameworkElement child in Children)
            {
                child.Width = ChildWidth - MarginLeft;
            }
        }
        private int FindColNo()
        {
            return (int)ActualWidth / (int)ChildMinWidth;
        }
        private bool ColNoChanged()
        {
            if (ColNo != FindColNo())
            {
                return true;
            }
            else return false;
        }
        public void Add(FrameworkElement Child)
        {
            Location location = GetLocation();
            Child.Width = ChildWidth - MarginLeft;
            Child.Height = Childheight - MarginTop;
            Canvas.SetLeft(Child, location.Left);
            Canvas.SetTop(Child, location.Top);
            this.Children.Add(Child);
        }
        private Location GetLocation()
        {
            Location location = new Location();
            if (Children.Count > 0)
            {
                UIElement element = Children[Children.Count - 1];
                location.Left = Canvas.GetLeft(element) + ChildWidth;
                location.Top = Canvas.GetTop(element);
                if (location.Left >= ActualWidth )
                {
                    location.Left = MarginLeft;
                    location.Top += Childheight;
                }
            }
            else
            {
                location.Left = MarginLeft;
                location.Top = MarginTop;
            }
            return location;
        }
    }
    class Location
    {
        public double Left { get; set; }
        public double Top { get; set; }
    }
}
