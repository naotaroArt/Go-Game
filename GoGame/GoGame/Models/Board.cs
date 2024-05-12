using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoGame.AppSettings;

namespace GoGame.Models
{
    public enum CellState { Black = 2, White = 1, Empty = 0}

    internal class Board
    {
        public int boardSize;
        public CellState[,] boardCellState;

        public Board()
        {
            boardSize = GameSettings.BoardSize;
            boardCellState = new CellState[boardSize, boardSize];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    boardCellState[i, j] = CellState.Empty;
                }
            }
        }

        public bool PlaceStone(int x, int y, CellState stoneColor)
        {
            if (!IsWithinBounds(x, y) || boardCellState[x, y] != CellState.Empty)
                return false;

            boardCellState[x, y] = stoneColor;
            return true;
        }

        public CellState GetStoneAt(int x, int y)
        {
            if (!IsWithinBounds(x, y))
                throw new ArgumentOutOfRangeException("Coordinates out of bounds");

            return boardCellState[x, y];
        }

        private bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < boardSize && y >= 0 && y < boardSize;
        }
    }
}
