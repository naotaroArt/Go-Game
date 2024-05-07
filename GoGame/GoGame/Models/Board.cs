using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoGame.AppSettings;

namespace GoGame.Models
{
    internal class Board
    {
        private int _boardSize = GameSettings.BoardSize;
        private CellState[,] _board;

        public Board()
        {
            _board = new CellState[_boardSize, _boardSize];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < _boardSize; i++)
            {
                for (int j = 0; j < _boardSize; j++)
                {
                    _board[i, j] = CellState.Empty;
                }
            }
        }

        public bool PlaceStone(int x, int y, CellState stoneColor)
        {
            if (!IsWithinBounds(x, y) || _board[x, y] != CellState.Empty)
                return false;

            _board[x, y] = stoneColor;
            return true;
        }

        public CellState GetStoneAt(int x, int y)
        {
            if (!IsWithinBounds(x, y))
                throw new ArgumentOutOfRangeException("Coordinates out of bounds");

            return _board[x, y];
        }

        private bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < _boardSize && y >= 0 && y < _boardSize;
        }
    }
}
