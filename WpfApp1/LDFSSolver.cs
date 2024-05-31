using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace WpfApp1
{
    public class LDFSSolver: Solvable
    {
        Stack<bool[,]> stack = new Stack<bool[,]>();
        Stack<bool[,]> temporary = new Stack<bool[,]>();

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
                result = LDFS();
                stack.Clear();
                temporary.Clear();
                GC.Collect();
            });

            if (result) 
            {
                movable.Set(this.board);
                MessageBox.Show($"Count of childrens: {count}");
            }

            Algorithms.SendOnAlgoritmEnded();
        }


        private bool LDFS()
        {
            DFS(board, 0);
            count += stack.Count;

            foreach (var item in stack)
            {
                if (IsSafe(item))
                {
                    this.board = item;
                    return true;
                }
            }

            for (int i = 1; i < 10; i++)
            {

                temporary = new Stack<bool[,]>(stack.ToArray());
                stack.Clear();

                while (temporary.Count != 0)
                {
                    DFS(temporary.Pop(), 0);
                    if (temporary.Count % 1000 == 0)
                    {
                        if (Process.GetCurrentProcess().WorkingSet64 > maxMemory * 2)
                        {
                            MessageBox.Show("Too much memory");
                            return false;
                        }
                    }
                }

                count += stack.Count;

                foreach (var item in stack)
                {
                    if (IsSafe(item))
                    {
                        this.board = item;
                        return true;
                    }
                }
            }

            return false;
        }
        private bool DFS(bool[,] desk, int row)
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
                    stack.Push(boardCopy);
                }
                else 
                {
                    desk[row, index] = false;
                    desk[row, index + 1] = true;
                    index++;

                    Array.Copy(desk, boardCopy, desk.Length);
                    stack.Push(boardCopy);
                }

            }
            if (DFS(desk, row + 1)) return true;
            return false;
        }
    }
}
