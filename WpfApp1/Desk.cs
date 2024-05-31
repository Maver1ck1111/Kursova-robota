using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Reflection;
using System.Windows.Threading;

namespace WpfApp1
{
    public class Desk : IMovable, IUtils
    {
        private readonly Grid chessbord;
        private readonly Grid deskzone;
        private List<(int row, int col)> buttons;
        private const int rows = 8;
        private const int cols = 8;
        private readonly int size;
        private bool[,] desk;

        public bool[,] Board => desk;

        public List<(int, int)> Queens => buttons;

        public Desk(Grid deskzone, int size)
        {
            desk = new bool[rows, cols];
            buttons = new List<(int row, int col)>();   

            chessbord = new Grid();
            chessbord.HorizontalAlignment = HorizontalAlignment.Center;
            chessbord.VerticalAlignment = VerticalAlignment.Center;
            chessbord.Margin = new Thickness(20, 20, 0, 50);
            deskzone.Children.Add(chessbord);

            this.deskzone = deskzone;
            this.size = size;


            SetGrid();
            CreateDesk();
            Marking();
        }

        private void Marking() 
        {
            StackPanel stackPanel1 = new StackPanel();
            stackPanel1.HorizontalAlignment = HorizontalAlignment.Center;
            stackPanel1.VerticalAlignment = VerticalAlignment.Top;
            stackPanel1.Orientation = Orientation.Horizontal;

            string[] horizontalMarking = "a b c d e f g h".Split(" ");

            for(int i = 0; i < rows; i++) 
            {
                Label label = new Label();

                if (i == 0) label.Margin = new Thickness(20, 0, 0, 0);

                label.Width = size;
                label.Height = 25;
                label.Content = horizontalMarking[i];
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                stackPanel1.Children.Add(label);
            }

            deskzone.Children.Add(stackPanel1);

            StackPanel stackPanel2 = new StackPanel();
            stackPanel2.HorizontalAlignment = HorizontalAlignment.Left;
            stackPanel2.VerticalAlignment = VerticalAlignment.Top;
            stackPanel2.Orientation = Orientation.Vertical;

            string[] verticalMarking = "1 2 3 4 5 6 7 8".Split(" ");

            for (int i = 0; i < cols; i++)
            {
                Label label = new Label();

                if (i == 0) label.Margin = new Thickness(0, 20, 0, 0);

                label.Width = 30;
                label.Height = size;
                label.Content = verticalMarking[i];
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                stackPanel2.Children.Add(label);
            }

            deskzone.Children.Add(stackPanel2);
        }

        private void SetGrid() 
        {
            for(int i = 0; i < rows; i++) 
            {
                chessbord.RowDefinitions.Add(new RowDefinition() {Height = new GridLength(size)});
                chessbord.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(size)});
            }
        }

        private void CreateDesk()
        {

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Button btn = new Button();
                    btn.Width = size;
                    btn.Height = size;
                    btn.Click += ButtonClick;

                    if ((i + j) % 2 == 0)
                        btn.Background = Brushes.White;
                    else
                        btn.Background = Brushes.Black;

                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);
                    chessbord.Children.Add(btn);
                }
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (buttons.Count == 8)
            {
                MessageBox.Show("Too much");
                return;
            }

            Button btn = (Button)sender;
            int getRow = Grid.GetRow(btn);
            int getCol = Grid.GetColumn(btn);

            if (desk[getRow, getCol]) 
            {
                MessageBox.Show("You can't place here");
                return;
            }

            Image image = new Image();

            if ((getRow + getCol) % 2 != 0)
                image.Source = new BitmapImage(new Uri("Image/white_queen.png", UriKind.RelativeOrAbsolute));
            else image.Source = new BitmapImage(new Uri("Image/black_queen.png", UriKind.RelativeOrAbsolute));
            

            desk[getRow, getCol] = true;
            btn.Content = image;

            buttons.Add((getRow, getCol));
        }

        public void ClearDesk()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    desk[i, j] = false;
                }
            }
            foreach (var button in buttons)
            {
                Button btn = new Button();
                Grid.SetRow(btn, button.row);
                Grid.SetColumn(btn, button.col);
                btn.Click += ButtonClick;
                btn.Height = size;
                btn.Width = size;


                if ((button.row + button.col) % 2 == 0)
                    btn.Background = Brushes.White;
                else
                    btn.Background = Brushes.Black;

                chessbord.Children.Add(btn);
            }

            buttons.Clear();
        }

        public void Set(bool[,] board)
        {
            bool[,] setBoard = new bool[rows, cols];
            Array.Copy(board, setBoard, board.Length);
            ClearDesk();

            for(int i = 0; i < rows; i++) 
            {
                for(int j = 0; j < cols; j++) 
                {
                    if (setBoard[i, j]) buttons.Add((i, j)); 
                }
            }

            for(int i = 0; i < buttons.Count; i++) 
            {
                int row = buttons[i].row;
                int col = buttons[i].col; 

                Button btn = new Button();
                Grid.SetRow(btn, row);
                Grid.SetColumn(btn, col);
                btn.Height = size;
                btn.Width = size;

                Image image = new Image();

                if ((row + col) % 2 != 0)
                {
                    btn.Background = Brushes.Black;
                    image.Source = new BitmapImage(new Uri("Image/white_queen.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    btn.Background = Brushes.White;
                    image.Source = new BitmapImage(new Uri("Image/black_queen.png", UriKind.RelativeOrAbsolute));
                }


                btn.Content = image;
                chessbord.Children.Add(btn);
            }

            desk = setBoard;
        }

        public void RemoveLastQuenn()
        {
            if (buttons.Count == 0)
            {
                MessageBox.Show("No queens");
                return;
            }
            var index = buttons[buttons.Count - 1];

            Button btn = new Button();
            Grid.SetRow(btn, index.row);
            Grid.SetColumn(btn, index.col);
            btn.Height = size;
            btn.Width = size;
            btn.Click += ButtonClick;

            desk[index.row, index.col] = false;

            if ((index.row + index.col) % 2 == 0)
                btn.Background = Brushes.White;
            else
                btn.Background = Brushes.Black;

            chessbord.Children.Add(btn);

            buttons.RemoveAt(buttons.Count - 1);
        }
    }
}
