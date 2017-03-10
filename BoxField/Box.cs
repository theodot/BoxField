using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxField
{
    class Box
    {
        public int x, y;

        /// <summary>
        /// Constructor method for a box
        /// </summary>
        /// <param name="_x">x postion on screen</param>
        /// <param name="_y">y position on screen</param>
        public Box (int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}
