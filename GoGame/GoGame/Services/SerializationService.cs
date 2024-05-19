using GoGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;
using GoGame.Views;

namespace GoGame.Services
{
    internal static class SerializationService
    {
        public static void SaveGame(Game game)
        {
            // Открываем диалоговое окно сохранения файла
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Файлы сохранения (*.json)|*.json|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                // Получаем путь к выбранному файлу
                string filePath = saveFileDialog.FileName;

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(game, options);
                File.WriteAllText(filePath, json);
            }
        }

        public static Game LoadGame()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Файлы сохранения (*.json)|*.json|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                // Получаем путь к выбранному файлу
                string filePath = openFileDialog.FileName;

                string json = File.ReadAllText(filePath);
                Game loadedGame = JsonSerializer.Deserialize<Game>(json);
                loadedGame.gameBoard = new GameBoard(loadedGame); // Восстанавливаем связанный GameBoard
                return loadedGame;
            }
            return null;
        }
    }
}
