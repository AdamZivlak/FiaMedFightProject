using FiaMedFight.Classes;
using System;
using System.Collections.Generic;
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

namespace FiaMedFight
{
    public sealed partial class FightScreenPopup : UserControl
    {
        private static Storyboard spinfightDiceAnimation;
        static string challenger;
        static string opponent;
        static bool isChallengersTurn;

        static string challengerTurnDescription = "Aktiv spelare attackerar med +1. \nRulla tärningen!";
        static string opponentTurnDescription = "Motståndaren får försvara sig. \nRulla tärningen!";

        public FightScreenPopup()
        {
            this.InitializeComponent();
            //challenger = GameManager.ActivePlayer();
            // create opponent
            spinfightDiceAnimation = this.Resources["SpinAnimation"] as Storyboard;
            Storyboard.SetTarget(spinfightDiceAnimation, spinningFightDice);
        }

        public static void BeginFight()
        {
            // get the elements from mainpage.xaml
            var popupElement = GameManager.gamePageGridFull.FindName("fightingPopup") as Popup;
            var dimmer = GameManager.gamePageGridFull.FindName("Dimmer") as Canvas;

            // show the popup for the fight
            dimmer.Visibility = Visibility.Visible;
            if (!popupElement.IsOpen) { popupElement.IsOpen = true; }

            challenger = "Röd";
            opponent = "Grön";

            isChallengersTurn = true;
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

            int result = Dice.RollAnyDice(6);
            Dice.ChangeDiceFace(button, result, Dice.imageSixSidedPaths);
            await SimpleDice_Animation(button);

            if (isChallengersTurn)
            {
                attackerResult.Text = $"{challenger}: " + result + " + 1";
                isChallengersTurn = false;
                fightingTurnDescription.Text = opponentTurnDescription;
            }
            else
            {
                opponentResult.Text = $"{opponent}: " + result;
                isChallengersTurn = true;
                fightingTurnDescription.Text = challengerTurnDescription;
            }
            fightingTurnDescription.Visibility = Visibility.Visible;

            //GameManager.PlayerRolledDice();
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