using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.Devices.SmartCards;
using System.ComponentModel.Design;

namespace FiaMedFight.Classes
{
    /// <summary>
    /// Represents a dice object with methods to roll and change its face.
    /// </summary>
    public class Dice
    {
        /// <summary>
        /// Whether the dice is rollable (clickable) or not
        /// </summary>

        public bool active = true;
        /// <summary>
        /// Paths to image files for each side of a six-sided dice.
        /// </summary>

        public static readonly string[] imageSixSidedPaths = new string[]
        {
        "ms-appx:///Assets/Dice/Face1.png",
        "ms-appx:///Assets/Dice/Face2.png",
        "ms-appx:///Assets/Dice/Face3.png",
        "ms-appx:///Assets/Dice/Face4.png",
        "ms-appx:///Assets/Dice/Face5.png",
        "ms-appx:///Assets/Dice/Face6.png"
        };
        /// <summary>
        /// Paths to image files for each side of a four-sided dice.
        /// </summary>
        public static readonly string[] imageFourSidedPaths = new string[3];


        /// <summary>
        /// The last rolled value shown on the current face of the dice.
        /// </summary>
        public int FaceValue { get; set; }

        /// <summary>
        /// The number of sides the dice has.
        /// </summary>
        public int Sides { get; set; }

        private string[] faceImages;

        /// <summary>
        /// Initializes a new instance of the Dice class with the specified number of sides.
        /// </summary>
        /// <param name="sides">The number of sides for the dice.</param>
        public Dice(int sides)
        {
            Sides = sides;
            switch (sides)
            {
                case 6: faceImages = imageSixSidedPaths; break;
                case 4: faceImages = imageFourSidedPaths; break; //empty atm
            }
            FaceValue = RollAnyDice(Sides);
        }

        /// <summary>
        /// Initializes a new instance of the Dice class with a button to associate with rolling.
        /// </summary>
        /// <param name="b">The button associated with the dice.</param>
        public Dice(Button b)
        {
            RollDice(b, this);
        }

        public void Activate()
        {
            this.active = true;
        }
        public void Deactivate()
        {
            this.active = false;
        }
        /// <summary>
        /// Changes the visual representation of the dice face on the provided button.
        /// </summary>
        /// <param name="b">The button representing the dice.</param>
        public void ChangeDiceFace(Button b)
        {
            string imagePath = faceImages[FaceValue - 1];
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
            b.Background = new ImageBrush { ImageSource = bitmapImage };
        }

        /// <summary>
        /// Changes the visual representation of the dice face on the provided button based on the given value without needing a Dice object.
        /// </summary>
        /// <param name="b">The button representing the dice.</param>
        /// <param name="rolledValue">The value rolled on the dice.</param>
        public static void ChangeDiceFace(Button b, int rolledValue, string[] imagePaths)
        {
            string imagePath = imagePaths[rolledValue - 1];
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
            b.Background = new ImageBrush { ImageSource = bitmapImage };
        }

        /// <summary>
        /// Changes the visual representation of the dice face on the provided button based on the dice object.
        /// </summary>
        /// <param name="b">The button representing the dice.</param>
        /// <param name="dice">The dice object.</param>
        public static void ChangeDiceFace(Button b, Dice dice)
        {
            string imagePath = imageSixSidedPaths[dice.FaceValue - 1];
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
            b.Background = new ImageBrush { ImageSource = bitmapImage };
        }

        /// <summary>
        /// Rolls the dice and changes the visual representation on the provided button.
        /// </summary>
        /// <param name="button">The button representing the dice.</param>
        public void RollThisDice(Button button)
        {
            Random random = new Random();
            FaceValue = 5;
            //FaceValue = random.Next(1, Sides + 1);
            ChangeDiceFace(button, this);
        }

        /// <summary>
        /// Rolls a dice with a specified maximum value and changes the visual representation on the provided button.
        /// </summary>
        /// <param name="button">The button representing the dice.</param>
        /// <param name="maxValue">The maximum value for the roll.</param>
        public static void RollDice(Button button, int maxValue, string[] imagePaths)
        {
            Random random = new Random();
            //ChangeDiceFace(button, random.Next(1, maxValue + 1), imagePaths);
            ChangeDiceFace(button, 5, imagePaths);
        }

        /// <summary>
        /// Rolls the provided dice object and changes the visual representation on the provided button.
        /// </summary>
        /// <param name="button">The button representing the dice.</param>
        /// <param name="dice">The dice object to roll.</param>
        public static void RollDice(Button button, Dice dice)
        {
            Random random = new Random();
            //dice.FaceValue = random.Next(1, dice.Sides + 1);
            dice.FaceValue = 5;
            ChangeDiceFace(button, dice);
        }

        /// <summary>
        /// Rolls a dice with a specified number of sides and returns the result without changing visuals.
        /// </summary>
        /// <param name="sides">The number of sides for the dice.</param>
        /// <returns>The result of the roll.</returns>
        public static int RollAnyDice(int sides)
        {
            Random random = new Random();
            return random.Next(1, sides + 1);
        }

        /// <summary>
        /// Rolls this dice object and updates the face value without changing visuals.
        /// </summary>
        public void RollAnyDice()
        {
            Random random = new Random();
            FaceValue = random.Next(1, Sides + 1);
        }
    }
}