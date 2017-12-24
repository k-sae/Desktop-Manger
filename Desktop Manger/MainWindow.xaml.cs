using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using MahApps.Metro.Controls;
using System.Diagnostics;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices;

namespace Desktop_Manger
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// TODO: update 5:
    // 1- improve the sidebar sensitivity
    // 2- read params from the imported item
    // 3- improve the shortcut loading time by adding images and other things on another thread
    // 4- add icon pack
    // 5- improve item dragging on home screen use a whole new algo ( depend on the distance from the point of click for the item dragging)
    // 6- improve performance by storing the whole ui element as binary file (new caching folder saving snapshots of the current state)
    public partial class MainWindow : MetroWindow
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpWindowClass, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);


        const int GWL_HWNDPARENT = -8;


        Shortcuts shortcuts_page = null;
        StackPanel selectedStP = new StackPanel();
        public MainWindow()
        {
            InitializeComponent();
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight - 1;
            CenterScreen();
            StartUp.Check();
            SetTheme();
            HomePage page1 = new HomePage(this.Height, this.Width);
            SelectStP(HomePage_stp);
            mainframe.Navigate(page1);
            Loaded += MainWindow_Loaded;
        }

        //Disable Frame Nave Bar
        private void myFrame_ContentRendered(object sender, EventArgs e)
        {
            mainframe.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }
        
        // Hold Window Colors
        public  void SetTheme()
        {
            NavBar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarBackground));
            HomePage_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarForeground));
            HomePage_Text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarForeground));
            Apps_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarForeground));
            Apps_Text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarForeground));
            Power_Text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarForeground));
            Power_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarForeground));
            Settings_Text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarForeground));
            Settings_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarForeground)); ;
        }
        
        // Maximize Widno
        public Task MaximizeWindow(Window window)
        {
            return Task.Factory.StartNew(() =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Thread.Sleep(50);
                    CenterScreen();
                }));
            });
        }
        // Maximize the app if its state changed to minimize
        private async void win1_StateChanged(object sender, EventArgs e)
        {
            //did it two times so if the first one failed
            SetAsBackground();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            SetAsBackground();
        }

        private void SetAsBackground()
        {
            IntPtr hprog = FindWindowEx(
            FindWindowEx(
                FindWindow("Progman", "Program Manager"),
                IntPtr.Zero, "SHELLDLL_DefView", ""
                ),
            IntPtr.Zero, "SysListView32", "FolderView"
         );

            SetWindowLong(new System.Windows.Interop.WindowInteropHelper(this).Handle, GWL_HWNDPARENT, hprog);
        }

        //make the app maximized at the center of the screen
        public void CenterScreen()
        {
            //two methods
            //method 1
            //this.WindowState = System.Windows.WindowState.Maximized;
            //with can resize

            //Or method 2:         
            this.WindowState = System.Windows.WindowState.Normal;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2) - 2;
            
        }
        public static Task sleep(int time)
        {
              return Task.Factory.StartNew(() =>
             {
                     Thread.Sleep(time);  
             });
        }
        private  void NavBar_stpanel_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation an = new DoubleAnimation();
            an.From = NavBar.Width;
            an.To = 100;
            an.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            NavBar.BeginAnimation(WidthProperty, an, HandoffBehavior.SnapshotAndReplace);

        }
        private  void NavBar_stpanel_MouseLeave(object sender, MouseEventArgs e)
        {
            Trace.WriteLine(e.GetPosition(null).X);
            if (e.GetPosition(null).X < 20)
            {
                return;
            }
            DoubleAnimation an = new DoubleAnimation();
            an.From = NavBar.ActualWidth;
            an.To = 0;
            an.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            NavBar.BeginAnimation(WidthProperty, an, HandoffBehavior.SnapshotAndReplace);
        }

        private void NaveItemsHover(object sender, MouseEventArgs e)
        {
            if (IsSelectedItem((StackPanel)sender))
            {
                return;
            }
            ((StackPanel)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarHover));
        }

        private void NaveItemsDefault(object sender, MouseEventArgs e)
        {
            if (IsSelectedItem((StackPanel)sender))
            {
                return;
            }
            ((StackPanel)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarBackground));
        }
        
        private bool IsSelectedItem(StackPanel item)
        {
            if (item.Name == selectedStP.Name)
            {
                return true;
            }
            else 
                 return false;

        }

        private void HomePage_stp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RemoveSelection();
            ((StackPanel)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarActive));
            selectedStP = ((StackPanel)sender);
            HomePage page1 = new HomePage(this.Height, this.Width);
            mainframe.Navigate(page1);
        }

        private void Apps_stp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(shortcuts_page == null)
            {
                shortcuts_page = new Shortcuts();
            }
            RemoveSelection();
            SelectStP(sender);
            mainframe.Navigate(shortcuts_page);
        }
        
        private void SelectStP(object sender)
        {
            
            ((StackPanel)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarActive));
            selectedStP = ((StackPanel)sender);
        }
        private void RemoveSelection()
        {
            selectedStP.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarBackground));
        }

        private void Power_stp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RemoveSelection();
            SelectStP(sender);
            power page = new power(grid1);
            mainframe.Navigate(page);
        }

        private void Settings_stp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RemoveSelection();
            SelectStP(sender);
            Settings page = new Settings();
            mainframe.Navigate(page);
        }

     
    }

}

