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
    /// when eve i had to add a new color control here is the list of functions i had to change
    /// 1-GenerateObj_controlls
    /// 2-SetTheme
    /// 3-Initiallize
    /// 4-Startup.DarkTheme
    /// Startup.SetCustomeTheme
    public partial class General_Settings : Page
    {
        private Settings SettingsPage { get; set; }
        public List<SettingsHolder> MyThemeChanger = new List<SettingsHolder>();
        //get the parent settings page as parameters to endable editing its theme after pressing the save button
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
            MainAppBackground_TBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
            MainAppForeground.Text = AppTheme.Foreground;
            MainAppForeground_TBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
        }
        private void SetTheme()
        {
            Grid1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(AppTheme.Background)));
            stp1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(Grid1.Background.ToString())));
            ThemeSettings_Tb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(Grid1.Background.ToString())));
            ThemeSettings_Tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(AppTheme.Foreground)));
            Save_Button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
            Save_Button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
            foreach (object obj1 in Grid2.Children)
            {
                if (obj1 is StackPanel)
                {
                    foreach(object obj2 in (obj1 as StackPanel).Children)
                    {
                        if (obj2 is TextBox)
                        {
                            (obj2 as TextBox).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(stp1.Background.ToString())));
                            (obj2 as TextBox).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
                        }
                    }
                }
                if (obj1 is TextBlock)
                {
                    (obj1 as TextBlock).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
                }
            }
           
        }
        //whenever the text changed the crossponding TextBlock change its Background if possible
        private void TBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //hold the parent stackpanel for the  TextBox
           StackPanel stp =(StackPanel)(sender as TextBox).Parent;
           TextBlock color = null;
            //get the Child TextBlock to change its background
           foreach(object child in stp.Children)
            {
                if(child is TextBlock)
                {
                    color = (child as TextBlock);
                }
            }
           //try change the background if possible
            try
            {
                color.Text = "";
                color.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString((sender as TextBox).Text));
                ChangeColor((sender as TextBox).Name, (sender as TextBox).Text);
            }
            //if not change its text tp "?"
            catch (Exception)
            {
                color.Text = "?";
                color.Background = Brushes.Transparent;
            }
        }
        //irritate through the ThemeChanger Class to find the object that should be changed
        private void ChangeColor(string Objectname, string Objectvalue)
        {
            foreach(SettingsHolder th in MyThemeChanger)
            {
                if (th.Name == Objectname)
                {
                    th.Value = Objectvalue;
                }
            }
        }
        //Initiallize the mytheme changer class with string name and and color
        private void Initiallize()
        {
            MyThemeChanger.Add(new SettingsHolder(NavBarBackground.Name, AppTheme.NavBarBackground));
            MyThemeChanger.Add(new SettingsHolder(NavBarForeground.Name, AppTheme.NavBarForeground));
            MyThemeChanger.Add(new SettingsHolder(NavBarHover.Name, AppTheme.NavBarHover));
            MyThemeChanger.Add(new SettingsHolder(NavBarActive.Name, AppTheme.NavBarActive));
            MyThemeChanger.Add(new SettingsHolder(MainAppBackground.Name, AppTheme.Background));
            MyThemeChanger.Add(new SettingsHolder(MainAppForeground.Name, AppTheme.Foreground));
        }
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Data.SaveMainWindowTheme(MyThemeChanger,SaveFiles.MainThemeFile);
            StartUp.SetCustomTheme();
            SetTheme();
            SettingsPage.SetTheme();
        }
    }
   
}
