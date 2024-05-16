using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using GoGame.Views;

namespace GoGame.Models
{
    internal class Game
    {
        public Board board;
        public static CellState playr1 = CellState.White;
        public static CellState playr2 = CellState.Black;
        public CellState currentMove;
        public GameBoard gameBoard;
        public int scoreWhite;
        public int scoreBlack;
        public bool endGame;
        public int numberOfPasses;

        public delegate void MyDelegate();
        public event MyDelegate ChangingCurrentMove;

        public Game()
        {
            board = new Board();
            currentMove = playr1;
            gameBoard = new GameBoard(this);
            scoreWhite = 0;
            scoreBlack = 0;
            endGame = false;
            numberOfPasses = 0;
        }

        public Game(GameBoard gameBoard, Board board)
        {
            this.board = board;
            currentMove = playr1;
            this.gameBoard = gameBoard;
            scoreWhite = 0;
            scoreBlack = 0;
            endGame = false;
            numberOfPasses = 0;
        }

        ~Game()
        {
            board = null;
            currentMove = playr1;
            gameBoard = null;
            scoreWhite = 0;
            scoreBlack = 0;
            ChangingCurrentMove = null;
        }
        public object Clone() => new Game(new GameBoard(this), new Board());

        public bool IsMoveValid(int x, int y, CellState stoneColor)
        {
            // Проверка на пустую клетку
            if (board.GetStoneAt(x, y) != CellState.Empty)
                return false;

            // Проверка на самоубийство
            if (IsSuicide(x, y, stoneColor))
                return false;

            // Проверка на запрет хода (ко рку)
            if (IsKo(x, y, stoneColor))
                return false;

            // Проверка на захват камней противника
            List<Stone> capturedStones = GetCapturedStones(x, y, stoneColor);
            if (capturedStones.Count > 0)
            {
                foreach (Stone s in capturedStones)
                {
                    board.boardStone[s.x, s.y].state = CellState.Empty;
                    foreach(Object child in gameBoard.gridBoardBig.Children)
                    {
                        if(child is Ellipse ellip)
                        {
                            string[] nameParts = ellip.Name.Split('_');
                            if (nameParts.Length == 3 && nameParts[0] == "ellipse")
                            {
                                if (int.TryParse(nameParts[1], out int ellipseX) && int.TryParse(nameParts[2], out int ellipseY))
                                {
                                    if (ellipseX == s.x && ellipseY == s.y)
                                    {                                        
                                        ellip.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            capturedStones.Clear();
            ChangingCurrentMove?.Invoke();
            return true;
        }

        private bool IsSuicide(int x, int y, CellState stoneColor)
        {
            // TODO: Реализовать проверку на самоубийство
            return ReChekIsSuicide(x,y,stoneColor);
        }

        private List<Stone> GetCapturedStones(int x, int y, CellState stoneColor)
        {
            List<Stone> stones = new List<Stone>();
            // TODO: Реализовать проверку на захват камней противника
            if (board.boardStone[x, y].stateTop != stoneColor && board.boardStone[x, y].stateTop != CellState.Empty && board.boardStone[x, y].stateTop != CellState.OutRange)
            {
                if (board.IsGroupOfStoneOnSuicide(x, y - 1, ref board.boardStone[x, y], stoneColor))
                {
                    foreach(Stone s in board.boardStone[x, y - 1].groupOfStones)
                    {
                        stones.Add(s);
                        board.boardStone[s.x, s.y].state = CellState.Empty;
                        if(currentMove == CellState.White)
                        {
                            scoreWhite++;
                        }
                        else
                        {
                            scoreBlack++;
                        }
                    }
                }
            }
            if (board.boardStone[x, y].stateBot != stoneColor && board.boardStone[x, y].stateBot != CellState.Empty && board.boardStone[x, y].stateBot != CellState.OutRange)
            {
                if (board.IsGroupOfStoneOnSuicide(x, y + 1, ref board.boardStone[x, y], stoneColor))
                {
                    foreach (Stone s in board.boardStone[x, y + 1].groupOfStones)
                    {
                        stones.Add(s);
                        board.boardStone[s.x, s.y].state = CellState.Empty;
                        if (currentMove == CellState.White)
                        {
                            scoreWhite++;
                        }
                        else
                        {
                            scoreBlack++;
                        }
                    }
                }
            }
            if (board.boardStone[x, y].stateLeft != stoneColor && board.boardStone[x, y].stateLeft != CellState.Empty && board.boardStone[x, y].stateLeft != CellState.OutRange)
            {
                if (board.IsGroupOfStoneOnSuicide(x - 1, y, ref board.boardStone[x, y], stoneColor))
                {
                    foreach (Stone s in board.boardStone[x - 1, y].groupOfStones)
                    {
                        stones.Add(s);
                        board.boardStone[s.x, s.y].state = CellState.Empty;
                        if (currentMove == CellState.White)
                        {
                            scoreWhite++;
                        }
                        else
                        {
                            scoreBlack++;
                        }
                    }
                }
            }
            if (board.boardStone[x, y].stateRight != stoneColor && board.boardStone[x, y].stateRight != CellState.Empty && board.boardStone[x, y].stateRight != CellState.OutRange)
            {
                if (board.IsGroupOfStoneOnSuicide(x + 1, y, ref board.boardStone[x, y], stoneColor))
                {
                    foreach (Stone s in board.boardStone[x + 1, y].groupOfStones)
                    {
                        stones.Add(s);
                        board.boardStone[s.x, s.y].state = CellState.Empty;
                        if (currentMove == CellState.White)
                        {
                            scoreWhite++;
                        }
                        else
                        {
                            scoreBlack++;
                        }
                    }
                }
            }
            return stones;
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
                if(board.IsGroupOfStoneOnSuicide(x, y - 1, ref board.boardStone[x, y], stoneColor))
                {
                    way--;
                }
            }
            else if(board.boardStone[x, y].stateTop == CellState.OutRange)
            {
                way--;
            }
            if(board.boardStone[x, y].stateBot != stoneColor && board.boardStone[x, y].stateBot != CellState.Empty && board.boardStone[x, y].stateBot != CellState.OutRange)
            {
                way--;
            }
            else if(board.boardStone[x, y].stateBot == stoneColor)
            {
                if (board.IsGroupOfStoneOnSuicide(x, y + 1, ref board.boardStone[x, y], stoneColor))
                {
                    way--;
                }
            }
            else if (board.boardStone[x, y].stateBot == CellState.OutRange)
            {
                way--;
            }
            if (board.boardStone[x, y].stateLeft != stoneColor && board.boardStone[x, y].stateLeft != CellState.Empty && board.boardStone[x, y].stateLeft != CellState.OutRange)
            {
                way--;
            }
            else if(board.boardStone[x, y].stateLeft == stoneColor)
            {
                if (board.IsGroupOfStoneOnSuicide(x - 1, y, ref board.boardStone[x, y], stoneColor))
                {
                    way--;
                }
            }
            else if (board.boardStone[x, y].stateLeft == CellState.OutRange)
            {
                way--;
            }
            if (board.boardStone[x, y].stateRight != stoneColor && board.boardStone[x, y].stateRight != CellState.Empty && board.boardStone[x, y].stateRight != CellState.OutRange)
            {
                way--;
            }
            else if(board.boardStone[x, y].stateRight == stoneColor)
            {
                if (board.IsGroupOfStoneOnSuicide(x + 1, y, ref board.boardStone[x, y], stoneColor))
                {
                    way--;
                }
            }
            else if (board.boardStone[x, y].stateRight == CellState.OutRange)
            {
                way--;
            }
            if (way == 0)
                return true;
            return false;
        }
    }
}
