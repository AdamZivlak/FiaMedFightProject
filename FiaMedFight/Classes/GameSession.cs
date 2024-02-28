using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiaMedFight.Classes
{
    public class GameSession
    {
        public List<GamePlayer> players = new List<GamePlayer>();
        public Dice dice = new Dice(6);
        public int active_player_index = 0;

        public GameSession() { }

        public void AddPlayer(GamePlayer player)
        {
            this.players.Add(player);
        }

    }
}
