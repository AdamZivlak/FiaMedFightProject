using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiaMedFight.Classes
{
    public static class GameManager
    {
        public static GameSession currentSession { get; set; }

        public static void StartGame(GameSession session)
        {
            currentSession = session;
        }
    }
}
