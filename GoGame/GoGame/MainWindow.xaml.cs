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
        public MainWindow()
        {
            InitializeComponent();
            _gameSetting = new GameSettings();
            _game = new Game(this);
        }

        ellipseCurrentMove
    }
}
