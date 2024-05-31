using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public class Algorithms
    {
        private DockPanel zone;
        private Grid buttonZone;
        private bool IsRunning = false;
        public static Action OnAlgorithmStarted;
        public static Action OnAlgorithmEnded;

        public Algorithms(DockPanel zone) 
        {
            this.zone = zone;

            OnAlgorithmStarted += OnAlgorithmStartedHandler;
            OnAlgorithmEnded += OnAlgorithmEndedHandler;


            SetGrid();
            SetButtons();
        }

        private void SetGrid() 
        {
            buttonZone = new Grid();
            for(int i = 0; i < 3; i++) 
            {
                buttonZone.RowDefinitions.Add(new RowDefinition());
            }
            zone.Children.Add(buttonZone);
        }

        private void SetButtons() 
        {
            for(int i = 0; i < 3; i++)
            {
                Button button = new Button();
                button.Content = i;
                button.Height = 80;
                button.Width = 100;
                if (i == 0)
                {
                    button.Click += LDFS;
                    button.Content = "LDFS";
                }
                else if(i == 1)
                {
                    button.Click += BFS;
                    button.Content = "BFS";
                }
                else 
                {
                    button.Click += IDS;
                    button.Content = "IDS";
                }

                Grid.SetRow(button, i);
                Grid.SetColumn(button, 0);
                buttonZone.Children.Add(button);
            }
        }

        public static void SendAlgorithmStarted() => OnAlgorithmStarted?.Invoke();
        public static void SendOnAlgoritmEnded() => OnAlgorithmEnded?.Invoke();
        private void OnAlgorithmStartedHandler() => IsRunning = true;
        private void OnAlgorithmEndedHandler() => IsRunning = false;

        private void IDS(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                MessageBox.Show("Wait, algo is running");
                return;
            }
            new IDSSolver().StartAsync();
        }

        private void BFS(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                MessageBox.Show("Wait, algo is running");
                return;
            }

            new BFSSolver().StartAsync();
        }

        private void LDFS(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                MessageBox.Show("Wait, algo is running");
                return;
            }

            new LDFSSolver().StartAsync();
        }
    }
}
