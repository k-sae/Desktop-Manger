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
    public partial class General_Settings : Page
    {
        public General_Settings()
        {
            InitializeComponent();
            GenerateObj_controlls();
        }
        private void GenerateObj_controlls()
        {
            NavBarBackground_TBox.Text = AppTheme.NavBarBackground;
            NavBarBackground_Rectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarBackground));
            NavBarForeground_TBox.Text = AppTheme.NavBarForeground;
            NavBarForeground_TBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarForeground));
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
            }
            catch (Exception)
            {
                color.Text = "?";
                color.Background = Brushes.Transparent;
            }
        }
    }
}
