using FiaMedFight.Classes;
using FiaMedFight.Templates;
using FiaMedFight.Utilities;
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
using Windows.UI.Xaml.Documents;
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
        internal string imageUri;
        internal GamePieceControl ghostPiece;

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
        /// Returns the player owning this GamePieceControl
        /// </summary>
        /// <returns></returns>
        public GamePlayer Player()
        {
            return GameManager.FindOrCreatePlayer(color);
        }

        /// <summary>
        /// Updates the image source for the game piece based on its color.
        /// </summary>
        /// <param name="color">The color of the game piece.</param>
        private void UpdateImageSource(string color) {
            imageUri = $"ms-appx:///Assets/Pieces/{color}.png";
            GamePieceImage.Source = new BitmapImage(new Uri(imageUri, UriKind.Absolute));
        }

        /// <summary>
        /// Activates the game piece. Makes it clickable.
        /// </summary>
        public void Activate()
        {
            this.active = true;
        }

        /// <summary>
        /// Deactivates the game piece. Makes it unclickable.
        /// </summary>
        public void Deactivate()
        {
            this.active = false;
        }

        /// <summary>
        /// Sets the coordinate based on the provided integer value.
        /// </summary>
        /// <param name="coordinate">The integer value of the coordinate to set.</param>
        public void SetCoordinateFromInt (int coordinate)
        {
            this.coordinate = "Coordinate" + coordinate;
        }

        /// <summary>
        /// Gets the X and Y coordinates from the current position to the specified target coordinate, with optional offsets.
        /// </summary>
        /// <param name="targetCoordinate">The target coordinate to calculate the position to.</param>
        /// <param name="offsetX">The optional X offset from the target position. A double between -1 and 1.</param>
        /// <param name="offsetY">The optional Y offset from the target position. A double between -1 and 1.</param>
        /// <returns>The point representing the X and Y coordinates from the current to the target position.</returns>
        public Point GetXYFromElementToCoordinate(UIElement elem, string targetCoordinate, double offsetX = 0, double offsetY = 0)
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
        /// <returns>The 'x:name' property of the target game board location.</returns>
        public string GetTargetCoordinateAsString(int stepsToMove)
        {
            int entranceToSafeZoneCoordinate = StringUtils.ExtractIntFromString(Player().entranceToSafeZoneCoordinate),
                currentCoordinate = StringUtils.ExtractIntFromString(this.coordinate),
                targetCoordinate = currentCoordinate + stepsToMove;
            
            if (stepsToMove == 0 || isInGoal())
                return this.coordinate;

            if (isInHomeBase())
                return "Coordinate" + (entranceToSafeZoneCoordinate + targetCoordinate); //Piece enters the board one or six steps after the entrance location.

            if (isInSafeZone())
                if (targetCoordinate == 6)
                    return "goalCoordinate";
                else if (targetCoordinate > 6)
                    return "overpassingTheGoal"; //This return value is used for deactivating the piece as it needs to enter the goal on an exact dice roll.
                else
                    return color + "SafeCoordinate" + targetCoordinate;

            if (coordinate == "Coordinate" + entranceToSafeZoneCoordinate) //If on first step into safe zone
            {
                if (stepsToMove == 6)
                    return "goalCoordinate";
                else if (stepsToMove > 6)
                    return "overpassingTheGoal";
                return color + "SafeCoordinate" + stepsToMove;
            }
            //default - Normal movement on the board:
            while (targetCoordinate >= 53) //Cannot rebase by % operator as coordinates are not 0-indexed
                targetCoordinate -= 52; 

            return "Coordinate" + targetCoordinate;
        }
        
        /// <summary>
        /// Handles the hover event for the game piece.
        /// </summary>
        private async void GamePiece_OnHover(object sender, PointerRoutedEventArgs e)
        {
            if (!active)
                return;

            ResizeAnimation(1.35, 30);
            await ShowGhostPieceOnTarget(GetTargetCoordinateAsString(GameManager.session.dice.FaceValue));

        }

        /// <summary>
        /// Spawns a see-through copy of this game piece on the target coordinate
        /// </summary>
        /// <param name="target">the x:Name value of the xaml element where to spawn the ghost piece</param>
        /// <returns>Completed Task when the spawn animation has finished</returns>
        private async Task ShowGhostPieceOnTarget(string target)
        {
            ghostPiece = new GamePieceControl(color, target);
            ghostPiece.Opacity = 0;
            Grid.SetRowSpan(ghostPiece, 2);
            Grid.SetColumnSpan(ghostPiece, 2);
            ghostPiece.MoveToNewGridCoordinate(target, 0, 0);
            await ghostPiece.ResizeAnimation(0.1, 1);
            GameManager.gameBoard.Children.Add(ghostPiece);
            ElementUtils.TransformDoubleProperty(ghostPiece, "Opacity", 0.4, 150);
            await ghostPiece.ResizeAnimation(0.3, 50);
            await ghostPiece.ResizeAnimation(1.0, 100);
            if(ghostPiece != null) ghostPiece.ResizeAnimation(0.7, 50);
        }

        /// <summary>
        /// Handles the end of hover event for the game piece. If a ghost piece has been spawned, it deletes the ghost piece.
        /// </summary>
        private async void GamePiece_EndHover(object sender, PointerRoutedEventArgs e)
        {
            if (!active) return;
            active = false;
            ResizeAnimation(1, 60);
            await RemoveGhostPieceFromBoard();
            active = true;
        }
        /// <summary>
        /// If a ghostpiece exist on the board, runs an animation for it and then removes it.
        /// </summary>
        /// <returns>Completed Task when the animation is completed</returns>
        private async Task RemoveGhostPieceFromBoard()
        {
            if (ghostPiece == null) return;

            ghostPiece.ResizeAnimation(0.1, 200);
            await ElementUtils.TransformDoubleProperty(ghostPiece, "Opacity", 0, 200);
            GameManager.gameBoard.Children.Remove(ghostPiece);
            ghostPiece = null;
            return;
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

            GameManager.gameBoard.Children.Remove(ghostPiece);
            GameManager.ActivePlayer().EndTurn();
            int steps = GameManager.session.dice.FaceValue;

            //Animates the movement by transformation
            ResizeAnimation(1.5, 100);
            Canvas.SetZIndex(this, 100);

            MainPage.walkingSoundManager.Play();

            while (steps-- > 0)
                await MoveStepsAsync(1, 300, 0, -1);

            int piecesOnSquare = 0,
                offsetZ;
            double offsetX,
                offsetY;

                        foreach (var p in Player().pieces)
                if (p.coordinate == coordinate)
                    piecesOnSquare++;
            
            switch(piecesOnSquare)
            {
                case 1:
                    offsetX = 0;
                    offsetY = -0.5;
                    offsetZ = 0;
                    break;
                case 2:
                    offsetX = 0.5;
                    offsetY = -0.25;
                    offsetZ = 1;
                    break;
                case 3:
                    offsetX = -0.5;
                    offsetY = -0.25;
                    offsetZ = 1;
                    break;
                case 4:
                    offsetX = 0;
                    offsetY = 0;
                    offsetZ = 2;
                    break;
                default:
                    offsetX = 0;
                    offsetY = -0.5;
                    offsetZ = 0;
                    break;
            }

            ResizeAnimation(1, 150);
            await AnimateToCoordinate(coordinate, 150, offsetX, offsetY);
            //await MoveStepsAsync(0, 150, 0, -0.5);

            MainPage.walkingSoundManager.Pause();
            MainPage.walkingSoundManager.Position = TimeSpan.Zero;

            //Resets the transform and actually moves the piece within the grid.
            MoveToNewGridCoordinate(coordinate, -1, 0, offsetZ);
            await ResetMovementTransform();
            AnimateToCoordinate(coordinate, 0, offsetX, offsetY);
            if (coordinate == "goalCoordinate")
            {
                GameManager.PlaySound("goalSound.mp3");
                ResizeAnimation(3, 1500);
                GameManager.GivePointsForPieceInGoal(this);
                await ElementUtils.TransformDoubleProperty(this, "Opacity", 0, 1500);
                GameManager.RemovePiece(this);
                GameManager.UpdateScoreBoard();
            }
            
            // Check for collision with opponent piece
            else if (!isInHomeBase() && !isInSafeZone())
                foreach (GamePieceControl piece in GameManager.gameBoard.Children.OfType<GamePieceControl>())
                    if (!GameManager.ActivePlayer().pieces.Contains(piece))
                        if (piece.coordinate == coordinate)
                            await FightScreenPopup.Collision(piece, this);

            GameManager.NextTurn();
        }

        /// <summary>
        /// Moves the game piece to a new grid coordinate with optional row and column offsets.
        /// </summary>
        /// <param name="endCoordinate">The end coordinate to move the game piece to.</param>
        /// <param name="rowOffset">Optional row offset. (integer)</param>
        /// <param name="columnOffset">Optional column offset. (integer)</param>
        private void MoveToNewGridCoordinate(string endCoordinate, int rowOffset = 0, int columnOffset = 0, int zOffset = 0)
        {
            (int targetRow, int targetColumn) = ElementUtils.GetElementRowAndColumn(GameManager.gameBoard, endCoordinate);
            targetRow += rowOffset;
            targetColumn += columnOffset;
            Grid.SetColumn(this, targetColumn);
            Grid.SetRow(this, targetRow);
            Canvas.SetZIndex(this, (targetRow * 2) + zOffset);
            coordinate = endCoordinate;
        }

        /// <summary>
        /// Resets the movement transform for the game piece, setting its translation transforms to zero.
        /// </summary>
        internal async Task ResetMovementTransform()
        {
            (this.RenderTransform as CompositeTransform).TranslateX = 0;
            (this.RenderTransform as CompositeTransform).TranslateY = 0;
            currentPoint = new Point(0, 0);
            await Task.Delay(25);
            return;
        }

        /// <summary>
        /// Transforms the game piece to move a specified number of steps with an animation over a set duration and with optional offsets.
        /// </summary>
        /// <param name="steps">The number of steps to move.</param>
        /// <param name="milliseconds">The duration of the animation in milliseconds.</param>
        /// <param name="offsetX">Optional X offset for each step. Any double between -1.0 and 1.0.</param>
        /// <param name="offsetY">Optional Y offset for each step. Any double between -1.0 and 1.0.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>In effect the offset is in </remarks>
        private Task MoveStepsAsync(int steps, int milliseconds, double offsetX = 0, double offsetY = 0)
        {
            string endCoordinate = GetTargetCoordinateAsString(steps);
            return AnimateToCoordinate(endCoordinate, milliseconds, offsetX, offsetY);
        }

        /// <summary>
        /// Transforms the game piece to move to a new coordinate with an animation over a set duration and with optional offsets.
        /// </summary>
        /// <param name="endCoordinate">The coordinate to move to.</param>
        /// <param name="milliseconds">The duration of the animation in milliseconds.</param>
        /// <param name="offsetX">Optional X offset for each step. Any double between -1.0 and 1.0.</param>
        /// <param name="offsetY">Optional Y offset for each step. Any double between -1.0 and 1.0.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>In effect the offset is in </remarks>
        public Task AnimateToCoordinate(string endCoordinate, int milliseconds, double offsetX, double offsetY)
        {
            this.RenderTransform = new CompositeTransform();
            Storyboard moveAnimation = new Storyboard();
            DoubleAnimation oDoubleAnimationX = new DoubleAnimation();
            DoubleAnimation oDoubleAnimationY = new DoubleAnimation();
            oDoubleAnimationX.Duration = new Duration(TimeSpan.FromMilliseconds(milliseconds));
            oDoubleAnimationY.Duration = new Duration(TimeSpan.FromMilliseconds(milliseconds));


            Point startPoint = currentPoint;
            Point endPoint = GetXYFromElementToCoordinate(this, endCoordinate, offsetX, offsetY);

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
        private Task ResizeAnimation(double factor, int milliseconds)
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
            var tcs = new TaskCompletionSource<bool>();

            resizeAnimation.Completed += (s, e) =>
            {
                tcs.SetResult(true);
            };
            resizeAnimation.Begin();

            return tcs.Task;
        }

        /// <summary>
        /// Repositions the piece to it's homeBase location within the active session's gameBoard Grid.
        /// </summary>
        public void MoveToHomeBase()
        {
            this.coordinate = color + "Base";
            var homeBase = GameManager.gameBoard.FindName(coordinate) as FrameworkElement;

            int baseColumn = Grid.GetColumn(homeBase);
            int baseRow = Grid.GetRow(homeBase);

            var player = this.Player();

            bool setRowAndColumn = false;

            //Move to another 'row' or 'column' if there is already a piece there.
            for (int row = baseRow + 4; row <= baseRow + 5 && !setRowAndColumn; row += 2)
            {
                for (int column = baseColumn + 1; column <= baseColumn + 7; column += 2)
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
