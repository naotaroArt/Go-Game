using GoGame.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace GoGame.ViewModels
{
    internal class MainWindowViewModel
    {
        public ICommand ShowGameBoardCommand { get; }

        public MainWindowViewModel()
        {
            ShowGameBoardCommand = new RelayCommand<object>(ShowGameBoard);
        }

        private void ShowGameBoard(object parameter)
        {
            // Отображаем игровое поле в главном окне
            MainWindow main = (MainWindow)parameter;
        }
    }
}
