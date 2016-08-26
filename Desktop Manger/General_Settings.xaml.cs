using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Desktop_Manger
{
    /// <summary>
    /// Interaction logic for General_Settings.xaml
    /// </summary>
    /// next i have to send main window as a parameter to change the theme colors directly
    public partial class General_Settings : Page
    {
        private Settings SettingsPage { get; set; }
        public static List<ThemeChanger> MyThemeChanger = new List<ThemeChanger>();
        public General_Settings(Settings Page)
        {
            InitializeComponent();
            SetTheme();
            GenerateObj_controlls();
            Initiallize();
            SettingsPage = Page;
        }
        private void GenerateObj_controlls()
        {
            NavBarBackground.Text = AppTheme.NavBarBackground;
            NavBarBackground_TBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarBackground));
            NavBarForeground.Text = AppTheme.NavBarForeground;
            NavBarForeground_TBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarForeground));
            NavBarHover.Text = AppTheme.NavBarHover;
            NavBarHover_TBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarHover));
            NavBarActive.Text = AppTheme.NavBarActive;
            NavBarActive_TBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarActive));
            MainAppBackground.Text = AppTheme.Background;
            NavBarActive_TBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
        }
        private void SetTheme()
        {
            Grid1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(AppTheme.Background)));
            stp1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(Grid1.Background.ToString())));
            ThemeSettings_Tb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(Grid1.Background.ToString())));
            foreach (object obj1 in Grid2.Children)
            {
                if (obj1 is StackPanel)
                {
                    foreach(object obj2 in (obj1 as StackPanel).Children)
                    {
                        if (obj2 is TextBox)
                        {
                            (obj2 as TextBox).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(stp1.Background.ToString())));
                        }
                    }
                }
            }
           
        }
        private void TBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           StackPanel stp =(StackPanel)(sender as TextBox).Parent;
           TextBlock color = null;
           foreach(object child in stp.Children)
            {
                if(child is TextBlock)
                {
                    color = (child as TextBlock);
                }
            }
            try
            {
                color.Text = "";
                color.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString((sender as TextBox).Text));
                ChangeColor((sender as TextBox).Name, (sender as TextBox).Text);
            }
            catch (Exception)
            {
                color.Text = "?";
                color.Background = Brushes.Transparent;
            }
        }
        private void ChangeColor(string Objectname, string Objectvalue)
        {
            foreach(ThemeChanger th in MyThemeChanger)
            {
                if (th.Name == Objectname)
                {
                    th.Value = Objectvalue;
                }
            }
        }
        private void Initiallize()
        {
            MyThemeChanger.Add(new ThemeChanger(NavBarBackground.Name, AppTheme.NavBarBackground));
            MyThemeChanger.Add(new ThemeChanger(NavBarForeground.Name, AppTheme.NavBarForeground));
            MyThemeChanger.Add(new ThemeChanger(NavBarHover.Name, AppTheme.NavBarHover));
            MyThemeChanger.Add(new ThemeChanger(NavBarActive.Name, AppTheme.NavBarActive));
            MyThemeChanger.Add(new ThemeChanger(MainAppBackground.Name, AppTheme.Background));
        }
        //i have to create a function in data class takes list of certain class that holds the the variable name and the value
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Data.SaveMainWindowTheme(MyThemeChanger);
            StartUp.SetCustomTheme();
            SetTheme();
            SettingsPage.SetTheme();
        }
    }

     public partial class ThemeChanger
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public ThemeChanger(string ObjectName, object originalcolor)
        {
            Name = ObjectName;
            Value = originalcolor;
        }
    }
}
