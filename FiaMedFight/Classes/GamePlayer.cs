using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using FiaMedFight.Templates;

namespace FiaMedFight.Classes
{
    public class GamePlayer
    {
        public string color { get; private set; }
        public List<GamePieceControl> pieces = new List<GamePieceControl>();
        public List<string> piece_coordinates = new List<string>();
        int score;

        public GamePlayer(string color, int gamePieces)
        {
            this.color = color;
            this.piece_coordinates = new List<string>();
            while (gamePieces-- > 0)
            {
                piece_coordinates.Add(color + "Base");
            }
            this.score = 0;
        }

        public void AddPoints(int points)
        {
            this.score += points;
        }
    }
}
