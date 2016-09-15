using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
namespace TileLayout
{
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
            worker.DoWork += (sender, e) => AnimateElement(worker,LoopEnd);
            //Background thread to Change the location by l1 and l2
            worker.ProgressChanged += (sender, e) => Progress(element,l1, l2);
            //finaly after the worker ends
            worker.RunWorkerCompleted += (sender, e) => SetTheExactlyLocation(element, NewLeft, NewTop);
            worker.RunWorkerAsync();
        }
        public void AnimateElement(object sender,double LoopEnd)
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
}
