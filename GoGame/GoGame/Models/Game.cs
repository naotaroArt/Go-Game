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
        public static CellState playr1 = CellState.White;
        public static CellState playr2 = CellState.Black;
        public CellState currentMove;
        public Views.GameBoard gameBoard;
        public MainWindow mainWindow;
        public int[] score;

        public Game(MainWindow mainWindow)
        {
            board = new Board();
            currentMove = playr1;
            gameBoard = new Views.GameBoard(this);
            mainWindow.contentControl.Content = gameBoard;
            this.mainWindow = mainWindow;
            score = new int[2] { 0, 0 };
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
            return false;
        }

        private List<(int, int)> GetCapturedStones(int x, int y, CellState stoneColor)
        {
            List<(int, int)> list = new List<(int, int)>();
            // TODO: Реализовать проверку на захват камней противника
            return list;
        }

        private bool IsKo(int x, int y, CellState stoneColor)
        {
            // TODO: Реализовать проверку на запрет хода (ко рку)
            return false;
        }

        
    }
}
