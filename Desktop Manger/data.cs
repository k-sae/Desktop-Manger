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
        public static void save(List<AppInfo> AppsList)
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
        public static List<AppInfo> Load(Canvas Parent)
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

        private static string GetVariable(string variable, string line)
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
            AppInfo ControlPanel = new AppInfo(@"C:\Windows\explorer.exe", "pack://application:,,,/Resources/Control_Panel_Icon.png");
            Canvas.SetLeft(ControlPanel, 0);
            Canvas.SetTop(ControlPanel, 160 * 2);
            ControlPanel.Parameters = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}\\::{21EC2020-3AEA-1069-A2DD-08002B30309D}";
            ParentCanvas.Children.Add(ControlPanel);
            ControlPanel.FileName.Text = "Controll Panel";
            ControlPanel.ParentCanvas = ParentCanvas;
            AppsList.Add(ControlPanel);
            Data.save(AppsList);
            return AppsList;
        }
    }
}
