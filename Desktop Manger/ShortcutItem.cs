using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

//TODO NextProject
// After Creating the Logic tree builder Create A logic tree builder file
//    Logic Tree:  
//                  ItemShortcut ------> Grid ItemHolder |---> Grid EventsHolder--> Image + TB
//                                                       |---> Canvas -->  Grid |--> ellipse
//                                                                              |--> TextBlock
//TODO: update 1:
//          1-enable the drag of elements from tile to another tile
namespace Desktop_Manger
{
    public class ShortcutItem : DMShortcutItem
    {
        bool AnimationInProgress = false;
        /* Temporary remove the edit parameters button
        Grid EditParametersButton = null; 
        */
        Grid EditTextButton = null;
        Grid EditBackgroundButton = null;
        Grid DeleteButton = null;
        Grid TheItemHolder = null;
        Grid TheEventsHolder = null;
        public ShortcutItem()
        {
            //this will hold the main children  Take a Look AT The visual logic tree 
            Grid Grid1 = new Grid();
            //this will hold the events and the visual elements
            Grid Grid2 = new Grid();
            TheEventsHolder = Grid2;
            TheEventsHolder.Background = Brushes.Transparent;
            TheItemHolder = Grid1;
            TheItemHolder.Children.Add(TheEventsHolder);
            TheEventsHolder.MouseLeftButtonUp += TheEventsHolder_MouseLeftButtonUp;
            TheItemHolder.Width = Width;
            TheItemHolder.Height = Height;
            Children.Add(TheItemHolder);
            MouseLeftButtonDown += Cnv_MouseLeftButtonDown;
            MouseMove += Cnv_MouseMove;
           
            //Responsible for the Appearance of edit Buttons
            MouseEnter += ShortcutItem_MouseEnter;
            MouseLeave += ShortcutItem_MouseLeave;
            Cursor = Cursors.Hand;
        }

        

