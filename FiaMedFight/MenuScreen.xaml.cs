using FiaMedFight.Templates;
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
using Windows.Media.Playback;
using Windows.Media.Core;


namespace FiaMedFight
{
    /// <summary>
    /// Represents the menu screen of the application.
    /// </summary>
    public sealed partial class MenuScreen : Page
    {
        public static MediaPlayer clickSoundManager;

        /// <summary>
        /// Collection of string bindings for game rules.
        /// </summary>
        static StringBindingCollection ruleStrings = new StringBindingCollection(0);

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuScreen"/> class.
        /// </summary>
        public MenuScreen()
        {
            this.InitializeComponent();
            ApplicationExitButton.Click += ApplicationExitButton_Click;
            clickSoundManager = new MediaPlayer();
            GameManager.PreloadSoundManagers("clickSound.mp3", clickSoundManager);
        }

        /// <summary>
        /// Click to exit the application.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private async void ApplicationExitButton_Click(object sender, RoutedEventArgs e)
        {
            GameManager.PlayClickSound(clickSoundManager);
            await Task.Delay(500);
            Application.Current.Exit();
        }


        /// <summary>
        /// Click to open the game options screen, to select number of players for the new game.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private async void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameManager.PlayClickSound(clickSoundManager);

            //await Task.Delay(500);

            // Define an exit animation for the PlayerSelectionScreen (eases out)
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

            // Navigate to the MainPage
            Frame.Navigate(typeof(PlayerSelectionScreen), null, new SuppressNavigationTransitionInfo());

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
        /// Click to navigate to the main game page.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ResumeGameButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: populate the gameboard with number of players and pieces here?
            MenuScreen.clickSoundManager.Play();
            GameManager.PlaySound("combatSound.mp3");

            Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Click to show the game rules popup.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void RulesOpenButton_Click(object sender, RoutedEventArgs e)
        {
            MenuScreen.clickSoundManager.Play();
            Dimmer.Visibility = Visibility.Visible;
            RulesPopup.Visibility = Visibility.Visible;

            PrevButton.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Visible;

            RulesOpenButton.Tag = 0;
            RulesHeaderTextBlock.Text = ruleStrings.Title[0];
            RulesBodyTextBlock.Text = ruleStrings.Rules[0];
        }

        /// <summary>
        /// Click to close the game rules popup.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            MenuScreen.clickSoundManager.Play();
            RulesPopup.Visibility = Visibility.Collapsed;
            Dimmer.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Click to navigate to the next page of game rules.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void RulesNextButton_Click(object sender, RoutedEventArgs e)
        {
            MenuScreen.clickSoundManager.Play();
            if (PrevButton.Visibility != Visibility.Visible)
                PrevButton.Visibility = Visibility.Visible;

            int pageIndex = (int)RulesOpenButton.Tag + 1;
            RulesOpenButton.Tag = pageIndex;

            RulesHeaderTextBlock.Text = ruleStrings.Title[pageIndex];
            RulesBodyTextBlock.Text = ruleStrings.Rules[pageIndex];

            if (pageIndex + 1 == ruleStrings.Rules.Count)
                NextButton.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Click to navigate to the previous page of game rules.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void RulesPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            MenuScreen.clickSoundManager.Play();
            if (NextButton.Visibility != Visibility.Visible)
                NextButton.Visibility = Visibility.Visible;

            int pageIndex = (int)RulesOpenButton.Tag - 1;
            RulesOpenButton.Tag = pageIndex;

            RulesHeaderTextBlock.Text = ruleStrings.Title[pageIndex];
            RulesBodyTextBlock.Text = ruleStrings.Rules[pageIndex];

            if (pageIndex == 0)
                PrevButton.Visibility = Visibility.Collapsed;
        }
    }
}
