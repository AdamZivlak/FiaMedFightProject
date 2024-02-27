using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace FiaMedFight.Classes
{
    internal class GamePlayer
    {
        public string color { get; private set; }
        public List<GamePiece> pieces = new List<GamePiece>();
        int score;

        public GamePlayer(string color, int gamePieces)
        {
            this.color = color;
            this.pieces = new List<GamePiece>();
            while (gamePieces-- > 0)
            {
                pieces.Add(new GamePiece(color));
            }
            this.score = 0;
        }

        public void AddPoints(int points)
        {
            this.score += points;
        }
    }
}
