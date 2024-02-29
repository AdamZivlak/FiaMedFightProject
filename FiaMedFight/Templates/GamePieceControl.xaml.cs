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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FiaMedFight.Templates
{
    public partial class GamePieceControl : UserControl
    {
        bool active = true;
        public string coordinate = "Coordinate1";
        public Point currentPoint = new Point(0, 0);
        public GamePieceControl()
        {
            this.InitializeComponent();
        }
        public void Activate()
        {
            //should make the piece clickable and movable
            this.active = true;
        }

        public void Deactivate()
        {
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

        public Point GetXYFromCoordinate(string gridName, string GameLocationName)
        {
            Page activePage = (Window.Current.Content as Frame)?.Content as Page;

            var targetControl = activePage.FindName(GameLocationName) as UIElement;
            var targetGrid = activePage.FindName(gridName) as Grid;

            if (targetControl != null && targetGrid != null)
            {
                GeneralTransform transform = targetControl.TransformToVisual(this);
                Point position = transform.TransformPoint(new Point(0, 0));
                Debug.WriteLine(position);
                return position;
            }
            return new Point(double.NaN, double.NaN);
        }
        public string GetEndCoordinateString(int dice_result)
        {
            int new_pos = GetCoordinateInt() + dice_result;
            if (new_pos > 52) new_pos -= 52;
            return "Coordinate" + new_pos;
        }
        private void GamePiece_OnHover(object sender, PointerRoutedEventArgs e)
        {
            var piece = sender as GamePieceControl;
            piece.Opacity = 0.5;
        }
        private void GamePiece_EndHover(object sender, PointerRoutedEventArgs e)
        {
            var piece = sender as GamePieceControl;
            piece.Opacity = 1;
        }

        private void GamePiece_Pressed(object sender, PointerRoutedEventArgs e) { }
        async private void GamePiece_Released(object sender, PointerRoutedEventArgs e) {
            var piece = sender as GamePieceControl;
            if (!piece.active) return;

            int diceRolled = GameManager.session.dice.FaceValue;

            while (diceRolled-- > 0)
            {
                await piece.MoveStepAsync();
            }
        }

        private Task MoveStepAsync()
        {
            this.RenderTransform = new CompositeTransform();
            Storyboard moveAnimation = new Storyboard();
            DoubleAnimation oDoubleAnimationX = new DoubleAnimation();
            DoubleAnimation oDoubleAnimationY = new DoubleAnimation();
            oDoubleAnimationX.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            oDoubleAnimationY.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            int diceRolled = GameManager.session.dice.FaceValue;

            string end_coordinate = this.GetEndCoordinateString(1);

            Point startPoint = this.currentPoint;
            Point endPoint = this.GetXYFromCoordinate("gameBoardGrid", end_coordinate);

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
            this.coordinate = end_coordinate;
            return tcs.Task;
        }



    }
}
