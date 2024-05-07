using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGame.Models
{
    internal class Game
    {
        public List<Stone> Board { get; }
        public Player CurrentPlayer { get; private set; }

        public Game(int boardSize, Player startingPlayer)
        {
            Board = new List<Stone>();
            // Инициализация игрового поля с пустыми камнями
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    Board.Add(new Stone(x, y, CellState.Empty));
                }
            }
            CurrentPlayer = startingPlayer;
        }
    }
}
