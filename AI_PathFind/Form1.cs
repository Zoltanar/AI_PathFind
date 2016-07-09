using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AI_PathFind.Properties;

namespace AI_PathFind
{
    public partial class Form1 : Form
    {
        //adjustable variables
        private static readonly int TimerMS = 50; //time between timer ticks
        private static readonly int ReplyMS = 1500; //time before reply disappears
        private static readonly int NumOfCells = 10; //cells per row/column (do not change, not implemented)

        //variables that change
        private static bool _clickBool;
        private static bool _pfkBool;
        private static bool _found;
        private static bool _deadEnd;
        private static Point _mouseLoc;
        private static readonly List<Rectangle> RedSquares = new List<Rectangle>();
        private static readonly List<Rectangle> YellowSquares = new List<Rectangle>();
        private static readonly List<Point> Directions = new List<Point>();
        private static readonly List<Point> Directions2 = new List<Point>();
        private static readonly List<Point> DePath = new List<Point>();
        private static readonly List<Point> Path = new List<Point>();
        private static Rectangle _start;
        private static Rectangle _end;
        private static Rectangle _current;
        private static readonly Random Rnd = new Random();

        //variables that don't change
        private static readonly Pen Blackpen = new Pen(Color.Black, 3);
        private static readonly Pen Greenpen = new Pen(Color.Green, 6);
        private static readonly Pen Orangepen = new Pen(Color.Orange, 6);
        private static readonly SolidBrush RedBrush = new SolidBrush(Color.Red);
        private static readonly SolidBrush BlueBrush = new SolidBrush(Color.Blue);
        private static readonly SolidBrush GreenBrush = new SolidBrush(Color.Green);
        private static readonly SolidBrush YellowBrush = new SolidBrush(Color.Yellow);
        private static readonly SolidBrush OrangeBrush = new SolidBrush(Color.Orange);
        private static readonly Regex Regex = new Regex(@"\d+,\d+");

