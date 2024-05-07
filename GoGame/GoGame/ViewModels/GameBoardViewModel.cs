using GoGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGame.ViewModels
{
    internal class GameBoardViewModel
    {
        public Game Game { get; }

        public GameBoardViewModel(Game game)
        {
            Game = game;
        }
    }
}
