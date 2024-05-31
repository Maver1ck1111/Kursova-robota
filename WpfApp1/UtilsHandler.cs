using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public class UtilsHandler
    {
        private IUtils utils;
        private List<(int, int)> queens;
        private bool[,] board;
        private StackPanel stack;
        private Grid buttonZone;
        private bool IsRunning = false;

        public UtilsHandler(StackPanel stack)
        {
            utils = Container.DIContainer.GetRequiredService<IUtils>();
            queens = utils.Queens;
            board = utils.Board;
            this.stack = stack;

            Algorithms.OnAlgorithmStarted += OnAlgorithmStartedHandler;
            Algorithms.OnAlgorithmEnded += OnAlgorithmEndedHandler;

            SetGrid();
            AddButtons();
        }

        private void SetGrid() 
        {
            buttonZone = new Grid();
            for (int i = 0; i < 4; i++)
            {
                buttonZone.ColumnDefinitions.Add(new ColumnDefinition());
            }

            stack.Children.Add(buttonZone);
        }

        private void AddButtons() 
        {
            for (int i = 0; i < 4; i++)
            {
                Button button = new Button();
                button.Height = 50;
                button.Width = 100;

                if (i == 0)
                {
                    button.Click += CreateFile;
                    button.Content = "Create file";
                }
                else if (i == 1)
                {
                    button.Click += RemoveQueen;
                    button.Content = "Remove last queen";
                }
                else if(i == 2)
                {
                    button.Click += CleanBoard;
                    button.Content = "Clean board";
                }
                else 
                {
                    button.Click += RandomDesk;
                    button.Content = "Random desk";
                }

                button.Margin = new Thickness(30,0,30,0);
                Grid.SetRow(button, 0);
                Grid.SetColumn(button, i);

                stack.Children.Add(button);
            }
        }

        private void RandomDesk(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                MessageBox.Show("Wait, algo is running");
                return;
            }

            bool[,] copyarray = new bool[8, 8];

            Random rand = new Random();
            for (int i = 0; i < 8; i++)
            {
                int row = rand.Next(8);
                int col = rand.Next(8);

                while (copyarray[row, col])
                {
                    row = rand.Next(8);
                    col = rand.Next(8);
                }

                copyarray[row, col] = true;
            }

            utils.Set(copyarray);
        }

        private void CleanBoard(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                MessageBox.Show("Wait, algo is running");
                return;
            }
            utils.ClearDesk();
        }
        private void RemoveQueen(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                MessageBox.Show("Wait, algo is running");
                return;
            }
            utils.RemoveLastQuenn();
        }
        private void OnAlgorithmStartedHandler() => IsRunning = true;
        private void OnAlgorithmEndedHandler() => IsRunning = false;

        private void CreateFile(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                MessageBox.Show("Wait, algo is running");
                return;
            }

            Window dialog = new Window
            {
                Title = "Enter name of file",
                Height = 150,
                Width = 300,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            TextBox inputTextBox = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 100,
                Margin = new Thickness(10)
            };

            Button okButton = new Button
            {
                Content = "OK",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 10)
            };

            okButton.Click += (s, args) =>
            {
                dialog.DialogResult = true;
            };

            dialog.Content = new StackPanel
            {
                Children =
                {
                    inputTextBox,
                    okButton
                }
            };

            if (dialog.ShowDialog() == true)
            {
                string userInput = inputTextBox.Text + ".txt";
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktop, userInput);

                int[,] desk = new int[8, 8];

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach(var queen in queens) 
                    {
                        var index = queen;
                        desk[index.Item1, index.Item2] = 1;
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            writer.Write(desk[i, j] == 1 ? "1 " : "0 ");
                        }
                        writer.WriteLine();
                    }
                }
            }

        }
    }
}
