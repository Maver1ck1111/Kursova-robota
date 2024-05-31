using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1
{
    public interface IMovable
    {
        bool[,] Board { get; }
        public List<(int, int)> Queens { get; }
        void Set(bool[,] desk);
    }
}
