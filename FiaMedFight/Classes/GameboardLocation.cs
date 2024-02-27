using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiaMedFight
{
    internal class GameboardLocation
    {
        public class EllipseLocation
        {
            public int Coordinate { get; private set; }
            public int Row { get; private set; }
            public int Column { get; private set; }

            public EllipseLocation(int coordinate, int row, int column)
            {
                Coordinate = coordinate;
                Row = row;
                Column = column;
            }
        }

        //public List<EllipseLocation> locations = new List<EllipseLocation>() { };

        public class SafeEllipseLocation
        {
            public int Coordinate { get; private set; }
            public int Row { get; private set; }
            public int Column { get; private set; }
            public int TeamColor { get; private set; }

            public SafeEllipseLocation(int coordinate, int row, int column, int teamColor)
            {
                Coordinate = coordinate;
                Row = row;
                Column = column;
                TeamColor = teamColor;
            }
        }

        public class Goal : EllipseLocation
        {
            bool isGoal = true;
            public Goal(int coordinate, int row, int column) : base(coordinate, row, column)
            { }
        }

        public class HomeBase
        {
            public EllipseLocation Location { get; private set; }
            public string TeamColor { get; private set; }

            public HomeBase(EllipseLocation location, string teamColor)
            {
                Location = location;
                TeamColor = teamColor;
            }
        }

    }
}
