using FiaMedFight.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Diagnostics;
using System.ServiceModel;
using Windows.Media.Control;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media;

namespace FiaMedFight.Classes
{
    public static class GameManager
    {
        /// <summary>
        /// Manages the overall game logic, including starting the game, managing players, and handling game piece movements on the board.
        /// </summary>
        internal static GameSession session { get; set; }
        /// <summary>
        /// The game board grid where game pieces are placed and moved.
        /// </summary>
        public static Grid gameBoard { get; set; }

        public static MainPage activePage { get; set; }

        /// <summary>
        /// Initiates a new game session, setting up the game environment.
        /// </summary>
        /// <param name="session">The game session to start.</param>
        internal static void LoadSession(GameSession sess)
        {
            GameManager.session = sess;
        }

        public static void RollDice(object sender)
        {
        }
        /// <summary>
        /// Adds a game piece control to the specified location on the game board. Adds the game piece to the active sessions players list of pieces.
        /// </summary>
        /// <param name="color">The color of the game piece and owner player..</param>
        /// <param name="coordinate">The coordinate on the game board where the game piece should be placed.</param>
        public static void AddGamePieceControl(string color, string coordinate)
        {
            var spawnLocation = gameBoard.FindName(coordinate) as FrameworkElement;

            var gamePieceControl = new GamePieceControl(color, coordinate);

            Grid.SetColumn(gamePieceControl, Grid.GetColumn(spawnLocation));
            Grid.SetRow(gamePieceControl, Grid.GetRow(spawnLocation) - 1);
            Canvas.SetZIndex(gamePieceControl, 1);
            Grid.SetColumnSpan(gamePieceControl, 2);
            Grid.SetRowSpan(gamePieceControl, 2);
            gamePieceControl.Opacity = 1;
            
            gameBoard.Children.Add(gamePieceControl);

            if (session != null)
            {
                GamePlayer player = FindOrCreatePlayer(color);
                player.pieces.Add(gamePieceControl);
            }
        }

        /// <summary>
        /// Adds a game piece control to the default 'homeBase' location on the board. Adds the game piece to the active sessions players list of pieces.
        /// </summary>
        /// <param name="color">The color of the game piece and owner player..</param>
        public static void AddGamePieceControl(string color)
        {
            GamePlayer player = FindOrCreatePlayer(color);

            var gamePieceControl = new GamePieceControl(color);
            Canvas.SetZIndex(gamePieceControl, 1);
            Grid.SetColumnSpan(gamePieceControl, 2);
            Grid.SetRowSpan(gamePieceControl, 2);
            gamePieceControl.Opacity = 1;
            gamePieceControl.MoveToHomeBase();
            gameBoard.Children.Add(gamePieceControl);

            player.pieces.Add(gamePieceControl);
        }               

        /// <summary>
        /// Finds an existing player by color or creates a new one if not found.
        /// </summary>
        /// <param name="color">The color used to identify the player.</param>
        /// <returns>A <see cref="GamePlayer"/> instance corresponding to the specified color.</returns>
        public static GamePlayer FindOrCreatePlayer(string color, string firstCoordinateAfterHomeBase = "Coordinate1")
        {
            if (session == null) session = new GameSession();
            
            var player = session.players.FirstOrDefault(listedPlayer => listedPlayer.color == color);

            if (player == null)
            {
                player = new GamePlayer(color, firstCoordinateAfterHomeBase);
                session.players.Add(player);
            }
            return player;
        }

        /// <summary>
        /// Returns the active player from the current session
        /// </summary>
        /// <returns></returns>
        public static GamePlayer ActivePlayer()
        {
            return session.players[session.activePlayerIndex];
        }

        /// <summary>
        /// Advances the game to the next player's turn.
        /// <list type="bullet">
        /// <item>Deactivates all pieces belonging to the current active player.</item>
        /// <item>Changes the active player to the next player in the player list.</item>
        /// <item>Updates the UI to display the name of the newly active player.</item>
        /// <item>Activates the dice for the new active player.</item>
        /// </list>
        /// </summary>
        public static void NextTurn()
        {
            int numberOfPlayers = session.players.Count;

            ActivePlayer().EndTurn(); // Deactivate all pieces
            session.activePlayerIndex = (session.activePlayerIndex + 1) % numberOfPlayers;

            if (ActivePlayer().pieces.Count == 0) { NextTurn(); } //End turn before rolling dice if all pieces in goal

            GUIChangeActivePlayer();

            session.dice.Activate();
        }

