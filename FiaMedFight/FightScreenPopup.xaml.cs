using FiaMedFight.Classes;
using FiaMedFight.Templates;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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

namespace FiaMedFight
{
    public sealed partial class FightScreenPopup : UserControl
    {
        private static Storyboard spinfightDiceAnimation;
        static GamePlayer challenger;
        static GamePlayer opponent;
        static bool isChallengersTurn;
        private static Dice fightDiceSix;

        static TextBlock turnDescription;
        static Button fightingDiceButton;

        static int turnsPerFight;

        static string challengerTurnDescription = "hi";
        static string opponentTurnDescription = "ho";

        public FightScreenPopup()
        {
            InitializeComponent();

            fightDiceSix = new Dice(6);
            spinfightDiceAnimation = Resources["SpinAnimation"] as Storyboard;
            Storyboard.SetTarget(spinfightDiceAnimation, spinningFightDice);

            fightingDiceButton = fightingDice;
            turnDescription = fightingTurnDescription;
        }

        public static async Task Collision(GamePieceControl opponentPiece, GamePieceControl challengerPiece, int fightMode = 0)
        {
            SetUpFight(opponentPiece, challengerPiece);

            // regular fight, 1 turn, challenger (attacking player) wins a draw
            if (fightMode == 0)
            {
                challengerTurnDescription = "~Vid lika vinner attackeraren~\n   Attackerande spelare börjar. \nRulla tärningen!";
                opponentTurnDescription = "~Vid lika vinner attackeraren~\n   Motståndaren får försvara sig. \nRulla tärningen!";

                int challengerResult = await TakeTurn(challengerTurnDescription);
                int opponentResult = await TakeTurn(opponentTurnDescription);

                if (challengerResult >= opponentResult)
                    FinishFight(opponentPiece);
                else
                    FinishFight(challengerPiece);
            }

            // best of three, challenger has +1 to hits, a draw repeats the turn
            //else if (fightMode == 1)
            //{
            //    turnsPerFight = 3;
            //    challengerTurnDescription = "Bäst av tre: \nAttackerande spelare slår med +1. \nRulla tärningen!";
            //    opponentTurnDescription = "Bäst av tre: \nMotståndaren får försvara sig. \nRulla tärningen!";
            //    turnDescription.Text = challengerTurnDescription;

            //    List<GamePieceControl> winner = new List<GamePieceControl>();
            //    do
            //    {
            //        int challengerResult = TakeTurn() + 1;
            //        int opponentResult = TakeTurn();

            //        if (challengerResult > opponentResult)
            //            winner.Add(challengerPiece);
            //        else if (challengerResult == opponentResult)
            //            continue;
            //        else
            //            winner.Add(opponentPiece);

            //        turnsPerFight--;
            //    } while (turnsPerFight > 0);

            //    int score = 0;
            //    foreach (GamePieceControl win in winner)
            //    {
            //        if (win == challengerPiece) { score++; }
            //        else { score--; }
            //    }
            //    if (score < 0) 
            //        FinishFight(challengerPiece);
            //    else if (score > 0) 
            //        FinishFight(opponentPiece);
            //}

            else return;
        }

        private static void SetUpFight(GamePieceControl opponentPiece, GamePieceControl challengerPiece)
        {
            opponent =  opponentPiece.Player();
            challenger = challengerPiece.Player();

            //UpdateImageSource(opponent.color, challengingPlayerImage);

            // get the visual elements from mainpage.xaml
            var popupElement = GameManager.gamePageGridFull.FindName("fightingPopup") as Popup;
            var dimmer = GameManager.gamePageGridFull.FindName("Dimmer") as Canvas;

            // show the popup for the fight
            dimmer.Visibility = Visibility.Visible;
            if (!popupElement.IsOpen) { popupElement.IsOpen = true; }

            isChallengersTurn = true;
            turnDescription.Visibility = Visibility.Visible;
        }
        
