using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Manger
{
    class PowerPlan
    {
        public string Name { get; set; }
        public string Id { get; set; }
        //contstactor 
        //see divideplanes funtion to see how to use it
        public PowerPlan(object Parent, string Id, string Name )
        {
            this.Id = Id;
            this.Name = Name;
            //Here create stackpanel 
            // create the textblock 
            //create the image 
        }
    }
}