        public Form1()
        {
            InitializeComponent();
            foundInLabel.Text = "";
            mouseCoordLabel.Text = "";
            sDistanceNumber.Text = "";
            replyLabel.Text = "";
            CreateWalls();
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            //rectangle sizes
            var size = new Size(panel.Width / NumOfCells, panel.Width / NumOfCells);
            var size3 = new Size(panel.Width / NumOfCells / 4, panel.Width / NumOfCells / 4);

            //distance between grid points
            var offX2 = (double)panel.Width / NumOfCells;
            var offY2 = (double)panel.Height / NumOfCells;
            var halfXPoint = offX2 / 2;
            var halfYPoint = offY2 / 2;
            var thirdXPoint = offX2 / 3;
            var thirdYPoint = offY2 / 3;

            var g = e.Graphics;

            if (_clickBool) //add obstacle square to collection upon click
            {
                //location of square must be at edge of grid, this section adjusts the location from the mouse
                var doubleX = (double)_mouseLoc.X / panel.Width * 100;
                var doubleY = (double)_mouseLoc.Y / panel.Height * 100;
                var mouseX = (int)(doubleX / 10) * 10;
                var mouseY = (int)(doubleY / 10) * 10;
                doubleX = Math.Floor(mouseX * panel.Width / 100f);
                doubleY = Math.Floor(mouseY * panel.Height / 100f);
                _mouseLoc.X = (int)doubleX;
                _mouseLoc.Y = (int)doubleY;

                var square = new Rectangle(_mouseLoc, size);
                if (RedSquares.Exists(x => x.Location == _mouseLoc))
                {
                    RedSquares.Remove(square);
                }
                else
                {
                    RedSquares.Add(square);
                }
                _clickBool = false;
            }
            if (_pfkBool && !_found && !_deadEnd) //find path
            {
                //set points for surrounding squares
                var surrR = new Point(_current.X + (int)offX2, _current.Y);
                var surrL = new Point(_current.X - (int)offX2, _current.Y);
                var surrD = new Point(_current.X, _current.Y + (int)offY2);
                var surrU = new Point(_current.X, _current.Y - (int)offY2);

                //clear old directions before adding new ones to list
                Directions.Clear();
                if (_end.X > _current.X)
                {
                    Directions.Add(surrR);
                }
                if (_end.Y > _current.Y)
                {
                    Directions.Add(surrD);
                }
                if (_end.X < _current.X)
                {
                    Directions.Add(surrL);
                }
                if (_end.Y < _current.Y)
                {
                    Directions.Add(surrU);
                }


                foreach (var p in Directions) //delete directions which are obstacles or previous path
                {
                    if (!(YellowSquares.Any(m => m.Location == p) || RedSquares.Any(m => m.Location == p)))
                    {
                        Directions2.Add(p);
                    }
                }

                if (Directions2.Any()) //head to random correct location
                {
                    var rand = Rnd.Next(Directions2.Count());
                    _current.Location = Directions2[rand];
                    Directions2.Clear();
                }

                else //if no correct locations, head to random square that isn't obstacle or previous path
                {
                    //add all directions
                    Directions.Add(surrR);
                    Directions.Add(surrL);
                    Directions.Add(surrD);
                    Directions.Add(surrU);

                    foreach (var p in Directions) //remove obstacles or previous path
                    {
                        if (!(YellowSquares.Any(m => m.Location == p) || RedSquares.Any(m => m.Location == p)))
                        {
                            Directions2.Add(p);
                        }
                    }
                    if (Directions2.Any()) //pick random direction
                    {
                        var rand = Rnd.Next(Directions2.Count());
                        _current.Location = Directions2[rand];
                        Directions2.Clear();
                    }

                    else //if no available locations, dead end has been reached.
                    {
                        _deadEnd = true;
                        foundInLabel.Text = $"Reached dead end in {YellowSquares.Count} moves. \n(Try again)";
                        foreach (var rect in YellowSquares)
                        {
                            var tPoint = new Point(rect.X + (int)halfXPoint, rect.Y + (int)halfYPoint);
                            DePath.Add(tPoint);
                        }
                    }
                }

                if (_current == _end) //if you reach the goal
                {
                    _found = true;
                    foundInLabel.Text = $"Found goal in \n{YellowSquares.Count} moves.";
                    foreach (var rect in YellowSquares) //draw path taken
                    {
                        var tPoint = new Point(rect.X + (int)halfXPoint, rect.Y + (int)halfYPoint);
                        Path.Add(tPoint);
                    }
                }
                //add to previous path
                YellowSquares.Add(_current);
                _pfkBool = false;
            }

            //draw tiles and grid
            if (RedSquares.Any()) //obstacles
            {
                g.FillRectangles(RedBrush, RedSquares.ToArray());
            }
            if (YellowSquares.Any()) //previous path
            {
                g.FillRectangles(YellowBrush, YellowSquares.ToArray());
            }
            g.FillRectangle(BlueBrush, _start);
            if (_start != _end) //only draws if both start and end have been assigned (they should be different)
            {
                _current.Size = size3;
                _current.Offset((int)thirdXPoint, (int)thirdYPoint);
                g.FillRectangle(OrangeBrush, _current);
                _current.Offset(-(int)thirdXPoint, -(int)thirdYPoint);
                _current.Size = size;
            }
            g.FillRectangle(GreenBrush, _end);
            if (Path.Any()) //draws path taken
            {
                //make a list of points and draw lines connecting them
                var fullPath = new List<Point>();
                var adStart = new Point(_start.X + (int)halfXPoint, _start.Y + (int)halfYPoint);
                var adEnd = new Point(_end.X + (int)halfXPoint, _end.Y + (int)halfYPoint);
                fullPath.Add(adStart);
                fullPath.AddRange(Path);
                fullPath.Add(adEnd);
                g.DrawLines(Greenpen, fullPath.ToArray());
            }
            if (DePath.Any()) //draws path taken (when end isn't reached)
            {
                //make a list of points and draw lines connecting them
                var fulldePath = new List<Point>();
                var adStart = new Point(_start.X + (int)halfXPoint, _start.Y + (int)halfYPoint);
                fulldePath.Add(adStart);
                fulldePath.AddRange(DePath);
                g.DrawLines(Orangepen, fulldePath.ToArray());
            }
            for (var y = 1; y <= NumOfCells; y++) //horizontal grid lines
            {
                g.DrawLine(Blackpen, 0, (float)y / NumOfCells * panel.Height, panel.Width,
                    (float)y / NumOfCells * panel.Height);
            }
            for (var x = 1; x <= NumOfCells; x++) //vertical grid lines
            {
                g.DrawLine(Blackpen, (float)x / NumOfCells * panel.Width, 0, (float)x / NumOfCells * panel.Width, panel.Height);
            }
        }


        private void panel_MouseClick(object sender, MouseEventArgs e)
        //get mouse location when it's clicked to create obstacle on location
        {
            _mouseLoc = e.Location;
            _clickBool = true;
            panel.Invalidate();
        }

