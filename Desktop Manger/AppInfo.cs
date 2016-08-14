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
using System.IO;
using IWshRuntimeLibrary;

namespace Desktop_Manger
{

    // to do: 
    //The Create Functions should be added to serprate Class File
    class AppInfo : Canvas
    {
        public Canvas ParentCanvas { get; set; }
        public  System.Windows.Controls.Image ShortcutIcon { get; set; }
        public TextBlock FileName { get; set; }
        public string ShortCutLocation { get; set; }

        private static List<Extension> Extensions = null;

        //Constructor
        public AppInfo()
        {
            if (Object.ReferenceEquals(Extensions, null))
            {
                Extensions = new List<Extension>();
                LoadCustomExtensionsIcons();
            }
            Width = 70;
            Height = 100;
            Background = System.Windows.Media.Brushes.Transparent;
            MouseLeftButtonDown += Cnv_MouseLeftButtonDown;
            MouseMove += Cnv_MouseMove;
            MouseLeftButtonUp += Cnv_MouseLeftButtonUp;
            Cursor = Cursors.Hand;
        }

        public AppInfo(string file) : this()
        {
            //To get the original exe file instead of shortcut
            ShortCutLocation = Path.GetExtension(file).ToLower() == ".lnk" ? GetOriginalFileURL(file) : file;
            CreateTextBlock(System.IO.Path.GetFileName(ShortCutLocation));
            //Check if a directory
            if (((FileAttributes)System.IO.File.GetAttributes(file)).HasFlag(FileAttributes.Directory))
            {
                CreateIconFromImage("pack://application:,,,/Resources/Folder_Icon.png");
            }
            else
            {
                string iconUrl = GetExtensionIconUrl(file);
                if (!Object.ReferenceEquals(iconUrl, null))
                {
                    CreateIconFromImage(iconUrl != "" ? iconUrl : file);
                }
                else
                {
                    CreateIconFromexe(ShortCutLocation);
                }
            }
            AddElements();
            return;

        }

        private void LoadCustomExtensionsIcons()
        {
            Debug.WriteLine("Loading Extensions ...");
            //TODO Load from a file
            Extensions.Add(new Extension(".mp3 .wav", "pack://application:,,,/Resources/Audio_Icon.png"));
            Extensions.Add(new Extension(".gif .png .jpg .jpeg", ""));
            Extensions.Add(new Extension(".mp4 .avi .mkv", "pack://application:,,,/Resources/Video_Icon.png"));
            Extensions.Add(new Extension(".doc .dot .docx .docm .dotx .dotm .docb", "pack://application:,,,/Resources/Word_Icon.png"));
            Extensions.Add(new Extension(".xls .xlt .xlm .xlsx .xlsm .xltx .xltm", "pack://application:,,,/Resources/Excel_Icon.png"));
            Extensions.Add(new Extension(".ppt .pot .pps .pptx .pptm .potx .potm .ppam .ppsx .ppsm .sldx .sldm", "pack://application:,,,/Resources/PowerPoint_Icon.png"));
            Extensions.Add(new Extension(".txt", "pack://application:,,,/Resources/Txt_File_Icon.png"));
            Extensions.Add(new Extension(".html", "pack://application:,,,/Resources/Html_Icon.png"));
        }

        //get the original exe file Location instead of the shortcut
        private string GetOriginalFileURL(string Location)
        {
            if (System.IO.File.Exists(Location))
            {
                // WshShellClass shell = new WshShellClass();
                WshShell shell = new WshShell(); 
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(Location);

                return link.TargetPath;
            }
            else return "";
        }
        private string GetExtensionIconUrl(string fileExt)
        {
            string extension = System.IO.Path.GetExtension(fileExt);
            foreach (Extension ex in Extensions)
            {
                if ((ex.Extensions.ToLower()).Contains(extension.ToLower()))
                {
                    return ex.URL;
                }
            }
            return null;
        }

