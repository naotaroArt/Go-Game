using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GoGame.Models;
using GoGame.AppSettings;
using GoGame.Services;
using System.Runtime.CompilerServices;
using GoGame.Views;

namespace GoGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Game game;
        private UndoRedoService undoRedoService;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();            
            undoRedoService = new UndoRedoService();       
        }       

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SerializationService.SaveGame(game);
        }

        public void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            game = null;
            game = SerializationService.LoadGame();
        }

        public void ChangingScore()
        {
            scorePlayer1.Content = "Сaptured \n" + Convert.ToString(game.scoreWhite);
            scorePlayer2.Content = "Сaptured \n" + Convert.ToString(game.scoreBlack);
        }

        public void ChangingCurrentPlayer()
        {
            if((string)currentPlayer.Content == "Player1")
            {
                currentPlayer.Content = "Player2";
            }
            else
            {
                currentPlayer.Content = "Player1";
            }
            ellipseCurrentMove.Fill = game.currentMove == CellState.White ? Brushes.Black : Brushes.White;
            contentControl.InvalidateVisual();
        }

        private void NewGameButton_ClickToStartNewGame(object sender, RoutedEventArgs e)
        {
            game = null;
            InitializeGame();
            undoRedoService = null;
            undoRedoService = new UndoRedoService();
        }

        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            if (game.endGame)
                return;
            game.numberOfPasses++;
            if (game.numberOfPasses == 2)
            {
                game.endGame = true;
                game.EndGame();
                return;
            }
            ChangingCurrentPlayer();
            game.currentMove = game.currentMove == CellState.White ? CellState.Black : CellState.White;
        }

        private void PassReset()
        {
            game.numberOfPasses = 0;
        }

        private void SelectingBoardSize9x9_Checked(object sender, RoutedEventArgs e)
        {
            game = null;
            GameSettings.BoardSize = 9;
            InitializeGame();
            undoRedoService = null;
            undoRedoService = new UndoRedoService();
        }

        private void SelectingBoardSize13x13_Checked(object sender, RoutedEventArgs e)
        {
            game = null;
            GameSettings.BoardSize = 13;
            InitializeGame();
            undoRedoService = null;
            undoRedoService = new UndoRedoService();
        }

        private void SelectingBoardSize19x19_Checked(object sender, RoutedEventArgs e)
        {
            game = null;
            GameSettings.BoardSize = 19;
            InitializeGame();
            undoRedoService = null;
            undoRedoService = new UndoRedoService();
        }

        private void InitializeGame()
        {
            game = new Game();
            contentControl.Content = game.gameBoard;
            currentPlayer.Content = "Player1";
            ellipseCurrentMove.Fill = game.currentMove == CellState.White ? Brushes.White : Brushes.Black;
            ChangingScore();
            game.ChangingCurrentMove += ChangingScore;
            game.ChangingCurrentMove += ChangingCurrentPlayer;
            game.ChangingCurrentMove += PassReset;
            game.ChangingCurrentMove += PushInStack;           
        }

        private void PushInStack()
        {
            undoRedoService.AddMove(game);
        }

        private void UnDo_Click(object sender, RoutedEventArgs e)
        {
            if (game.endGame)
                return;
            Game previousGame = undoRedoService.UndoMove();
            if (previousGame != null)
            {
                game = null;
                game = previousGame;
                contentControl.Content = game.gameBoard;
                ellipseCurrentMove.Fill = game.currentMove == CellState.White ? Brushes.White : Brushes.Black;
                game.gameBoard.Refresh();
            }
        }

        private void ReDo_Click(object sender, RoutedEventArgs e)
        {
            if (game.endGame)
                return;
            Game nextGame = undoRedoService.RedoMove();
            if (nextGame != null)
            {
                game = null;
                game = nextGame;
                contentControl.Content = game.gameBoard;
                ellipseCurrentMove.Fill = game.currentMove == CellState.White ? Brushes.White : Brushes.Black;
                game.gameBoard.Refresh();
            }
        }
    }
}
