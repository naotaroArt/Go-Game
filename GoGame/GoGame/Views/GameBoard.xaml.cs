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

namespace GoGame.Views
{
    /// <summary>
    /// Логика взаимодействия для GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        private Game game; 
        internal GameBoard(Game game)
        {
            InitializeComponent();
            FillGridWithBorders();
            FillGridWithEllipse();
            this.game = game;
        }

        private void FillGridWithBorders()
        {
            // Заполняем каждую ячейку Grid элементом Border с черной границей
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Border border = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1)
                    };

                    // Добавляем Border в соответствующую ячейку Grid
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);
                    gridBoard.Children.Add(border);
                }
            }
        }

        private void FillGridWithEllipse()
        {
            // Заполняем каждую ячейку Grid кнопкой
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Ellipse ellipse = new Ellipse
                    {
                        Name = $"ellipse_{row}_{col}",
                        Width = 40,
                        Height = 40,
                        Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)), // Полупрозрачный цвет
                    };

                    Grid.SetRow(ellipse, row);
                    Grid.SetColumn(ellipse, col);
                    gridBoardBig.Children.Add(ellipse);


                    // Устанавливаем обработчик события нажатия кнопки

                    ellipse.MouseDown += Ellipse_Click;
                    ellipse.MouseEnter += Ellipse_MouseEnter;
                    ellipse.MouseLeave += Ellipse_MouseLeave;

                }
            }
        }

        private void Ellipse_Click(object sender, RoutedEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            int row = Grid.GetRow(ellipse);
            int col = Grid.GetColumn(ellipse);
            if (game.board.boardCellState[col, row] == CellState.Empty && game.IsMoveValid(col, row, game.currentMove))
            {
                game.board.boardCellState[col, row] = game.currentMove;               
                ellipse.Fill = game.currentMove == CellState.White ? Brushes.White : Brushes.Black;          
                game.currentMove = game.currentMove == CellState.White ? CellState.Black : CellState.White; 
                game.mainWindow.ellipseCurrentMove.Fill = game.currentMove == CellState.White ? Brushes.White : Brushes.Black;
                MessageBox.Show($"Button clicked: x:{col} y:{row}");
            }
            else
            {
                MessageBox.Show($"Button clicked: x:{col} y:{row}\n move is impossible");
            }                          
        }

        private void Ellipse_MouseEnter(object sender, RoutedEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            int row = Grid.GetRow(ellipse);
            int col = Grid.GetColumn(ellipse);      
            if (game.board.boardCellState[col, row] != CellState.Empty)
                return;          
            if (ellipse != null)
            {
                if(game.currentMove == CellState.White)
                    ellipse.Fill = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
                else
                    ellipse.Fill = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
            }
        }

        private void Ellipse_MouseLeave(object sender, RoutedEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            int row = Grid.GetRow(ellipse);
            int col = Grid.GetColumn(ellipse);
            if (game.board.boardCellState[col, row] != CellState.Empty)
                return;           
            ellipse.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }  
    }
}
