using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Desktop_Manger
{
    public abstract class DMShortcutItem : Canvas
    {
        public readonly string SupportedIconExtensions = "Supported Extensions|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.Ico;*.exe|Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|Icon (*.ico)|*.Ico|Excutable file (*.exe) |*.exe|BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff";

        public bool IsThereisErrors = false;
        public Canvas ParentCanvas { get; set; }
        public System.Windows.Controls.Image ShortcutIcon { get; set; }
        //TODO: update 1:
        //              1-Try to Change FileName to uneditable Textbox instead of TextBlock
        public TextBlock FileName { get; set; }
        //Testing this with ShortcutItem apply it later to AppInfo
        public TextBox FileName_beta { get; set; }
        public string ShortCutLocation { get; set; }
        public string IconSourceLocation { get; set; }
        public string Parameters = "";

        public DMShortcutItem(string ShortCutLocation, string IconSourceLocation)
        {
            this.ShortCutLocation = ShortCutLocation;
            this.IconSourceLocation = IconSourceLocation;
            LoadDesign();
           
            new Thread(new ThreadStart(() =>
            {
                ImageSource src = GetImageSource();
                //GetImageSource();
                Dispatcher.Invoke(() =>
                {
                    ShortcutIcon.Source = src;

                });
            })).Start();
        }

        public void CreateIconFromImage(string Location)
        {
            System.Windows.Controls.Image img = LayoutObjects.CreateImage();
            img.Source = LayoutObjects.GetImageSource(Location);
            ShortcutIcon = img;
        }
        public void StartProccess()
        {
            try
            {
                if (Path.GetExtension(ShortCutLocation).ToUpper() == ".EXE" || Path.GetExtension(ShortCutLocation).ToUpper() == ".BAT")
                {


                    ProcessStartInfo info = new ProcessStartInfo();
                    //use this to start app as admin if needed
                    info.UseShellExecute = true;
                    info.Arguments = Parameters;
                    info.FileName = ShortCutLocation;
                    info.WorkingDirectory = Path.GetDirectoryName(ShortCutLocation);
                    Process.Start(info);
                }
                else
                {
                    Process.Start(ShortCutLocation, Parameters);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("File Not Found");
            }
            catch (System.ComponentModel.Win32Exception e)
            {

                MessageBox.Show(e.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
            }

        }
        public void ChangeImage(/*StackPanel mystp,*/ ImageSource imgsrc)
        {
            ShortcutIcon.Source = imgsrc;

        }
        public TextBox CreateTextBox(string text)
        {
            TextBox tb = new TextBox();
            if (text.Length > 31)
            {
                text = text.Substring(0, 31) + "... " + text[text.Length - 4] + text[text.Length - 3] + text[text.Length - 2] + text[text.Length - 1];
            }
            tb.Text = text;
            tb.Focusable = false;
            tb.BorderThickness = new Thickness(0);
            tb.Cursor = Cursors.Hand;
            tb.IsReadOnly = true;
            tb.FontSize = 12;
            tb.Background = System.Windows.Media.Brushes.Transparent;
            tb.Foreground = new SolidColorBrush((System.Windows.Media.Color)
                System.Windows.Media.ColorConverter.ConvertFromString
                (AppTheme.HomePageShortCutFontColor));
            tb.TextAlignment = TextAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(2, 2, 2, 5);
            tb.LostFocus += TextBox_LostFocus;
            return tb;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            LayoutObjects.SealTextBox(sender as TextBox);
        }

        protected abstract ImageSource GetImageSource();
        protected abstract void LoadDesign();
    }
}
