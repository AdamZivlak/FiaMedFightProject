using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

namespace FiaMedFight.Classes
{
    internal class Dice
    {
        public int FaceValue { get; set; }
        public int Sides { get; set; }

        //public Button DiceButton { get; set; }
        public static readonly string[] imagePaths = new string[]
        {
            "ms-appx:///Assets/Dice/Face1.png",
            "ms-appx:///Assets/Dice/Face2.png",
            "ms-appx:///Assets/Dice/Face3.png",
            "ms-appx:///Assets/Dice/Face4.png",
            "ms-appx:///Assets/Dice/Face5.png",
            "ms-appx:///Assets/Dice/Face6.png"
        };

        public Dice(int sides)
        {
            Sides = sides;
            FaceValue = RollAnyDice(Sides);
        }

        public Dice(Button b)
        {
            RollDice(b, this);
        }

        //if caller has a dice-object
        public void ChangeDiceFace(Button b)
        {
            string imagePath = imagePaths[FaceValue - 1];
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
            b.Background = new ImageBrush { ImageSource = bitmapImage };
        }
        //with dice-result as param
        public static void ChangeDiceFace(Button b, int rolledValue)
        {
            string imagePath = imagePaths[rolledValue - 1];
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
            b.Background = new ImageBrush { ImageSource = bitmapImage };
        }
        //with dice-object as param
        public static void ChangeDiceFace(Button b, Dice dice)
        {
            string imagePath = imagePaths[dice.FaceValue - 1];
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
            b.Background = new ImageBrush { ImageSource = bitmapImage };
        }

        //if caller has a dice-object
        public void RollThisDice(Button button)
        {
            Random random = new Random();
            FaceValue = random.Next(1, Sides+1);
            ChangeDiceFace(button, FaceValue);
        }
        //rolling without dice-object
        public static void RollDice(Button button, int maxValue)
        {
            Random random = new Random();
            ChangeDiceFace(button, random.Next(1, maxValue+1));
        }
        //rolling with dice as param
        public static void RollDice(Button button, Dice dice)
        {
            Random random = new Random();
            dice.FaceValue = random.Next(1, dice.Sides+1);
            ChangeDiceFace(button, dice.FaceValue);
        }
        //return result without changing visuals
        public static int RollAnyDice(int sides)
        {
            Random random = new Random();
            return random.Next(1, sides+1);
        }
        //roll this dice without cvhanging visuals
        public void RollAnyDice()
        {
            Random random = new Random();
            FaceValue = random.Next(1, Sides + 1);
        }
    }
}
