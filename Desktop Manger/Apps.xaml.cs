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
using TileLayout;

namespace Desktop_Manger
{
    /// <summary>
    /// Interaction logic for Apps.xaml
    /// </summary>
    public partial class Apps : Page
    {
        Tile tl = null;
        int index = 0;
        public Apps()
        {
            InitializeComponent();
            // Tile ti = new Tile();
            Tile ti = new Tile();
            ti.Background = Brushes.Blue;
            Grid1.Children.Add(ti);
            tl = ti;
        }
     
        private void SetTheme()
        {
            Grid1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stp = new StackPanel();
            stp.Background = Brushes.Red;
            TextBlock tb = new TextBlock();
            tb.Text = index.ToString();
            tb.TextAlignment = TextAlignment.Center;
            index++;
            stp.Children.Add(tb);
            tl.Add(stp);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
