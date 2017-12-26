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
    //       1-add run as admin to Context menue
    public class AppInfo : DMShortcutItem

    {
        private Stopwatch stp = new Stopwatch();
        private static List<Extension> Extensions = null;
        //Constructor

        public AppInfo(string file, string IconSource = "Default") : base(file, IconSource)
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

            //To get the original exe file instead of shortcut
            ShortCutLocation = Path.GetExtension(file).ToLower() == ".lnk" ? LayoutObjects.GetOriginalFileURL(file) : file;
            FileName = LayoutObjects.CreateTextBlock(System.IO.Path.GetFileName(ShortCutLocation));

            //Check if a directory
            try
            {
                if (((FileAttributes)System.IO.File.GetAttributes(file)).HasFlag(FileAttributes.Directory))
                {
                    CreateIconFromImage("pack://application:,,,/Resources/Folder_Icon.png");
                }
                else
                {
                    //check If Icon is new created or loaded 
                    file = IconSource == "Default" ? file : IconSource; //if loaded the Icon will Be change
                    IconSourceLocation = GetExtensionIconUrl(file);

                    if (!Object.ReferenceEquals(IconSourceLocation, null))
                    {
                        IconSourceLocation = IconSourceLocation != "" ? IconSourceLocation : file;
                        CreateIconFromImage(IconSourceLocation != "" ? IconSourceLocation : file);
                    }
                    else
                    {
                        IconSourceLocation = ShortCutLocation;
                        CreateIconFromexe(ShortCutLocation);
                    }
                }

                AddElements();
            }
            catch(FileNotFoundException)
            {
                MessageBox.Show("A file is missing\nCan't find " + file);
                IsThereisErrors = true;
            }
            catch(DirectoryNotFoundException)
            {
                MessageBox.Show("Directory is missing\n Cant find " + file);
                IsThereisErrors = true;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error happened while adding " + file + " \nMay save File corrupted or no permission to access this file\ndeleted shortcut from DM\nerror: " + e.Message);
                IsThereisErrors = true;
            }
           
            
            return;

        }

        private void LoadCustomExtensionsIcons()
        {
            Debug.WriteLine("Loading Extensions ...");
            //TODO: Update 2
            //Load from a file
            Extensions.Add(new Extension(".mp3 .wav", "pack://application:,,,/Resources/Audio_Icon.png"));
            Extensions.Add(new Extension(".gif .png .jpg .jpeg", ""));
            Extensions.Add(new Extension(".mp4 .avi .mkv", "pack://application:,,,/Resources/Video_Icon.png"));
            Extensions.Add(new Extension(".doc .dot .docx .docm .dotx .dotm .docb", "pack://application:,,,/Resources/Word_Icon.png"));
            Extensions.Add(new Extension(".xls .xlt .xlm .xlsx .xlsm .xltx .xltm", "pack://application:,,,/Resources/Excel_Icon.png"));
            Extensions.Add(new Extension(".ppt .pot .pps .pptx .pptm .potx .potm .ppam .ppsx .ppsm .sldx .sldm", "pack://application:,,,/Resources/PowerPoint_Icon.png"));
            Extensions.Add(new Extension(".txt", "pack://application:,,,/Resources/Txt_File_Icon.png"));
            Extensions.Add(new Extension(".html", "pack://application:,,,/Resources/Html_Icon.png"));
        }

        
        private string GetExtensionIconUrl(string fileExt)
        {
            string extension = System.IO.Path.GetExtension(fileExt);
            if (extension == "") return "pack://application:,,,/Resources/Unknown_Icon.png";

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
            img.Margin = new Thickness(thickness, thickness, thickness,2);
            img.HorizontalAlignment = HorizontalAlignment.Center;
            return img;
        }
        public void CreateIconFromexe(string Location)
        {
            System.Windows.Controls.Image img = LayoutObjects.CreateImage();
            img.Source = LayoutObjects.GetIcon(Location);
            ShortcutIcon = img;
            
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
            //Add Change Icon to ContextMenue
            MenuItem ChangeIcon = new MenuItem();
            ChangeIcon.Header = "Change Icon";
            ChangeIcon.Click += ChangeIcon_Click;
            MenuItem EditParameters = new MenuItem();
            //Add EditParameters to ContextMenue
            EditParameters.Header = "Edit Parameters";
            EditParameters.Click += EditParameters_Click;
            mnu.Items.Add(OpenWith);
            mnu.Items.Add(Rename);
            mnu.Items.Add(ChangeIcon);
            mnu.Items.Add(EditParameters);
            mnu.Items.Add(RemoveItem);
            return mnu;
        }

        private void EditParameters_Click(object sender, RoutedEventArgs e)
        {
            ParametersEdit_Layout();
        }
        public async void ParametersEdit_Layout()
        {
            StackPanel mystp = CreateParameters_StackPanel();
            mystp.Children.Add(CreateParameters_WarningTextBlock());
            TextBox TBox = CreateParameters_TextBox();
            mystp.Children.Add(TBox);
            mystp.Children.Add(CreateParameters_SaveButton(TBox));
            mystp.Children.Add(CreateParameters_CloseButton(mystp));
            Canvas.SetLeft(mystp, ParentCanvas.Width - 800);
            ParentCanvas.Children.Add(mystp);
            for (int i = 0; i < 100; i++)
            {
                mystp.Width += 8;
                mystp.Height += 0.2;
              await MainWindow.sleep(1);
            }
            TBox.Focus();
        }
        private StackPanel CreateParameters_StackPanel()
        {
            StackPanel mystp = new StackPanel();
            mystp.Orientation = Orientation.Horizontal;
            mystp.Background = System.Windows.Media.Brushes.Black;
            mystp.Focusable = true;
            mystp.LostFocus += (sender, e) => CreateParamerers_Removestp(mystp);
            mystp.Width = 0;
            mystp.Height = 0;
            return mystp;
        }
        private TextBox CreateParameters_TextBox()
        {
            TextBox tb = new TextBox();
            tb.Width = 400;
            tb.Text = Parameters;
            return tb;
        }
        private TextBlock CreateParameters_WarningTextBlock()
        {
            TextBlock tb = new TextBlock();
            tb.Width = 350;
            tb.Text = "Warning Don't Change this unless u know what u are doing ";
            tb.Foreground = System.Windows.Media.Brushes.Red;
            return tb;
        }
        private TextBlock CreateParameters_SaveButton(TextBox tb)
        {
            TextBlock TBlock = new TextBlock();
            TBlock.FontFamily = new System.Windows.Media.FontFamily("Segoe MDL2 Assets");
            TBlock.Text = "\xE001";
            TBlock.Margin = new Thickness(2);
            TBlock.FontSize = 16;
            TBlock.Width = 25;
            TBlock.Cursor = Cursors.Hand;
            TBlock.MouseLeftButtonUp += (sender, e) => CreateParameters_SaveButton_LeftButtonUp(TBlock, tb);
            TBlock.Foreground = System.Windows.Media.Brushes.Yellow;
            return TBlock;
        }
        private TextBlock CreateParameters_CloseButton(StackPanel stb)
        {
            TextBlock TBlock = new TextBlock();
            TBlock.FontFamily = new System.Windows.Media.FontFamily("Segoe MDL2 Assets");
            TBlock.Text = "\xE10A";
            TBlock.Margin = new Thickness(2);
            TBlock.MouseLeftButtonUp += (sender, e) => CreateParamerers_Removestp(stb);
            TBlock.FontSize = 16;
            TBlock.Width = 25;
            TBlock.Cursor = Cursors.Hand;
            TBlock.Foreground = System.Windows.Media.Brushes.Red;
            return TBlock;
        }
        private async void CreateParamerers_Removestp(StackPanel stp)
        {
			//defined this so data is saved before closing the stp
            await MainWindow.sleep(100);
            ParentCanvas.Children.Remove(stp);
        }
        private void CreateParameters_SaveButton_LeftButtonUp(object sender, TextBox mytb)
        {
            Parameters = mytb.Text;
            Data.SaveIcons(HomePage.AppsList);
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
            dlg.Filter = SupportedIconExtensions;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                IconSourceLocation = dlg.FileName;
                if (Path.GetExtension(dlg.FileName).ToUpper() == ".ICO" || Path.GetExtension(dlg.FileName).ToUpper() == ".EXE")
                {
                    ChangeImage(LayoutObjects.GetIcon(dlg.FileName));
                }
                else
                {
                    ChangeImage( LayoutObjects.GetImageSource(dlg.FileName));
                }
                
            }
            Data.SaveIcons(HomePage.AppsList);
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
            TextBox mytb = CreateRenameTextBox();
            FileName.Visibility = Visibility.Collapsed;           
            mystp.Children.Add(mytb);
            mytb.Focus();
        }
        //change the class of this later
        private TextBox CreateRenameTextBox()
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
            Data.SaveIcons(HomePage.AppsList);
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
            //Remove it from the List as well as from the parent Canvas
            HomePage.AppsList.Remove((AppInfo)mycanvas);
            ParentCanvas.Children.Remove(mycanvas);
            Data.SaveIcons(HomePage.AppsList);
        }

        private void Stp_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Panel).Background = System.Windows.Media.Brushes.Transparent; 
        }

        private void Stp_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Panel).Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(AppTheme.HomePageShortCutsHover));
        }

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
            
            (sender as Canvas).ReleaseMouseCapture();
            stp.Stop();
            
            if (stp.Elapsed < new TimeSpan(0, 0, 0, 0, 150))
            {
                StartProccess();
            }
            else
            {
                autoCorrectLocation(sender);
                Data.SaveIcons(HomePage.AppsList);
            }
            stp.Reset();
            
        }

        private void Cnv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            stp.Start();
            (sender as Canvas).CaptureMouse();
        }

        //Auto Correct Location of Canvas
        public static void autoCorrectLocation(object sender)
        {
            if ((Canvas.GetTop(sender as Canvas) % HomePageLayout.CanvasHeight) < HomePageLayout.CanvasHeight/2)
            {
                Canvas.SetTop(sender as Canvas, (Canvas.GetTop(sender as Canvas))-(Canvas.GetTop(sender as Canvas) % HomePageLayout.CanvasHeight));
            }
            else
            {
                Canvas.SetTop(sender as Canvas, (Canvas.GetTop(sender as Canvas)) + (HomePageLayout.CanvasHeight - (Canvas.GetTop(sender as Canvas) % HomePageLayout.CanvasHeight)));

            }
            if ((Canvas.GetLeft(sender as Canvas) % HomePageLayout.CanvasWidth) < HomePageLayout.CanvasWidth/2)
            {
                Canvas.SetLeft(sender as Canvas, (Canvas.GetLeft(sender as Canvas)) - (Canvas.GetLeft(sender as Canvas) % HomePageLayout.CanvasWidth));

            }
            else
            {
                Canvas.SetLeft(sender as Canvas, (Canvas.GetLeft(sender as Canvas)) + (HomePageLayout.CanvasWidth - (Canvas.GetLeft(sender as Canvas) % HomePageLayout.CanvasWidth)));

            }

        }

        protected override ImageSource GetImageSource()
        {
            return null;
        }

        protected override void LoadDesign()
        {
           
        }
        //That's All Folks
    }
}
