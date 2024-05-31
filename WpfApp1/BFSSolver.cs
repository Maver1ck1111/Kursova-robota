using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class BFSSolver: Solvable
    {
        private Queue<bool[,]> queue = new Queue<bool[,]>();
        private Queue<bool[,]> temporary = new Queue<bool[,]>();

        public override async void StartAsync() 
        {
            if (queens.Count != 8)
            {
                MessageBox.Show("Place 8 queens");
                return;
            }

            if (IsSafe(board))
            {
                MessageBox.Show("The board is safe");
                return;
            }

            if (Mix()) return;

            Algorithms.SendAlgorithmStarted();

            queue.Enqueue(board);

            bool result = false;
            await Task.Run(()=>
            {
                result = BFS();
                queue.Clear();
                temporary.Clear();
                GC.Collect();
            });

            if (result)
            {
                movable.Set(board);
                MessageBox.Show($"Count of childrens: {count}");
            }

            Algorithms.SendOnAlgoritmEnded();
        }

        private bool BFS()
        {
            do
            {
                temporary.Clear();

                while (queue.Count != 0)
                {
                    bool[,] board1 = queue.Peek();
                    foreach (var position in FindIndexes(queue.Dequeue()))
                    {
                        var index = position;
                        for (int i = 0; i < size - 1; i++)
                        {
                            bool[,] boardCopy = new bool[size, size];
                            if (index.Item2 == size - 1)
                            {
                                board1[index.Item1, index.Item2] = false;
                                board1[index.Item1, 0] = true;
                                index.Item2 = 0;

                                Array.Copy(board1, boardCopy, board1.Length);
                                temporary.Enqueue(boardCopy);
                            }
                            else 
                            {
                                board1[index.Item1, index.Item2] = false;
                                board1[index.Item1, index.Item2 + 1] = true;
                                index.Item2++;

                                Array.Copy(board1, boardCopy, board1.Length);
                                temporary.Enqueue(boardCopy);
                            }
                        }
                    }

                    if(queue.Count % 1000 == 0) 
                    {
                        if (Process.GetCurrentProcess().WorkingSet64 > maxMemory * 2)
                        {
                            MessageBox.Show("Too much memory");
                            return false;
                        }
                    }
                }

                count += temporary.Count;

                foreach (var item in temporary)
                {
                    if (IsSafe(item))
                    {
                        this.board = item;
                        return true;
                    }
                }

                queue = new Queue<bool[,]>(temporary.ToArray());

            } while (temporary.Count != 0);

            return false;
        }
    }
}
