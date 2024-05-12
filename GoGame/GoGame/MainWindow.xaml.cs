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
        }       

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _serializationService.SaveGame(_game);
        }

        public void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            _game = _serializationService.LoadGame();
        }
    }
}