        private static TaskCompletionSource<int> turnCompleted;
        private static async Task<int> TakeTurn(string text)
        {
            turnDescription.Text = text;

            turnCompleted = new TaskCompletionSource<int>();

            // Wait until the turn is completed (when dice-button is clicked and completed)
            int result = await turnCompleted.Task;
            turnCompleted = null;

            return result;
        }

        private static async void FinishFight(GamePieceControl loser)
        {
            // get the visual elements from mainpage.xaml
            var popupElement = GameManager.gamePageGridFull.FindName("fightingPopup") as Popup;
            var dimmer = GameManager.gamePageGridFull.FindName("Dimmer") as Canvas;

            await Task.Delay(1500);
            // hide the popup for the fight
            turnDescription.Visibility = Visibility.Collapsed;
            if (popupElement.IsOpen) { popupElement.IsOpen = false; }
            dimmer.Visibility = Visibility.Collapsed;

            await Task.Delay(1500);
            loser.MoveToHomeBase();
        }

        public static string TranslateColourSwedish(string colourEnglish)
        {
            switch(colourEnglish.ToLower())
            {
                case "red":
                    return "Röd";
                case "green":
                    return "Grön";
                case "yellow":
                    return "Gul";
                case "blue":
                    return "Blå";
                default:
                    return colourEnglish;
            }
        }
        private void UpdateImageSource(string color, Image image)
        {
            string imageName = $"ms-appx:///Assets/Pieces/{color}.png";
            image.Source = new BitmapImage(new Uri(imageName, UriKind.Absolute));
        }

        //static void CreateOpponent()
        //{

        //}

        //private void ClosePopup()
        //{
        //    // close the Popup
        //    Popup p = this.Parent as Popup;
        //    if (p != null) { p.IsOpen = false; }

        //}

        private async void fightingDice_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            fightingTurnDescription.Visibility = Visibility.Collapsed;

            fightDiceSix.RollThisDice(button);

            await SimpleDice_Animation(button);

            if (isChallengersTurn)
            {
                attackerResult.Text = $"{TranslateColourSwedish(challenger.color)}: " + fightDiceSix.FaceValue;
                isChallengersTurn = false;
                //fightingTurnDescription.Text = opponentTurnDescription;
            }
            else
            {
                opponentResult.Text = $"{TranslateColourSwedish(opponent.color)}: " + fightDiceSix.FaceValue;
                isChallengersTurn = true;
                //fightingTurnDescription.Text = challengerTurnDescription;
            }

            fightingTurnDescription.Visibility = Visibility.Visible;

            // Signal that the turn is completed
            if (turnCompleted != null)
            {
                turnCompleted.SetResult(fightDiceSix.FaceValue);
            }
        }

        /// <summary>
        /// Handles the Click animation of a Dice button.
        /// <list> This method performs several steps:
        /// <item> 1. It makes the button invisible and shows an image to indicate that the dice is rolling. </item>
        /// <item> 2. It starts a spinning animation to visually represent the dice roll. </item>
        /// <item> 3. Upon completion of the animation, it updates the UI to show the dice's face value and makes the button visible again. </item>
        /// </list>
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        private Task SimpleDice_Animation(Button button)
        {
            button.Visibility = Visibility.Collapsed;
            spinningFightDice.Visibility = Visibility.Visible;

            var tcs = new TaskCompletionSource<bool>();

            EventHandler<object> animationCompletedHandler = null;
            animationCompletedHandler = (sender, args) =>
            {
                // Unregister the event handler to ensure it's called only once.
                spinfightDiceAnimation.Completed -= animationCompletedHandler;

                spinningFightDice.Visibility = Visibility.Collapsed;
                button.Visibility = Visibility.Visible;

                // Mark the Task as completed.
                tcs.SetResult(true);
            };

            // Register the event handler.
            spinfightDiceAnimation.Completed += animationCompletedHandler;

            spinfightDiceAnimation.Begin();
            return tcs.Task;

        }
    }
}