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

namespace Desktop_Manger
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private bool isNavBarShown = false;

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
        }

        //Disable Frame Nave Bar
        private void myFrame_ContentRendered(object sender, EventArgs e)
        {
            mainframe.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }
        
        // Hold Window Colors
        private void SetTheme()
        {
            NavBar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarBackGround));
            HomePage_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.ForeGround));
            HomePage_Text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.ForeGround));
            Apps_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.ForeGround));
            Apps_Text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.ForeGround));
            Power_Text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.ForeGround));
            Power_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.ForeGround));

        }
        
        // Maximize Widno
        public Task MaximizeWindow(Window window)
        {
            return Task.Factory.StartNew(() =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Thread.Sleep(100);
                    CenterScreen();
                }));
            });
        }
        // Maximize the app if its state changed to minimize
        private async void win1_StateChanged(object sender, EventArgs e)
        {
            //did it two times so if the first one failed
            await MaximizeWindow(this);
            await MaximizeWindow(this);
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
        public Task sleep(int time)
        {
              return Task.Factory.StartNew(() =>
             {
                 this.Dispatcher.Invoke((Action)(() =>
                 {
                     Thread.Sleep(time);
                 }));
             });
        }
        private async void NavBar_stpanel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isNavBarShown) return;
            for (int i = 0; i < 100; i++)
            {
                await sleep(1);
                NavBar.Width += 1;
            }
            isNavBarShown = true;
        }
        private async void NavBar_stpanel_MouseLeave(object sender, MouseEventArgs e)
        {
            Trace.WriteLine(e.GetPosition(null).X);
            if (e.GetPosition(null).X < 100 || !isNavBarShown) return;
            for (int i = 0; i < 100; i++)
            {
                await sleep(1);
                NavBar.Width -= 1;
            }
            isNavBarShown = false;
        }

        private void NaveItemsHover(object sender, MouseEventArgs e)
        {
            if (IsSelectedItem((StackPanel)sender))
            {
                return;
            }
            ((StackPanel)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Hover));
        }

        private void NaveItemsDefault(object sender, MouseEventArgs e)
        {
            if (IsSelectedItem((StackPanel)sender))
            {
                return;
            }
            ((StackPanel)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarBackGround));
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
            ((StackPanel)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Active));
            selectedStP = ((StackPanel)sender);
            HomePage page1 = new HomePage(this.Height, this.Width);
            mainframe.Navigate(page1);
        }

        private void Apps_stp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RemoveSelection();
            SelectStP(sender);
            Apps page1 = new Apps();
            mainframe.Navigate(page1);
        }
        
        private void SelectStP(object sender)
        {
            
            ((StackPanel)sender).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Active));
            selectedStP = ((StackPanel)sender);
        }
        private void RemoveSelection()
        {
            selectedStP.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.NavBarBackGround));
        }

    }

}

