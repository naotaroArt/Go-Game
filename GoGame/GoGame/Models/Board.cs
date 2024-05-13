using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using GoGame.AppSettings;

namespace GoGame.Models
{
    public enum CellState { Black = 2, White = 1, Empty = 0, OutRange = 3}
    public enum Direction { top, bot, left, right }

    struct Stone
    {
        public int x, y;
        public CellState state;
        public CellState stateTop;
        public CellState stateBot;
        public CellState stateLeft;
        public CellState stateRight;

        public List<Stone> groupOfStones;      
    }

    internal class Board
    {
        public int boardSize;
        public Stone[,] boardStone;

        public Board()
        {
            boardSize = GameSettings.BoardSize;
            boardStone = new Stone[boardSize, boardSize];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    boardStone[i, j] = new Stone();
                    boardStone[i, j].state = CellState.Empty;
                    boardStone[i, j].x = i;
                    boardStone[i, j].y = j;
                    boardStone[i, j].stateTop = CellState.Empty;
                    boardStone[i, j].stateBot = CellState.Empty;
                    boardStone[i, j].stateLeft = CellState.Empty;
                    boardStone[i, j].stateRight = CellState.Empty;
                    boardStone[i, j].groupOfStones = new List<Stone>();
                }
            }
        }

        public bool PlaceStone(int x, int y, CellState stoneColor)
        {
            if (!IsWithinBounds(x, y) || boardStone[x, y].state != CellState.Empty)
                return false;

            boardStone[x, y].state = stoneColor;
            return true;
        }

        public CellState GetStoneAt(int x, int y)
        {
            if (!IsWithinBounds(x, y))
                throw new ArgumentOutOfRangeException("Coordinates out of bounds");

            return boardStone[x, y].state;
        }

        private bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < boardSize && y >= 0 && y < boardSize;
        }

        public void СheckWayStone(Stone stone)
        {
            try
            {
                stone.stateTop = boardStone[stone.x, stone.y-1].state;
            }
            catch { stone.stateTop = CellState.OutRange; }
            try
            {
                stone.stateBot = boardStone[stone.x, stone.y + 1].state;
            }
            catch { stone.stateBot = CellState.OutRange; }
            try
            {
                stone.stateLeft = boardStone[stone.x-1, stone.y].state;
            }
            catch { stone.stateLeft = CellState.OutRange; }
            try
            {
                stone.stateRight = boardStone[stone.x+1, stone.y].state;
            }
            catch { stone.stateRight = CellState.OutRange; }
        }

        public void CheckGroupOfStone(Stone stone)
        {
            try
            {
                if (stone.state == stone.stateTop)
                {
                    foreach (Stone s in ReCheckGroupOfStone(boardStone[stone.x, stone.y - 1], Direction.bot))
                        stone.groupOfStones.Add(s);
                }
            }
            catch { }
            try
            {
                if (stone.state == stone.stateBot)
                {
                    foreach (Stone s in ReCheckGroupOfStone(boardStone[stone.x, stone.y + 1], Direction.top))
                        stone.groupOfStones.Add(s);
                }
            }
            catch { }
            try
            {
                if (stone.state == stone.stateLeft)
                {
                    foreach (Stone s in ReCheckGroupOfStone(boardStone[stone.x+1, stone.y], Direction.right))
                        stone.groupOfStones.Add(s);
                }
            }
            catch { }
            try
            {
                if (stone.state == stone.stateRight)
                {
                    foreach (Stone s in ReCheckGroupOfStone(boardStone[stone.x - 1, stone.y], Direction.left))
                        stone.groupOfStones.Add(s);
                }
            }
            catch { }
        }

        public List<Stone> ReCheckGroupOfStone(Stone stone, Direction direct)
        {
            List<Stone> stones = new List<Stone>();
            if (direct != Direction.top && stone.state == stone.stateTop)
            {
                foreach (Stone s in ReCheckGroupOfStone(boardStone[stone.x, stone.y - 1], Direction.bot))
                    stones.Add(s);
            }
            if (direct != Direction.bot && stone.state == stone.stateBot)
            {
                foreach (Stone s in ReCheckGroupOfStone(boardStone[stone.x, stone.y + 1], Direction.top))
                    stones.Add(s);
            }
            if (direct != Direction.left && stone.state == stone.stateLeft)
            {
                foreach (Stone s in ReCheckGroupOfStone(boardStone[stone.x + 1, stone.y], Direction.right))
                    stones.Add(s);
            }
            if (direct != Direction.right && stone.state == stone.stateRight)
            {
                foreach (Stone s in ReCheckGroupOfStone(boardStone[stone.x - 1, stone.y - 1], Direction.left))
                    stones.Add(s);
            }
            stones.Add(stone);
            return stones;
        }

        public void UpdateBordStons()
        {
            for(int i = 0; i < boardSize; i++)
            {
                for(int j = 0; j < boardSize; j++)
                {
                    СheckWayStone(boardStone[i, j]);
                    CheckGroupOfStone(boardStone[i, j]);
                }
            }
        }

        public bool IsGrouoOfStoneOnSuicide(int x, int y, List<Stone> stones)
        {
            boardStone[x, y].state = CellState.White;
            foreach(Stone s in stones)
            {
                if(s.stateTop == CellState.Empty 
                    || s.stateBot == CellState.Empty 
                    || s.stateLeft == CellState.Empty 
                    || s.stateRight == CellState.Empty)
                {
                    boardStone[x, y].state = CellState.Empty;
                    return false;
                }
            }
            boardStone[x, y].state = CellState.Empty;
            return true;
        }
    }
}
