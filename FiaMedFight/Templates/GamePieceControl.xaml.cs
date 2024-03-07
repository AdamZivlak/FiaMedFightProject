using FiaMedFight.Classes;
using FiaMedFight.Templates;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.Input.Custom;
using Windows.Gaming.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FiaMedFight.Templates
{
    /// <summary>
    /// Represents a control for a game piece in the FiaMedFight game.
    /// </summary>
    /// <remarks>
    /// This control manages the appearance and behavior of a game piece on the game board.
    /// </remarks>
    public partial class GamePieceControl : UserControl
    {
        public bool active = false;
        public string coordinate;
        public Point currentPoint = new Point(0, 0);
        public string color;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePieceControl"/> class at a specified coordinate.
        /// </summary>
        public GamePieceControl(string color, string coordinate)
        {
            this.color = color;
            this.coordinate = coordinate;
            InitializeComponent();
            UpdateImageSource(color);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePieceControl"/> class at the default location (homebase).
        /// </summary>
        public GamePieceControl(string color)
        {
            this.color = color;
            this.coordinate = color + "Base";
            InitializeComponent();
            UpdateImageSource(color);
        }

        public GamePlayer Player()
        {
            return GameManager.FindOrCreatePlayer(color);
        }

        /// <summary>
        /// Updates the image source for the game piece based on its color.
        /// </summary>
        /// <param name="color">The color of the game piece.</param>
        private void UpdateImageSource(string color) {
            string imageName = $"ms-appx:///Assets/Pieces/{color}.png";
            GamePieceImage.Source = new BitmapImage(new Uri(imageName, UriKind.Absolute));
        }

        /// <summary>
        /// Activates the game piece.
        /// </summary>
        public void Activate()
        {
            this.active = true;
        }

        /// <summary>
        /// Deactivates the game piece.
        /// </summary>
        public void Deactivate()
        {
            this.active = false;
        }

        /// <summary>
        /// Gets the integer value of the coordinate.
        /// </summary>
        /// <returns>The integer value of the coordinate.</returns>
        public int GetCoordinateInt()
        {
            string coordinate_num = this.coordinate.Substring(10);
            return int.Parse(coordinate_num);
        }
        public int ExtractIntFromString(string str)
        {
            string numbers = "";
            int result;
            foreach(char c in str)
                if (char.IsDigit(c))
                    numbers += c;
            if(int.TryParse(numbers, out result)) return result;
            return 0;
        }

        /// <summary>
        /// Sets the coordinate based on the provided integer value.
        /// </summary>
        /// <param name="coordinate">The integer value of the coordinate to set.</param>
        public void SetCoordinateFromInt(int coordinate)
        {
            this.coordinate = "Coordinate" + coordinate;
        }

        // <summary>
        /// Gets the X and Y coordinates from the current position to the specified target coordinate, with optional offsets.
        /// </summary>
        /// <param name="targetCoordinate">The target coordinate to calculate the position to.</param>
        /// <param name="offsetX">The optional X offset from the target position. A double between -1 and 1.</param>
        /// <param name="offsetY">The optional Y offset from the target position. A double between -1 and 1.</param>
        /// <returns>The point representing the X and Y coordinates from the current to the target position.</returns>
        public Point GetXYFromSelfToCoordinate(string targetCoordinate, double offsetX = 0, double offsetY = 0)
        {
            var targetElement = GameManager.gameBoard.FindName(targetCoordinate) as FrameworkElement;

            GeneralTransform transform = targetElement.TransformToVisual(this);
            Point position = transform.TransformPoint(new Point(0, 0));

            if (offsetX != 0)
                position.X += targetElement.ActualHeight * offsetX;
            if (offsetY != 0)
                position.Y += targetElement.ActualHeight * offsetY;
            
            return position;
        }

        /// <summary>
        /// Gets the string representation of the end coordinate after moving a certain number of steps.
        /// </summary>
        /// <param name="stepsToMove">The number of steps on the game board to move</param>
        /// <returns>The string representation of the end coordinate.</returns>
        public string GetEndCoordinateString(int stepsToMove)
        {
            int enterSafeZoneCoordinate = ExtractIntFromString(Player().firstCoordinateAfterHomeBase) - 1,
                currentCoordinate = ExtractIntFromString(this.coordinate),
                targetCoordinate = currentCoordinate + stepsToMove;
            if (targetCoordinate >= 53) targetCoordinate -= 52;

            if (stepsToMove == 0 || coordinate.ToLower().StartsWith("goal")) return this.coordinate;

            else if (coordinate.ToLower().StartsWith(color + "base")) //If in home base
            {
                return "Coordinate" + (enterSafeZoneCoordinate + targetCoordinate);
            }

            else if (coordinate.ToLower().StartsWith(color + "safe")) //If in safe zone
            {
                if (targetCoordinate == 6)
                { 
                    return "goalCoordinate";
                }
                else if (targetCoordinate > 6)
                {     
                    return "overpassingTheGoal";
                }
                else
                {
                    return color + "SafeCoordinate" + targetCoordinate;
                }
            }

            else if (coordinate == "Coordinate" + enterSafeZoneCoordinate) //If moving into safe zone
            {
                if (stepsToMove == 6)
                {
                    return "goalCoordinate";
                }
                else if (stepsToMove > 6)
                { 
                    return "overpassingTheGoal";
                }
                return color + "SafeCoordinate" + stepsToMove;
            }

            return "Coordinate" + targetCoordinate;
        }
        
        /// <summary>
        /// Handles the hover event for the game piece.
        /// </summary>
        private void GamePiece_OnHover(object sender, PointerRoutedEventArgs e)
        {
            if (active)
                ResizeAnimation(1.35, 30);
        }

        /// <summary>
        /// Handles the end of hover event for the game piece.
        /// </summary>
        private void GamePiece_EndHover(object sender, PointerRoutedEventArgs e)
        {
            if (active)
                ResizeAnimation(1, 60);
        }

        /// <summary>
        /// Handles the press event for the game piece.
        /// </summary>
        private void GamePiece_Pressed(object sender, PointerRoutedEventArgs e) 
        { 
        }

        /// <summary>
        /// Handles the release event for the game piece.
        /// </summary>
        async private void GamePiece_Released(object sender, PointerRoutedEventArgs e)
        {
            if (!active) return;

            GameManager.ActivePlayer().EndTurn(); //TODO: Should call GameManager to deactivate all pieces on the board?
            int steps = GameManager.session.dice.FaceValue;

            //Animates the movement by transformation
            ResizeAnimation(1.35, 100);
            while (steps-- > 0)
                await MoveStepsAsync(1, 300, 0, -1);
            ResizeAnimation(1, 150);
            await MoveStepsAsync(0, 150, 0, -0.5);

            //Resets the transform and actually moves the piece within the grid.
            ResetMovementTransform();
            MoveToNewGridCoordinate(coordinate, -1);
            if (coordinate == "goalCoordinate")
            {
                //GoalAnimation(); //TODO: Fix confetti animation
                ResizeAnimation(3.00, 1000);
                ChangeOpacityAnimation(0, 1500);
            }
            GameManager.NextTurn();
        }

        private void GoalAnimation()
        {
            //TODO: Finish and test this animation.
            var confettiArea = GameManager.activePage.FindName("confettiArea") as FrameworkElement;

            Storyboard confetti = GameManager.activePage.FindName("confettiAnimation") as Storyboard;
            //Storyboard.SetTarget(confetti, confettiArea);
            
            confettiArea.Visibility = Visibility.Visible;
            confetti.Begin();
        }

        /// <summary>
        /// Moves the game piece to a new grid coordinate with optional row and column offsets.
        /// </summary>
        /// <param name="endCoordinate">The end coordinate to move the game piece to.</param>
        /// <param name="rowOffset">Optional row offset. (integer)</param>
        /// <param name="columnOffset">Optional column offset. (integer)</param>
        private void MoveToNewGridCoordinate(string endCoordinate, int rowOffset = 0, int columnOffset = 0)
        {
            (int targetRow, int targetColumn) = GameManager.GetElementRowAndColumn(endCoordinate);
            Grid.SetColumn(this, targetColumn + columnOffset);
            Grid.SetRow(this, targetRow + rowOffset);
            coordinate = endCoordinate;
        }

        /// <summary>
        /// Resets the movement transform for the game piece, setting its translation transforms to zero.
        /// </summary>
        private void ResetMovementTransform()
        {
            (this.RenderTransform as CompositeTransform).TranslateX = 0;
            (this.RenderTransform as CompositeTransform).TranslateY = 0;
            currentPoint = new Point(0, 0);
        }

        /// <summary>
        /// Moves the game piece a specified number of steps with an animation over a set duration and with optional offsets.
        /// </summary>
        /// <param name="steps">The number of steps to move.</param>
        /// <param name="milliseconds">The duration of the animation in milliseconds.</param>
        /// <param name="offsetX">Optional X offset for each step. Any double between -1.0 and 1.0.</param>
        /// <param name="offsetY">Optional Y offset for each step. Any double between -1.0 and 1.0.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>In effect the offset is in </remarks>
        private Task MoveStepsAsync(int steps, int milliseconds, double offsetX = 0, double offsetY = 0)
        {
            this.RenderTransform = new CompositeTransform();
            Storyboard moveAnimation = new Storyboard();
            DoubleAnimation oDoubleAnimationX = new DoubleAnimation();
            DoubleAnimation oDoubleAnimationY = new DoubleAnimation();
            oDoubleAnimationX.Duration = new Duration(TimeSpan.FromMilliseconds(milliseconds));
            oDoubleAnimationY.Duration = new Duration(TimeSpan.FromMilliseconds(milliseconds));

            string endCoordinate = GetEndCoordinateString(steps);

            Point startPoint = currentPoint;
            Point endPoint = GetXYFromSelfToCoordinate(endCoordinate, offsetX, offsetY);

            oDoubleAnimationX.From = startPoint.X;
            oDoubleAnimationX.To = endPoint.X;
            oDoubleAnimationY.From = startPoint.Y;
            oDoubleAnimationY.To = endPoint.Y;

            this.currentPoint = endPoint;

            Storyboard.SetTarget(oDoubleAnimationX, this);
            Storyboard.SetTargetProperty(oDoubleAnimationX, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
            Storyboard.SetTarget(oDoubleAnimationY, this);
            Storyboard.SetTargetProperty(oDoubleAnimationY, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");

            moveAnimation.Children.Add(oDoubleAnimationX);
            moveAnimation.Children.Add(oDoubleAnimationY);

            var tcs = new TaskCompletionSource<bool>();
            moveAnimation.Completed += (s, e) =>
            {
                tcs.SetResult(true);
            };

            moveAnimation.Begin();
            this.coordinate = endCoordinate;            
            return tcs.Task;
        }
        /// <summary>
        /// Applies a resize animation to the game piece, scaling it by a specified factor over a set duration.
        /// </summary>
        /// <param name="factor">The factor by which to scale the game piece.</param>
        /// <param name="milliseconds">The duration of the animation in milliseconds.</param>
        private void ResizeAnimation(double factor, int milliseconds)
        {
            Storyboard resizeAnimation = new Storyboard();
            DoubleAnimation scaleXAnimation = new DoubleAnimation()
            {
                To = factor,
                Duration = TimeSpan.FromMilliseconds(milliseconds)
            };
            Storyboard.SetTarget(scaleXAnimation, scaleTransform);
            Storyboard.SetTargetProperty(scaleXAnimation, "ScaleX");
             
            DoubleAnimation scaleYAnimation = new DoubleAnimation()
            {
                To = factor,
                Duration = TimeSpan.FromMilliseconds(milliseconds)
            };
            Storyboard.SetTarget(scaleYAnimation, scaleTransform);
            Storyboard.SetTargetProperty(scaleYAnimation, "ScaleY");

            resizeAnimation.Children.Add(scaleXAnimation);
            resizeAnimation.Children.Add(scaleYAnimation);

            resizeAnimation.Begin();
        }
        private void ChangeOpacityAnimation(double opacity, int milliseconds)
        {
            Storyboard opacityStoryboard = new Storyboard();
            DoubleAnimation opacityAnimation = new DoubleAnimation()
            {
                To = opacity,
                Duration = TimeSpan.FromMilliseconds(milliseconds)
            };
            Storyboard.SetTarget(opacityAnimation, pawnImage);
            Storyboard.SetTargetProperty(opacityAnimation, "Opacity");

            opacityStoryboard.Children.Add(opacityAnimation);

            opacityStoryboard.Begin();
        }
        /// <summary>
        /// Repositions the piece to it's homeBase location within the active session's gameBoard Grid.
        /// </summary>
        public void MoveToHomeBase()
        {
            string homeBaseCoordinate = color + "Base";
            var homeBase = GameManager.gameBoard.FindName(homeBaseCoordinate) as FrameworkElement;

            int baseColumn = Grid.GetColumn(homeBase);
            int baseRow = Grid.GetRow(homeBase);

            var player = this.Player();

            bool setRowAndColumn = false;

            //Move to another 'row' or 'column' if there is already a piece there.
            for (int row = baseRow + 1; row <= baseRow + 5 && !setRowAndColumn; row += 2)
            {
                for (int column = baseColumn + 1; column <= baseColumn + 5; column += 2)
                {
                    setRowAndColumn = true;
                    foreach (GamePieceControl p in player.pieces)
                    {
                        if (Grid.GetColumn(p) == column && Grid.GetRow(p) == row)
                        {
                            setRowAndColumn = false;
                            break;
                        }
                    }
                    if (setRowAndColumn)
                    {
                        Grid.SetRow(this, row);
                        Grid.SetColumn(this, column);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns 'true' if the piece is standing in the goal
        /// </summary>

        public bool isInGoal()
        {
            return coordinate.ToLower().StartsWith("goal");
        }
        /// <summary>
        /// Returns 'true' if the piece is standing in it's home base
        /// </summary>
        
        public bool isInHomeBase()
        {
            return coordinate.ToLower().StartsWith(color + "base");
        }
        /// <summary>
        /// Returns 'true' if the piece is standing in a safe zone coordinate
        /// </summary>
        
        public bool isInSafeZone()
        {
            return coordinate.ToLower().StartsWith(color + "safe");
        }
    }
}
