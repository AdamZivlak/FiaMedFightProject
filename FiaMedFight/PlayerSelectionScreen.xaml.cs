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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayerSelectionScreen : Page
    {
        public PlayerSelectionScreen()
        {
            this.InitializeComponent();
        }

        private void GameStartButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage mainPage = new MainPage();
            Frame.Navigate(typeof(MainPage));
        }

        private void GreenButton_Click(object sender, RoutedEventArgs e)
        {
            GreenImage.Visibility = Visibility.Visible;
        }

        private void YellowButton_Click(object sender, RoutedEventArgs e)
        {
            YellowImage.Visibility = Visibility.Visible;
        }

        private void RedButton_Click(object sender, RoutedEventArgs e)
        {
            RedImage.Visibility = Visibility.Visible;
        }

        private void BlueButton_Click(object sender, RoutedEventArgs e)
        {
            BlueImage.Visibility = Visibility.Visible;
        }

        private void RedoPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            ResetImages();
        }

        private void ResetImages()
        {
            // Set the Visibility property of all images to Collapsed
            GreenImage.Visibility = Visibility.Collapsed;
            YellowImage.Visibility = Visibility.Collapsed;
            RedImage.Visibility = Visibility.Collapsed;
            BlueImage.Visibility = Visibility.Collapsed;
        }
    }
}
