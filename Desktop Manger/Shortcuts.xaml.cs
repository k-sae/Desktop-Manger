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
    /// TODO:
    ///     1-In TileLyout I have to Extend Its Height According to the summ of its Children Height
    public partial class Shortcuts : Page
    {
        Tile Tile1 = null;
        Tile Tile2 = null;
        Tile Tile3 = null;
        public Shortcuts()
        {
            InitializeComponent();
            SetTheme();
            Tile1 = CreateTile(0, 0);
            Tile2 = CreateTile(0, 1);
            Tile3 = CreateTile(1, 0);
            Grid.SetColumnSpan(Tile3, 2);
            Grid1.Children.Add(Tile1);
            Grid1.Children.Add(Tile2);
            Grid1.Children.Add(Tile3);
            
        }
        private Tile CreateTile(int row, int col)
        {
            Tile ti = new Tile();
            ti.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(AppTheme.Background)));
            ti.Margin = new Thickness(5);
            Grid.SetRow(ti, row);
            Grid.SetColumn(ti, col);
            ti.AllowAnimation = true;
            ti.AllowDrop = true;
            ti.Drop += Tile_Drop;
            return ti;
        }

        private void Tile_Drop(object sender, DragEventArgs e)
        {
            string[] Files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach(string File in Files)
            {
                StackPanel canv = new StackPanel();
                canv.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor((sender as Tile).Background.ToString())));
                (sender as Tile).Add(canv);
            }
           
        }

        private void SetTheme()
        {
            Grid1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
        }

     
    }
}
