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
    /// Interaction logic for HomePage_Settings.xaml
    /// </summary>
    public partial class HomePage_Settings : Page
    {
        public  List<ThemeChanger> MyThemeChanger = new List<ThemeChanger>();
        public HomePage_Settings()
        {
            InitializeComponent();
            SetTheme();
            Initialize();
            GenerateObjectControls();
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
                    foreach (object obj2 in (obj1 as StackPanel).Children)
                    {
                        if (obj2 is TextBox)
                        {
                            (obj2 as TextBox).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(stp1.Background.ToString())));
                            (obj2 as TextBox).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
                        }
                        else if(obj2 is Button)
                        {
                          (obj2 as Button).Background =  new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
                          (obj2 as Button).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
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
            StackPanel stp = (StackPanel)(sender as TextBox).Parent;
            TextBlock color = null;
            //get the Child TextBlock to change its background
            foreach (object child in stp.Children)
            {
                if (child is TextBlock)
                {
                    color = (child as TextBlock);
                }
            }
            //try change the background if possible
            try
            {
                color.Text = "";
                color.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString((sender as TextBox).Text));
                ChangeValue((sender as TextBox).Name, (sender as TextBox).Text);
            }
            //if not change its text tp "?"
            catch (Exception)
            {
                if (color != null)
                {
                    color.Text = "?";
                    color.Background = Brushes.Transparent;
                }
               
            }
        }
        //irritate through the ThemeChanger Class to find the object that should be changed
        private void ChangeValue(string Objectname, string Objectvalue)
        {
            foreach (ThemeChanger th in MyThemeChanger)
            {
                if (th.Name == Objectname)
                {
                    th.Value = Objectvalue;
                }
            }
        }
        //initialize objects upon navigation to page
        private void GenerateObjectControls()
        {
            ItemHover.Text = AppTheme.HomePageShortCutsHover;
            ItemHover_TBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.HomePageShortCutsHover));
            HomePageFontColor.Text = AppTheme.HomePageShortCutFontColor;
            HomePageFontColor_TBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.HomePageShortCutFontColor));
            HomePageBackground.Text = AppTheme.HomePageBackground;
        }
        private void Initialize()
        {
            MyThemeChanger.Add(new ThemeChanger(ItemHover.Name, AppTheme.HomePageShortCutsHover));
            MyThemeChanger.Add(new ThemeChanger(HomePageFontColor.Name, AppTheme.HomePageShortCutFontColor));
            MyThemeChanger.Add(new ThemeChanger(HomePageBackground.Name, AppTheme.HomePageBackground));
        }
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Data.SaveMainWindowTheme(MyThemeChanger,SaveFiles.HomePageThemeFile);
            StartUp.SetCustomTheme();
        }

        private void HomePageBackground_Btn_Click(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            StackPanel stp = (StackPanel)(sender as Button).Parent;
            dlg.DefaultExt = ".mp4";
            dlg.Filter = "Video (*.Mp4, avi)|*.Mp4;*.avi|Image Files (*.jp*, *.png,...)|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
            dlg.Multiselect = true;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                foreach(object obj in stp.Children)
                {
                    if (obj is TextBox)
                    {
                        (obj as TextBox).Text = dlg.FileName;
                        ChangeValue((obj as TextBox).Name, dlg.FileName);
                    }
                }
            }
        }
    }
}
