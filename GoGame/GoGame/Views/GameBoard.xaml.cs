﻿using System;
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

                    ellipse.MouseDown += Button_Click;
                    ellipse.MouseEnter += Button_MouseEnter;
                    ellipse.MouseLeave += Button_MouseLeave;

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
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

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            int row = Grid.GetRow(ellipse);
            int col = Grid.GetColumn(ellipse);
            if (game.board.boardCellState[col, row] != CellState.Empty)
                return;           
            ellipse.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        private void ApplyCustomButtonStyle(Button button)
        {
            // Создание нового стиля для кнопки
            Style customButtonStyle = new Style(typeof(Button));

            // Установка свойства Template для определения внешнего вида кнопки
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.BackgroundProperty, Brushes.Transparent);
            FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("Content") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            textBlockFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            textBlockFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            borderFactory.AppendChild(textBlockFactory);
            template.VisualTree = borderFactory;
            customButtonStyle.Setters.Add(new Setter(Button.TemplateProperty, template));

            // Применение стиля к кнопке
            button.Style = customButtonStyle;
        }        
    }
}
