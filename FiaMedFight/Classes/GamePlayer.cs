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
        
        /// <summary>
        /// The 'x:name' attribute value of the first gameLocation to move to from homeBase.
        /// </summary>
        public string firstCoordinateAfterHomeBase;

        private bool isPlayerTurn;
        int score;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePlayer"/> class with the specified color and number of game pieces.
        /// </summary>
        /// <param name="color">The color of the player.</param>
        /// <param name="gamePieces">The number of game pieces.</param>
        /// <param name="firstCoordinateAfterHomeBase">The number on the first game Location to move to after homeBase</param>
        public GamePlayer(string color, string firstCoordinateAfterHomeBase = "Coordinate1")
        {
            this.color = color;
            this.score = 0;
            this.firstCoordinateAfterHomeBase = firstCoordinateAfterHomeBase;
            pieces = new List<GamePieceControl>();
            isPlayerTurn = false;
        }

        /// <summary>
        /// Adds points to the player's score.
        /// </summary>
        /// <param name="points">The points to add.</param>
        public void AddPoints(int points)
        {
            this.score += points;
        }

        /// <summary>
        /// Starts the player's turn 
        /// </summary>
        public void StartTurn()
        {
            // Todo: Uppdatera spelarens tillgängliga handlingar, t.ex antal tärningskast eller antal drag
            // Exemepel diceRollsLeft = 1;
            string targetCoordinate;
            int diceResult = GameManager.session.dice.FaceValue;

            foreach (GamePieceControl piece in pieces)
            {
                if (piece.isInHomeBase() && diceResult != 1 && diceResult != 6)
                    continue;

                targetCoordinate = piece.GetEndCoordinateString(diceResult);
                if (piece.isInGoal() || targetCoordinate == "overpassingTheGoal")
                    continue;

                piece.Activate();
            }
        }
        
        /// <summary>
        /// Ends the player's turn
        /// </summary>
        public void EndTurn()
        {
            foreach(GamePieceControl piece in pieces)
            {
                piece.Deactivate();
            }
            // Todo: 
            isPlayerTurn = false;
        }

        /// <summary>
        /// Checks if it's the player's turn.
        /// </summary>
        /// <returns>True if it's the player's turn, otherwise it's false</returns>
        public bool IsPlayerTurn()
        { 
            return isPlayerTurn; 
        }
    }
}
