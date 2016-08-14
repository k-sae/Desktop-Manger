﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using IWshRuntimeLibrary;
namespace Desktop_Manger
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    /// To Do:
    ///         I Have to change the string array for file extension to normal string with string.contain
    public partial class HomePage : Page
    {
        double PageHeight, PageWidth;
        public static List<AppInfo> AppsList = new List<AppInfo>();
        //int AppsListCount = 0;
        public HomePage(double height, double width)
        {
            
            InitializeComponent();
            PageHeight = height;
            PageWidth = width;
            canv1.Height = height;
            canv1.Width = width;
            HomePageLayout layout = new HomePageLayout();
            layout.ParentCanvas = canv1;
            layout.onStart(canv1);
           // layout.SetVideoAsBackground(@"D:\Videos\bf4.mp4");

        }

        private void canv1_Drop(object sender, DragEventArgs e)
        {
            HomePageLayout layout = new HomePageLayout();
            string audiofile = ".MP3 .wav";
            string ImageFile = ".gif .png .jpg .jpeg" ;
            string VideoFile = ".mp4 .avi .mkv";
            string WordFile = ".doc .dot .docx .docm .dotx .dotm .docb";
            string ExcelFile = ".xls .xlt .xlm .xlsx .xlsm .xltx .xltm";
            string PowerPointFile = ".ppt .pot .pps .pptx .pptm .potx .potm .ppam .ppsx .ppsm .sldx .sldm";
            string[] Files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string file in Files)
            {
                AppInfo app = new AppInfo();
                app.ParentCanvas = canv1;
                app.HolderCanvas = app.CreateCanvas(PageHeight, PageWidth);
                app.ShortCutLocation = file;
                app.CreateTextBlock(System.IO.Path.GetFileName(file));
                if (System.IO.Path.GetExtension(file).ToUpper() ==".LNK" )
                {
                    app.ShortCutLocation = GetOriginalFileURL(file);
                    app.CreateIconFromexe(app.ShortCutLocation);
                    app.CreateTextBlock(System.IO.Path.GetFileName(app.ShortCutLocation));
                }
                else if (System.IO.Path.GetExtension(file).ToUpper() == ".EXE")
                {
                    app.CreateIconFromexe(file);
                }
                else if (checkExtension(ImageFile,System.IO.Path.GetExtension(file)))
                {
                    app.CreateIconFromImage(file);
                }
                else if (checkExtension(audiofile, System.IO.Path.GetExtension(file)))
                {
                    app.CreateIconFromImage("pack://application:,,,/Resources/Audio_Icon.png");
                }
                else if (checkExtension(VideoFile, System.IO.Path.GetExtension(file)))
                {
                    app.CreateIconFromImage("pack://application:,,,/Resources/Video_Icon.png");
                }
                else if (System.IO.Path.GetExtension(file).ToUpper() == ".TXT")
                {
                    app.CreateIconFromImage("pack://application:,,,/Resources/Txt_File_Icon.png");
                }
                else if (checkExtension(WordFile, System.IO.Path.GetExtension(file)))
                {
                    app.CreateIconFromImage("pack://application:,,,/Resources/Word_Icon.png");
                }
                else if (checkExtension(ExcelFile, System.IO.Path.GetExtension(file)))
                {
                    app.CreateIconFromImage("pack://application:,,,/Resources/Excel_Icon.png");

                }
                else if (checkExtension(PowerPointFile, System.IO.Path.GetExtension(file)))
                {
                    app.CreateIconFromImage("pack://application:,,,/Resources/PowerPoint_Icon.png");
                }
                else if (((FileAttributes)System.IO.File.GetAttributes(file)).HasFlag(FileAttributes.Directory))
                {
                    app.CreateIconFromImage("pack://application:,,,/Resources/Folder_Icon.png");
                }
                else if(System.IO.Path.GetExtension(file).ToUpper() == ".HTML")
                {
                    app.CreateIconFromImage("pack://application:,,,/Resources/Html_Icon.png");
                }
                else
                {
                    app.CreateIconFromImage("pack://application:,,,/Resources/Unknown_Icon.png");
                }

                app.AddElements();
                AppsList.Add(app);

            }
        }
        private bool checkExtension(string Extensions,string FileExt )
        {
            if(FileExt == "" || FileExt == null || FileExt.Length < 3)
            {
                return false;
            }
            if ((Extensions.ToUpper()).Contains(FileExt.ToUpper()))
            {
                return true;
            }
            else return false;
        }

        private void Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            canv1.Focus();
        }

        private string GetOriginalFileURL(string Location)
        {
            if (System.IO.File.Exists(Location))
            {
                // WshShellClass shell = new WshShellClass();
                WshShell shell = new WshShell(); //Create a new WshShell Interface
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(Location); //Link the interface to our shortcut

                return link.TargetPath;
            }
            else return "";
        }
    }
}
