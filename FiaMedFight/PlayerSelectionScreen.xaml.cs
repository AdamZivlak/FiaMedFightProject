using FiaMedFight.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
        public static GameSession sess = new GameSession();

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerSelectionScreen"/> class.
        /// </summary>
        public PlayerSelectionScreen()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Navigates to the Game Board page when the button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void GameStartButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage mainPage = new MainPage();
            Frame.Navigate(typeof(MainPage));
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
            sess.AddPlayer(new GamePlayer("green", "Coordinate44"));
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
            sess.AddPlayer(new GamePlayer("yellow", "Coordinate18"));
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
            sess.AddPlayer(new GamePlayer("red", "Coordinate31"));
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
            sess.AddPlayer(new GamePlayer("blue", "Coordinate5"));
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

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }


}

