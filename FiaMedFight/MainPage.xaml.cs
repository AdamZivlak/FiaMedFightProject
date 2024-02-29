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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FiaMedFight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static Storyboard spinAnimation;
        static Storyboard moveAnimation;
        static Page activePage;
        static Grid targetGrid;
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            
            // Get the storyboard animation from the resource dictionary
            spinAnimation = this.Resources["SpinAnimation"] as Storyboard;
            Storyboard.SetTarget(spinAnimation, SpinningImage);            
        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            activePage = (Window.Current.Content as Frame)?.Content as Page;
            targetGrid = activePage.FindName("gameBoardGrid") as Grid;
        }

        /// <summary>
        /// Handles the Click event of a Dice button. This method initiates the dice roll process,
        /// starts an animation, and updates the UI to reflect the roll's outcome.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        /// <remarks>
        /// This method performs several steps:
        /// 1. It casts the sender to a Button type and rolls the dice associated with it.
        /// 2. It makes the button invisible and shows an image to indicate that the dice is rolling.
        /// 3. It starts a spinning animation to visually represent the dice roll.
        /// 4. Upon completion of the animation, it updates the UI to show the dice's face value and makes the button visible again.
        /// </remarks>
        private void SimpleDice_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {            
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
            };
        }
    }
}
