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
        public static void SaveIcons(List<AppInfo> AppsList)
        {
            string[] lines = new string[AppsList.Count];
            int i = 0;
            foreach (AppInfo app in AppsList)
            {
                lines[i] += "Top=\"" + Canvas.GetTop(app) + "\"\t";
                lines[i] += "Left=\"" + Canvas.GetLeft(app) + "\"\t";
                lines[i] += "Text=\"" + app.FileName.Text + "\"\t";
                lines[i] += GetBasicInfo(app);
                i++;
            }
            File.WriteAllLines(SaveFiles.Location() + SaveFiles.AppInfoFile, lines);
        }
        //gets the common info
        private static string GetBasicInfo(DMShortcutItem app)
        {
            string line = "Width=\"" + app.Width + "\"\t";
            line += "Height=\"" + app.Height + "\"\t";
            line += "ShortCutLocation=\"" + app.ShortCutLocation + "\"\t";
            line += "ShortCutIcon=\"" + app.IconSourceLocation + "\"\t";
            line += "Parameters=\"" + app.Parameters + "\"\t";
            return line;
        }
        public static void SaveShortcuts(List<ShortcutsSaveData> shortcuts)
        {
            string[] lines = new string[shortcuts.Count];
            int i = 0;
            foreach(ShortcutsSaveData shortcut in shortcuts)
            {
                lines[i] += "Group=\"" + shortcut.ParentTile + "\"\t";
                lines[i] += "Text=\"" + shortcut.item.FileName_beta.Text + "\"\t";
                lines[i] += GetBasicInfo(shortcut.item);
                i++;
            }
            File.WriteAllLines(SaveFiles.Location() + SaveFiles.ShortcutsFile, lines);
        }
        public static List<ShortcutsSaveData> LoadShortcuts()
        {
            List<ShortcutsSaveData> shortcuts = new List<ShortcutsSaveData>();
            if (File.Exists(SaveFiles.Location() + SaveFiles.ShortcutsFile))
            {
                string[] lines = File.ReadAllLines(SaveFiles.Location() + SaveFiles.ShortcutsFile);
                foreach(string line in lines)
                {
                    try
                    {
                        ShortcutItem app = new ShortcutItem(GetVariable("ShortCutLocation", line), GetVariable("ShortCutIcon", line));
                        app.FileName_beta.Text = GetVariable("Text", line);
                        app.Parameters = GetVariable("Parameters", line);
                        if (!app.IsThereisErrors)
                        {
                            shortcuts.Add(new ShortcutsSaveData(GetVariable("Group", line), app));
                        }
                        
                    }
                    catch (Exception) { }
                }
            }
                return shortcuts;
        }
        public static List<AppInfo> LoadIcons(Canvas Parent)
        {
            List<AppInfo> AppsList = new List<AppInfo>();
            if(File.Exists(SaveFiles.Location() + SaveFiles.AppInfoFile))
            {
                string[] lines = File.ReadAllLines(SaveFiles.Location() + SaveFiles.AppInfoFile);
                foreach (string line in lines)
                {
                    try
                    {
                        AppInfo app = new AppInfo(GetVariable("ShortCutLocation", line), GetVariable("ShortCutIcon", line));
                        Canvas.SetTop(app, int.Parse(GetVariable("Top", line)));
                        Canvas.SetLeft(app, int.Parse(GetVariable("Left", line)));
                        app.FileName.Text = GetVariable("Text", line);
                        app.Parameters = GetVariable("Parameters", line);
                        Parent.Children.Add(app);
                        app.ParentCanvas = Parent;
                        if (!app.IsThereisErrors)
                            AppsList.Add(app);
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                AppsList = Initialize(AppsList, Parent);
            }
            SaveIcons(AppsList);
            return AppsList;
        }

        public static string GetVariable(string variable, string line)
        {
            variable += "=\"";
            if (!line.Contains(variable))
            {
                throw new Exception("Variable not Found");
            }
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
            //control panel
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
            return AppsList;
        }
        public static void SaveMainWindowTheme(List<SettingsHolder> data, string FileName)
        {
            string[] Variable = new string[data.Count()];
            int i = 0;
            foreach (SettingsHolder item in data)
            {
                Variable[i] = item.Name + "=\"" + item.Value + "\"";
                i++; 
            }
            File.WriteAllLines(SaveFiles.Location() + FileName, Variable);
        }
       
    } 
}
