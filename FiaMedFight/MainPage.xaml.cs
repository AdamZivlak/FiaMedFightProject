using FiaMedFight.Classes;
using static FiaMedFight.PlayerSelectionScreen;
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FiaMedFight
{
    /// <summary>
    /// Represents the Game Screen page of the application.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// An animation for spinning the Dice.
        /// </summary>
        static Storyboard spinAnimation;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            spinAnimation = this.Resources["SpinAnimation"] as Storyboard;
            Storyboard.SetTarget(spinAnimation, SpinningImage);
        }

        /// <summary>
        /// Handles the Loaded event of the MainPage. Initializes the gameboard to GameManager and adds game pieces for each player in the session.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            GameManager.gameBoard = gameBoardGrid;
            GameManager.activePage = this;

            GameManager.LoadSession(sess);

            foreach (GamePlayer player in sess.players)
            {
                for (int i = 0; i < 4; i++)
                    GameManager.AddGamePieceControl(player.color);
            }//For debugging replace with: GameManager.AddGamePieceControl(player.color, player.color + "SafeCoordinate" + (i + 1));
            GameManager.ActivateScoreBoard();
            GameManager.GUIChangeActivePlayer();

        }  

        /// <summary>
        /// Handles the Click event of a Dice button. This method initiates the dice roll process,
        /// calls and awaits an animation function, and updates the UI to reflect the roll's outcome.
        /// <list> This method performs several steps:
        /// <item> 1. It casts the sender to a Button type and rolls the dice associated with it. </item>
        /// <item> 2. It calls a function to show an animation</item>
        /// <item> 3. After awaiting the animation it calls a game logic method in GameManager to signal the dice have been rolled.</item>
        /// </list>
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private async void SimpleDice_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!GameManager.session.dice.active) return;

            GameManager.session.dice.Deactivate();

            var button = sender as Button;
            GameManager.session.dice.RollThisDice(button);

            await SimpleDice_Animation(button);
            
            GameManager.PlayerRolledDice();
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
            SpinningImage.Visibility = Visibility.Visible;

            var tcs = new TaskCompletionSource<bool>();

            EventHandler<object> animationCompletedHandler = null;
            animationCompletedHandler = (sender, args) =>
            {
                // Unregister the event handler to ensure it's called only once.
                spinAnimation.Completed -= animationCompletedHandler;

                SpinningImage.Visibility = Visibility.Collapsed;
                button.Visibility = Visibility.Visible;
                ResultText.Text = "You rolled: " + GameManager.session.dice.FaceValue;

                // Mark the Task as completed.
                tcs.SetResult(true);
            };

            // Register the event handler.
            spinAnimation.Completed += animationCompletedHandler;

            spinAnimation.Begin();
            return tcs.Task;
        }

        /// <summary>
        /// Returns to the menu screen when the button is clicked without ending the game session.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuScreen));
        }

        /// <summary>
        /// Ends current game session when the button is clicked. Displays the Game Over image and the returns to the menu page.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private async void QuitGameButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GameOverDialog));
            await Task.Delay(4000);
            Frame.Navigate(typeof(MenuScreen));
        }
   }
}