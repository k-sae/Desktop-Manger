using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Desktop_Manger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }
    //set app theme
    public static class AppTheme
    {
        public static string NavBarBackground { get; set; }
        public static string NavBarForeground { get; set; }
        public static string NavBarHover { get; set; }
        public static string NavBarActive { get; set; }
        public static string HomePageShortCutsHover { get; set; }
        public static string HomePageShortCutFontColor { get; set; }
    }
}
