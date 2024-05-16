using GoGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

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

                string json = JsonSerializer.Serialize(game);
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
                return JsonSerializer.Deserialize<Game>(json);
            }
            return JsonSerializer.Deserialize<Game>("");
        }
    }
}
