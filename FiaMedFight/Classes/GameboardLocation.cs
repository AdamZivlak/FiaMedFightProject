using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiaMedFight
{
    internal class GameboardLocation
    {
        /// <summary>
        /// Represents a location (Ellipse) on the game board.
        /// </summary>
        public class EllipseLocation
        {
            /// <summary>
            /// Gets the coordinate of the Ellipse.
            /// </summary>
            public int Coordinate { get; private set; }

            /// <summary>
            /// Gets the row index of the Ellipse.
            /// </summary>
            public int Row { get; private set; }

            /// <summary>
            /// Gets the column index of the Ellipse.
            /// </summary>
            public int Column { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="EllipseLocation"/> class with the specified coordinate, row, and column.
            /// </summary>
            /// <param name="coordinate">The coordinate of the Ellipse.</param>
            /// <param name="row">The row index of the Ellipse.</param>
            /// <param name="column">The column index of the Ellipse.</param>
            public EllipseLocation(int coordinate, int row, int column)
            {
                Coordinate = coordinate;
                Row = row;
                Column = column;
            }
        }

        /// <summary>
        /// Represents the safe location for a game piece on the game board.
        /// </summary>
        public class SafeEllipseLocation
        {
            /// <summary>
            /// Gets the coordinate of the safelocation-Ellipse.
            /// </summary>
            public int Coordinate { get; private set; }

            /// <summary>
            /// Gets the row index of the safelocation-Ellipse.
            /// </summary>
            public int Row { get; private set; }

            /// <summary>
            /// Gets the column index of the safelocation-Ellipse.
            /// </summary>
            public int Column { get; private set; }

            /// <summary>
            /// Gets the team color associated with the safelocation-Ellipse.
            /// </summary>
            public int TeamColor { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="SafeEllipseLocation"/> class with the specified coordinate, row, column, and team color.
            /// </summary>
            /// <param name="coordinate">The coordinate of the safelocation-Ellipse.</param>
            /// <param name="row">The row index of the safelocation-Ellipse.</param>
            /// <param name="column">The column index of the safelocation-Ellipse.</param>
            /// <param name="teamColor">The team color associated with the safelocation-Ellipse.</param>
            public SafeEllipseLocation(int coordinate, int row, int column, int teamColor)
            {
                Coordinate = coordinate;
                Row = row;
                Column = column;
                TeamColor = teamColor;
            }
        }

        /// <summary>
        /// Represents a goal location on the game board.
        /// </summary>
        public class Goal : EllipseLocation
        {
            bool isGoal = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="Goal"/> class with the specified coordinate, row, and column.
            /// </summary>
            /// <param name="coordinate">The coordinate of the goal.</param>
            /// <param name="row">The row index of the goal.</param>
            /// <param name="column">The column index of the goal.</param>
            public Goal(int coordinate, int row, int column) : base(coordinate, row, column)
            { }
        }

        /// <summary>
        /// Represents a home base location on the game board.
        /// </summary>
        public class HomeBase
        {
            /// <summary>
            /// Gets the location of the home base.
            /// </summary>
            public EllipseLocation Location { get; private set; }

            /// <summary>
            /// Gets the team color associated with the home base.
            /// </summary>
            public string TeamColor { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="HomeBase"/> class with the specified location and team color.
            /// </summary>
            /// <param name="location">The location of the home base.</param>
            /// <param name="teamColor">The team color associated with the home base.</param>
            public HomeBase(EllipseLocation location, string teamColor)
            {
                Location = location;
                TeamColor = teamColor;
            }
        }
    }
}
