using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                lines[i] += "ShortCutIcon=\"" + App.ShortcutIcon.Source.ToString() + "\"\t";
                lines[i] += "Text=\"" + App.FileName.Text + "\"\t";
                i++;
            }
            File.WriteAllLines(StartUp.Location() + "AppInfo.dms", lines);
        }

        
    }
}
