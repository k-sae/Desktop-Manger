using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Desktop_Manger
{
    class Tile : Grid
    {
        
        public double ChildPanelWidth { get; set; }
        public double ChildPanelHeight { get; set; }
        public int NoOfItemsPerRow { get; set; }
        //Constructor
        public Tile(int NoOfItemsPerRpw)
        {
            this.NoOfItemsPerRow = NoOfItemsPerRpw;
            SetNoOfColumns();
        }
        //Set the Column No
        private void SetNoOfColumns()
        {
            for (int i = 0; i < NoOfItemsPerRow; i++)
            {
                ColumnDefinition clm = new ColumnDefinition();
                clm.Width = new GridLength(1, GridUnitType.Star);
                this.ColumnDefinitions.Add(clm);
            }
           
        }
        // add Child to the current grid
        public void Add(Panel Child)
        {
           
            if (this.Children.Count % NoOfItemsPerRow == 0)
            {
                RowDefinition rwd = new RowDefinition();
                rwd.Height = new GridLength(1, GridUnitType.Auto);
                this.RowDefinitions.Add(rwd);
            }
            Child.VerticalAlignment = VerticalAlignment.Top;
            Tile.SetRow(Child, this.Children.Count / NoOfItemsPerRow);
            Tile.SetColumn(Child, this.Children.Count % NoOfItemsPerRow);
            this.Children.Add(Child);
        }
        public void Remove(Panel Child)
        {
            int Index = this.Children.IndexOf(Child);
            this.Children.Remove(Child);
            for (int i = Index; i < this.Children.Count; i++)
            {
                int col = Tile.GetColumn(this.Children[i]);
                int rw = Tile.GetRow(this.Children[i]);
                if(col == 0 && rw != 0)
                {
                    Tile.SetRow(this.Children[i], rw - 1);
                    Tile.SetColumn(this.Children[i], NoOfItemsPerRow - 1);
                }
                else if(col != 0)
                Tile.SetColumn(this.Children[i], col - 1);
            }
        } 

    }
}
