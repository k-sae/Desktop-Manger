using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Desktop_Manger
{
    class LayoutObjects
    {
        public static ImageSource GetIcon(string fileName)
        {
            Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle,
                        new Int32Rect(0, 0, icon.Width, icon.Height),
                        BitmapSizeOptions.FromEmptyOptions());
        }
        public static System.Windows.Controls.Image CreateImage()
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Width = 50;
            img.Height = 50;
            img.Margin = new Thickness(0, 0, 0, 10);
            img.HorizontalAlignment = HorizontalAlignment.Center;
            return img;
        }
        public static ImageSource GetImageSource(string Location)
        {
            var converter = new ImageSourceConverter();

            return (ImageSource)converter.ConvertFromString(Location); ;
        }
        //get the original exe file Location instead of the shortcut
        public static string GetOriginalFileURL(string Location)
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
        //Create TextBlock
        public static TextBlock CreateTextBlock(string text)
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
            tb.Margin = new Thickness(2, 2, 2, 5);
            return tb;

        }
        public static Ellipse CreateEllipse()
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Height = 20;
            ellipse.Width = 20;
            ellipse.Fill = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#70ffffff"));
            Canvas.SetTop(ellipse, ellipse.Height * -1);
            Panel.SetZIndex(ellipse, 10);
            return ellipse;
        }
        public static void UnSealTextBox(TextBox textbox)
        {
            textbox.Cursor = Cursors.IBeam;
            textbox.Background = System.Windows.Media.Brushes.White;
            textbox.BorderThickness = new Thickness(1);
            textbox.IsReadOnly = false;
            textbox.Focusable = true;
            textbox.Focus();
        }
        public static void SealTextBox(TextBox textbox)
        {
            textbox.Background = System.Windows.Media.Brushes.Transparent;
            textbox.BorderThickness = new Thickness(0);
            textbox.Cursor = Cursors.Hand;
            textbox.IsReadOnly = true;
            textbox.Focusable = false;
        }
    }
}
