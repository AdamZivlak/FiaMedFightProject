using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiaMedFight.Classes
{
    public static class GameManager
    {
        internal static GameSession session { get; set; }

        internal static void StartGame(GameSession session)
        {
            GameManager.session = session;
        }

        public static void RollDice(object sender)
        {
        }
    }
}
