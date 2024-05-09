using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGame.Models
{
    internal class Game
    {
        public Board board;
        public CellState playr1 = CellState.White;
        public CellState playr2 = CellState.Black;

        public Game()
        {
            board = new Board();          
        }

        public bool IsMoveValid(int x, int y, CellState stoneColor)
        {
            // Проверка на пустую клетку
            if (board.GetStoneAt(x, y) != CellState.Empty)
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
                    board.boardCellState[capturedX, capturedY] = CellState.Empty;
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