        public ShortcutItem(string file, string IconSource = "Default") : this()
        {
            ShortCutLocation = System.IO.Path.GetExtension(file).ToLower() == ".lnk" ? LayoutObjects.GetOriginalFileURL(file) : file;
            // FileName = LayoutObjects.CreateTextBlock(System.IO.Path.GetFileNameWithoutExtension(ShortCutLocation));
            FileName_beta = CreateTextBox(System.IO.Path.GetFileNameWithoutExtension(ShortCutLocation));
            LoadGeneralDesign();
            //Check if a directory
            try
            {
                //set the default folder Image
                if (((FileAttributes)System.IO.File.GetAttributes(file)).HasFlag(FileAttributes.Directory))
                {
                    //CreateIconFromImage("pack://application:,,,/Resources/Folder_Icon.png");
                    ShortcutIcon.Source = LayoutObjects.GetImageSource("pack://application:,,,/Resources/Folder_Icon.png");
                    LoadFolderDesign();
                }
                //else set the the Default Design
                else
                {
                    if (IconSource == "Default")
                    {
                        LoadDefaultDesign();
                    }
                    else LoadCustomDesign(IconSource);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("A file is missing\nCan't find " + file);
                IsThereisErrors = true;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Directory is missing\n Cant find " + file);
                IsThereisErrors = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error happened while adding " + file + " \nMay save File corrupted or no permission to access this file\ndeleted shortcut from DM\nerror: " + e.Message);
                IsThereisErrors = true;
            }
            SizeChanged += ShortcutItem_SizeChanged;
            return;
        }
        private void LoadFolderDesign()
        {
            ShortcutIcon.Stretch = Stretch.Fill;
            ShortcutIcon.Width = 50;
            ShortcutIcon.Height = 50;
        }
        private void LoadDefaultDesign()
        {
            IconSourceLocation = "Default";
            //add them to the holder
            Viewbox viewbox = new Viewbox();
            TextBlock tb = LayoutObjects.CreateTextBlock(System.IO.Path.GetFileNameWithoutExtension(ShortCutLocation));
            tb.Foreground = new SolidColorBrush((System.Windows.Media.Color)
                System.Windows.Media.ColorConverter.ConvertFromString
                (AppTheme.Foreground));
            viewbox.Child = tb;
            TheEventsHolder.Children.Add(viewbox);
        }
        //Load The Image which The User Have Choosen
        private void LoadCustomDesign(string ImageLocation)
        {
            if (System.IO.Path.GetExtension(ImageLocation).ToUpper() == ".ICO" || System.IO.Path.GetExtension(ImageLocation).ToUpper() == ".EXE")
            {
                ChangeImage(LayoutObjects.GetIcon(ImageLocation));
            }
            else
            {
                ChangeImage(LayoutObjects.GetImageSource(ImageLocation));
            }
        }
        //General Design
        private void LoadGeneralDesign()
        {
            //Add Transperent Image so user be able to change it later
            ShortcutIcon = new Image();
            ShortcutIcon.Stretch = Stretch.Fill;
            TheEventsHolder.Children.Add(ShortcutIcon);
            FileName_beta.TextAlignment = TextAlignment.Left;
            FileName_beta.VerticalAlignment = VerticalAlignment.Bottom;
            FileName_beta.Foreground = new SolidColorBrush((System.Windows.Media.Color)
                System.Windows.Media.ColorConverter.ConvertFromString
                (AppTheme.Foreground));
            Canvas canv = new Canvas();
            //prevent the overflow of elements == overflow=hidden in html
            TheItemHolder.ClipToBounds = true;
            TheItemHolder.Children.Add(canv);
            //>>>>>>>Delete Button Section
            Grid DelButton_g = CreateRoundButton("\xE10A", 1);
            DelButton_g.MouseLeftButtonUp += DelButton_g_MouseLeftButtonUp;
            DeleteButton = DelButton_g;
            canv.Children.Add(DelButton_g);
            //<<<<<<<<<<<
            //>>>>>>>Edit Background Button Section
            Grid EditBackgroundButton_g = CreateRoundButton("\xE7B5", 2);
            EditBackgroundButton_g.MouseLeftButtonUp += EditBackgroundButton_g_MouseLeftButtonUp;
            EditBackgroundButton = EditBackgroundButton_g;
            canv.Children.Add(EditBackgroundButton_g);
            //<<<<<<<<<<
            //>>>>>>>>>> Edit Text Section
            Grid EditTextButton_g = CreateRoundButton("\xE70F", 3);
            EditTextButton_g.MouseLeftButtonUp += EditTextButton_g_MouseLeftButtonUp;
            FileName_beta.LostFocus += FileName_beta_LostFocus;
            EditTextButton = EditTextButton_g;
            canv.Children.Add(EditTextButton_g);
            //<<<<<<<<<<
            //>>>>>>>>>>EditParameters
            /* EditParameters
            Grid EditParametersButton_g = CreateRoundButton("\xE78B", 4);
            EditParametersButton_g.MouseLeftButtonUp += EditParametersButton_g_MouseLeftButtonUp;
            EditParametersButton = EditParametersButton_g;
            canv.Children.Add(EditParametersButton);
            */
            //<<<<<<<<<<
            TheEventsHolder.Children.Add(FileName_beta);
        }

        private void FileName_beta_LostFocus(object sender, RoutedEventArgs e)
        {
            Data.SaveShortcuts(Shortcuts.ShortcutItems);
        }

        private Grid CreateRoundButton(string Text, int Order)
        {
            Grid button = new Grid();
            button.Width = 20;
            button.Height = 20;
            
            TextBlock tb = new TextBlock();
            Panel.SetZIndex(tb, 20);
            tb.Text = Text;
            tb.FontFamily = new System.Windows.Media.FontFamily("Segoe MDL2 Assets");
            tb.TextAlignment = TextAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            Ellipse DelButton = LayoutObjects.CreateEllipse();
            button.MouseEnter += (sender, e) => Button_MouseEnter(DelButton);
            button.MouseLeave += (sender, e) => Button_MouseLeave(DelButton);
            button.Children.Add(DelButton);
            button.Children.Add(tb);
            SetTop(button, DelButton.Height * -1);
            SetLeft(button, TheItemHolder.Width - button.Width * Order - 2 * Order );
            Panel.SetZIndex(button, 10);
            return button;
        }
        private void Button_MouseLeave(Ellipse ell)
        {
            ell.Fill = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#70ffffff"));
        }
        private void Button_MouseEnter(Ellipse ell)
        {
            ell.Fill =  new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#ffffffff"));
        }
        private void EditParametersButton_g_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Edit Parameters!!");
        }
        private void EditTextButton_g_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LayoutObjects.UnSealTextBox(FileName_beta);

        }
        private void EditBackgroundButton_g_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = SupportedIconExtensions;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                IconSourceLocation = dlg.FileName;
                if (System.IO.Path.GetExtension(dlg.FileName).ToUpper() == ".ICO" || System.IO.Path.GetExtension(dlg.FileName).ToUpper() == ".EXE")
                {
                    ChangeImage(LayoutObjects.GetIcon(dlg.FileName));
                }
                else
                {
                    ChangeImage(LayoutObjects.GetImageSource(dlg.FileName));
                }
                //Remove the Text Before Replacing it with an Image
                foreach (object child in TheEventsHolder.Children)
                {
                    if (child is Viewbox)
                    {
                        TheEventsHolder.Children.Remove(child as UIElement);
                        break;
                    }
                }
                Data.SaveShortcuts(Shortcuts.ShortcutItems);
            }  
            }
        private void DelButton_g_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Shortcuts.RemoveChild(this);
            Data.SaveShortcuts(Shortcuts.ShortcutItems);
        }
        private void ShortcutItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TheItemHolder.Width = e.NewSize.Width;
            TheItemHolder.Height = e.NewSize.Height;
            SetLeft(DeleteButton, e.NewSize.Width - DeleteButton.Width - 2);
            SetLeft(EditBackgroundButton, e.NewSize.Width - (EditBackgroundButton.Width) * 2 - 2 * 2);
            SetLeft(EditTextButton, e.NewSize.Width - (EditTextButton.Width) * 3 - 2 * 3);
            /* EditParameters
            SetLeft(EditParametersButton, e.NewSize.Width - (EditParametersButton.Width) * 4 - 2 * 4);
            */
        }
        private void TheEventsHolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StartProccess();
        }

        //Update 1
        private void Cnv_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
        //Update 1
        private void Cnv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private async void ShortcutItem_MouseEnter(object sender, MouseEventArgs e)
        {
            //>>>>>From (-) Top to Middle Animation
            AnimationInProgress = true; //to prevent the mouseleave Animation till this Animation finishes
            DoubleAnimation an = new DoubleAnimation();
            an.From = GetTop(DeleteButton);
            an.To = DeleteButton.Height;
            an.Duration = new Duration(TimeSpan.FromMilliseconds(150));
            DeleteButton.BeginAnimation(TopProperty,an, HandoffBehavior.SnapshotAndReplace);
            await MainWindow.sleep(75);
            EditBackgroundButton.BeginAnimation(TopProperty, an, HandoffBehavior.SnapshotAndReplace);
            await MainWindow.sleep(75);
            EditTextButton.BeginAnimation(TopProperty, an, HandoffBehavior.SnapshotAndReplace);
            /* EditParameters
            await MainWindow.sleep(66);
            EditParametersButton.BeginAnimation(TopProperty, an, HandoffBehavior.SnapshotAndReplace);
            */
            //<<<<<<<
            //>>>>>>From Middle To (zero) Top
            an.From = GetTop(DeleteButton);
            an.To = 2;
            an.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            DeleteButton.BeginAnimation(TopProperty,an, HandoffBehavior.SnapshotAndReplace);
            await MainWindow.sleep(75);
            EditBackgroundButton.BeginAnimation(TopProperty, an, HandoffBehavior.SnapshotAndReplace);
            await MainWindow.sleep(75);
            EditTextButton.BeginAnimation(TopProperty, an, HandoffBehavior.SnapshotAndReplace);
            /*
            await MainWindow.sleep(66);
            EditParametersButton.BeginAnimation(TopProperty, an, HandoffBehavior.SnapshotAndReplace);
            */
            //<<<<<<<<<
            AnimationInProgress = false;
        }
        private async void ShortcutItem_MouseLeave(object sender, MouseEventArgs e)
        {
            while (AnimationInProgress)
            {
                await MainWindow.sleep(5);
            }
            DoubleAnimation an = new DoubleAnimation();
            an.From = GetTop(DeleteButton);
            an.To = DeleteButton.Height * -1;
            an.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            DeleteButton.BeginAnimation(TopProperty, an, HandoffBehavior.SnapshotAndReplace);
            await MainWindow.sleep(100);
            EditBackgroundButton.BeginAnimation(TopProperty, an, HandoffBehavior.SnapshotAndReplace);
            await MainWindow.sleep(100);
            EditTextButton.BeginAnimation(TopProperty, an, HandoffBehavior.SnapshotAndReplace);
            /* EditParameters
            await MainWindow.sleep(100);
            EditParametersButton.BeginAnimation(TopProperty, an, HandoffBehavior.SnapshotAndReplace);
            */
        }
    }
}
