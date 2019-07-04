using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4_Ivashchenko
{
    class EdgeCompare : IComparer<Edge>
    {
        public int Compare(Edge e1, Edge e2)
        {
            float delta = e1.xmin - e2.xmin;
            float dif = 0;
            if (delta > dif)
                return 1;
            else if (delta < dif)
                return -1;
            else
                return 0;
        }
    }
}
