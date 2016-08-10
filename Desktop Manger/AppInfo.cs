using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;
namespace Desktop_Manger
{
    class AppInfo
    {
        public Canvas ParentCanvas { get; set; }
        public Canvas HolderCanvas { get; set; }
        public  System.Windows.Controls.Image ShortcutIcon { get; set; }
        public TextBlock Text { get; set; }
        public string ShortCutLocation { get; set; }
        public static ImageSource GetIcon(string fileName)
        {
            Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle,
                        new Int32Rect(0, 0, icon.Width, icon.Height),
                        BitmapSizeOptions.FromEmptyOptions());
        }
        private System.Windows.Controls.Image CreateImage()
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Width = 50;
            img.Height = 50;
            img.Margin = new Thickness(0, 0, 0, 10);
            img.HorizontalAlignment = HorizontalAlignment.Center;
            return img;
        }
        public void CreateIconFromexe(string Location)
        {
            System.Windows.Controls.Image img = CreateImage();
            img.Source = GetIcon(Location);
            ShortcutIcon = img;
            
        }
        public void CreateIconFromImage(string Location)
        {
            System.Windows.Controls.Image img = CreateImage();
            /* BitmapImage bimage = new BitmapImage();
             bimage.BeginInit();
             bimage.UriSource = new Uri(Location, UriKind.Relative);
             bimage.EndInit();*/
            var converter = new ImageSourceConverter();

            img.Source = (ImageSource)converter.ConvertFromString(Location); ;
            ShortcutIcon = img;
        }
        public void CreateTextBlock(string text)
        {
            TextBlock tb = new TextBlock();
            if (text.Length > 34)
            {
                text = text.Substring(0, 34) + "...";
            }
            tb.Text = text;
            tb.FontSize = 12;
            tb.Background = System.Windows.Media.Brushes.Transparent;
            tb.Foreground = new SolidColorBrush((System.Windows.Media.Color)
                System.Windows.Media.ColorConverter.ConvertFromString
                (AppTheme.HomePageShortCutFontColor));
            tb.TextAlignment = TextAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
            Text = tb;

        }
        public void AddElements()
        {
            StackPanel stp = CreateStackPanel();
            
           // stp.Height = 100;
            stp.Children.Add(ShortcutIcon);
            stp.Children.Add(Text);
            HolderCanvas.Children.Add(stp);
           
            //ParentCanvas.Children.Add(HolderCanvas);
        }

        private StackPanel CreateStackPanel()
        {
            StackPanel stp = new StackPanel();
            stp.Orientation = Orientation.Vertical;
            stp.Width = 70;
            stp.MouseEnter += Stp_MouseEnter;
            stp.MouseLeave += Stp_MouseLeave;
            stp.ContextMenu = CreateContextMenu();
            return stp;
        }
        private ContextMenu CreateContextMenu()
        {
            ContextMenu mnu = new ContextMenu();
            MenuItem RemoveItem = new MenuItem();
            RemoveItem.Header = "Remove";
            mnu.Items.Add(RemoveItem);
            RemoveItem.Click += RemoveItem_Click;
            return mnu;
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            StackPanel mystp = null;
            if (mnu != null)
            {
                ContextMenu MyContextMenu = (ContextMenu)mnu.Parent;
                mystp = MyContextMenu.PlacementTarget as StackPanel;
            }
            Canvas mycanvas = (Canvas)mystp.Parent;
            ParentCanvas.Children.Remove(mycanvas);
        }

        private void Stp_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as StackPanel).Background = System.Windows.Media.Brushes.Transparent; 
        }

        private void Stp_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(AppTheme.HomePageShortCutsHover));
        }

        public Canvas CreateCanvas(double height, double width, double left = 0, double top = 0)
        {
            Canvas cnv = new Canvas();
            cnv.Width = 70;
            cnv.Height = 100;
            cnv.Background = System.Windows.Media.Brushes.Transparent;
            if (left == 0 && top == 0)
            {
                Canvas.SetTop(cnv, height / 2 - cnv.Height / 2);
                Canvas.SetLeft(cnv, width / 2 - cnv.Width / 2);
            }
            else
            {
                Canvas.SetTop(cnv, top);
                Canvas.SetLeft(cnv, left);
            }
            cnv.MouseLeftButtonDown += Cnv_MouseLeftButtonDown;
            cnv.MouseMove += Cnv_MouseMove;
            cnv.MouseLeftButtonUp += Cnv_MouseLeftButtonUp;    
            cnv.Cursor = Cursors.Hand;
            //  cnv.MouseLeave += Cnv_MouseLeave;
            ParentCanvas.Children.Add(cnv);
            return cnv;
        }

        Stopwatch stp = new Stopwatch();
        private void Cnv_MouseMove(object sender, MouseEventArgs e)
        {

            if ((sender as Canvas).IsMouseCaptured && stp.Elapsed > new TimeSpan(0, 0, 0, 0, 50))
            {
                Canvas.SetTop((sender as Canvas), e.GetPosition(ParentCanvas).Y - (sender as Canvas).Height / 2);
                Canvas.SetLeft((sender as Canvas), e.GetPosition(ParentCanvas).X - (sender as Canvas).Width / 2);
            }
        }

        private void Cnv_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            autoCorrectLocation(sender);
            (sender as Canvas).ReleaseMouseCapture();
            stp.Stop();
            
            if (stp.Elapsed < new TimeSpan(0, 0, 0, 0, 100))
            {
                StartProccess();
            }
            stp.Reset();
        }

        private void Cnv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            stp.Start();
            (sender as Canvas).CaptureMouse();
        }

        //Auto Correct Location of Canvas
        private void autoCorrectLocation(object sender)
        {
            if ((Canvas.GetTop(sender as Canvas) % 160) < 80)
            {
                Canvas.SetTop(sender as Canvas, (Canvas.GetTop(sender as Canvas))-(Canvas.GetTop(sender as Canvas) % 160));
            }
            else
            {
                Canvas.SetTop(sender as Canvas, (Canvas.GetTop(sender as Canvas)) + (160 - (Canvas.GetTop(sender as Canvas) % 160)));

            }
            if ((Canvas.GetLeft(sender as Canvas) % 100) < 50)
            {
                Canvas.SetLeft(sender as Canvas, (Canvas.GetLeft(sender as Canvas)) - (Canvas.GetLeft(sender as Canvas) % 100));

            }
            else
            {
                Canvas.SetLeft(sender as Canvas, (Canvas.GetLeft(sender as Canvas)) + (100 - (Canvas.GetLeft(sender as Canvas) % 100)));

            }

        }

        private void StartProccess()
        {
            try
            {
                Process.Start(ShortCutLocation);
            }
            catch(System.IO.FileNotFoundException)
            {
                MessageBox.Show("File Not Found");
            }
            catch(Exception ex)
            {
                MessageBox.Show("error " + ex.ToString());
            }
           
        }
    }
}
