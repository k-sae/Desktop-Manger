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
    /// TODO: update 0:
    ///     1-In TileLyout I have to Extend Its Height According to the summ of its Children Height
    public partial class Shortcuts : Page
    {
       static Tile Tile1 = null;
       static Tile Tile2 = null;
       static Tile Tile3 = null;
        public Shortcuts()
        {
            InitializeComponent();
            SetTheme();
            Tile1 = CreateTile(1, 0);
            Tile2 = CreateTile(1, 1);
            Tile3 = CreateTile(3, 0);
            Grid.SetColumnSpan(Tile3, 2);
            Grid1.Children.Add(Tile1);
            Grid1.Children.Add(Tile2);
            Grid1.Children.Add(Tile3);
            
        }
        private Tile CreateTile(int row, int col)
        {
            Tile ti = new Tile();
            ti.ChildMinWidth = 200;
            ti.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(AppTheme.Background)));
            ti.Margin = new Thickness(5);
            Grid.SetRow(ti, row);
            Grid.SetColumn(ti, col);
            ti.AllowAnimation = true;
            ti.AllowDrop = true;
            ti.Focusable = true;
            ti.MouseDown += Ti_MouseDown;
            ti.Drop += Tile_Drop;
            ti.AnimationSpeed = 5;
            return ti;
        }

        private void Ti_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Tile).Focus();
        }

        private void Tile_Drop(object sender, DragEventArgs e)
        {
            string[] Files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach(string File in Files)
            {
                //Dont Forget to Check for errors
                ShortcutItem canv = new ShortcutItem(File);
                canv.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor((sender as Tile).Background.ToString())));
                (sender as Tile).Add(canv);
            }
           
        }

        private void SetTheme()
        {
            Grid1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
            foreach(object child in Grid1.Children)
            {
                if (child is TextBox)
                {
                    (child as TextBox).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
                }
            }
        }
        public static void RemoveChild(ShortcutItem item)
        {
            Tile parent = (item.Parent as Tile);
            parent.Remove(item);
        }
        private void Groups_tb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LayoutObjects.MakeTextBoxEditable(sender as TextBox);
        }

        private void Groups_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            LayoutObjects.SealTextBox(sender as TextBox);
        }
    }
}
