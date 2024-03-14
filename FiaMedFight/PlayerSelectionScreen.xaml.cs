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
using static System.Net.Mime.MediaTypeNames;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FiaMedFight
{
    /// <summary>
    /// Represents the game option selection screen of the application. This is where the user selects number of players and their color.
    /// </summary>
    public sealed partial class PlayerSelectionScreen : Page
    {
        /// <summary>
        /// The game session used for player selection, to store the selected players until game launch.
        /// </summary>
        public static GameSession sess;

        /// <summary>
        /// Declare the timer variable at class level
        /// </summary>
        private DispatcherTimer timer;

        public static int fightMode = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerSelectionScreen"/> class.
        /// </summary>
        public PlayerSelectionScreen()
        {
            this.InitializeComponent();

            // Create a DispatcherTimer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2); // Set the interval .. seconds
            timer.Tick += Timer_Tick; // Add event handler for the Tick event
            timer.Start(); // Start the timer

            //GameManager.ClearSession();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Adds operations each time the page is navigated to. 
            sess = new GameSession();
        }

        /// <summary>
        /// Event handler for the click event of the game start button.
        /// </summary>
        /// <param name="sender">The object that fired the event.</param>
        /// <param name="e">Event arguments containing information about the event.</param>
        private async void GameStartButton_Click(object sender, RoutedEventArgs e)
        {
            var exitAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(exitAnimation, this); // Set the target of the animation to the PlayerSelectionScreen
            Storyboard.SetTargetProperty(exitAnimation, "(UIElement.Opacity)"); // Set the target property to Opacity

            // Create a storyboard for the exit animation
            var exitStoryboard = new Storyboard();
            exitStoryboard.Children.Add(exitAnimation);

            // Begin the exit animation on the PlayerSelectionScreen
            exitStoryboard.Begin();

            // Wait for the exit animation to complete
            await Task.Delay(500);

            GameManager.LoadSession(sess);
            // Navigate to the MainPage
            Frame.Navigate(typeof(MainPage), null, new SuppressNavigationTransitionInfo());

            // Define an entrance animation for the MainPage (eases in)
            var entranceAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTarget(entranceAnimation, Frame.Content as UIElement); // Set the target of the animation to the MainPage
            Storyboard.SetTargetProperty(entranceAnimation, "(UIElement.Opacity)"); // Set the target property to Opacity

            // Create a storyboard for the entrance animation
            var entranceStoryboard = new Storyboard();
            entranceStoryboard.Children.Add(entranceAnimation);

            // Begin the entrance animation on the MainPage
            entranceStoryboard.Begin();
        }

        /// <summary>
        /// Navigates to the menu screen when the button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void MenuOpenButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuScreen));
        }

        /// <summary>
        /// Adds a player with green color and a starting position to the game session.
        /// Shows the green player's image on the selection screen.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void GreenButton_Click(object sender, RoutedEventArgs e)
        {
            sess.AddPlayer(new GamePlayer("green", "Coordinate42"));
            GreenImage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Adds a player with yellow color and a starting position to the game session.
        /// Shows the yellow player's image on the selection screen.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void YellowButton_Click(object sender, RoutedEventArgs e)
        {
            sess.AddPlayer(new GamePlayer("yellow", "Coordinate16"));
            YellowImage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Adds a player with red color and a starting position to the game session.
        /// Shows the red player's image on the selection screen.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void RedButton_Click(object sender, RoutedEventArgs e)
        {
            sess.AddPlayer(new GamePlayer("red", "Coordinate29"));
            RedImage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Adds a player with blue color and a starting position to the game session.
        /// Shows the blue player's image on the selection screen.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void BlueButton_Click(object sender, RoutedEventArgs e)
        {
            sess.AddPlayer(new GamePlayer("blue", "Coordinate3"));
            BlueImage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Clears the player list in the game session and resets player images on the selection screen.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void RedoPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            sess.players.Clear();
            ResetImages();
        }

        /// <summary>
        /// Resets all player images to hidden.
        /// </summary>
        private void ResetImages()
        {
            GreenImage.Visibility = Visibility.Collapsed;
            YellowImage.Visibility = Visibility.Collapsed;
            RedImage.Visibility = Visibility.Collapsed;
            BlueImage.Visibility = Visibility.Collapsed;
        }

        /*private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }*/

        /// <summary>
        /// Event handler for when the mouse pointer enters the stack panel.
        /// </summary>
        /// <param name="sender">The object that fired the event.</param>
        /// <param name="e">Event arguments containing information about the pointer event.</param>
        private void StackPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            infoImage.Visibility = Visibility.Visible;
            timer.Stop();
        }

        /// <summary>
        /// Event handler for when the mouse pointer exits the stack panel.
        /// </summary>
        /// <param name="sender">The object that fired the event.</param>
        /// <param name="e">Event arguments containing information about the pointer event.</param>
        private void StackPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            infoImage.Visibility = Visibility.Collapsed;
            /* timer.Start(); // Enable this if the image should keep changing opacity */
        }

        /// <summary>
        /// Event handler for the timer tick event.
        /// </summary>
        /// <param name="sender">The object that fired the event.</param>
        /// <param name="e">Event arguments containing information about the event.</param>
        private void Timer_Tick(object sender, object e)
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = 0.3; // Start opacity
            opacityAnimation.To = 1; // End opacity
            opacityAnimation.Duration = TimeSpan.FromSeconds(1.5); // Animation duration

            // Apply the animation to the Opacity property of the image
            Storyboard.SetTarget(opacityAnimation, cartoonFigure);
            Storyboard.SetTargetProperty(opacityAnimation, "Opacity");

            // Create a storyboard and add the opacity animation to it
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(opacityAnimation);

            // Begin the storyboard
            storyboard.Begin();
        }

        private void GameMode2Button_Click(object sender, RoutedEventArgs e)
        {
            fightMode = 1;
        }

        private void GameMode1Button_Click(object sender, RoutedEventArgs e)
        {
            fightMode = 0;
        }
    }
}