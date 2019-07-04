using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project4_Ivashchenko
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bitmap;

            listOfVertices = new List<Point>();
            linesPoints = new List<Point>();
            //EdgeTable = new List<Edge>();

            comboBox1.Items.Insert(0, "Red");
            comboBox1.Items.Insert(1, "Blue");
            comboBox1.Items.Insert(2, "Green");
        }

        Bitmap bitmap;
        List<Point> listOfVertices;
        List<Point> linesPoints;
        //List<Edge> EdgeTable;

        int X, Y;

        struct _line
        {
            public int x1, y1;
            public int x2, y2;
        };

        int roundThis(int a)
        {
            return (int)(a + 0.5);
        }

        int xmax, xmin, ymax, ymin;
        Pen pen = new Pen(Color.Black, 1);
        Pen pen1 = new Pen(Color.Red, 2);

        void CyrusBeck(_line a)
        {
            int[] p = new int[4];
            int[] q = new int[4];
            int i, dx, dy, flag = 1;
            double u1 = 0, u2 = 1, temp;
            dx = a.x2 - a.x1;
            dy = a.y2 - a.y1;
            p[0] = -dx; q[0] = a.x1 - xmin;
            p[1] = dx; q[1] = xmax - a.x1;
            p[2] = -dy; q[2] = a.y1 - ymin;
            p[3] = dy; q[3] = ymax - a.y1;

            if (p[0] == 0 && p[3] == 0) //point clipping
            {
                if (a.x1 >= xmin && a.x1 <= xmax && a.y1 >= ymin && a.y1 <= ymax)
                    putPixel(a.x1, a.y1, Color.Red);
                else
                    return;
            }

            if (p[0] == 0)
                if (q[0] * q[1] <= 0)
                    return; //for parallel lines
            if (p[2] == 0)
                if (q[2] * q[3] <= 0)
                    return;

            for (i = 0; i < 4; i++)
            {
                if (p[i] < 0 && flag == 1)
                {
                    temp = (double)q[i] / (double)p[i];
                    if (temp > u2)
                        flag = 0;
                    else
                    if (temp > u1)
                        u1 = temp;
                }
                else
                if (p[i] > 0 && flag == 1)
                {
                    temp = (double)q[i] / (double)p[i];
                    if (temp < u1)
                        flag = 0;
                    else
                    if (temp < u2)
                        u2 = temp;
                }
            }
            if (u1 >= u2 || flag == 0)
                return;
            temp = a.x1;
            i = a.y1;
            a.x1 = (int)(temp + u1 * dx);
            a.x2 = (int)(temp + u2 * dx);
            a.y1 = (int)(i + u1 * dy);
            a.y2 = (int)(i + u2 * dy);

            pictureBox1.Image = bitmap;
            Graphics g = Graphics.FromImage(bitmap);
            
            g.DrawLine(pen1,roundThis(a.x1), roundThis(a.y1), roundThis(a.x2), roundThis(a.y2));
        }

        void drawWindow(int xmin, int ymin, int xmax, int ymax)
        {
            pictureBox1.Image = bitmap;
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawLine(pen,xmin,ymax,xmax, ymax);
            g.DrawLine(pen,xmax, ymax, xmax, ymin);
            g.DrawLine(pen,xmax, ymin, xmin, ymin);
            g.DrawLine(pen, xmin,  ymin, xmin, ymax);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                xmin = 100; ymin = 100; xmax = 250; ymax = 240;

                Graphics g = Graphics.FromImage(bitmap);

                drawWindow(xmin, ymin, xmax, ymax);
                for (int i = 0; i < aLine.Length - 1; i++)
                {
                    CyrusBeck(aLine[i]);
                }
                pictureBox1.Image = bitmap;
            }
            else
                return;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Image);
            X = ((MouseEventArgs)e).X;
            Y = ((MouseEventArgs)e).Y;

            label1.Text = "x: " + X;
            label2.Text = "y: " + (pictureBox1.Height - Y);

            putPixel(X, pictureBox1.Height - Y, Color.Black);
            listOfVertices.Add(new Point(X, pictureBox1.Height - Y));

            linesPoints.Add(new Point(X, Y));
            pictureBox1.Image = bitmap;

            if(checkBox3.Checked)
            {
                Point p = new Point(X, Y);
                var color = comboBox1.Text;
                if (color == "Blue")
                    FloodFill(bitmap, p, bitmap.GetPixel(X,Y), Color.Blue);
                if (color == "Red")
                    FloodFill(bitmap, p, bitmap.GetPixel(X, Y), Color.Red);
                if (color == "Green")
                    FloodFill(bitmap, p, bitmap.GetPixel(X, Y), Color.YellowGreen);
            }


        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label3.Text = "[" + e.X + ";" + (pictureBox1.Height - e.Y) + "]";
        }

        private void putPixel(int x, int y)
        {
            if (x < 0) x = 0;
            if (x >= bitmap.Width) x = bitmap.Width - 1;
            if (y < 0) y = 0;
            if (y >= bitmap.Height) y = bitmap.Height - 1;

            bitmap.SetPixel(x, bitmap.Height - 1 - y, Color.Black);
        }

        private void putPixel(int x, int y, Color color)
        {
            if (x < 0) x = 0;
            if (x >= bitmap.Width) x = bitmap.Width - 1;
            if (y < 0) y = 0;
            if (y >= bitmap.Height) y = bitmap.Height - 1;

            bitmap.SetPixel(x, bitmap.Height - 1 - y, color);

        }

        private void SymmetricLineHigh(int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int xi = 1;
            if (dx < 0)
            {
                dx = -dx;
                xi = -1;
            }
            int D = 2 * dx - dy;

            int xf = x0, yf = y0;
            int xb = x1, yb = y1;
            putPixel(xf, yf);
            putPixel(xb, yb);
            while (yf < yb)
            {
                yf++; yb--;

                if (D > 0)
                {
                    xf += xi; xb -= xi;
                    D = D - 2 * dy;
                }
                D = D + 2 * dx;
                putPixel(xf, yf);
                putPixel(xb, yb);

            }
        }
        private void SymmetricLineLow(int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int yi = 1;

            if (dy < 0)
            {
                dy = -dy;
                yi = -1;
            }
            int D = 2 * dy - dx;

            int xf = x0, yf = y0;
            int xb = x1, yb = y1;

            putPixel(xf, yf);
            putPixel(xb, yb);

            while (xf < xb)
            {
                xf++; xb--;

                if (D > 0)
                {
                    yf += yi; yb -= yi;
                    D = D - 2 * dx;
                }
                D = D + 2 * dy;
                putPixel(xf, yf);
                putPixel(xb, yb);
            }
        }
        private void SymmetricLine(int x1, int y1, int x2, int y2, bool isDecard)
        {
            if (!isDecard)
            {
                y1 = pictureBox1.Height - y1;
                y2 = pictureBox1.Height - y2;
            }

            if (Math.Abs(y2 - y1) < Math.Abs(x2 - x1))
                if (x1 > x2)
                    SymmetricLineLow(x2, y2, x1, y1);
                else
                    SymmetricLineLow(x1, y1, x2, y2);
            else
                if (y1 > y2)
                SymmetricLineHigh(x2, y2, x1, y1);
            else
                SymmetricLineHigh(x1, y1, x2, y2);

            pictureBox1.Image = bitmap;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //if filling mode is on, it ignores less that 3 points
            if (checkBox1.Checked)
            {
                if (listOfVertices.Count < 3) return;
            }


            for (int i = 1; i < listOfVertices.Count; i++)
            {
                float dx = listOfVertices[i - 1].X - listOfVertices[i].X;
                float dy = listOfVertices[i - 1].Y - listOfVertices[i].Y;

                SymmetricLine(listOfVertices[i - 1].X, listOfVertices[i - 1].Y, listOfVertices[i].X, listOfVertices[i].Y, true);

                int ymax = Math.Max(listOfVertices[i - 1].Y, listOfVertices[i].Y);
                int ymin = Math.Min(listOfVertices[i - 1].Y, listOfVertices[i].Y);
                if (ymin == ymax) continue;
                float xmin = (ymin == listOfVertices[i - 1].Y) ? listOfVertices[i - 1].X : listOfVertices[i].X;

                Edge edge = new Edge(ymax, ymin, xmin, dx / dy, null);

                //EdgeTable.Add(edge);

            }
            SymmetricLine(listOfVertices[0].X, listOfVertices[0].Y, listOfVertices[listOfVertices.Count - 1].X, listOfVertices[listOfVertices.Count - 1].Y, true);

            int ymax_last = Math.Max(listOfVertices[0].Y, listOfVertices[listOfVertices.Count - 1].Y);
            int ymin_last = Math.Min(listOfVertices[0].Y, listOfVertices[listOfVertices.Count - 1].Y);
            float xmin_last = (ymin_last == listOfVertices[0].Y) ? listOfVertices[0].X : listOfVertices[listOfVertices.Count - 1].X;
            Edge last_edge = new Edge(ymax_last,
                                      ymin_last,
                                      xmin_last,
                    ((float)(listOfVertices[listOfVertices.Count - 1].X - listOfVertices[0].X)) / ((float)(listOfVertices[listOfVertices.Count - 1].Y - listOfVertices[0].Y)),
                    null);

            //if (last_edge.ymin != last_edge.ymax)
            //    EdgeTable.Add(last_edge);

            //for (int i = 0; i < EdgeTable.Count; i++)
            //    EdgeTable[i].next = EdgeTable[(i + 1) % EdgeTable.Count];


            pictureBox1.Image = bitmap;
            //listOfVertices.Clear();
        }


        List<Edge> ActiveEdgeTable = new List<Edge>();
        private void fillPolygon()
        {
            int k = 0;
            int N = listOfVertices.Count;

            int[] indices = new int[N];
            for (int j = 0; j < N - 1; j++)
                indices[j] = j;

            int i = indices[k];

            int y = ymin = listOfVertices[indices[0]].Y;
            int ymax = listOfVertices[indices[N - 1]].Y;

            while( y < ymax)
            {

                while(listOfVertices[i].Y == y)
                {
                    if (listOfVertices[i - 1].Y > listOfVertices[i].Y)
                    {
                        //ActiveEdgeTable.Add(listOfVertices[i], listOfVertices[i - 1]);

                        float dx = listOfVertices[i - 1].X - listOfVertices[i].X;
                        float dy = listOfVertices[i - 1].Y - listOfVertices[i].Y;

                        //int ymaxx = Math.Max(listOfVertices[i - 1].Y, listOfVertices[i].Y);
                        //int yminn = Math.Min(listOfVertices[i - 1].Y, listOfVertices[i].Y);
                        if (ymin == ymax) continue;
                        float xmin = (ymin == listOfVertices[i - 1].Y) ? listOfVertices[i - 1].X : listOfVertices[i].X;

                        Edge edge = new Edge(ymax, ymin, xmin, dx / dy, null);
                        ActiveEdgeTable.Add(edge);

                            
                    }

                    if (listOfVertices[i + 1].Y > listOfVertices[i].Y)
                    {
                        //ActiveEdgeTable.Add(listOfVertices[i], listOfVertices[i + 1]);
                        float dx = listOfVertices[i + 1].X - listOfVertices[i].X;
                        float dy = listOfVertices[i + 1].Y - listOfVertices[i].Y;

                        //int ymax = Math.Max(listOfVertices[i + 1].Y, listOfVertices[i].Y);
                        //int ymin = Math.Min(listOfVertices[i + 1].Y, listOfVertices[i].Y);
                        if (ymin == ymax) continue;
                        float xmin = (ymin == listOfVertices[i + 1].Y) ? listOfVertices[i + 1].X : listOfVertices[i].X;

                        Edge edge = new Edge(ymax, ymin, xmin, dx / dy, null);
                        ActiveEdgeTable.Add(edge);
                    }

                    k++;
                    i = indices[k];
                }

                //sort AET by x value
                EdgeCompare ec = new EdgeCompare();
                ActiveEdgeTable.Sort(ec);

                //fill pixels between pairs of intersections
                for (int j = 0; 2 * j + 1 < ActiveEdgeTable.Count; j++)
                    SymmetricLine((int)Math.Round(ActiveEdgeTable[2 * j].xmin), y, (int)Math.Round(ActiveEdgeTable[2 * j + 1].xmin), y, true);

                //increment y
                ++y;

                //remove from AET edges for which y = ymax
                for (int j = 0; j < ActiveEdgeTable.Count; j++)
                    if (y >= ActiveEdgeTable[j].ymax)
                        ActiveEdgeTable.Remove(ActiveEdgeTable[j--]);

                if (y >= 100000)
                {
                    MessageBox.Show("Y > 100 000");
                    break;
                }

                //for each edge in AET x += 1/m
                foreach (Edge edge in ActiveEdgeTable)
                    edge.xmin += edge.w;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                fillPolygon();

                //listOfVertices.Clear();
                //EdgeTable.Clear();
            }
            else
                return;
        }

        _line[] aLine;
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int c = 0;

            if (e.Button == MouseButtons.Right && checkBox2.Checked)
            {
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                for (int i = 0; i < linesPoints.Count -1;i += 2)
                {
                    g.DrawLine(pen, linesPoints[i].X, linesPoints[i].Y, linesPoints[i + 1].X, linesPoints[i + 1].Y);
                    c++;
                }
                aLine = new _line[c];

                for(int i = 0;i< aLine.Length -1; i++)
                {
                    for (int j = 0; j < linesPoints.Count -1; j += 2)
                    {
                        aLine[i].x1 = linesPoints[j].X;
                        aLine[i].y1 = linesPoints[j].Y;
                        aLine[i].x2 = linesPoints[j + 1].X;
                        aLine[i].y2 = linesPoints[j + 1].Y;
                    }
                }
            }

        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            checkBox1.Checked = checkBox2.Checked = checkBox3.Checked = false;
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bitmap;
            listOfVertices.Clear();
            linesPoints.Clear();
            //EdgeTable.Clear();
            label1.Text = "x:";
            label2.Text = "y:";
        }

        private void FloodFill(Bitmap bmp, Point pt, Color targetColor, Color replacementColor)
        {
            Stack<Point> pixels = new Stack<Point>();
            targetColor = bmp.GetPixel(pt.X, pt.Y);
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Point a = pixels.Pop();
                if (a.X < bmp.Width && a.X > 0 &&
                        a.Y < bmp.Height && a.Y > 0)
                {

                    if (bmp.GetPixel(a.X, a.Y) == targetColor)
                    {
                        bmp.SetPixel(a.X, a.Y, replacementColor);
                        pixels.Push(new Point(a.X - 1, a.Y));
                        pixels.Push(new Point(a.X + 1, a.Y));
                        pixels.Push(new Point(a.X, a.Y - 1));
                        pixels.Push(new Point(a.X, a.Y + 1));
                    }
                }
            }
            pictureBox1.Refresh();
            return;
        }
    }
}