        public static ImageSource GetIcon(string fileName)
        {
            Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle,
                        new Int32Rect(0, 0, icon.Width, icon.Height),
                        BitmapSizeOptions.FromEmptyOptions());
        }
        //create image with custom thickness to enable resizing later
        private System.Windows.Controls.Image CreateImage(int thickness = 10)
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Margin = new Thickness(thickness, thickness, thickness,10);
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
            img.Source = GetImageSource(Location);
            ShortcutIcon = img;
        }
        public ImageSource GetImageSource(string Location)
        {
            var converter = new ImageSourceConverter();

            return (ImageSource)converter.ConvertFromString(Location); ;
        }
        public void CreateTextBlock(string text)
        {
           
            TextBlock tb = new TextBlock();
            if (text.Length > 31)
            {
                text = text.Substring(0, 31) + "... " + text[text.Length - 4] + text[text.Length - 3] + text[text.Length - 2] + text[text.Length - 1];
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
            FileName = tb;

        }
        public void AddElements()
        {
            StackPanel stp = CreateStackPanel();
            stp.Children.Add(ShortcutIcon);
            stp.Children.Add(FileName);
            Children.Add(stp);
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
            //add Remove To ContextMenu
            MenuItem RemoveItem = new MenuItem();
            RemoveItem.Header = "Remove";
            RemoveItem.Click += RemoveItem_Click;
            //Add Open with to ContextMenu
            MenuItem OpenWith = new MenuItem();
            OpenWith.Header = "Open With";
            OpenWith.Click += OpenWith_Click;
            //add Rename to ContextMenu
            MenuItem Rename = new MenuItem();
            Rename.Header = "Rename";
            Rename.Click += Rename_Click;
            MenuItem ChangeIcon = new MenuItem();
            ChangeIcon.Header = "Change Icon";
            ChangeIcon.Click += ChangeIcon_Click;
            mnu.Items.Add(OpenWith);
            mnu.Items.Add(Rename);
            mnu.Items.Add(ChangeIcon);
            mnu.Items.Add(RemoveItem);
            return mnu;
        }

        private void ChangeIcon_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            StackPanel mystp = null;
            if (mnu != null)
            {
                ContextMenu MyContextMenu = (ContextMenu)mnu.Parent;
                mystp = MyContextMenu.PlacementTarget as StackPanel;
            }

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Image Files (*.jp*, *.png,...)|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|Icon (*.ico)|*.Ico|Excutable file (*.exe) |*.exe|BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                if (Path.GetExtension(dlg.FileName).ToUpper() == ".ICO" || Path.GetExtension(dlg.FileName).ToUpper() == ".EXE")
                {
                    ChangeImage(mystp, GetIcon(dlg.FileName));
                }
                else
                {
                    ChangeImage(mystp, GetImageSource(dlg.FileName));
                }
                
            }
        }
        private void ChangeImage(StackPanel mystp, ImageSource imgsrc)
        {

            object img = new object();
            foreach (object child in mystp.Children)
            {
                
               if (child is System.Windows.Controls.Image)
                {
                    img = child;
                    break;
                }
            }
            (img as System.Windows.Controls.Image).Source = imgsrc;
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            StackPanel mystp = null;
            if (mnu != null)
            {
                ContextMenu MyContextMenu = (ContextMenu)mnu.Parent;
                mystp = MyContextMenu.PlacementTarget as StackPanel;
            }
            TextBox mytb = CreateTextBox();
            FileName.Visibility = Visibility.Collapsed;
            mytb.Focus();
            mystp.Children.Add(mytb);
        }

        private TextBox CreateTextBox()
        {
            TextBox tb = new TextBox();
            tb.Text = FileName.Text;
            tb.Focusable = true;
            tb.LostFocus += Tb_LostFocus;
            tb.TextWrapping = TextWrapping.Wrap;
            return tb;
        }

        private void Tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text.Length > 34)
            {
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 34) + "...";
            }
            FileName.Text = (sender as TextBox).Text;

            FileName.Visibility = Visibility.Visible;
            StackPanel mystp = null;
            mystp = (StackPanel)(sender as TextBox).Parent;
            mystp.Children.Remove(sender as TextBox);
        }

        private void OpenWith_Click(object sender, RoutedEventArgs e) 
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();
            process.StandardInput.WriteLine(@"openwith " + "\""+ ShortCutLocation + "\"");
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            Panel mystp = null;
            if (mnu != null)
            {
                ContextMenu MyContextMenu = (ContextMenu)mnu.Parent;
                 mystp = MyContextMenu.PlacementTarget as Panel;
            }
            Canvas mycanvas = (Canvas)mystp.Parent;
            ParentCanvas.Children.Remove(mycanvas);
        }

        private void Stp_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Panel).Background = System.Windows.Media.Brushes.Transparent; 
        }

        private void Stp_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Panel).Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(AppTheme.HomePageShortCutsHover));
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

        //save the changes made by User to Local drive
    }
}
