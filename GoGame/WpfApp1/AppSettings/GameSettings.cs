using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGame.AppSettings
{
    internal class GameSettings
    {
        public static int BoardSize { get; set; }

        public GameSettings(int boardSize)
        {
            BoardSize = boardSize;
        }
    }
}
