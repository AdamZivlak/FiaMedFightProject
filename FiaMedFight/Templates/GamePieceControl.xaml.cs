using FiaMedFight.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        bool active = true; //TODO: Should default to false when GameManeger starts managing turns.
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
        /// <param name="dice_result">The result of the dice roll.</param>
        /// <returns>The string representation of the end coordinate.</returns>
        public string GetEndCoordinateString(int dice_result)
        {
            int new_pos = GetCoordinateInt() + dice_result;
            if (new_pos > 52) new_pos -= 52;
            return "Coordinate" + new_pos;
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

            Deactivate(); //TODO: Should call GameManager to deactivate all pieces on the board?
            int steps = GameManager.session.dice.FaceValue;
            string endCoordinate = GetEndCoordinateString(steps);

            //Animates the movement by transformation
            ResizeAnimation(1.35, 100);
            while (steps-- > 0)
                await MoveStepsAsync(1, 300, 0, -1);
            ResizeAnimation(1, 150);
            await MoveStepsAsync(0, 150, 0, -0.5);

            //Resets the transform and actually moves the piece within the grid.
            ResetMovementTransform();
            MoveToNewGridCoordinate(endCoordinate, -1);

            //Reactivates piece for testing purposes. Should stay deactivated until GameManager activates on next turn.
            Activate();
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
        /// <param name="offsetX">Optional X offset for each step.</param>
        /// <param name="offsetY">Optional Y offset for each step.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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
        // <summary>
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
    }
}
