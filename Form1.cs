using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms.VisualStyles;

namespace Gol
{
    public partial class Form1 : Form
    {
        private Point[][] points = new Point[100][];
        private float stepx;
        private float stepy;
        private Dictionary<string, Point> liveOnes = new Dictionary<string, Point>();
        private Dictionary<string, Point> toKill = new Dictionary<string, Point>();
        private Dictionary<string, Point> toBreed = new Dictionary<string, Point>();


        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Point[100];

                for (int y = 0; y < points[i].Length; y++)
                {
                    Point p = new Point();
                    p.x = i;
                    p.y = y;
                    points[i][y] = p;

                }
            }

            points[3][3].alive = true;
            liveOnes.Add(points[3][3].getAddress(), points[3][3]);
            

        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); 
            


            Graphics g = e.Graphics;

            g.FillRectangle(Brushes.White, 0, 0, this.Width, this.Height);
            

            float curx = 0;
            float cury = 0;

            for (int i = 0; i < points.Length; i++) 
            {
                g.DrawLine(Pens.Gray, new PointF(0, cury), new PointF(this.Width, cury));
                g.DrawLine(Pens.Gray, new PointF(curx, 0), new PointF(curx, this.Height));
                curx += stepx;
                cury += stepy;
            }

            curx = 0;
            cury = 0;
            
            for (int i = 0; i < points.Length; i++)
            {
                for (int j = 0; j < points.Length; j++) 
                {
                    if (points[i][j].alive)
                    {
                        float boxx = i * stepx;
                        float boxy = j * stepy;
                        g.FillRectangle(Brushes.Black, boxx, boxy, stepx, stepy);
                    }
                }

            }


        }
        
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            MouseEventArgs arg = (MouseEventArgs)e;


            if (arg != null )
            {
                int x = (int)((float)arg.X /stepx);
                int y = (int)((float)arg.Y /stepy);
                string address = points[x][y].getAddress();

                if(arg.Button == MouseButtons.Left)
                {
                    if (x >= 0 && x <= 100 && y >= 0 && y <= 100)
                    {
                        points[x][y].alive = !points[x][y].alive;
                        

                        if (points[x][y].alive)
                        {                            
                            liveOnes.Add(address, points[x][y]);
                        }
                        else
                        {
                            liveOnes.Remove(address);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(aliveNeighbors(x, y).ToString());
                }
                this.Refresh();
            }
           
        }

        private void kill()
        {
            foreach (Point p in toKill.Values)
            {
                p.alive = false;
                liveOnes.Remove(p.getAddress());
            }
        }

        private void breed()
        {
            foreach (Point p in toBreed.Values)
            {
                p.alive = true;
                if (!liveOnes.ContainsKey(p.getAddress()))
                {
                    liveOnes.Add(p.getAddress(), p);
                }
                
            }
        }

        private void step()
        {
            Dictionary<string, Point> deaders = new Dictionary<string, Point>();
            

            foreach (Point p in liveOnes.Values)
            {
                int n = aliveNeighbors(p.x, p.y);
                
                if (n < 2)
                {
                    toKill.Add(p.getAddress(), p);
                    
                    foreach(Point p2 in getDeadNeighbors(p.x, p.y))
                    {
                        if(!deaders.ContainsKey(p2.getAddress())) 
                        {
                            deaders.Add(p2.getAddress(), p2);
                        }
                        
                    }
                }
            }


            foreach (Point p in liveOnes.Values)
            {
                int n = aliveNeighbors(p.x, p.y);

                if (n < 2)
                {
                    if(!toKill.ContainsKey(p.getAddress()))
                    {
                        toKill.Add(p.getAddress(), p);
                    }
                    
                }
            }


            foreach (Point p in liveOnes.Values)
            {
                int n = aliveNeighbors(p.x, p.y);

                if (n > 3)
                {
                    if(!toKill.ContainsKey(p.getAddress()))
                    {
                        toKill.Add(p.getAddress(), p);
                    }
                    
                }
            }

            foreach(Point p in deaders.Values)
            {
                if(aliveNeighbors(p.x, p.y) == 3)
                {                    
                    toBreed.Add(p.getAddress(), p);
                }
            }

            kill();
            breed();

            
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if(e.KeyCode == Keys.Space)
            {
                step();
                this.Refresh();
            }
        }

        private int aliveNeighbors(int x, int y)
        {
            int alive = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if(i >= 0 && i <= 100 && j >= 0 && j <= 100 && !(i == x && y == j))
                    {
                        if (points[i][j].alive)
                        {
                            ++alive;
                        }
                    }
                }
            }
            return alive;
        }

        private List<Point> getDeadNeighbors(int x, int y)
        {
            List<Point> ret = new List<Point>();

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i <= 100 && j >= 0 && j <= 100 && !(i == x && y == j))
                    {
                        if (!points[i][j].alive)
                        {
                            ret.Add(points[i][j]);
                        }
                    }
                }
            }
            return ret;
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            stepx = this.Width / 100.0f;
            stepy = this.Height / 100.0f;
        }
    }
}
