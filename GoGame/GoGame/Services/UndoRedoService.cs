using GoGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGame.Services
{
    internal class UndoRedoService
    {
        private Stack<Game> _undoStack;
        private Stack<Game> _redoStack;

        public UndoRedoService()
        {
            _undoStack = new Stack<Game>();
            _redoStack = new Stack<Game>();
        }

        public void AddMove(Game game)
        {
            Game game1 = (Game)game.Clone();

            _undoStack.Push(game1);
            _redoStack.Clear(); // Очищаем стек переходов назад, когда добавляем новый ход
        }

        public Game UndoMove()
        {
            if (_undoStack.Count == 0)
                return null;

            Game previousGame = _undoStack.Pop();
            _redoStack.Push(previousGame);
            return previousGame;
        }

        public Game RedoMove()
        {
            if (_redoStack.Count == 0)
                return null;

            Game nextGame = _redoStack.Pop();
            _undoStack.Push(nextGame);
            return nextGame;
        }
    }
}
