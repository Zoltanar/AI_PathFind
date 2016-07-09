using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AI_PathFind
{
    public partial class Form1 : Form
    {
        //adjustable variables
        private static int timerMS = 50; //time between timer ticks
        private static int replyMS = 1500; //time before reply disappears
        private static int numOfCells = 10; //cells per row/column (do not change, not implemented)

        //variables that change
        private static bool clickBool = false;
        private static bool pfkBool = false;
        private static bool found = false;
        private static bool deadEnd = false;
        private static Point mouseLoc;
        private static List<Rectangle> redSquares = new List<Rectangle>();
        private static List<Rectangle> yellowSquares = new List<Rectangle>();
        private static List<Point> directions = new List<Point>();
        private static List<Point> directions2 = new List<Point>();
        private static List<Point> dePath = new List<Point>();
        private static List<Point> path = new List<Point>();
        private static Rectangle start = new Rectangle();
        private static Rectangle end = new Rectangle();
        private static Rectangle current = new Rectangle();
        private static Random rnd = new Random();

        //variables that don't change
        private static Pen blackpen = new Pen(Color.Black, 3);
        private static Pen greenpen = new Pen(Color.Green, 6);
        private static Pen orangepen = new Pen(Color.Orange, 6);
        private static SolidBrush redBrush = new SolidBrush(Color.Red);
        private static SolidBrush blueBrush = new SolidBrush(Color.Blue);
        private static SolidBrush greenBrush = new SolidBrush(Color.Green);
        private static SolidBrush yellowBrush = new SolidBrush(Color.Yellow);
        private static SolidBrush orangeBrush = new SolidBrush(Color.Orange);
        private static Regex regex = new Regex(@"\d+,\d+");

        public Form1()
        {
            InitializeComponent();
            foundInLabel.Text = "";
            mouseCoordLabel.Text = "";
            sDistanceNumber.Text = "";
            replyLabel.Text = "";

            Size size = new Size(panel.Width / numOfCells, panel.Width / numOfCells);
            Size size2 = new Size(panel.Width / numOfCells / 2, panel.Width / numOfCells / 2);

#if DEBUG
            //Testing Variables
            double sX = (double)0 / numOfCells * panel.Width;
            double sY = (double)0 / numOfCells * panel.Height;
            double eX = (double)5 / numOfCells * panel.Width;
            double eY = (double)5 / numOfCells * panel.Height;
            Point startP = new Point((int)sX, (int)sY);
            Point endP = new Point((int)eX, (int)eY);
            start = new Rectangle(startP, size);
            current = start;
            current.Size = size2;
            yellowSquares.Add(current);
            current.Size = size;
            end = new Rectangle(endP, size);
#endif
            createWalls();
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            //rectangle sizes
            Size size = new Size(panel.Width / numOfCells, panel.Width / numOfCells);
            Size size2 = new Size(panel.Width / numOfCells / 2, panel.Width / numOfCells / 2);
            Size size3 = new Size(panel.Width / numOfCells / 4, panel.Width / numOfCells / 4);
            
            //distance between grid points
            double offX2 = (double)panel.Width / numOfCells;
            double offY2 = (double)panel.Height / numOfCells;
            double halfXPoint = offX2 / 2;
            double halfYPoint = offY2 / 2;
            double thirdXPoint = offX2 / 3;
            double thirdYPoint = offY2 / 3;

            Graphics g = e.Graphics;

            if (clickBool) //add obstacle square to collection upon click
            {
                //location of square must be at edge of grid, this section adjusts the location from the mouse
                var doubleX = (double)mouseLoc.X / panel.Width * 100;
                var doubleY = (double)mouseLoc.Y / panel.Height * 100;
                int mouseX = (((int)(doubleX / 10)) * 10);
                int mouseY = (((int)(doubleY / 10)) * 10);
                doubleX = mouseX * panel.Width / 100;
                doubleY = mouseY * panel.Height / 100;
                Debug.WriteLine("mouseX = " + mouseX + "|| mouseY = " + mouseY);
                Debug.WriteLine("doubleX = " + doubleX + "|| doubleY = " + doubleY);
                mouseLoc.X = (int)doubleX;
                mouseLoc.Y = (int)doubleY;

                Rectangle square = new Rectangle(mouseLoc, size);
                if (redSquares.Exists(x => x.Location == mouseLoc))
                {
                    Debug.WriteLine("a square was found (it was deleted)");
                    redSquares.Remove(square);
                }
                else
                {
                    redSquares.Add(square);
                }
                clickBool = false;
            }
            if (pfkBool && !found && !deadEnd) //find path
            {
                Debug.WriteLine("start location = " + current.Location.ToString());

                //set initial values
                bool left = false;
                bool right = false;
                bool down = false;
                bool up = false;

                //set points for surrounding squares
                Point surrR = new Point(current.X + (int)offX2, current.Y);
                Point surrL = new Point(current.X - (int)offX2, current.Y);
                Point surrD = new Point(current.X, current.Y + (int)offY2);
                Point surrU = new Point(current.X, current.Y - (int)offY2);

                //clear old directions before adding new ones to list
                directions.Clear();
                if (end.X > current.X)
                {
                    right = true;
                    directions.Add(surrR);
                }
                if (end.Y > current.Y)
                {
                    down = true;
                    directions.Add(surrD);
                }
                if (end.X < current.X)
                {
                    left = true;
                    directions.Add(surrL);
                }
                if (end.Y < current.Y)
                {
                    up = true;
                    directions.Add(surrU);
                }

                
                foreach (Point p in directions) //delete directions which are obstacles or previous path
                { if (!(yellowSquares.Any(m => m.Location == p) || redSquares.Any(m => m.Location == p))) { directions2.Add(p); } }
                
                if (directions2.Any()) //head to random correct location
                {
                    int rand = rnd.Next(directions2.Count());
                    current.Location = directions2[rand];
                    directions2.Clear();
                }

                else //if no correct locations, head to random square that isn't obstacle or previous path
                {
                    //add all directions
                    directions.Add(surrR);
                    directions.Add(surrL);
                    directions.Add(surrD);
                    directions.Add(surrU);
                    
                    foreach (Point p in directions) //remove obstacles or previous path
                    { if (!(yellowSquares.Any(m => m.Location == p) || redSquares.Any(m => m.Location == p))) { directions2.Add(p); } }
                    if (directions2.Any()) //pick random direction
                    {
                        
                        int rand = rnd.Next(directions2.Count());
                        current.Location = directions2[rand];
                        directions2.Clear();
                    }

                    else //if no available locations, dead end has been reached.
                    {
                        deadEnd = true;
                        foundInLabel.Text = "Reached dead end in " + yellowSquares.Count().ToString() + " moves. \n(Try again)";
                        foreach (Rectangle rect in yellowSquares)
                        {
                            Point tPoint = new Point(rect.X + (int)halfXPoint, rect.Y + (int)halfYPoint);
                            dePath.Add(tPoint);
                        }
                    }
                }

                Debug.WriteLine("final location = " + current.Location.ToString());
                Debug.WriteLine("============================");
                if (current == end) //if you reach the goal
                {
                    found = true;
                    foundInLabel.Text = "Found goal in \n" + yellowSquares.Count() + " moves.";
                    foreach (Rectangle rect in yellowSquares) //draw path taken
                    {
                        Point tPoint = new Point(rect.X + (int)halfXPoint, rect.Y + (int)halfYPoint);
                        path.Add(tPoint);
                    }
                }
                //add to previous path
#if DEBUG
                current.Size = size2;
                yellowSquares.Add(current);
                current.Size = size;
#else
                yellowSquares.Add(current);
#endif
                pfkBool = false;
            }

            //draw tiles and grid
            if (redSquares.Any()) //obstacles
            {
                g.FillRectangles(redBrush, redSquares.ToArray());
            }
#if DEBUG
            if (start != null) //start tile
            {
                g.FillRectangle(blueBrush, start);
            }
            if (yellowSquares.Any()) //previous path
            {
                g.FillRectangles(yellowBrush, yellowSquares.ToArray());
            }
#else
            if (yellowSquares.Any()) //previous path
            {
                g.FillRectangles(yellowBrush, yellowSquares.ToArray());
            }
            if (start != null) //start tile
            {
                g.FillRectangle(blueBrush, start);
            }
#endif
            if (current != null) //current tile
            {
                if (start != end) //only draws if both start and end have been assigned (they should be different)
                {
                    current.Size = size3;
                    current.Offset((int)thirdXPoint, (int)thirdYPoint);
                    g.FillRectangle(orangeBrush, current);
                    current.Offset(-(int)thirdXPoint, -(int)thirdYPoint);
                    current.Size = size;
                }
            }
            if (end != null) //end ile
            {
                g.FillRectangle(greenBrush, end);
            }
            if (path.Any()) //draws path taken
            {
                //make a list of points and draw lines connecting them
                List<Point> fullPath = new List<Point>();
                Point adStart = new Point(start.X + (int)halfXPoint, start.Y + (int)halfYPoint);
                Point adEnd = new Point(end.X + (int)halfXPoint, end.Y + (int)halfYPoint);
                fullPath.Add(adStart);
                fullPath.AddRange(path);
                fullPath.Add(adEnd);
                g.DrawLines(greenpen, fullPath.ToArray());
            }
            if (dePath.Any()) //draws path taken (when end isn't reached)
            {
                //make a list of points and draw lines connecting them
                List<Point> fulldePath = new List<Point>();
                Point adStart = new Point(start.X + (int)halfXPoint, start.Y + (int)halfYPoint);
                fulldePath.Add(adStart);
                fulldePath.AddRange(dePath);
                g.DrawLines(orangepen, fulldePath.ToArray());
            }
            for (int y = 1; y <= numOfCells; y++) //horizontal grid lines
            {
                g.DrawLine(blackpen, 0, (float)y / numOfCells * panel.Height, panel.Width, (float)y / numOfCells * panel.Height);
            }
            for (int x = 1; x <= numOfCells; x++) //vertical grid lines
            {
                g.DrawLine(blackpen, (float)x / numOfCells * panel.Width, 0, (float)x / numOfCells * panel.Width, panel.Height);
            }
        }


        private void panel_MouseClick(object sender, MouseEventArgs e) //get mouse location when it's clicked to create obstacle on location
        {
            mouseLoc = e.Location;
            clickBool = true;
            panel.Invalidate();
        }
        
        private void panel_MouseMove(object sender, MouseEventArgs e) //display coordinates of mouse over panel
        {
#if DEBUG
            mouseCoordLabel.Text = " X: " + e.X + "  Y: " + e.Y;
#endif

        }

        private void startButt(object sender, EventArgs e) //make a new start tile 
        {
            string str = startTB.Text;
            int dot = str.IndexOf('.');
            if (regex.IsMatch(str) & dot == -1) //must match format X,Y
            {
                replyLabel.Text = "";
                //get coords from string and create tile
                string[] coords = str.Split(',');
                double greenx = (double)int.Parse(coords[0]) / numOfCells * panel.Width;
                double greeny = (double)int.Parse(coords[1]) / numOfCells * panel.Height;
                Debug.WriteLine("startx = " + greenx + "|| starty = " + greeny);
                Point point = new Point((int)greenx, (int)greeny);
                Debug.WriteLine("Start Square at: " + point.ToString());
                Size size = new Size(panel.Width / numOfCells, panel.Width / numOfCells);
                Size size2 = new Size(panel.Width / numOfCells / 2, panel.Width / numOfCells / 2);
                start = new Rectangle(point, size);

                //reset variables
                deadEnd = false;
                found = false;
                yellowSquares.Clear();
                path.Clear();
                dePath.Clear();
                current = start;
                current.Size = size2;
                yellowSquares.Add(current);
                current.Size = size;
                panel.Invalidate();

                //write distance in form
                double dX = (double)(start.Location.X - end.Location.X) / panel.Width;
                double dY = (double)(start.Location.Y - end.Location.Y) / panel.Height;
                double sDistance = Math.Sqrt(dX * dX + dY * dY) * numOfCells;
                sDistanceNumber.Text = sDistance.ToString("G4") + " tiles.";
            }
            else //reply if it's not in correct format
            {
                replyTimer.Start();
                replyTimer.Interval = replyMS;
                startTB.Text = "";
                replyLabel.ForeColor = System.Drawing.Color.Red;
                replyLabel.Text = "Coordinates entered must follow the format.";
            }


        }

        private void endButt(object sender, EventArgs e) //make a new end tile
        {
            string str = endTB.Text;
            int dot = str.IndexOf('.');
            if (regex.IsMatch(str) & dot == -1) //must match format X,Y
            {
                replyLabel.Text = "";
                //get coords from string and create tile
                string[] coords = str.Split(',');
                double dgreenx = (double)int.Parse(coords[0]) / numOfCells * panel.Width;
                double dgreeny = (double)int.Parse(coords[1]) / numOfCells * panel.Height;
                Debug.WriteLine("endx = " + dgreenx + "|| endy = " + dgreeny);
                Point point = new Point((int)dgreenx, (int)dgreeny);
                Debug.WriteLine("End Square at: " + point.ToString());
                Size size = new Size(panel.Width / numOfCells, panel.Width / numOfCells);
                Size size2 = new Size(panel.Width / numOfCells / 2, panel.Width / numOfCells / 2);
                end = new Rectangle(point, size);

                //reset variables
                deadEnd = false;
                found = false;
                yellowSquares.Clear();
                path.Clear();
                dePath.Clear();
                current = start;
                current.Size = size2;
                yellowSquares.Add(current);
                current.Size = size;
                panel.Invalidate();

                //write distance in form
                double dX = (double)(start.Location.X - end.Location.X) / panel.Width;
                double dY = (double)(start.Location.Y - end.Location.Y) / panel.Height;
                double sDistance = Math.Sqrt(dX * dX + dY * dY) * numOfCells;
                sDistanceNumber.Text = sDistance.ToString("G4") + " tiles.";
            }
            else //reply if it's not in correct format
            {
                replyTimer.Start();
                replyTimer.Interval = replyMS;
                endTB.Text = "";
                replyLabel.ForeColor = System.Drawing.Color.Red;
                replyLabel.Text = "Coordinates entered must follow the format.";
            }
        }

        private void pfkButt_Click(object sender, EventArgs e) //find path to goal
        {
            if(found || deadEnd) //reset if it has found a path already
            {
                Size size = new Size(panel.Width / numOfCells, panel.Width / numOfCells);
                Size size2 = new Size(panel.Width / numOfCells / 2, panel.Width / numOfCells / 2);
                deadEnd = false;
                found = false;
                yellowSquares.Clear();
                path.Clear();
                dePath.Clear();
                current = start;
                current.Size = size2;
                yellowSquares.Add(current);
                current.Size = size;
            }
            //start timer to automatically repaint panel
            pfkTimer.Start();
            pfkTimer.Interval = timerMS;
            pfkBool = true;
            panel.Invalidate();
        }

        private void startTB_Enter(object sender, KeyPressEventArgs e) //enable enter on textbox
        {
            if (e.KeyChar == (char)13)
            {
                startButt(sender, e);
            }
        }

        private void endTB_Enter(object sender, KeyPressEventArgs e) //enable enter on textbox
        {
            if (e.KeyChar == (char)13)
            {
                endButt(sender, e);
            }
        }

        private void randCoordButt_Click(object sender, EventArgs e) //make random start and end tiles
        {
            //reset variables
            deadEnd = false;
            found = false;
            yellowSquares.Clear();
            path.Clear();
            dePath.Clear();
            Random rCoord = new Random();
            Point startP = new Point();
            Point endP = new Point();
            bool randomDone = false;
            while (!randomDone) //avoid creating start/end tiles on obstacles
            {
                //random points
                randomDone = true;
                double rsX = (double)rCoord.Next(1, numOfCells) / numOfCells * panel.Width;
                double rsY = (double)rCoord.Next(1, numOfCells) / numOfCells * panel.Height;
                double reX = (double)rCoord.Next(1, numOfCells) / numOfCells * panel.Width;
                double reY = (double)rCoord.Next(1, numOfCells) / numOfCells * panel.Height;
                startP = new Point((int)rsX, (int)rsY);
                endP = new Point((int)reX, (int)reY);
                if (redSquares.Any(m => m.Location == startP) || redSquares.Any(m => m.Location == endP))
                {
                    randomDone = false;
                }
            }
            Debug.WriteLine("Random Start: " + startP.ToString());
            Debug.WriteLine("Random End: " + endP.ToString());
            Size size = new Size(panel.Width / numOfCells, panel.Width / numOfCells);
            Size size2 = new Size(panel.Width / numOfCells / 2, panel.Width / numOfCells / 2);
            start = new Rectangle(startP, size);
            //reset more variables
            current = start;
            current.Size = size2;
            yellowSquares.Add(current);
            current.Size = size;
            end = new Rectangle(endP, size);
            foundInLabel.Text = "";
            panel.Invalidate();

            //write distance in form
            double dX = (double)(start.Location.X - end.Location.X) / panel.Width;
            double dY = (double)(start.Location.Y - end.Location.Y) / panel.Height;
            double sDistance = Math.Sqrt(dX * dX + dY * dY) * numOfCells;
            sDistanceNumber.Text = sDistance.ToString("G4") + " tiles.";
        }

        private void pfkTimer_Tick(object sender, EventArgs e) //repaint every tick
        {
            if (found || deadEnd)
            { pfkTimer.Stop(); }
            else
            {
                pfkBool = true;
                panel.Invalidate();
            }
        }

        private void replyTimer_Tick(object sender, EventArgs e) //stop showing reply after tick
        {
            replyLabel.Text = "";
            replyTimer.Stop();
        } 

        private void rndObButt_Click(object sender, EventArgs e) //create random obstacles
        {
            //clear current obstacles before creating creating obstacles covering 20% of the area
            redSquares.Clear();
            createWalls();
            Size size = new Size(panel.Width / numOfCells, panel.Width / numOfCells);
            Random rnd = new Random();        
            for(int i = 1; i <= (int)numOfCells*numOfCells/5; i++)
            {
                double currObX = (double)rnd.Next(1, numOfCells) / numOfCells * panel.Width;
                double currObY = (double)rnd.Next(1, numOfCells) / numOfCells * panel.Height;
                Point ObP = new Point((int)currObX, (int)currObY);
                Rectangle currOb = new Rectangle(ObP, size);
                if (currOb != start && currOb != end) //add if not a start/end spot
                {
                    redSquares.Add(currOb);
                }
                else //go down a number if it was occupied by start/end 
                {
                    i--;
                }
            }
            panel.Invalidate();
        }

        private void createWalls() //create walls of obstacles that delimit the area
        {
            Size size = new Size(panel.Width / numOfCells, panel.Width / numOfCells);
            double lX = (double)-1 / numOfCells * panel.Width;
            double lX2 = (double)10 / numOfCells * panel.Width;
            double lY = (double)-1 / numOfCells * panel.Height;
            double lY2 = (double)10 / numOfCells * panel.Height;
            double offX2 = (double)panel.Width / numOfCells;
            double offY2 = (double)panel.Height / numOfCells;
            Point lP = new Point((int)lX, 0);
            Point lP2 = new Point((int)lX2, 0);
            Point lP3 = new Point(0, (int)lY);
            Point lP4 = new Point(0, (int)lY2);
            Rectangle limit = new Rectangle(lP, size);
            Rectangle limit2 = new Rectangle(lP2, size);
            Rectangle limit3 = new Rectangle(lP3, size);
            Rectangle limit4 = new Rectangle(lP4, size);
            for (int i = 0; i <= 9; i++)
            {
                redSquares.Add(limit);
                limit.Offset(0, (int)offY2);
                redSquares.Add(limit2);
                limit2.Offset(0, (int)offY2);
                redSquares.Add(limit3);
                limit3.Offset((int)offX2, 0);
                redSquares.Add(limit4);
                limit4.Offset((int)offX2, 0);
            }
        }
    }

}
