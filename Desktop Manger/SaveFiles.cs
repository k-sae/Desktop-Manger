using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Manger
{
    class SaveFiles
    {
        public static string MainThemeFile = "Theme.dmt";
        public static string HomePageThemeFile = "HomePage.dmt";
        public static string AppInfoFile = "AppInfo.dms";
        public static string ShortcutsFile = "Shortcuts.dms";
        public static string ShortcutsGroupsNameFile = "GroupsName.dms";
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
