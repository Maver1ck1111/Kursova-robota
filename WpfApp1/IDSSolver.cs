using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace WpfApp1
{
    public class IDSSolver: Solvable
    {
        private Queue<bool[,]> queue = new Queue<bool[,]> ();

        public override async void StartAsync() 
        {
            if (queens.Count != 8)
            {
                MessageBox.Show("Place 8 queens");
                return;
            }

            if (IsSafe(board))
            {
                MessageBox.Show("The bord is safe");
                return;
            }

            if (Mix()) return;

            Algorithms.SendAlgorithmStarted();

            bool result = false;

            await Task.Run(() =>
            {
                queue.Enqueue(board);

                while (queue.Count > 0)
                {
                    result = IDS(queue.Dequeue(), 0);
                    if (result) break;
                }

                queue.Clear();
                GC.Collect();

            });

            if (result)
            {
                movable.Set(this.board);
                MessageBox.Show($"Count of childrens: {count}");
            }

            Algorithms.SendOnAlgoritmEnded();
        }

        private bool IDS(bool[,] desk, int row)
        {
            if (row == size) return false;
            int index = FindQueen(desk, row);

            for (int i = 0; i < size - 1; i++)
            {
                bool[,] boardCopy = new bool[size, size];
                if (index == size - 1)
                {
                    desk[row, index] = false;
                    desk[row, 0] = true;
                    index = 0;

                    Array.Copy(desk, boardCopy, desk.Length);
                    queue.Enqueue(boardCopy);
                    count++;
                }
                else 
                {
                    desk[row, index] = false;
                    desk[row, index + 1] = true;
                    index++;

                    Array.Copy(desk, boardCopy, desk.Length);
                    queue.Enqueue(boardCopy);
                    count++;
                }


                if (IsSafe(desk))
                {
                    this.board = desk;
                    return true;
                }

            }

            if (queue.Count % 1000 == 0)
            {
                if (Process.GetCurrentProcess().WorkingSet64 > maxMemory  * 2)
                {
                    MessageBox.Show("Too much memory");
                    queue.Clear();
                    return false;
                }
            }
            if (IDS(desk, row + 1)) return true;
            return false;
        }
    }
}
