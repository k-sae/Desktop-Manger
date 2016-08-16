using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Desktop_Manger
{
    class HomePageLayout
    {
        //Set the Parent Canvas
        //you should specify this first before using other functions
        public static int CanvasHeight = 130;
        public static int CanvasWidth = 100;
        public Canvas ParentCanvas { get; set; }
        public  void onStart( Canvas ParentCanvas)
        { 
        }
        //Replay the video After it ends
        private  void BackGroundPlayer_MediaEnded(object sender, RoutedEventArgs e) 
        {
            ((MediaElement)sender).Stop();
            ((MediaElement)sender).Play();
        }
        //set Video As BackGround
        public  void SetVideoAsBackground(string Location)
        {
            Uri vLocation = new Uri(Location);
            MediaElement player = new MediaElement();
            player.Source = vLocation;
            player.MediaEnded += BackGroundPlayer_MediaEnded;
            player.Width = ParentCanvas.Width;
            player.Height = ParentCanvas.Height;
            player.LoadedBehavior = MediaState.Manual;
            ParentCanvas.Children.Add(player);
            player.Play();
        }
      
    }
}
 