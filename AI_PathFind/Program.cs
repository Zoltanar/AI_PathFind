using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_PathFind
{
    static class Program
    {

        /// Pathfinding program by Gustavo Abranches (15/11/2015)
        /// This program will attempt to find a path between start and end tiles,
        /// it knows where the end is but can only see obstacles directly adjacent to current tile,
        /// not diagonally.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());



        }
    }
}
