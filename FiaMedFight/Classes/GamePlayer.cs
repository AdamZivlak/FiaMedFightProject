using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using FiaMedFight.Templates;

namespace FiaMedFight.Classes
{
    /// <summary>
    /// Represents a player in the game
    /// </summary>
    public class GamePlayer
    {
        /// <summary>
        /// Gets the colorm of the player.
        /// </summary>
        public string color { get; private set; }

        /// <summary>
        /// Gets the list of game piece controls owned by the player.
        /// </summary>
        public List<GamePieceControl> pieces = new List<GamePieceControl>();

        int score;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePlayer"/> class with the specified color and number of game pieces.
        /// </summary>
        /// <param name="color">The color of the player.</param>
        /// <param name="gamePieces">The number of game pieces.</param>
        public GamePlayer(string color)
        {
            this.color = color;
            this.score = 0;
        }

        /// <summary>
        /// Adds points to the player's score.
        /// </summary>
        /// <param name="points">The points to add.</param>
        public void AddPoints(int points)
        {
            this.score += points;
        }
    }
}
