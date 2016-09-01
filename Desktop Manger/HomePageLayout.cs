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
        public static Canvas ParentCanvas { get; set; }
        public static int CanvasHeight = 130;
        public static int CanvasWidth = 100;
        public  void onStart( Canvas ParentCanvas)
        { 
        }
        //Replay the video After it ends
        private static void BackGroundPlayer_MediaEnded(object sender, RoutedEventArgs e) 
        {
            ((MediaElement)sender).Stop();
            ((MediaElement)sender).Play();
        }
        //set Video As BackGround
        private static void SetVideoAsBackground(string Location)
        {
            Uri vLocation = new Uri(Location,UriKind.RelativeOrAbsolute);
            MediaElement player = new MediaElement();
            player.Source = vLocation;
            player.MediaEnded += BackGroundPlayer_MediaEnded;
            player.Width = ParentCanvas.Width;
            player.Height = ParentCanvas.Height;
            player.LoadedBehavior = MediaState.Manual;
            ParentCanvas.Children.Add(player);
            player.Play();
        }
        //TODO
        //use try and catch to check the validation of url
        private static void SetImageAsBackground(string location)
        {
            Image img = new Image();
            img.Source = LayoutObjects.GetImageSource(location);
            img.Width = ParentCanvas.Width;
            img.Height = ParentCanvas.Height;
            img.Stretch = Stretch.Fill;
            ParentCanvas.Children.Add(img);

        }
        public static void SetBackground()
        {
            if (IsVideo(System.IO.Path.GetExtension(AppTheme.HomePageBackground)))
            {
                SetVideoAsBackground(AppTheme.HomePageBackground);
            }
            else if(IsImage(System.IO.Path.GetExtension(AppTheme.HomePageBackground)))
            {
                SetImageAsBackground(AppTheme.HomePageBackground);
            }
            else
            {
                SetImageAsBackground(AppTheme.HomePageBackground);
                //set default for unknow extenstions
            }
        }
        private static bool IsVideo(string extension)
        {
            string videoextenstions = ".mp4 .avi";
            if (videoextenstions.ToUpper().Contains(extension.ToUpper()))
            {
                return true;
            }
            else return false;
        }
        private static bool IsImage(string extension)
        {
            string ImageExtenstions = "*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|Video (*.Mp4, avi)|*.Mp4;*.avi";
            if(ImageExtenstions.ToUpper().Contains(extension.ToUpper()))
            {
                return true;
            }
            else return false;
        }
    }
}
 