        private void StartButt(object sender, EventArgs e) //make a new start tile 
        {
            var str = startTB.Text;
            var dot = str.IndexOf('.');
            if (Regex.IsMatch(str) & dot == -1) //must match format X,Y
            {
                replyLabel.Text = "";
                //get coords from string and create tile
                string[] coords = str.Split(',');
                var greenx = (double)int.Parse(coords[0]) / NumOfCells * panel.Width;
                var greeny = (double)int.Parse(coords[1]) / NumOfCells * panel.Height;
                var point = new Point((int)greenx, (int)greeny);
                var size = new Size(panel.Width / NumOfCells, panel.Width / NumOfCells);
                var size2 = new Size(panel.Width / NumOfCells / 2, panel.Width / NumOfCells / 2);
                _start = new Rectangle(point, size);

                //reset variables
                _deadEnd = false;
                _found = false;
                YellowSquares.Clear();
                Path.Clear();
                DePath.Clear();
                _current = _start;
                _current.Size = size2;
                YellowSquares.Add(_current);
                _current.Size = size;
                panel.Invalidate();

                //write distance in form
                var dX = (double)(_start.Location.X - _end.Location.X) / panel.Width;
                var dY = (double)(_start.Location.Y - _end.Location.Y) / panel.Height;
                var sDistance = Math.Sqrt(dX * dX + dY * dY) * NumOfCells;
                sDistanceNumber.Text = $"{sDistance.ToString("G4")} tiles.";
            }
            else //reply if it's not in correct format
            {
                replyTimer.Start();
                replyTimer.Interval = ReplyMS;
                startTB.Text = "";
                replyLabel.ForeColor = Color.Red;
                replyLabel.Text = Resources.coordinates_must_follow_format;
            }
        }

        private void EndButt(object sender, EventArgs e) //make a new end tile
        {
            var str = endTB.Text;
            var dot = str.IndexOf('.');
            if (Regex.IsMatch(str) & dot == -1) //must match format X,Y
            {
                replyLabel.Text = "";
                //get coords from string and create tile
                string[] coords = str.Split(',');
                var dgreenx = (double)int.Parse(coords[0]) / NumOfCells * panel.Width;
                var dgreeny = (double)int.Parse(coords[1]) / NumOfCells * panel.Height;
                var point = new Point((int)dgreenx, (int)dgreeny);
                var size = new Size(panel.Width / NumOfCells, panel.Width / NumOfCells);
                var size2 = new Size(panel.Width / NumOfCells / 2, panel.Width / NumOfCells / 2);
                _end = new Rectangle(point, size);

                //reset variables
                _deadEnd = false;
                _found = false;
                YellowSquares.Clear();
                Path.Clear();
                DePath.Clear();
                _current = _start;
                _current.Size = size2;
                YellowSquares.Add(_current);
                _current.Size = size;
                panel.Invalidate();

                //write distance in form
                var dX = (double)(_start.Location.X - _end.Location.X) / panel.Width;
                var dY = (double)(_start.Location.Y - _end.Location.Y) / panel.Height;
                var sDistance = Math.Sqrt(dX * dX + dY * dY) * NumOfCells;
                sDistanceNumber.Text = $"{sDistance.ToString("G4")} tiles.";
            }
            else //reply if it's not in correct format
            {
                replyTimer.Start();
                replyTimer.Interval = ReplyMS;
                endTB.Text = "";
                replyLabel.ForeColor = Color.Red;
                replyLabel.Text = Resources.coordinates_must_follow_format;
            }
        }

        private void pfkButt_Click(object sender, EventArgs e) //find path to goal
        {
            if (_found || _deadEnd) //reset if it has found a path already
            {
                var size = new Size(panel.Width / NumOfCells, panel.Width / NumOfCells);
                var size2 = new Size(panel.Width / NumOfCells / 2, panel.Width / NumOfCells / 2);
                _deadEnd = false;
                _found = false;
                YellowSquares.Clear();
                Path.Clear();
                DePath.Clear();
                _current = _start;
                _current.Size = size2;
                YellowSquares.Add(_current);
                _current.Size = size;
            }
            //start timer to automatically repaint panel
            pfkTimer.Start();
            pfkTimer.Interval = TimerMS;
            _pfkBool = true;
            panel.Invalidate();
        }

        private void startTB_Enter(object sender, KeyPressEventArgs e) //enable enter on textbox
        {
            if (e.KeyChar == (char)13)
            {
                StartButt(sender, e);
            }
        }

