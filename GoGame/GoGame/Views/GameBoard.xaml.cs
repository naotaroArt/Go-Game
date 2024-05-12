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

namespace GoGame.Views
{
    /// <summary>
    /// Логика взаимодействия для GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        public GameBoard()
        {
            InitializeComponent();
            FillGridWithBorders();
            FillGridWithButtons();
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

        private void FillGridWithButtons()
        {
            // Заполняем каждую ячейку Grid кнопкой
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    // Создаем кнопку
                    Button button = new Button
                    {
                        Name = $"button_{row}_{col}",
                       Background = Brushes.Transparent,
                        BorderBrush = Brushes.Transparent,
                    };


                    Ellipse ellipse = new Ellipse
                    {
                        Width = 50,
                        Height = 50,
                        Fill = new SolidColorBrush(Color.FromArgb(128, 128, 128, 128)), // Полупрозрачный цвет
                        Visibility = Visibility.Hidden // Изначально скрываем эллипс

                    };
                    Panel.SetZIndex(button, 2);
                    Panel.SetZIndex(ellipse, 1);

                    Grid.SetRow(ellipse, row);
                    Grid.SetColumn(ellipse, col);
                    gridBoardBig.Children.Add(ellipse);

                    ApplyCustomButtonStyle(button);

                    // Устанавливаем обработчик события нажатия кнопки

                    button.Click += Button_Click;
                    button.MouseEnter += Button_MouseEnter;
                    button.MouseLeave += Button_MouseLeave;

                    // Добавляем кнопку в соответствующую ячейку Grid
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    gridBoardBig.Children.Add(button);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string[] parts = button.Name.Split('_');
            if (parts.Length == 3)
            {
                int row, col;
                if (int.TryParse(parts[1], out row) && int.TryParse(parts[2], out col))
                {
                    MessageBox.Show($"Button clicked: x:{col} y:{row}");
                }
            }
        }

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Ellipse ellipse = null;
            if (button != null)
            {
                button.Background = Brushes.Transparent;
                foreach (var child in gridBoardBig.Children)
                {
                    if (child is Ellipse)
                    {
                        ellipse = (Ellipse)child;
                    }
                }
                int row = Grid.GetRow(button);
                int col = Grid.GetColumn(button);
                Grid.SetRow(ellipse, row);
                Grid.SetColumn(ellipse, col);
                ellipse.Visibility = Visibility.Visible;               
            }
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            Ellipse ellipseToHidden = null;
            foreach (var child in gridBoardBig.Children)
            {
                if (child is Ellipse)
                {
                    ellipseToHidden = (Ellipse)child;
                    break;
                }
            }

            if (ellipseToHidden != null)
            {
                ellipseToHidden.Visibility = Visibility.Hidden;
            }
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
