using FiaMedFight.Templates;
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
    public sealed partial class MenuScreen : Page
    {
        public MenuScreen()
        {
            this.InitializeComponent();
            ApplicationExitButton.Click += ApplicationExitButton_Click;
            DataContext = StringBindingCollection.GetCollection();
        }

        //stänger programmet
        private void ApplicationExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            //SelectPlayersPopup.Visibility = Visibility.Visible;
        }

        private void GameStartButton_Click(object sender, RoutedEventArgs e)
        {
            // Skapa en instans av MainPage
            MainPage mainPage = new MainPage();

            // Navigera till MainPage
            Frame.Navigate(typeof(MainPage));

        }

        private void RulesOpenButton_Click(object sender, RoutedEventArgs e)
        {
            RulesPopup.Visibility = Visibility.Visible;
        }
    }
}