        private void endTB_Enter(object sender, KeyPressEventArgs e) //enable enter on textbox
        {
            if (e.KeyChar == (char)13)
            {
                EndButt(sender, e);
            }
        }

        private void randCoordButt_Click(object sender, EventArgs e) //make random start and end tiles
        {
            //reset variables
            _deadEnd = false;
            _found = false;
            YellowSquares.Clear();
            Path.Clear();
            DePath.Clear();
            var rCoord = new Random();
            var startP = new Point();
            var endP = new Point();
            var randomDone = false;
            while (!randomDone) //avoid creating start/end tiles on obstacles
            {
                //random points
                randomDone = true;
                var rsX = (double)rCoord.Next(1, NumOfCells) / NumOfCells * panel.Width;
                var rsY = (double)rCoord.Next(1, NumOfCells) / NumOfCells * panel.Height;
                var reX = (double)rCoord.Next(1, NumOfCells) / NumOfCells * panel.Width;
                var reY = (double)rCoord.Next(1, NumOfCells) / NumOfCells * panel.Height;
                startP = new Point((int)rsX, (int)rsY);
                endP = new Point((int)reX, (int)reY);
                if (RedSquares.Any(m => m.Location == startP) || RedSquares.Any(m => m.Location == endP))
                {
                    randomDone = false;
                }
            }
            var size = new Size(panel.Width / NumOfCells, panel.Width / NumOfCells);
            var size2 = new Size(panel.Width / NumOfCells / 2, panel.Width / NumOfCells / 2);
            _start = new Rectangle(startP, size);
            //reset more variables
            _current = _start;
            _current.Size = size2;
            YellowSquares.Add(_current);
            _current.Size = size;
            _end = new Rectangle(endP, size);
            foundInLabel.Text = "";
            panel.Invalidate();

            //write distance in form
            var dX = (double)(_start.Location.X - _end.Location.X) / panel.Width;
            var dY = (double)(_start.Location.Y - _end.Location.Y) / panel.Height;
            var sDistance = Math.Sqrt(dX * dX + dY * dY) * NumOfCells;
            sDistanceNumber.Text = $"{sDistance.ToString("G4")} tiles.";
        }

        private void pfkTimer_Tick(object sender, EventArgs e) //repaint every tick
        {
            if (_found || _deadEnd)
            {
                pfkTimer.Stop();
            }
            else
            {
                _pfkBool = true;
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
            RedSquares.Clear();
            CreateWalls();
            var size = new Size(panel.Width / NumOfCells, panel.Width / NumOfCells);
            var rnd = new Random();
            for (var i = 1; i <= NumOfCells * NumOfCells / 5; i++)
            {
                var currObX = (double)rnd.Next(1, NumOfCells) / NumOfCells * panel.Width;
                var currObY = (double)rnd.Next(1, NumOfCells) / NumOfCells * panel.Height;
                var obP = new Point((int)currObX, (int)currObY);
                var currOb = new Rectangle(obP, size);
                if (currOb != _start && currOb != _end) //add if not a start/end spot
                {
                    RedSquares.Add(currOb);
                }
                else //go down a number if it was occupied by start/end 
                {
                    i--;
                }
            }
            panel.Invalidate();
        }

        private void CreateWalls() //create walls of obstacles that delimit the area
        {
            var size = new Size(panel.Width / NumOfCells, panel.Width / NumOfCells);
            var lX = (double)-1 / NumOfCells * panel.Width;
            var lX2 = (double)10 / NumOfCells * panel.Width;
            var lY = (double)-1 / NumOfCells * panel.Height;
            var lY2 = (double)10 / NumOfCells * panel.Height;
            var offX2 = (double)panel.Width / NumOfCells;
            var offY2 = (double)panel.Height / NumOfCells;
            var lP = new Point((int)lX, 0);
            var lP2 = new Point((int)lX2, 0);
            var lP3 = new Point(0, (int)lY);
            var lP4 = new Point(0, (int)lY2);
            var limit = new Rectangle(lP, size);
            var limit2 = new Rectangle(lP2, size);
            var limit3 = new Rectangle(lP3, size);
            var limit4 = new Rectangle(lP4, size);
            for (var i = 0; i <= 9; i++)
            {
                RedSquares.Add(limit);
                limit.Offset(0, (int)offY2);
                RedSquares.Add(limit2);
                limit2.Offset(0, (int)offY2);
                RedSquares.Add(limit3);
                limit3.Offset((int)offX2, 0);
                RedSquares.Add(limit4);
                limit4.Offset((int)offX2, 0);
            }
        }
    }
}