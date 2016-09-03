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
using System.Diagnostics;
using System.IO;

namespace Desktop_Manger
{
    /// <summary>
    /// Interaction logic for power.xaml
    /// </summary>
    public partial class power : Page
    {
        public static string loc = SaveFiles.Location() + "file.txt";
        private static List<PowerPlan> CurrentPowerPlanes = new List<PowerPlan>();
        public power()
        {
            InitializeComponent();
            GETplans();
            divdeplans();
            SetTheme();

        }


        private static void GETplans()
        {

            if (File.Exists(loc))
            {
                File.Delete(loc);
                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = new Process { StartInfo = startInfo };
                process.Start();
                process.StandardInput.WriteLine(@"powercfg -LIST >> " + "\"" + loc + "\"");
                process.StandardInput.WriteLine("exit");
                process.WaitForExit();
            }
            else
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = new Process { StartInfo = startInfo };
                process.Start();
                process.StandardInput.WriteLine(@"powercfg -LIST >> " + "\"" + loc + "\"");
                process.StandardInput.WriteLine("exit");
                process.WaitForExit();
            }
        }
        private void divdeplans()
        {
            string[] lines = File.ReadAllLines(loc);


            for (int i = 0; i < lines.Count() - 3; i++)
            {
                //Constractor
                PowerPlan bed = new PowerPlan(Grid1, GetStrBetweenTags(lines[i + 3], "GUID: ", "  ("), GetStrBetweenTags(lines[i + 3], "(", ")"),i+1,1);
                CurrentPowerPlanes.Add(bed);
                //CreateStackpanel(i + 1);
                //EdiT_Layout(i + 1, CurrentPowerPlanes[i].Name);
            }
            

        }
        public static string GetStrBetweenTags(string value,
                                       string startTag,
                                       string endTag)
        {

            if (value.Contains(startTag) && value.Contains(endTag))
            {
                int index = value.IndexOf(startTag) + startTag.Length;
                return value.Substring(index, value.IndexOf(endTag) - index);
            }
            else
                return null;
        }

        public StackPanel CreateStackpanel(int row)
        {
            StackPanel ST = new StackPanel();
            Grid.SetColumn(ST, 1);
            Grid.SetRow(ST, row + 1);
            ST.Height = 50;
            ST.Orientation = Orientation.Horizontal;
            ST.Margin = new Thickness(50,0, 0, 0);
            ST.VerticalAlignment = VerticalAlignment.Top;
            ST.MouseLeftButtonUp += new MouseButtonEventHandler(stackpanel_click);
         
            St1.Children.Add(ST);
        
            return ST;
        }
        public TextBlock CreateTXTblock(string content)
        {
            TextBlock tb = new TextBlock();
            tb.Text = content;
            tb.FontSize = 18;
            tb.Name = "tb1";
            tb.Margin = new Thickness(20, 5, 0, 0);

            return tb;
        }
        public Image DefaultImages(string name)
        {
            Image img = new Image();
            img.MaxHeight = 50;
            img.MaxWidth = 50;
            img.HorizontalAlignment = HorizontalAlignment.Left;

            if (name == "Balanced")

                img.Source = new BitmapImage(new Uri(@"Resources\balance-icon.png", UriKind.RelativeOrAbsolute));

            else if (name == "High performance")

                img.Source = new BitmapImage(new Uri(@"Resources\highper icon.png", UriKind.RelativeOrAbsolute));


            else if (name == "Power saver")
                img.Source = new BitmapImage(new Uri(@"Resources\powersaver.png", UriKind.RelativeOrAbsolute));

            else
                img.Source = new BitmapImage(new Uri(@"Resources\power button.png", UriKind.RelativeOrAbsolute));

            return img;
        }
        public void EdiT_Layout(int num, string content)
        {
            StackPanel stp = CreateStackpanel(num);
            TextBlock tb = CreateTXTblock(content);
            Image im = DefaultImages(content);
            stp.Children.Add(im);
            stp.Children.Add(tb);
          
        }
        private void SetTheme()
        {
           
            Grid1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
            foreach (object obj1 in St1.Children)
            {
                if (obj1 is StackPanel)
                {
                    foreach (object obj2 in (obj1 as StackPanel).Children)
                    {
                        if (obj2 is TextBlock)
                        {
                            (obj2 as TextBlock).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
                        }
                    }
                }
            }
            foreach (object obj1 in Grid1.Children)
            {
                if(obj1 is TextBlock)
                {
                    (obj1 as TextBlock).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.PowerTxtforeground));
                }
                if (obj1 is StackPanel)
                {
                    foreach (object obj2 in (obj1 as StackPanel).Children)
                    {
                        if (obj2 is TextBlock)
                        {

                            (obj2 as TextBlock).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
                        }
                    }
                }
            }
        }
        public void stackpanel_click(object sender, EventArgs e)
        {
            
        }
        
       



    }
}
    

 
