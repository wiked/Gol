using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gol
{
    internal class Point
    {
        public int x {  get; set; }
        public int y { get; set; }
        public bool alive { get; set; } = false;  
        
        public string getAddress()
        {
            string address = "";
            if (x < 10)
            {
                address += "0";
            }

            address += x.ToString();

            if (y < 10)
            {
                address += "0";
            }
            address += y.ToString();
            return address;
        }
    }
}