        /// <summary>
        /// Changes the GUI to show who is the active player.
        /// TODO: MBG-111
        /// </summary>
        public static void GUIChangeActivePlayer()
        {
            var activePlayerTextBox = gameBoard.FindName("ActivePlayerText") as TextBlock;
            activePlayerTextBox.Text = "Active Player: " + ActivePlayer().color;
        }


        /// <summary>
        /// Makes the scoreboards for players in the game visible.
        /// </summary>
        public static void ActivateScoreBoard()
        {
            for (int i = 0; i < session.players.Count; i++)
            {
                var player = session.players[i];
                string colorBrush = player.color + "Brush";

                var scoreboard = gameBoard.FindName("scorePlayer" + i) as TextBlock;
                scoreboard.Text = player.color + ": " + player.score.ToString("D5");
                if (activePage.Resources.TryGetValue(colorBrush, out object brush))
                {
                    scoreboard.Foreground = brush as SolidColorBrush;
                }
            }
        }

        /// <summary>
        /// Updates the GUI to show the score value for the active player
        /// </summary>
        public static void UpdateScoreBoard()
        {
            var player = ActivePlayer();
            var scoreboard = gameBoard.FindName("scorePlayer" + session.activePlayerIndex) as TextBlock;
            scoreboard.Text = player.color + ": " + player.score.ToString("D5");
        }

        /// <summary>
        /// Handles the action after a player rolls the dice.
        /// This method performs the following steps:
        /// <list type="number">
        /// <item>Deactivates the dice.</item>
        /// <item>Activates all pieces belonging to the current active player.</item>
        /// <item>Checks if any of the active player's pieces can move. If none can move, advances to the next player's turn.</item>
        /// </list>
        /// </summary>
        public static void PlayerRolledDice()
        {
            ActivePlayer().StartTurn(); // Activate all pieces except those in home base

            if (!ActivePlayer().pieces.Any(p => p.active == true))
            {
                NextTurn();
            }
            
        }

        /// <summary>
        /// Triggered when a piece enters the goal area. Adds points to owning player, plays animations and removes piece.
        /// </summary>
        /// <param name="piece">The GamePieceControl that triggered the event</param>
        public static async void GivePointsForPieceInGoal(GamePieceControl piece)
        {
            GamePlayer player = piece.Player();
            int points = 0, 
                bonus = 0, 
                numPlayers = session.players.Count;
            string bonusMessage = "", brushColor = "";

            foreach (var p in session.players)
            {
                points += p.pieces.Count * 100;
            }
            player.AddPoints(points);
            activePage.ShowPoints(points);
            await Task.Delay(400);

            //Give bonus points for first three pieces to reach goal
            switch (session.numPiecesReachedGoal)
            {
                case 0:
                    bonus = 200 * numPlayers;
                    bonusMessage = $"GOLD BONUS! {bonus} POINTS!";
                    brushColor = "goldBrush";
                    break;
                case 1:
                    bonus = 100 * numPlayers;
                    bonusMessage = $"SILVER BONUS! {bonus} POINTS!";
                    brushColor = "silverBrush";
                    break;
                case 2:
                    bonus = 50 * numPlayers;
                    bonusMessage = $"BRONZE BONUS! {bonus} POINTS!";
                    brushColor = "bronzeBrush";
                    break;
                default:
                    break;
            }
            if(bonus > 0)
            {
                player.AddPoints(bonus);
                activePage.ShowBonus(bonusMessage, brushColor);
            }

            //Give bonus points for first team to reach goal with all pieces
            if (player.pieces.Count == 1)
            {
                if (session.numFullTeamsReachedGoal == 0)
                { 
                    bonus = 200 * numPlayers;
                    bonusMessage = $"WINNER BONUS! {bonus} POINTS!";
                    brushColor = "goldBrush";
                    player.AddPoints(bonus);
                }
                else
                {
                    bonusMessage = $"{player.color.ToUpper()} HAS FINISHED!";
                    brushColor = "goldBrush";
                }
                activePage.ShowBonus(bonusMessage, brushColor);

                session.numFullTeamsReachedGoal++;

                if (session.numFullTeamsReachedGoal == numPlayers -1) //If only one player has pieces left on the board, end the game
                {
                    session.complete = true;
                    await Task.Delay(1000);
                    activePage.Frame.Navigate(typeof(GameOverDialog));

                }
            }
            session.numPiecesReachedGoal += 1;
        }

        /// <summary>
        /// Removes a GamePieceControl both from the GamePlayer object's list and from the xaml Page
        /// </summary>
        /// <param name="piece"></param>
        public static void RemovePiece(GamePieceControl piece)
        {
            GameManager.ActivePlayer().pieces.Remove(piece);
            GameManager.gameBoard.Children.Remove(piece);
        }

    }
}

