using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Desktop_Manger
{
    public class PowerPlan
    {
        public string Name { get; set; }
        public string Id { get; set; }
        //contstactor 
        //see divideplanes funtion to see how to use it
        public PowerPlan(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;



            //Here create stackpanel 
            // create the textblock 

        }



    }
}
