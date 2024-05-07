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
    internal class SerializationService
    {
        public void SaveGame(Game game, string filePath)
        {
            string json = JsonSerializer.Serialize(game);
            File.WriteAllText(filePath, json);
        }

        public Game LoadGame(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Game>(json);
        }
    }
}
