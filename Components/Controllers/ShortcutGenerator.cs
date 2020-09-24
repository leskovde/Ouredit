using System.Text.Json;
using System.IO;
using Components.DataRepository;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Components.Controllers
{
    [Sauerova]
    public class ShortcutGenerator
    {
        private const string JSONFilePath = @"commands.json";
        public ShortcutGenerator()
        {
            //Creates JSON file with available commands as keys and their corresponding shortcuts
            if (!System.IO.File.Exists(JSONFilePath))
            {
                using (StreamWriter sw = File.CreateText(JSONFilePath))
                {
                    sw.Write(SerializeToFile(CommandList.Get()));
                }
            }
        }

        /// <summary>
        /// Indented serialization of data for writing to files.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string SerializeToFile<T>(T data) => JsonSerializer.Serialize<T>(data, new JsonSerializerOptions { WriteIndented = true, });

        public async Task<Dictionary<string, string>> GetCurrentCommandListAsync()
        {
            if (System.IO.File.Exists(JSONFilePath))
            {
                string content;
                using (StreamReader sr = new StreamReader(JSONFilePath))
                {
                    content = await sr.ReadToEndAsync();
                }
                return JsonSerializer.Deserialize<Dictionary<string, string>>(content);
            }
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Rewrites asynchronously a shortcut for a specific command in commands.json file.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="newShortcut"></param>
        /// <returns></returns>
        public async Task<bool> ChangeShortcutAsync(string command, string newShortcut)
        {
            Dictionary<string, string> commandList = await GetCurrentCommandListAsync();
            if (EnsureUnambiguityAsync(commandList, newShortcut))
            {
                if (commandList.ContainsKey(command))
                {
                    commandList[command] = newShortcut;
                    await System.IO.File.WriteAllTextAsync(JSONFilePath, SerializeToFile(commandList));
                    return true;
                }
            }
            return false;
        }

        private bool EnsureUnambiguityAsync(Dictionary<string, string> commandList, string newShortcut)
        {
            if (commandList.ContainsKey(newShortcut))
            {
                return false;
            }
            return true;
        }
    }
}
