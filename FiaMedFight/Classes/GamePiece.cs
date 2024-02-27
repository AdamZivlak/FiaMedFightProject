using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;
using Windows.UI.WebUI;
using Windows.Web.Http;

namespace FiaMedFight.Classes
{
    internal class GamePiece
    {
        string coordinate, color;
        bool active = false;
        public GamePiece(string color)
        {
            this.color = color;
            this.coordinate = color + "Base";
        }
        public GamePiece(string color, string coordinate)
        {
            this.color = color;
            this.coordinate = coordinate;
        }
        public void Activate() {
            //should make the piece clickable and movable
            this.active = true; 
        }
        public void Deactivate() {
            //should make the piece not clickable
            this.active = false;
        }
        public int GetCoordinateInt()
        {
            string coordinate_num = this.coordinate.Substring(10);
            return int.Parse(coordinate_num);
        }
        public void SetCoordinateFromInt(int coordinate)
        {
            this.coordinate = "Coordinate" + coordinate;
        }
        public void Move(int result)
        {
            int new_pos = GetCoordinateInt() + result;
            SetCoordinateFromInt(new_pos);
            //Update the GUI to move the piece
        }
    }
}
