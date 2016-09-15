using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TileLayout
{
    //TODO
    //1-upgrade the remove fun
    //2- Try the Animation Class
    //3- if Animation Class failed Try to make My Own Animation Class
    class Tile : Canvas
    {
        int ColNo = 0;
        double ChildMinWidth = 200;
        double ChildWidth = 300;
        double Childheight = 100;
        double MarginLeft = 5;
        double MarginTop = 5;
        //Constructor
        public Tile()
        {
            this.SizeChanged += TileLayout_SizeChanged;
        }

        private void TileLayout_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ActualWidth == 0)
            {
                return;
            }

            if (ColNoChanged())
            {
                ChangeChildrenLocation();
            }
            ColNo = FindColNo();
            if (ColNo != 0)
            {
                ChildWidth = ActualWidth / ColNo;

            }
            ChangeChildWidth();
        }


        private async void ChangeChildrenLocation(int index = 0)
        {

            //For some reason i had to put these :\
            await sleep(100);
            for (int i = index; i < Children.Count - 1; i++)
            {
                while (Animator.Worker.IsBusy)
                {
                    //Dont procced if another worker is in progress
                    await sleep(50);
                }
                Location location = GetLocation(i);
                ChangeChildLocation(Children[i + 1] as FrameworkElement, location);
            }
        }
        private void ChangeChildLocation(FrameworkElement Child, Location location)
        {
            Animator animator = new Animator();
            animator.AnimationSpeed = 4;
            animator.Animate(Child, location.Left, location.Top);
        }

        public static Task sleep(int time)
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(time);
            });
        }
        private void ChangeChildWidth()
        {
            int i = 0;
            foreach (FrameworkElement child in Children)
            {
                if (i != 0)
                {
                    Canvas.SetLeft(child, Canvas.GetLeft(child) + (((ChildWidth - MarginLeft) - child.Width) * i));
                }
                if (ColNo != 0 && (i + 1) % ColNo == 0)
                {
                    i = -1;
                }
                child.Width = ChildWidth - MarginLeft;
                i++;
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

            Location location = GetLocation(Children.Count - 1);
            Child.Width = ChildWidth - MarginLeft;
            Child.Height = Childheight - MarginTop;
            Canvas.SetLeft(Child, location.Left);
            Canvas.SetTop(Child, location.Top);
            this.Children.Add(Child);



        }
        public void Remove(int index)
        {
            Children.RemoveAt(index);
            ChangeChildrenLocation(index - 1);
        }
        private Location GetLocation(int Index)
        {
            Location location = new Location();
            if (Index >= 0 && FindColNo() > 0)
            {
                UIElement element = Children[Index];
                location.Left = Canvas.GetLeft(element) + ChildWidth;
                location.Top = Canvas.GetTop(element);
                if ((Index + 1) % FindColNo() == 0)
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
