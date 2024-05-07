using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGame.Models
{
    internal class GoGameRules
    {
        private const int BoardSize = 9;
        private CellState[,] board;

        public GoGameRules()
        {
            board = new CellState[BoardSize, BoardSize];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    board[i, j] = CellState.Empty;
                }
            }
        }

        public bool IsMoveValid(int x, int y, CellState stoneColor)
        {
            // Проверка на пустую клетку
            if (board[x, y] != CellState.Empty)
                return false;

            // Проверка на самоубийство
            if (IsSuicide(x, y, stoneColor))
                return false;

            // Проверка на захват камней противника
            List<(int, int)> capturedStones = GetCapturedStones(x, y, stoneColor);
            if (capturedStones.Count > 0)
            {
                foreach (var (capturedX, capturedY) in capturedStones)
                {
                    board[capturedX, capturedY] = CellState.Empty;
                }
                return true;
            }

            // Проверка на запрет хода (ко рку)
            if (IsKo(x, y, stoneColor))
                return false;

            return true;
        }

        private bool IsSuicide(int x, int y, CellState stoneColor)
        {
            // TODO: Реализовать проверку на самоубийство
            throw new NotImplementedException();
        }

        private List<(int, int)> GetCapturedStones(int x, int y, CellState stoneColor)
        {
            // TODO: Реализовать проверку на захват камней противника
            throw new NotImplementedException();
        }

        private bool IsKo(int x, int y, CellState stoneColor)
        {
            // TODO: Реализовать проверку на запрет хода (ко рку)
            throw new NotImplementedException();
        }
    }
}
