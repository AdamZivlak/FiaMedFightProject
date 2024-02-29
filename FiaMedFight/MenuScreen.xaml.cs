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
            MenuExitButton.Click += MenuExitButton_Click;
            DataContext = StringBindingCollection.GetCollection();
        }

        private void MenuExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Här ska vi skriva kod som stänger hela programmet! Mustafa kan göra det här?
            Application.Current.Exit();
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
            // Här ska öppnas en popup-ruta med regler, Minna fixar detta
        }
    }
}
