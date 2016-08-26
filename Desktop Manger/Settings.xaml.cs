using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for power.xaml
    /// </summary>
    public partial class Settings : Page
    {
        StackPanel selectedStP = new StackPanel();
        public Settings()
        {
            InitializeComponent();
            selectedStP = General_stp;
            General_Settings page = new General_Settings(this);
            SubFrame.Navigate(page);
            SetTheme();
        }

        public void SetTheme()
        {
            Grid1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
        }
        private void General_stp_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((sender as StackPanel) != selectedStP)
            {
                (sender as StackPanel).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF329DF9"));
            }
           
        }

        private void General_stp_MouseLeave(object sender, MouseEventArgs e)
        {
            if ((sender as StackPanel) != selectedStP)
            {
                (sender as StackPanel).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000"));
            }
        }

        private void General_stp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RemoveSelection();
            SelectStP(sender);
            General_Settings page = new General_Settings(this);
            SubFrame.Navigate(page); 
        }
        private void RemoveSelection()
        {
            selectedStP.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000"));
        }
        private void SelectStP(object sender)
        {

            ((StackPanel)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF004581"));
            selectedStP = ((StackPanel)sender);
        }
        private void myFrame_ContentRendered(object sender, EventArgs e)
        {
            SubFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void HomePage_stp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RemoveSelection();
            SelectStP(sender);
            HomePage_Settings page = new HomePage_Settings();
            SubFrame.Navigate(page);
        }
    }
}
