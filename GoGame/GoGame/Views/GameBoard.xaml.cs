﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using GoGame.AppSettings;
using GoGame.Models;
using GoGame.Services;

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
                        Name = $"ellipse_{row}_{col}",
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
            if (game.board.boardStone[col, row].state == CellState.Empty && game.IsMoveValid(col, row, game.currentMove))
            {
                game.board.boardStone[col, row].state = game.currentMove;               
                ellipse.Fill = game.currentMove == CellState.White ? Brushes.White : Brushes.Black;
                game.board.UpdateBordStons();
                game.currentMove = game.currentMove == CellState.White ? CellState.Black : CellState.White;
                InvalidateVisual();
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
            if (game.board.boardStone[col, row].state != CellState.Empty)
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
            if (game.board.boardStone[col, row].state != CellState.Empty)
                return;           
            ellipse.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }  
    }
}
