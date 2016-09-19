using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    
    class Tile : Canvas
    {
        int ColNo = 0;
        public double ChildMinWidth = 200;
        double ChildWidth = 300;
        public double Childheight = 100;
        public double MarginLeft = 5;
        public  double MarginTop = 5;
        public bool AllowAnimation = false;
        public double AnimationSpeed = 4;
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
                if (AllowAnimation)
                {
                    while (Animator.Worker.IsBusy)
                    {
                        //Dont procced if another worker is in progress
                        await sleep(50);
                    }
                }
               
                Location location = GetLocation(i);
                ChangeChildLocation(Children[i + 1] as FrameworkElement, location);
            }
        }
        private void ChangeChildLocation(FrameworkElement Child, Location location)
        {
            if (AllowAnimation)
            {
                Animator animator = new Animator();
                animator.AnimationSpeed = AnimationSpeed;
                animator.Animate(Child, location.Left, location.Top);
            }
            else
            {
                Canvas.SetLeft(Child, location.Left);
                Canvas.SetTop(Child, location.Top);
            }
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
        public void Remove(FrameworkElement Child)
        {
            int index = Children.IndexOf(Child);
            Children.Remove(Child);
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
    class Animator
    {
        public double AnimationSpeed = 4;
        public static BackgroundWorker Worker = new BackgroundWorker();
        public void Animate(FrameworkElement element, double NewLeft, double NewTop)
        {
            //using pythagoras theorem to find the new itemlocation
            //iam using the Equation ( ((l1/(l1+l2)) + (l2/(l1+l2))) * root(l1^2 + l2^2) ) for Animation along x-axis and y-axis
            double l1 = NewLeft - Canvas.GetLeft(element);
            double l2 = NewTop - Canvas.GetTop(element);
            double LoopEnd = ((l1 * l1) + (l2 * l2));
            LoopEnd = Math.Sqrt(LoopEnd);
            //set the z-index so the element stay on top along the animation
            Panel.SetZIndex(element, 100);
            BackgroundWorker worker = new BackgroundWorker();
            Worker = worker;
            worker.WorkerReportsProgress = true;
            //Background loop is here 
            worker.DoWork += (sender, e) => AnimateElement(worker, LoopEnd);
            //Background thread to Change the location by l1 and l2
            worker.ProgressChanged += (sender, e) => Progress(element, l1, l2);
            //finaly after the worker ends
            worker.RunWorkerCompleted += (sender, e) => SetTheExactlyLocation(element, NewLeft, NewTop);
            worker.RunWorkerAsync();
        }
        public void AnimateElement(object sender, double LoopEnd)
        {

            for (int i = 0; i < (LoopEnd) / AnimationSpeed; i++)
            {
                Thread.Sleep(1);
                // change the progress in order to invoke the progress function 
                (sender as BackgroundWorker).ReportProgress(i);

            }
        }
        public void Progress(FrameworkElement element, double l1, double l2)
        {
            Canvas.SetLeft(element, Canvas.GetLeft(element) + (l1 / (Math.Abs(l1) + Math.Abs(l2)) * AnimationSpeed));
            Canvas.SetTop(element, Canvas.GetTop(element) + (l2 / (Math.Abs(l1) + Math.Abs(l2))) * AnimationSpeed);
        }
        public void SetTheExactlyLocation(FrameworkElement element, double NewLeft, double NewTop)
        {

            Panel.SetZIndex(element, 0);
            Canvas.SetLeft(element, NewLeft);
            Canvas.SetTop(element, NewTop);
        }

    }
    class Location
    {
        public double Left { get; set; }
        public double Top { get; set; }
    }
}
