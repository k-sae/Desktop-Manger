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
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading;

namespace Desktop_Manger
{
    /// <summary>
    /// Interaction logic for power.xaml
    /// </summary>
    /// TODO 
    ///     1-Remind me To Fix performance TOMORROW
    public partial class power : Page
    {
        public static BackgroundWorker PowerWorker = new BackgroundWorker();
        public static Thread PowerWorkerThread = null;
        public static string loc = SaveFiles.Location() + "file.txt";
        public static List<PowerItem> PowerItems = new List<PowerItem>();
        Panel ParentGrid = null;
        public power(Panel ParentGrid)
        {
            InitializeComponent();
            GETplans();
            SetTheme();
            this.ParentGrid = ParentGrid;

        }


        private  void GETplans()
        {
            BackgroundWorker CmdWorkerThread = new BackgroundWorker();
            CmdWorkerThread.DoWork += CmdWorkerThread_DoWork;
            CmdWorkerThread.RunWorkerCompleted += CmdWorkerThread_RunWorkerCompleted;
            CmdWorkerThread.RunWorkerAsync();

        }

        private void CmdWorkerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            divdeplans();
        }

        private void CmdWorkerThread_DoWork(object sender, DoWorkEventArgs e)
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
            TileLayout.Tile ti = new TileLayout.Tile()
            {
                Width = St1.Width,
                ChildMinWidth = 400,
                Childheight = 100,
                Height = 300,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(AppTheme.Background))),
                Margin = new Thickness(5)
            };
            St1.Children.Add(ti);
            for (int i = 0; i < lines.Count() - 3; i++)
            {
                //Constractor
                PowerPlan bed = new PowerPlan(GetStrBetweenTags(lines[i + 3], "GUID: ", "  ("), GetStrBetweenTags(lines[i + 3], "(", ")"));
                //u should have used (i + 3) instead of (i)
                if (lines[i + 3].Contains("*"))
                {
                    PowerItems.Add(new PowerItem(bed, true));
                    ti.Add(PowerItems[PowerItems.Count - 1]);

                }else
                {
                    PowerItems.Add(new PowerItem(bed));
                    ti.Add(PowerItems[PowerItems.Count - 1]);
                }

                

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
        private void SetTheme()
        {
            Grid1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
            St.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(Grid1.Background.ToString())));
            St_text.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
            power_txt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
            St_text1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Foreground));
            foreach (object obj1 in St1.Children)
            {
                if (obj1 is StackPanel)
                {
                    (obj1 as StackPanel).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.Background));
                }
            }

        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(AppTheme.GetAnotherColor(AppTheme.Background)));
        }
        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString((AppTheme.Background)));
        }

        private void Shutdown_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            int x = Int32.Parse(CounterDown_tb.Text);
            DoWork(x, "shutdown /s");
        }


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Restart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int x = Int32.Parse(CounterDown_tb.Text);
            DoWork(x, "shutdown /r /f");
        }

        private async void Sleep_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int x = Int32.Parse(CounterDown_tb.Text) * 60000;
            await (MainWindow.sleep(x));

        }

        private  void Hibernate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int x = Int32.Parse(CounterDown_tb.Text);
            DoWork(x, "shutdown /h /f");
           
        }
       private void DoWork(int time, string Arguments)
        {
            if (PowerWorker.IsBusy)
            {
                PowerWorker.CancelAsync();
                PowerWorkerThread.Abort();
                PowerWorker = new BackgroundWorker();
            }
            PowerTimer Timer = new PowerTimer();
            ParentGrid.Children.Add(Timer);
            Grid.SetColumn(Timer, 1);
            PowerWorker.WorkerReportsProgress = true;
            PowerWorker.WorkerSupportsCancellation = true;
            PowerWorker.DoWork += (sender, e) => DoBackGroundWork(PowerWorker, time, Timer);
            PowerWorker.ProgressChanged += Bw_ProgressChanged;
            PowerWorker.RunWorkerCompleted += (sender, e) => ProgressCompleted(Arguments);
            PowerWorker.RunWorkerAsync();
            Timer.WorkerThread = PowerWorkerThread;
        }
        private void ProgressCompleted(string args)
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
            process.StandardInput.WriteLine(args);
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
        }
        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            (e.UserState as PowerTimer).Timer.Text = TimerDividor(e.ProgressPercentage);
        }


        private void DoBackGroundWork(object sender, int time, PowerTimer timer)
        {
            PowerWorkerThread = Thread.CurrentThread;
            for (int i = time * 60; i > 0; i--)
            {
                Thread.Sleep(1000);
                (sender as BackgroundWorker).ReportProgress(i, timer);
            }
        }
        private  string TimerDividor(int time)
        {
            int h, m, sec;
            h = (time / 3600);
            time = time % 3600;
            m = (time / 60);
            sec = (time % 60);
            string h1 = h < 10 ? "0" + h.ToString() : h.ToString();         
            string m1 = m<10 ?"0"+m.ToString():m.ToString();
            string sec1 = sec < 10 ? "0" + sec.ToString() : sec.ToString();
            return h1 + ":" + m1 + ":" + sec1;
        }
       
    }

}

    

 
