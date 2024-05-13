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
                    board.boardStone[capturedX, capturedY].state = CellState.Empty;
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
            return ReChekIsSuicide(x,y,stoneColor);
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

        private bool ReChekIsSuicide(int x, int y, CellState stoneColor)
        {
            int way = 4;
            if (board.boardStone[x,y].stateTop != stoneColor && board.boardStone[x, y].stateTop != CellState.Empty && board.boardStone[x,y].stateTop != CellState.OutRange)
            {
                way--;
            }
            else if(board.boardStone[x, y].stateTop == stoneColor)
            {
                if(board.IsGrouoOfStoneOnSuicide(x, y, board.boardStone[x, y - 1].groupOfStones))
                {
                    way--;
                }
            }
            if(board.boardStone[x, y].stateBot != stoneColor && board.boardStone[x, y].stateBot != CellState.Empty && board.boardStone[x, y].stateBot != CellState.OutRange)
            {
                way--;
            }
            else if(board.boardStone[x, y].stateBot == stoneColor)
            {
                if (board.IsGrouoOfStoneOnSuicide(x, y, board.boardStone[x, y + 1].groupOfStones))
                {
                    way--;
                }
            }
            if(board.boardStone[x, y].stateLeft != stoneColor && board.boardStone[x, y].stateLeft != CellState.Empty && board.boardStone[x, y].stateLeft != CellState.OutRange)
            {
                way--;
            }
            else if(board.boardStone[x, y].stateLeft == stoneColor)
            {
                if (board.IsGrouoOfStoneOnSuicide(x, y, board.boardStone[x - 1, y].groupOfStones))
                {
                    way--;
                }
            }
            if(board.boardStone[x, y].stateRight != stoneColor && board.boardStone[x, y].stateRight != CellState.Empty && board.boardStone[x, y].stateRight != CellState.OutRange)
            {
                way--;
            }
            else if(board.boardStone[x, y].stateRight == stoneColor)
            {
                if (board.IsGrouoOfStoneOnSuicide(x, y, board.boardStone[x + 1, y].groupOfStones))
                {
                    way--;
                }
            }
            if (way == 0)
                return true;
            return false;
        }
    }
}
