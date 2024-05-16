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

namespace GoGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game _game;
        private static GameSettings _gameSetting;
        private SerializationService _serializationService;        

        public MainWindow()
        {
            InitializeComponent();
            _gameSetting = new GameSettings();
            _game = new Game(this);
            ellipseCurrentMove.Fill = Brushes.White;
            _serializationService = new SerializationService();
            _game.ChangingCurrentMove += ChangingScore;
            _game.ChangingCurrentMove += ChangingCurrentPlayer;
        }       

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _serializationService.SaveGame(_game);
        }

        public void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            _game = _serializationService.LoadGame();
        }

        public void ChangingScore()
        {
            scorePlayer1.Content = "Score: " + Convert.ToString(_game.scoreWhite);
            scorePlayer2.Content = "Score: " + Convert.ToString(_game.scoreBlack);
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

        }
    }
}
