using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Desktop_Manger
{
    class TileLayout : Panel
    {
        private double tileWidth;
        private double tileHeight;
        private double xSpacing;
        private double ySpacing;
        private List<Tile> children;
        public double TileWidth
        {
            get
            {
                return tileWidth;
            }
            set
            {
                //TODO
            }
        }
        public double TileHeight
        {
            get
            {
                return tileHeight;
            }
            set
            {
                //TODO
            }
        }
        public double XSpacing
        {
            get
            {
                return xSpacing;
            }
            set
            {
                //TODO
            }
        }
        public double YSpacing
        {
            get
            {
                return ySpacing;
            }
            set
            {
                //TODO
            }
        }

        //Constructor
        public TileLayout()
        {
            //TODO
        }

        public void AddTile(Tile tile)
        {
            //TODO
        }

        public void RemoveTile(Tile tile)
        {
            //TODO
        }

    }
}
