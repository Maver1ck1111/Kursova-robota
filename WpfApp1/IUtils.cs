using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public interface IUtils
    {
        List<(int, int)> Queens { get; }
        bool[,] Board { get; }
        void ClearDesk();
        void RemoveLastQuenn();
        void Set(bool[,] desk);
    }
}
