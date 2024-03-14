using FiaMedFight.Classes;
using FiaMedFight.Templates;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FiaMedFight
{
    /// <summary>
    /// Represents a user control for managing fights between game pieces.
    /// </summary>
    public sealed partial class FightScreenPopup : UserControl
    {
        private static Storyboard spinfightDiceAnimation;
        static GamePlayer challenger;
        static GamePlayer opponent;

        static Windows.UI.Xaml.Shapes.Rectangle challengerPawn;
        static Windows.UI.Xaml.Shapes.Rectangle opponentPawn;
        static TextBlock turnDescription;
        static TextBlock fightDescription;
        static Grid fightScreen;

        static bool isChallengersTurn;
        private static Dice fightDiceSix;
        static int turnsPerFight;

        static string challengerTurnDescription = "";
        static string opponentTurnDescription = "";

        static Style winStyle;
        static Style loseStyle;

        /// <summary>
        /// Initializes a new instance of the FightScreenPopup class.
        /// </summary>
        public FightScreenPopup()
        {
            InitializeComponent();

            fightDiceSix = new Dice(6);
            spinfightDiceAnimation = Resources["SpinAnimation"] as Storyboard;
            Storyboard.SetTarget(spinfightDiceAnimation, spinningFightDice);

            loseStyle = Resources["LosingCross"] as Style;
            winStyle = Resources["WinningStar"] as Style;

            challengerPawn = challengerFightPiece;
            opponentPawn = opponentFightPiece;

            turnDescription = fightingTurnDescription;
            fightDescription = fightingHeaderDescription;
            fightScreen = fightScreenOverlay;
        }

        /// <summary>
        /// Handles collision between game pieces initiating a fight.
        /// </summary>
        /// <param name="opponentPiece">The game piece controlled by the opponent player.</param>
        /// <param name="challengerPiece">The game piece controlled by the challenger player.</param>
        public static async Task Collision(GamePieceControl opponentPiece, GamePieceControl challengerPiece)
        {
            SetUpFight(opponentPiece, challengerPiece);

            // Regular fight, 1 turn, challenger (attacking player) wins a draw
            if (PlayerSelectionScreen.fightMode == 0)
            {
                fightDescription.Text = "~ Vid lika vinner attackeraren ~";
                challengerTurnDescription = "Attackerande spelare börjar. \nRulla tärningen!";
                opponentTurnDescription = "Motståndaren får försvara sig. \nRulla tärningen!";

                int challengerResult = await TakeTurn(challengerTurnDescription);
                int opponentResult = await TakeTurn(opponentTurnDescription);

                if (challengerResult >= opponentResult)
                    FinishFight(opponentPiece);
                else
                    FinishFight(challengerPiece);
            }

            // Best of three, challenger has +1 to hits, a draw repeats the turn
            else if (PlayerSelectionScreen.fightMode == 1)
            {
                StackPanel challengerRounds = fightScreen.FindName("challengerRounds") as StackPanel;
                StackPanel opponentRounds = fightScreen.FindName("opponentRounds") as StackPanel;

                turnsPerFight = 3;
                fightDescription.Text = "~ Bäst av tre ~";

                challengerTurnDescription = "Attackerande spelare slår med +1. \nRulla tärningen!";
                opponentTurnDescription = "Motståndaren får försvara sig. \nRulla tärningen!";


                List<GamePieceControl> winner = new List<GamePieceControl>();
                do
                {
                    ClearResult();

                    int challengerResult = await TakeTurn(challengerTurnDescription) + 1;
                    // Modifies the result to add +1 to result text after challenger turn
                    if (!isChallengersTurn) 
                    {
                        var specialResult = fightScreen.FindName("attackerResult") as TextBlock;
                        specialResult.Text = specialResult.Text + " + 1";
                    }

                    int opponentResult = await TakeTurn(opponentTurnDescription);

                    if (challengerResult == opponentResult) 
                        continue;

                    else if (challengerResult > opponentResult)
                    {
                        winner.Add(challengerPiece);
                        challengerRounds.Children.Add(new Image() { Style = winStyle } );
                        opponentRounds.Children.Add(new Image() { Style = loseStyle } );
                    }
                    else
                    {
                        winner.Add(opponentPiece);
                        opponentRounds.Children.Add(new Image() { Style = winStyle } );
                        challengerRounds.Children.Add(new Image() { Style = loseStyle } );
                    }
                    turnsPerFight--;

                } while (turnsPerFight > 0);

                int score = 0;
                foreach (GamePieceControl win in winner)
                {
                    if (win == challengerPiece) { score++; }
                    else { score--; }
                }
                if (score < 0)
                    await FinishFight(challengerPiece);
                else if (score > 0)
                    await FinishFight(opponentPiece);

                ClearFight(challengerRounds);
                ClearFight(opponentRounds);
            }

            else return;
        }

        private static void ClearResult()
        {
            var attackResultText = fightScreen.FindName("attackerResult") as TextBlock;
            attackResultText.Visibility = Visibility.Collapsed;
            var opponentResultText = fightScreen.FindName("opponentResult") as TextBlock;
            opponentResultText.Visibility = Visibility.Collapsed;
        }

        static void ClearFight(StackPanel rounds)
        {
            foreach (var child in rounds.Children)
            {
                rounds.Children.Remove(child);
            }
        }

        private static void SetUpFight(GamePieceControl opponentPiece, GamePieceControl challengerPiece)
        {
            opponent =  opponentPiece.Player();
            challenger = challengerPiece.Player();

            SetFighterImage(opponent.color, opponentPawn);
            SetFighterImage(challenger.color, challengerPawn);

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

        private static async Task FinishFight(GamePieceControl loser)
        {
            // get the visual elements from mainpage.xaml
            var popupElement = GameManager.gamePageGridFull.FindName("fightingPopup") as Popup;
            var dimmer = GameManager.gamePageGridFull.FindName("Dimmer") as Canvas;

            await Task.Delay(1500);
            // hide the popup for the fight
            turnDescription.Visibility = Visibility.Collapsed;
            if (popupElement.IsOpen) { popupElement.IsOpen = false; }
            dimmer.Visibility = Visibility.Collapsed;

            await loser.AnimateToCoordinate(loser.color + "Base", 1000, 0,0);
            loser.MoveToHomeBase();
            loser.ResetMovementTransform();
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
        
        private static void SetFighterImage(string color, Windows.UI.Xaml.Shapes.Rectangle fighter)
        {
            string imageName = $"ms-appx:///Assets/Pieces/{color}.png";
            fighter.Fill = new ImageBrush() { ImageSource = new BitmapImage(new Uri(imageName, UriKind.Absolute)) };
        }
   
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
                attackerResult.Visibility = Visibility.Visible;
                isChallengersTurn = false;
            }
            else
            {
                opponentResult.Text = $"{TranslateColourSwedish(opponent.color)}: " + fightDiceSix.FaceValue;
                opponentResult.Visibility = Visibility.Visible;
                await Task.Delay(1000);
                isChallengersTurn = true;
            }

            // Signal that the turn is completed
            if (turnCompleted != null)
            {
                turnCompleted.SetResult(fightDiceSix.FaceValue);
            }

            fightingTurnDescription.Visibility = Visibility.Visible;
            
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