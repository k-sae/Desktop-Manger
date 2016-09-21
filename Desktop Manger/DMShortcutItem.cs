using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Desktop_Manger
{
    public class DMShortcutItem : Canvas
    {
        public bool IsThereisErrors = false;
        public Canvas ParentCanvas { get; set; }
        public System.Windows.Controls.Image ShortcutIcon { get; set; }
        //TODO update 1:
        //              1-Try to Change FileName to uneditable Textbox instead of TextBlock
        public TextBlock FileName { get; set; }
        public string ShortCutLocation { get; set; }
        public string IconSourceLocation { get; set; }
        public string Parameters = "";
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
                    info.UseShellExecute = false;
                    info.Arguments = Parameters;
                    info.FileName = ShortCutLocation;
                    info.WorkingDirectory = Path.GetDirectoryName(ShortCutLocation);
                    Process.Start(info);
                }
                else
                {
                    Process.Start(ShortCutLocation);
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
            //TODO: update 2:
            //              1-Remove this unnessarcy code seems useless
            /*
            object img = new object();
            foreach (object child in mystp.Children)
            {
                
               if (child is System.Windows.Controls.Image)
                {
                    img = child;
                    break;
                }
            }
            (img as System.Windows.Controls.Image).Source = imgsrc;*/
            ShortcutIcon.Source = imgsrc;

        }
    }
}
