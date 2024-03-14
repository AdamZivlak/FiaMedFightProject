using FiaMedFight.Templates;
using FiaMedFight.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FiaMedFight
{
    internal partial class GameOverDialog

    {
        /// <summary>
        /// A list of all elements added to the results popup. Used for removing all those elements.
        /// </summary>
        List<TextBlock> resultsEntries = new List<TextBlock>();

        public GameOverDialog()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // If the session is finished, display results popup. 
            if (GameManager.session.complete)
            {
                await Task.Delay(500);

                Dimmer.Visibility = Visibility.Visible;
                ResultsPopup.Visibility = Visibility.Visible;
                var sortedPlayers = GameManager.session.players.OrderByDescending(p => p.score).ToList();
                
                for (int i = 0; i < sortedPlayers.Count; i++) //Populate results list with one line per player.
                {
                    TextBlock position = new TextBlock();
                    TextBlock color = new TextBlock();
                    TextBlock points = new TextBlock();
                    if (Resources.TryGetValue("ResultsEntryStyle", out object retrievedStyle))
                    {
                        position.Style = retrievedStyle as Style;
                        color.Style = retrievedStyle as Style;
                        points.Style = retrievedStyle as Style;
                    }
                    Grid.SetColumn(position, 0);
                    Grid.SetRow(position, i + 1);
                    Grid.SetColumn(color, 1);
                    Grid.SetRow(color, i + 1);
                    Grid.SetColumn(points, 2);
                    Grid.SetRow(points, i + 1);

                    position.Text = $"{i + 1}.";
                    color.Text = $"{sortedPlayers[i].color}";
                    points.Text = $"{sortedPlayers[i].score}";

                    resultsEntries.Add(position); //Add objects to list to be able to easily refer to them later.
                    resultsEntries.Add(color);
                    resultsEntries.Add(points);

                    ResultsPopup.Children.Add(position);
                    ResultsPopup.Children.Add(color);
                    ResultsPopup.Children.Add(points);
                }

            }

        }
        private async void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(TextBlock entry in resultsEntries)
            {
                ResultsPopup.Children.Remove(entry);
            }
            Dimmer.Visibility = Visibility.Collapsed;
            ResultsPopup.Visibility = Visibility.Collapsed;
            await Task.Delay(1000);
            Frame.Navigate(typeof(MenuScreen));
        }
    }
}
