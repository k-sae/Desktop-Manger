using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Desktop_Manger
{
    class Data
    {
        //TODO:
        //      Fix the error for ShotcutSource For exe and icon files
        public static void SaveIcons(List<AppInfo> AppsList)
        {
            string[] lines = new string[AppsList.Count];
            int i = 0;
            foreach (AppInfo App in AppsList)
            {
                lines[i] += "Top=\"" + Canvas.GetTop(App) + "\"\t";
                lines[i] += "Left=\"" + Canvas.GetLeft(App) + "\"\t";
                lines[i] += "Width=\"" + App.Width + "\"\t";
                lines[i] += "Height=\"" + App.Height + "\"\t";
                lines[i] += "ShortCutLocation=\"" + App.ShortCutLocation + "\"\t";
                lines[i] += "ShortCutIcon=\"" + App.IconSourceLocation + "\"\t";
                lines[i] += "Text=\"" + App.FileName.Text + "\"\t";
                lines[i] += "Parameters=\"" + App.Parameters + "\"\t";
                i++;
            }
            File.WriteAllLines(StartUp.Location() + "AppInfo.dms", lines);
        }
        public static List<AppInfo> LoadIcons(Canvas Parent)
        {
            List<AppInfo> AppsList = new List<AppInfo>();
            if(File.Exists(StartUp.Location() + "AppInfo.dms"))
            {
                string[] lines = File.ReadAllLines(StartUp.Location() + "AppInfo.dms");
                foreach (string line in lines)
                {
                    AppInfo app = new AppInfo(GetVariable("ShortCutLocation", line), GetVariable("ShortCutIcon", line));
                    Canvas.SetTop(app, Int32.Parse(GetVariable("Top", line)));
                    Canvas.SetLeft(app, Int32.Parse(GetVariable("Left", line)));
                    app.FileName.Text = GetVariable("Text", line);
                    app.Parameters = GetVariable("Parameters", line);
                    Parent.Children.Add(app);
                    app.ParentCanvas = Parent;
                    AppsList.Add(app);
                }
            }
            else
            {
                AppsList = Initialize(AppsList, Parent);
            }
            return AppsList;
        }

        public static string GetVariable(string variable, string line)
        {
            variable += "=\"";
            int start = line.IndexOf(variable) + variable.Length;
            int end = line.IndexOf("\"", start);
            return line.Substring(start, end - start);
        }
        private static List<AppInfo> Initialize(List<AppInfo> AppsList, Canvas ParentCanvas)
        {
            AppInfo ThisPc = new AppInfo(@"C:\Windows\explorer.exe", "pack://application:,,,/Resources/This_Pc.png");
            Canvas.SetLeft(ThisPc, 0);
            Canvas.SetTop(ThisPc, 160);
            ParentCanvas.Children.Add(ThisPc);
            ThisPc.FileName.Text = "This PC";
            ThisPc.ParentCanvas = ParentCanvas;
            AppsList.Add(ThisPc);
            //added control panel
            AppInfo ControlPanel = new AppInfo(@"C:\Windows\explorer.exe", "pack://application:,,,/Resources/Control_Panel_Icon.png");
            Canvas.SetLeft(ControlPanel, 0);
            Canvas.SetTop(ControlPanel, HomePageLayout.CanvasHeight * 5);
            ControlPanel.Parameters = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}\\::{21EC2020-3AEA-1069-A2DD-08002B30309D}";
            ParentCanvas.Children.Add(ControlPanel);
            ControlPanel.FileName.Text = "Controll Panel";
            ControlPanel.ParentCanvas = ParentCanvas;
            AppsList.Add(ControlPanel);
            //Recycle Bin
            AppInfo RecycleBin = new AppInfo(@"C:\Windows\explorer.exe", "pack://application:,,,/Resources/Recycle_Bin_Icon.png");
            Canvas.SetLeft(RecycleBin, 0);
            Canvas.SetTop(RecycleBin, HomePageLayout.CanvasHeight * 3);
            RecycleBin.Parameters = "/N,::{645FF040-5081-101B-9F08-00AA002F954E}";
            ParentCanvas.Children.Add(RecycleBin);
            RecycleBin.FileName.Text = "Recycle Bin";
            RecycleBin.ParentCanvas = ParentCanvas;
            AppsList.Add(RecycleBin);
            //Network
            AppInfo network = new AppInfo(@"C:\Windows\explorer.exe", "pack://application:,,,/Resources/Network_Icon.png");
            Canvas.SetLeft(network, 0);
            Canvas.SetTop(network, HomePageLayout.CanvasHeight * 4);
            network.Parameters = "::{208D2C60-3AEA-1069-A2D7-08002B30309D}";
            ParentCanvas.Children.Add(network);
            network.FileName.Text = "Network";
            network.ParentCanvas = ParentCanvas;
            AppsList.Add(network);
            //Documents
            AppInfo Documents = new AppInfo(@"C:\Windows\explorer.exe", "pack://application:,,,/Resources/My_Documents_Icon.png");
            Canvas.SetLeft(Documents, 0);
            Canvas.SetTop(Documents, HomePageLayout.CanvasHeight * 2);
            Documents.Parameters = "::{450D8FBA-AD25-11D0-98A8-0800361B1103}";
            ParentCanvas.Children.Add(Documents);
            Documents.FileName.Text = "My Douments";
            Documents.ParentCanvas = ParentCanvas;
            AppsList.Add(Documents);
            Data.SaveIcons(AppsList);
            Data.SaveIcons(AppsList);
            return AppsList;
        }
        public static void SaveMainWindowTheme(List<ThemeChanger> data)
        {
            string[] Variable = new string[data.Count()];
            int i = 0;
            foreach (ThemeChanger item in data)
            {
                Variable[i] = item.Name + "=\"" + item.Value + "\"";
                i++; 
            }
            File.WriteAllLines(StartUp.Location() + "Theme.dmt", Variable);
        }
       
    } 
}
