﻿using System;
using System.Collections.Generic;
using GoGame.AppSettings;

namespace GoGame.Models
{
    public enum CellState { Black = 2, White = 1, Empty = 0, OutRange = 3}

    struct Stone : ICloneable
    {
        public int x, y;
        public CellState state;
        public CellState stateTop;
        public CellState stateBot;
        public CellState stateLeft;
        public CellState stateRight;

        public List<Stone> groupOfStones;

        public Stone(int x, int y, CellState state)
        {
            this.x = x;
            this.y = y;
            this.state = state;
            this.stateTop = CellState.Empty;
            this.stateBot = CellState.Empty;
            this.stateLeft = CellState.Empty;
            this.stateRight = CellState.Empty;
            this.groupOfStones = new List<Stone>();
        }

        public object Clone()
        {
            return new Stone(x, y, state);
        }
    }

    internal class Board : ICloneable
    {
        public int boardSize;
        public Stone[,] boardStone;
        readonly List<Stone> varStones;
        public CellState[,] stonesForCheckKo1;
        public CellState[,] stonesForCheckKo2;

        public Board()
        {
            varStones = new List<Stone>();
            boardSize = GameSettings.BoardSize;
            boardStone = new Stone[boardSize, boardSize];
            stonesForCheckKo1 = new CellState[boardSize, boardSize];
            stonesForCheckKo2 = new CellState[boardSize, boardSize];
            InitializeBoard();
        }

        private Board(int boardSize, List<Stone> varStones, Stone[,] boardStone, CellState[,] stonesForCheckKo1, CellState[,] stonesForCheckKo2)
        {
            this.boardSize = boardSize;
            this.varStones = varStones;
            this.boardStone = (Stone[,])boardStone.Clone();
            this.stonesForCheckKo1 = (CellState[,])stonesForCheckKo1.Clone();
            this.stonesForCheckKo2 = (CellState[,])stonesForCheckKo2.Clone();
            InitializeBoard();
        }

        public object Clone()
        {
            return new Board(boardSize, varStones, (Stone[,])boardStone.Clone(), (CellState[,])stonesForCheckKo1.Clone(), (CellState[,])stonesForCheckKo2.Clone());
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    boardStone[i, j] = new Stone
                    {
                        state = CellState.Empty,
                        x = i,
                        y = j,
                        stateTop = CellState.Empty,
                        stateBot = CellState.Empty,
                        stateLeft = CellState.Empty,
                        stateRight = CellState.Empty,
                        groupOfStones = new List<Stone>()
                    };
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

        public void СheckWayStone(ref Stone stone)
        {
            if(stone.y != 0)
            {
                stone.stateTop = boardStone[stone.x, stone.y-1].state;
            }
            else { stone.stateTop = CellState.OutRange; }
            if(stone.y !=  boardSize - 1)
            {
                stone.stateBot = boardStone[stone.x, stone.y + 1].state;
            }
            else { stone.stateBot = CellState.OutRange; }
            if (stone.x != 0)
            {
                stone.stateLeft = boardStone[stone.x-1, stone.y].state;
            }
            else { stone.stateLeft = CellState.OutRange; }
            if(stone.x != boardSize - 1)
            {
                stone.stateRight = boardStone[stone.x+1, stone.y].state;
            }
            else{ stone.stateRight = CellState.OutRange; }
        }

        public void CheckGroupOfStone(ref Stone stone)
        {
            if (stone.state == CellState.White || stone.state == CellState.Black)
                stone.groupOfStones.Add(stone);
            if (stone.y != 0)
            {
                if (stone.state != CellState.Empty && stone.state == stone.stateTop)
                {
                    foreach (Stone s in ReCheckGroupOfStone(ref boardStone[stone.x, stone.y - 1]))
                    {
                        if(!stone.groupOfStones.Contains(s))
                            stone.groupOfStones.Add(s);
                    }
                }
            }
            if (stone.y != boardSize - 1)
            {
                if (stone.state != CellState.Empty && stone.state == stone.stateBot)
                {
                    foreach (Stone s in ReCheckGroupOfStone(ref boardStone[stone.x, stone.y + 1]))
                    {
                        if (!stone.groupOfStones.Contains(s))
                            stone.groupOfStones.Add(s);
                    }
                }
            }
            if (stone.x != 0)
            {
                if (stone.state != CellState.Empty && stone.state == stone.stateLeft)
                {
                    foreach (Stone s in ReCheckGroupOfStone(ref boardStone[stone.x - 1, stone.y]))
                    {
                        if (!stone.groupOfStones.Contains(s))
                            stone.groupOfStones.Add(s);
                    }
                }
            }
            if (stone.x != boardSize - 1)
            {
                if (stone.state != CellState.Empty && stone.state == stone.stateRight)
                {
                    foreach (Stone s in ReCheckGroupOfStone(ref boardStone[stone.x + 1, stone.y]))
                    {
                        if (!stone.groupOfStones.Contains(s))
                            stone.groupOfStones.Add(s);
                    }
                }
            }
            varStones.Clear();
        }

        public List<Stone> ReCheckGroupOfStone(ref Stone stone)
        {
            List<Stone> stones = new List<Stone>();
            if (varStones.Contains(stone) || stone.state == CellState.OutRange)
            {
                return stones;
            }
            varStones.Add(stone);
            stones.Add(stone);

            if (stone.state == stone.stateTop && stone.stateTop != CellState.OutRange)
            {
                if (stone.y != 0)
                {
                    foreach (Stone s in ReCheckGroupOfStone(ref boardStone[stone.x, stone.y - 1]))
                        stones.Add(s);
                }
            }
            if (stone.state == stone.stateBot && stone.stateBot != CellState.OutRange)
            {
                if (stone.y != boardSize)
                {
                    foreach (Stone s in ReCheckGroupOfStone(ref boardStone[stone.x, stone.y + 1]))
                        stones.Add(s);
                }
            }
            if (stone.state == stone.stateLeft && stone.stateLeft != CellState.OutRange)
            {
                if (stone.x != 0)
                {
                    foreach (Stone s in ReCheckGroupOfStone(ref boardStone[stone.x - 1, stone.y]))
                        stones.Add(s);
                }
            }
            if (stone.state == stone.stateRight && stone.stateRight != CellState.OutRange)
            {
                if (stone.x != boardSize)
                {
                    foreach (Stone s in ReCheckGroupOfStone(ref boardStone[stone.x + 1, stone.y]))
                        stones.Add(s);
                }
            }                        
            return stones;
        }

        public void UpdateBordStons()
        {
            for(int i = 0; i < boardSize; i++)
            {
                for(int j = 0; j < boardSize; j++)
                {
                    СheckWayStone(ref boardStone[i, j]);
                    boardStone[i, j].groupOfStones.Clear();
                }
            }
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    CheckGroupOfStone(ref boardStone[i, j]);
                }
            }
        }

        public bool IsGroupOfStoneOnSuicide(int x, int y, ref Stone stone, CellState currentMove)
        {
            bool flag = true;
            boardStone[stone.x, stone.y].state = currentMove;
            UpdateBordStons();
            foreach (Stone s in boardStone[x, y].groupOfStones)
            {
                if(s.stateTop == CellState.Empty 
                    || s.stateBot == CellState.Empty 
                    || s.stateLeft == CellState.Empty 
                    || s.stateRight == CellState.Empty)
                {
                    flag = false;
                }
            }
            boardStone[stone.x, stone.y].state = CellState.Empty;
            UpdateBordStons();
            return flag;
        }
    }
}
