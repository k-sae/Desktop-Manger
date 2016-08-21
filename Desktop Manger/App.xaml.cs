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
        public static string NavBarBackGround { get; set; }
        public static string ForeGround { get; set; }
        public static string Hover { get; set; }
        public static string Active { get; set; }
        public static string HomePageShortCutsHover { get; set; }
        public static string HomePageShortCutFontColor { get; set; }
        public static string POWERFOREGROUND { get;  set; }
    }
}
