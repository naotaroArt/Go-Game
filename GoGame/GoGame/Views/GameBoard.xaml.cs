using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GoGame.AppSettings;
using GoGame.Models;

namespace GoGame.Views
{
    /// <summary>
    /// Логика взаимодействия для GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        private Game _game; 
        internal GameBoard(Game game)
        {
            InitializeComponent();
            FillGridWithBorders();
            FillGridWithEllipse();
            _game = game;
        }

        private void FillGridWithBorders()
        {
            for (int i = 0; i < GameSettings.BoardSize - 1; i++)
            {
                gridBoard.RowDefinitions.Add(new RowDefinition());
                gridBoard.ColumnDefinitions.Add(new ColumnDefinition());
            }
            // Заполняем каждую ячейку Grid элементом Border с черной границей
            for (int row = 0; row < GameSettings.BoardSize - 1; row++)
            {
                for (int col = 0; col < GameSettings.BoardSize - 1; col++)
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
            Grid.SetColumnSpan(gridBoard, GameSettings.BoardSize);
            Grid.SetRowSpan(gridBoard, GameSettings.BoardSize);
            gameBoard.Height = GameSettings.BoardSize * 50 + 10;
            gameBoard.Width = GameSettings.BoardSize * 50 + 10;
            gridBoard.Margin = new Thickness(25);
            for (int i = 0; i < GameSettings.BoardSize; i++)
            {
                gridBoardBig.RowDefinitions.Add(new RowDefinition());
                gridBoardBig.ColumnDefinitions.Add(new ColumnDefinition());
            }
            // Заполняем каждую ячейку Grid
            for (int row = 0; row < GameSettings.BoardSize; row++)
            {
                for (int col = 0; col < GameSettings.BoardSize; col++)
                {
                    Ellipse ellipse = new Ellipse
                    {
                        Name = $"ellipse_{col}_{row}",
                        Width = ((gameBoard.Width - 10) / (GameSettings.BoardSize + 1)) - 5,
                        Height = ((gameBoard.Height - 10) / (GameSettings.BoardSize + 1)) - 5,
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
            if (_game.board.boardStone[col, row].state == CellState.Empty && _game.IsMoveValid(col, row, _game.currentMove))
            {
                _game.board.boardStone[col, row].state = _game.currentMove;               
                ellipse.Fill = _game.currentMove == CellState.White ? Brushes.White : Brushes.Black;
                _game.board.UpdateBordStons();
                _game.MoveIsMade();
                _game.currentMove = _game.currentMove == CellState.White ? CellState.Black : CellState.White;
                InvalidateVisual();
               // Refresh();
                //MessageBox.Show($"Button clicked: x:{col} y:{row}");
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
            if (_game.board.boardStone[col, row].state != CellState.Empty)
                return;          
            if (ellipse != null)
            {
                if(_game.currentMove == CellState.White)
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
            if (_game.board.boardStone[col, row].state != CellState.Empty)
                return;           
            ellipse.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }  

        public void Refresh()
        {
            foreach (Object child in gridBoardBig.Children)
            {
                if (child is Ellipse ellip)
                {
                    string[] nameParts = ellip.Name.Split('_');
                    if (nameParts.Length == 3 && nameParts[0] == "ellipse")
                    {
                        if (int.TryParse(nameParts[1], out int ellipseX) && int.TryParse(nameParts[2], out int ellipseY))
                        {
                            if (_game.board.boardStone[ellipseX, ellipseY].state == CellState.White)
                                ellip.Fill = Brushes.White;
                            if (_game.board.boardStone[ellipseX, ellipseY].state == CellState.Black)
                                ellip.Fill = Brushes.Black;
                            if(_game.board.boardStone[ellipseX, ellipseY].state == CellState.Empty)
                                ellip.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                        }
                    }
                }
            }
        }
    }
}
