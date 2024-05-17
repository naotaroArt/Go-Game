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
        private Game _game;
        private UndoRedoService undoRedoService;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();            
            undoRedoService = new UndoRedoService();       
        }       

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SerializationService.SaveGame(_game);
        }

        public void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            _game = null;
            _game = SerializationService.LoadGame();
        }

        public void ChangingScore()
        {
            scorePlayer1.Content = "Сaptured \n" + Convert.ToString(_game.scoreWhite);
            scorePlayer2.Content = "Сaptured \n" + Convert.ToString(_game.scoreBlack);
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
            ellipseCurrentMove.Fill = _game.currentMove == CellState.White ? Brushes.Black : Brushes.White;
            contentControl.InvalidateVisual();
        }

        private void NewGameButton_ClickToStartNewGame(object sender, RoutedEventArgs e)
        {
            _game = null;
            InitializeGame();
            undoRedoService = null;
            undoRedoService = new UndoRedoService();
        }

        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            if (_game.endGame)
                return;
            _game.numberOfPasses++;
            if (_game.numberOfPasses == 2)
            {
                _game.endGame = true;
                _game.EndGame();
                return;
            }
            ChangingCurrentPlayer();
            _game.currentMove = _game.currentMove == CellState.White ? CellState.Black : CellState.White;
        }

        private void PassReset()
        {
            _game.numberOfPasses = 0;
        }

        private void SelectingBoardSize9x9_Checked(object sender, RoutedEventArgs e)
        {
            _game = null;
            GameSettings.BoardSize = 9;
            InitializeGame();
            undoRedoService = null;
            undoRedoService = new UndoRedoService();
        }

        private void SelectingBoardSize13x13_Checked(object sender, RoutedEventArgs e)
        {
            _game = null;
            GameSettings.BoardSize = 13;
            InitializeGame();
            undoRedoService = null;
            undoRedoService = new UndoRedoService();
        }

        private void SelectingBoardSize19x19_Checked(object sender, RoutedEventArgs e)
        {
            _game = null;
            GameSettings.BoardSize = 19;
            InitializeGame();
            undoRedoService = null;
            undoRedoService = new UndoRedoService();
        }

        private void InitializeGame()
        {
            _game = new Game();
            contentControl.Content = _game.gameBoard;
            currentPlayer.Content = "Player1";
            ellipseCurrentMove.Fill = _game.currentMove == CellState.White ? Brushes.White : Brushes.Black;
            ChangingScore();
            _game.ChangingCurrentMove += ChangingScore;
            _game.ChangingCurrentMove += ChangingCurrentPlayer;
            _game.ChangingCurrentMove += PassReset;
            _game.ChangingCurrentMove += PushInStack;           
        }

        private void PushInStack()
        {
            undoRedoService.AddMove(_game);
        }

        private void UnDo_Click(object sender, RoutedEventArgs e)
        {
            if (_game.endGame)
                return;
            Game previousGame = undoRedoService.UndoMove();
            if (previousGame != null)
            {
                _game = null;
                _game = previousGame;
                contentControl.Content = _game.gameBoard;
                ellipseCurrentMove.Fill = _game.currentMove == CellState.White ? Brushes.White : Brushes.Black;
                _game.gameBoard.Refresh();
            }
        }

        private void ReDo_Click(object sender, RoutedEventArgs e)
        {
            if (_game.endGame)
                return;
            Game nextGame = undoRedoService.RedoMove();
            if (nextGame != null)
            {
                _game = null;
                _game = nextGame;
                contentControl.Content = _game.gameBoard;
                ellipseCurrentMove.Fill = _game.currentMove == CellState.White ? Brushes.White : Brushes.Black;
                _game.gameBoard.Refresh();
            }
        }
    }
}
