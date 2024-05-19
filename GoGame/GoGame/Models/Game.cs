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
using GoGame.AppSettings;
using GoGame.Views;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoGame.Models
{
    internal class Game : ICloneable
    {
        public Board board;
        public static CellState playr1 = CellState.Black;
        public static CellState playr2 = CellState.White;
        public CellState currentMove;
        [JsonIgnore]
        public GameBoard gameBoard;
        public double scoreWhite;
        public double scoreBlack;
        public bool endGame;
        public int numberOfPasses;
        public int countMove;
        public bool startKo;
        public bool useKomi;

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
            countMove = 0;
            startKo = false;
            useKomi = false;
        }

        public Game(Board board, CellState currentMove, double scoreWhite, double scoreBlack, bool endGame, int numberOfPasses, int countMove, bool startKo, bool useKomi)
        {
            this.board = board;
            this.currentMove = currentMove;
            this.scoreWhite = scoreWhite;
            this.scoreBlack = scoreBlack;
            this.endGame = endGame;
            this.numberOfPasses = numberOfPasses;
            this.countMove = countMove;
            this.startKo = startKo;
            this.useKomi = useKomi;
            this.gameBoard = new GameBoard(this);
        }

        ~Game()
        {
            board = null;
            currentMove = playr1;
            gameBoard = null;
            scoreWhite = 0;
            scoreBlack = 0;
            ChangingCurrentMove = null;
            endGame = false;
            numberOfPasses = 0;
            countMove = 0;
            startKo = false;
            useKomi = false;
        }
        public object Clone()
        {
            Board clonedBoard = board.Clone() as Board;
            return new Game(clonedBoard, currentMove, scoreWhite, scoreBlack, endGame, numberOfPasses, countMove, startKo, useKomi);
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
            List<Stone> capturedStones = GetCapturedStones(x, y, stoneColor);
            if (capturedStones.Count > 0)
            {
                UpdateStonesForKo();
                // Проверка на запрет хода (ко рку)
                if (IsKo())
                {
                    foreach(Stone s in capturedStones)
                    {
                        if(currentMove == CellState.White)
                        {
                            board.boardStone[s.x, s.y].state = CellState.Black;
                            scoreWhite--;
                        }
                        else if(currentMove == CellState.Black)
                        {
                            board.boardStone[s.x, s.y].state = CellState.White;
                            scoreBlack--;
                        }
                    }
                    return false;
                }
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
            countMove++;
            capturedStones.Clear();
            return true;
        }

        private bool IsSuicide(int x, int y, CellState stoneColor)
        {
            // TODO: Реализовать проверку на самоубийство
            return ReChekIsSuicide(x,y,stoneColor);
        }

        public void MoveIsMade()
        {
            ChangingCurrentMove?.Invoke();
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

        private bool IsKo()
        {
            if (countMove <= 1)
                return false;
            bool flag = true;
            // TODO: Реализовать проверку на запрет хода (ко рку)
            if (startKo)
            {
                for (int i = 0; i < GameSettings.BoardSize; i++)
                {
                    for (int j = 0; j < GameSettings.BoardSize; j++)
                    {
                        if(board.stonesForCheckKo1[i, j] != board.boardStone[i, j].state)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < GameSettings.BoardSize; i++)
                {
                    for (int j = 0; j < GameSettings.BoardSize; j++)
                    {
                        if (board.stonesForCheckKo2[i, j] != board.boardStone[i, j].state)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }
           
            return flag;
        }

        private void UpdateStonesForKo()
        {
            if (startKo)
            {
                for(int i = 0; i< GameSettings.BoardSize; i++)
                {
                    for (int j = 0; j < GameSettings.BoardSize; j++)
                    {
                        if (board.boardStone[i,j].state == CellState.Empty)
                            board.stonesForCheckKo1[i, j] = CellState.Empty;
                        else if(board.boardStone[i, j].state == CellState.White)
                            board.stonesForCheckKo1[i, j] = CellState.White;
                        else if(board.boardStone[i, j].state == CellState.Black)
                            board.stonesForCheckKo1[i, j] = CellState.Black;
                    }
                }
                startKo = false;
            }
            else
            {
                for (int i = 0; i < GameSettings.BoardSize; i++)
                {
                    for (int j = 0; j < GameSettings.BoardSize; j++)
                    {
                        if (board.boardStone[i, j].state == CellState.Empty)
                            board.stonesForCheckKo2[i, j] = CellState.Empty;
                        else if (board.boardStone[i, j].state == CellState.White)
                            board.stonesForCheckKo2[i, j] = CellState.White;
                        else if (board.boardStone[i, j].state == CellState.Black)
                            board.stonesForCheckKo2[i, j] = CellState.Black;
                    }
                }
                startKo = true;
            }
        }

        private bool ReChekIsSuicide(int x, int y, CellState stoneColor)
        {
            int way = 4;
            if (board.boardStone[x,y].stateTop != stoneColor && board.boardStone[x, y].stateTop != CellState.Empty && board.boardStone[x,y].stateTop != CellState.OutRange)
            {
                if (!board.IsGroupOfStoneOnSuicide(x, y - 1, ref board.boardStone[x, y], stoneColor))
                {
                    way--;
                }
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
                if (!board.IsGroupOfStoneOnSuicide(x, y + 1, ref board.boardStone[x, y], stoneColor))
                {
                    way--;
                }
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
                if (!board.IsGroupOfStoneOnSuicide(x - 1, y, ref board.boardStone[x, y], stoneColor))
                {
                    way--;
                }
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
                if (!board.IsGroupOfStoneOnSuicide(x + 1, y, ref board.boardStone[x, y], stoneColor))
                {
                    way--;
                }
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

        public void EndGame()
        {
            if (endGame)
            {
                CalculateTerritoryScores();
                if(useKomi)
                    scoreWhite += GameSettings.KamiScore;
                gameBoard.IsEnabled = false;
                if(scoreBlack > scoreWhite)
                {
                    MessageBox.Show($"Win Player1 with a point difference of {scoreBlack-scoreWhite} stones\n score Player1 = {scoreBlack} \n score Player2 = {scoreWhite}");
                }
                else if(scoreWhite > scoreBlack)
                {
                    MessageBox.Show($"Win Player2 with a point difference of {scoreWhite - scoreBlack} stones\n score Player1 = {scoreBlack} \n score Player2 = {scoreWhite}");
                }
                else
                {
                    MessageBox.Show($"Draw \n score Player1 = {scoreBlack} \n score Player2 = {scoreWhite}");
                }
            }
        }

        // Метод для подсчета очков захваченной территории
        public void CalculateTerritoryScores()
        {
            bool[,] visited = new bool[board.boardSize, board.boardSize];

            for (int x = 0; x < board.boardSize; x++)
            {
                for (int y = 0; y < board.boardSize; y++)
                {
                    if (!visited[x, y] && board.boardStone[x, y].state == CellState.Empty)
                    {
                        var territory = GetTerritory(x, y, visited);
                        if (territory != null)
                        {
                            if (territory.Item2 == CellState.White)
                            {
                                scoreWhite += territory.Item1.Count;
                            }
                            else if (territory.Item2 == CellState.Black)
                            {
                                scoreBlack += territory.Item1.Count;
                            }
                        }
                    }
                }
            }
        }

        private Tuple<List<Stone>, CellState> GetTerritory(int startX, int startY, bool[,] visited)
        {
            List<Stone> territory = new List<Stone>();
            Queue<Stone> queue = new Queue<Stone>();
            CellState boundaryState = CellState.Empty;
            bool isEnclosed = true;

            queue.Enqueue(board.boardStone[startX, startY]);
            visited[startX, startY] = true;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                territory.Add(current);

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (neighbor.state == CellState.Empty && !visited[neighbor.x, neighbor.y])
                    {
                        queue.Enqueue(neighbor);
                        visited[neighbor.x, neighbor.y] = true;
                    }
                    else if (neighbor.state == CellState.Black || neighbor.state == CellState.White)
                    {
                        if (boundaryState == CellState.Empty)
                        {
                            boundaryState = neighbor.state;
                        }
                        else if (boundaryState != neighbor.state)
                        {
                            isEnclosed = false;
                        }
                    }
                }
            }

            return isEnclosed ? Tuple.Create(territory, boundaryState) : null;
        }

        private IEnumerable<Stone> GetNeighbors(Stone stone)
        {
            int[][] directions = new int[][]
            {
            new int[] { 0, -1 }, // Top
            new int[] { 0, 1 },  // Bottom
            new int[] { -1, 0 }, // Left
            new int[] { 1, 0 }   // Right
            };

            foreach (var direction in directions)
            {
                int newX = stone.x + direction[0];
                int newY = stone.y + direction[1];

                if (newX >= 0 && newX < board.boardSize && newY >= 0 && newY < board.boardSize)
                {
                    yield return board.boardStone[newX, newY];
                }
            }
        }
    }
}
