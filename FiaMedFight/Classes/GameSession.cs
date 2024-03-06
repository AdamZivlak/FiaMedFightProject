using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiaMedFight.Classes
{
    /// <summary>
    /// Represents a session of the game.
    /// </summary>
    public class GameSession
    {
        /// <summary>
        /// Gets the list of players in the game session.
        /// </summary>
        public List<GamePlayer> players = new List<GamePlayer>();

        /// <summary>
        /// Gets the dice used in the game session.
        /// </summary>
        public Dice dice = new Dice(6);

        /// <summary>
        /// Gets or sets the index of the active player.
        /// </summary>
        public int activePlayerIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSession"/> class.
        /// </summary>
        public GameSession() { }

        /// <summary>
        /// Adds a player to the game session.
        /// </summary>
        /// <param name="player">The player to add.</param>
        public void AddPlayer(GamePlayer player)
        {
            this.players.Add(player);
        }

    }
}
