using GoGame.Models;
using GoGame.Views;
using System.Collections.Generic;

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
            // Клонируем текущее состояние игры и добавляем его в стек undo
            Game clonedGame = (Game)game.Clone();
            _undoStack.Push(clonedGame);
            // Очищаем стек redo, так как мы начали новую ветку действий
            _redoStack.Clear();
        }

        public Game UndoMove()
        {
            if (_undoStack.Count == 0)
                return null;

            Game previousGame = _undoStack.Pop();
            _redoStack.Push((Game)previousGame.Clone());
            previousGame.gameBoard = new GameBoard(previousGame);
            return previousGame;
        }

        public Game RedoMove()
        {
            if (_redoStack.Count == 0)
                return null;

            Game nextGame = _redoStack.Pop();
            _undoStack.Push((Game)nextGame.Clone());
            nextGame.gameBoard = new GameBoard(nextGame);
            return nextGame;
        }
    }
}
