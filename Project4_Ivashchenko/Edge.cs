using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4_Ivashchenko
{
    class Edge
    {
        public int ymax;
        public int ymin;
        public float xmin;
        public float w;
        public Edge next;

        public Edge(int ymax, int ymin, float xmin, float w, Edge next)
        {
            this.ymax = ymax;
            this.ymin = ymin;
            this.xmin = xmin;
            this.w = w;
            this.next = next;
        }
    }
}
