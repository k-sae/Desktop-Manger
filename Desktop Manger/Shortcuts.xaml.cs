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
    /// NOTE: Load Function here act little bit diffrent from the Appinfo Load Funtion
    /// TODO: update 1:
    ///     1-In TileLyout I have to Extend Its Height According to the summ of its Children Height
    ///     2-although have to find a solution for the overflow of left in canvas 
    ///                                                                             # where items have -ve margin on left at the begaining 
    public partial class Shortcuts : Page
    {
       public static List<ShortcutsSaveData> ShortcutItems = new List<ShortcutsSaveData>();
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
            //Grid11.Children.Add(Tile1);
            //Grid12.Children.Add(Tile2);
            ScrollViewer0.Content = Tile1;
            ScrollViewer1.Content = Tile2;
            ScrollViewer2.Content = Tile3;
            SetScrollViewer(ScrollViewer0);
            SetScrollViewer(ScrollViewer1);
            SetScrollViewer(ScrollViewer2);
            //Grid13.Children.Add(Tile3);
            Load();
        }
        public void SetScrollViewer(ScrollViewer viewer)
        {
            viewer.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(AppTheme.Background)));
            viewer.AllowDrop = true;
            viewer.Drop += Tile_Drop;
            viewer.MouseDown += Ti_MouseDown;
        }
        private void Load()
        {

            List<TextBox> GroupsNames = Data.LoadGroupNames();
            foreach(TextBox GroupName in GroupsNames)
            {
                foreach (object child in Grid1.Children)
                {
                    if (child is TextBox)
                    {
                        if ((child as TextBox).Name == GroupName.Name )
                        {
                            (child as TextBox).Text = GroupName.Text;
                        }
                    }
                    //thats all folks
                }
            }
            ShortcutItems = Data.LoadShortcuts();
            foreach(ShortcutsSaveData item in ShortcutItems)
            {
                Tile parent = GetParent(item.ParentTile);
                item.item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(ScrollViewer1.Background.ToString())));
                parent.Add(item.item);
            }
            Data.SaveShortcuts(ShortcutItems);
        }
        private Tile GetParent(string group)
        {
            if (group == "Tile2")
            {
                return Tile2;
            }
            else if (group == "Tile1")
            {
                return Tile1;
            }
            else return Tile3;
            
        }
        private Tile CreateTile(int row, int col)
        {
            Tile ti = new Tile()
            {
                ChildMinWidth = 200,

                Margin = new Thickness(5),
                AllowDrop = true,
                Focusable = true,
                AnimationDelay = 200
            };
            ti.VerticalAlignment = VerticalAlignment.Top;
            //ti.MouseDown += Ti_MouseDown;
            //ti.Drop += Tile_Drop;
            
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

                ShortcutItem shortcutitem = new ShortcutItem(File)
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor((sender as ScrollViewer).Background.ToString())))
                };
                if (!shortcutitem.IsThereisErrors)
                {
                    Tile Tile = (sender as ScrollViewer).Content as Tile;
                    Tile.Add(shortcutitem);
                    ShortcutItems.Add(new ShortcutsSaveData(FindTileName(sender as ScrollViewer), shortcutitem));
                }
            }
            Data.SaveShortcuts(ShortcutItems);
        }
        private string FindTileName(ScrollViewer tile)
        {
            if (tile.Content as Tile == Tile1)
            {
                return "Tile1";
            }
            else if (tile.Content as Tile == Tile2) { return "Tile2"; }
            else return "Tile3";
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
            foreach(ShortcutsSaveData shortcutitem in ShortcutItems)
            {
                if (shortcutitem.item == item)
                {
                    ShortcutItems.Remove(shortcutitem);break;
                }
            }
            Data.SaveShortcuts(ShortcutItems);
        }
        private void Groups_tb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LayoutObjects.UnSealTextBox(sender as TextBox);
        }

        private void Groups_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            LayoutObjects.SealTextBox(sender as TextBox);
            List<TextBox> GroupNames = new List<TextBox>();
            GroupNames.Add(Group1_tb);
            GroupNames.Add(Group2_tb);
            GroupNames.Add(Group3_tb);
            Data.SaveGroupNames(GroupNames);
        }
    }
   public class ShortcutsSaveData
    {
        public ShortcutsSaveData(string ParentTile, ShortcutItem item)
        {
            this.ParentTile = ParentTile;
            this.item = item;
        }
        public string ParentTile { get; set; }
        public ShortcutItem item { get; set; }
    }

}
