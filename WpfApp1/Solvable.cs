using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class Solvable
    {
        protected IMovable movable;
        protected bool[,] board;
        protected List<(int, int)> queens;
        protected int size;
        protected int count = 0;
        protected const long maxMemory = 1000000000;

        public Solvable()
        {
            movable = Container.DIContainer.GetRequiredService<IMovable>();
            this.board = movable.Board;
            this.size = movable.Board.GetLength(0);
            this.queens = movable.Queens;
        }

        public virtual async void StartAsync() { }

        protected bool Mix() 
        {
            List<(int, int)> time = FindIndexes(board);
            List<(int, int)> main = new List<(int, int)>();

            bool[] safe = new bool[size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] == true) safe[i] = true;
                }
            }

            for (int i = 0; i < time.Count; i++)
            {
                for (int j = i + 1; j < time.Count; j++)
                {
                    if (time[i].Item1 == time[j].Item1)
                    {
                        main.Add(time[j]);
                        time.RemoveAt(j);
                        j--;
                    }
                }
            }

            for (int i = 0; i < size; i++)
            {
                if (!safe[i])
                {
                    board[main[count].Item1, main[count].Item2] = false;
                    board[i, main[count].Item2] = true;
                }
            }

            int conflicts = Conflicts(board);
            bool[,] timedesk = new bool[size, size];
            Array.Copy(board, timedesk, board.Length);

            foreach (var position in FindIndexes(timedesk))
            {
                var index = position;
                for (int i = 0; i < size - 1; i++)
                {
                    if (index.Item2 == size - 1)
                    {
                        timedesk[index.Item1, index.Item2] = false;
                        timedesk[index.Item1, 0] = true;
                        index.Item2 = 0;
                    }
                    else 
                    {
                        timedesk[index.Item1, index.Item2] = false;
                        timedesk[index.Item1, index.Item2 + 1] = true;
                        index.Item2++;
                    }

                    count++;

                    if (IsSafe(timedesk)) 
                    {
                        Array.Copy(timedesk, board, board.Length);
                        movable.Set(timedesk);
                        MessageBox.Show($"Count of childrens: {count}");
                        return true;
                    }

                    if (Conflicts(timedesk) < conflicts)
                    {
                        Array.Copy(timedesk, board, board.Length);
                        conflicts = Conflicts(timedesk);
                    }

                }
            }
            return false;
        }
        private int Conflicts(bool[,] desk)
        {
            int countOfConflicts = 0;
            foreach (var index in FindIndexes(desk))
            {
                int row = index.Item1;
                int col = index.Item2;

                for (int i = 0; i < size; i++)
                {
                    if (desk[i, col] && i != row) countOfConflicts++;
                }

                for (int i = row - 1, j = col + 1; i >= 0 && j < size; i--, j++)
                {
                    if (desk[i, j]) countOfConflicts++;
                }

                for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--)
                {
                    if (desk[i, j]) countOfConflicts++;
                }

                for (int j = 0; j < size; j++)
                {
                    if (desk[row, j] && j != col) countOfConflicts++;
                }
            }

            return countOfConflicts;
        }
        protected bool IsSafe(bool[,] desk)
        {
            foreach (var index in FindIndexes(desk))
            {
                int row = index.Item1;
                int col = index.Item2;

                for (int i = 0; i < size; i++)
                {
                    if (desk[i, col] && i != row) return false;
                }

                for (int i = row - 1, j = col + 1; i >= 0 && j < size; i--, j++)
                {
                    if (desk[i, j]) return false;
                }

                for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--)
                {
                    if (desk[i, j]) return false;
                }

                for (int j = 0; j < size; j++)
                {
                    if (desk[row, j] && j != col) return false;
                }
            }

            return true;
        }
        protected List<(int, int)> FindIndexes(bool[,] board)
        {
            List<(int, int)> indexes = new List<(int, int)>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j])
                    {
                        indexes.Add((i, j));
                    }
                }
            }
            return indexes;
        }
        protected int FindQueen(bool[,] desk, int row)
        {
            int index = 0;
            for (int i = 0; i < size; i++)
            {
                if (desk[row, i]) index = i;
            }
            return index;
        }
    }
}
