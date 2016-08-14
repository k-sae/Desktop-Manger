using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

    }
}
