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
            if (!Directory.Exists(Location()))
            {
                Directory.CreateDirectory(Location());
                return true;
            }
            return true;
        }
        //set defualt dark theme
        public static void DarkTheme()
        {
            AppTheme.NavBarForeground = "#ffffffff";
            AppTheme.NavBarBackground = "#ff000000";
            AppTheme.Hover = "#FFEC670A"; 
            AppTheme.Active = "#FFFF6C18";
            AppTheme.HomePageShortCutsHover = "#3FFF8000";
            AppTheme.HomePageShortCutFontColor = "#ffffffff";
        }
        public static void LightTheme()
        {
            AppTheme.NavBarForeground = "#000";
            AppTheme.NavBarBackground = "#fff";
            AppTheme.Hover = "#f00"; // "#FFEC670A"
            AppTheme.Active = "#FFFF6C18";
        }
        public static void Check()
        {
            if (IsFirstTime())
            {
                DarkTheme();

            }
        }
        public static string Location()
        {
            string path = Path.GetPathRoot(Environment.SystemDirectory);
            string userName = pickusername();
            string URI = path + @"Users\" + userName + @"\Documents\Desktop Manger\";
            return URI;
        }
        private static string pickusername()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            userName = ReverseString(userName);
            string temp = "";
            foreach (char item in userName)
            {
                if (item == '\\')
                {
                    break;
                }
                temp += item;
            }
            userName = ReverseString(temp);
            return userName;
        }
        private static string ReverseString(string mystring)
        {
            char[] arr = mystring.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
