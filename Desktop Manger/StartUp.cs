using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Manger
{
    class StartUp
    {
        //check if the app opened for the first time
        public static bool IsFirstTime()
        {
            if (Directory.Exists(SaveFiles.Location()))
            {
                if (File.Exists(SaveFiles.Location() + SaveFiles.ThemeFile))
                {
                    return false;
                }
                else return true;
                
            }
            else
            {
                Directory.CreateDirectory(SaveFiles.Location());
                return true;
            }
            
        }
        //set defualt dark theme
        public static void DarkTheme()
        {
            AppTheme.NavBarForeground = "#ffffffff";
            AppTheme.NavBarBackground = "#ff000000";
            AppTheme.NavBarHover = "#FFEC670A"; 
            AppTheme.NavBarActive = "#FFFF6C18";
            AppTheme.HomePageShortCutsHover = "#3FFF8000";
            AppTheme.HomePageShortCutFontColor = "#ffffffff";
            AppTheme.Background = "#000";
            AppTheme.Foreground = "#fff";
        }
        public static void SetCustomTheme()
        {
            string data = File.ReadAllText(SaveFiles.Location() + SaveFiles.ThemeFile);
            try{AppTheme.Background = Data.GetVariable("MainAppBackground", data); }catch (Exception) { }
            try { AppTheme.Foreground = Data.GetVariable("MainAppForeground", data); } catch (Exception) { }
            try {AppTheme.NavBarBackground = Data.GetVariable("NavBarBackground", data); }catch (Exception) { }
            try{AppTheme.NavBarForeground = Data.GetVariable("NavBarForeground",  data); }catch (Exception) { }
            try{AppTheme.NavBarHover = Data.GetVariable("NavBarHover",  data); }catch (Exception) { }
            try{AppTheme.NavBarActive = Data.GetVariable("NavBarActive",  data); }catch (Exception) { }
        }
        public static void Check()
        {
            DarkTheme();
            if (!IsFirstTime())
            {
                SetCustomTheme();
            }
           
                
            
        }
       
    }
}
