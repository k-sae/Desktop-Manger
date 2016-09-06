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
                if (File.Exists(SaveFiles.Location() + SaveFiles.MainThemeFile))
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
            AppTheme.HomePageBackground = "Resources/Videos/p4fun_intro0.mp4";
        }
        public static void SetCustomTheme()
        {
            string ThemeFile = File.ReadAllText(SaveFiles.Location() + SaveFiles.MainThemeFile);
            string HomePageThemeFile = File.ReadAllText(SaveFiles.Location() + SaveFiles.HomePageThemeFile);
            try{AppTheme.Background = Data.GetVariable("MainAppBackground", ThemeFile); }catch (Exception) { }
            try { AppTheme.Foreground = Data.GetVariable("MainAppForeground", ThemeFile); } catch (Exception) { }
            try {AppTheme.NavBarBackground = Data.GetVariable("NavBarBackground", ThemeFile); }catch (Exception) { }
            try{AppTheme.NavBarForeground = Data.GetVariable("NavBarForeground",  ThemeFile); }catch (Exception) { }
            try{AppTheme.NavBarHover = Data.GetVariable("NavBarHover",  ThemeFile); }catch (Exception) { }
            try{AppTheme.NavBarActive = Data.GetVariable("NavBarActive",  ThemeFile); }catch (Exception) { }
            try { AppTheme.HomePageShortCutsHover = Data.GetVariable("ItemHover", HomePageThemeFile); } catch (Exception) { }
            try { AppTheme.HomePageShortCutFontColor = Data.GetVariable("HomePageFontColor", HomePageThemeFile); } catch (Exception) { }
            try { AppTheme.HomePageBackground = Data.GetVariable("HomePageBackground", HomePageThemeFile); } catch (Exception) { }
            try { AppTheme.HomePageVideoSound = Data.GetVariable("IsMuted", HomePageThemeFile); } catch (Exception) { }
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
