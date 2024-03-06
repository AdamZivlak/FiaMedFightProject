﻿using FiaMedFight.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

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

        /// <summary>
        /// Initiates a new game session, setting up the game environment.
        /// </summary>
        /// <param name="session">The game session to start.</param>
        internal static void StartGame(GameSession session)
        {
            GameManager.session = session;
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
        /// Finds an existing player by color or creates a new one if not found.
        /// </summary>
        /// <param name="color">The color used to identify the player.</param>
        /// <returns>A <see cref="GamePlayer"/> instance corresponding to the specified color.</returns>
        public static GamePlayer FindOrCreatePlayer(string color)
        {
            var player = session.players.FirstOrDefault(listedPlayer => listedPlayer.color == color);

            if (player == null)
            {
                player = new GamePlayer(color);
                session.players.Add(player);
            }

            return player;
        }
        /// <summary>
        /// Retrieves the row and column indices of a specified element by its name within the game board.
        /// </summary>
        /// <param name="childElementName">The name of the child element within the game board grid.</param>
        /// <returns>A tuple containing the row and column indices of the element.</returns>
        public static (int, int) GetElementRowAndColumn(string childElementName)
        {
            var targetElement = gameBoard.FindName(childElementName) as FrameworkElement;
            int newColumn = Grid.GetColumn(targetElement);
            int newRow = Grid.GetRow(targetElement);
            return (newRow, newColumn);
        }
    }
}