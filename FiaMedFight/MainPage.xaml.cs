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
        /// Handles the Loaded event of the MainPage.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize this page to GameManager:
            GameManager.gameBoard = gameBoardGrid;

            // Setup test session:
            GameSession session = new GameSession();
            session.AddPlayer(new GamePlayer("green", "Coordinate44"));
            session.AddPlayer(new GamePlayer("blue", "Coordinate5"));
            session.AddPlayer(new GamePlayer("yellow", "Coordinate18"));
            session.AddPlayer(new GamePlayer("red", "Coordinate31"));
            GameManager.StartGame(session);

            // Spawn test pieces (also adds them to each GamePlayer's list of pieces):
            GameManager.AddGamePieceControl("red");
            GameManager.AddGamePieceControl("red");
            GameManager.AddGamePieceControl("red");
            GameManager.AddGamePieceControl("blue");
            GameManager.AddGamePieceControl("blue");
            GameManager.AddGamePieceControl("blue");
            GameManager.AddGamePieceControl("green");
            GameManager.AddGamePieceControl("green");
            GameManager.AddGamePieceControl("green");
            GameManager.AddGamePieceControl("yellow");
            GameManager.AddGamePieceControl("yellow");
            GameManager.AddGamePieceControl("yellow");
        }

        /// <summary>
        /// Handles the Click event of a Dice button. This method initiates the dice roll process,
        /// starts an animation, and updates the UI to reflect the roll's outcome.
        /// <list> This method performs several steps:
        /// <item> 1. It casts the sender to a Button type and rolls the dice associated with it. </item>
        /// <item> 2. It makes the button invisible and shows an image to indicate that the dice is rolling. </item>
        /// <item> 3. It starts a spinning animation to visually represent the dice roll. </item>
        /// <item> 4. Upon completion of the animation, it updates the UI to show the dice's face value and makes the button visible again. </item>
        /// </list>
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void SimpleDice_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!GameManager.session.dice.active) return;

            GameManager.session.dice.Deactivate();

            var button = sender as Button;
            GameManager.session.dice.RollThisDice(button);

            button.Visibility = Visibility.Collapsed;
            SpinningImage.Visibility = Visibility.Visible;

            spinAnimation.Begin();
            spinAnimation.Completed += delegate (object self, object btn)
            {
                SpinningImage.Visibility = Visibility.Collapsed;
                button.Visibility = Visibility.Visible;
                ResultText.Text = "You rolled: " + GameManager.session.dice.FaceValue;
                GameManager.PlayerRolledDice();
            };
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuScreen));
        }

        private async void QuitGameButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GameOverDialog));
            await Task.Delay(5000);
            Frame.Navigate(typeof(MenuScreen));
        }
   }
}