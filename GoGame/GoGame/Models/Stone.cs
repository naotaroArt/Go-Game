using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGame.Models
{
    public enum CellState { Black, White, Empty }
    internal class Stone
    {
        public int X { get; }
        public int Y { get; }
        public CellState Color { get; }

        public Stone(int x, int y, CellState color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }
}
