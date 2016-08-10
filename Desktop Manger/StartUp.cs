using System;
using System.Collections.Generic;
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
            return true;
        }
        //set defualt dark theme
        public static void DarkTheme()
        {
            AppTheme.ForeGround = "#ffffffff";
            AppTheme.NavBarBackGround = "#ff000000";
            AppTheme.Hover = "#FFEC670A"; 
            AppTheme.Active = "#FFFF6C18";
            AppTheme.HomePageShortCutsHover = "#3FFF8000";
            AppTheme.HomePageShortCutFontColor = "#ffffffff";
        }
        public static void LightTheme()
        {
            AppTheme.ForeGround = "#000";
            AppTheme.NavBarBackGround = "#fff";
            AppTheme.Hover = "#f00"; // "#FFEC670A"
            AppTheme.Active = "#FFFF6C18";
        }
        public static void Check()
        {
            if (IsFirstTime())
            {
               //LightTheme();
                DarkTheme();
            }
        }
    }
}
