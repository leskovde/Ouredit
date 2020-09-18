using System.Text.Json;
using System.IO;
using Components.DataRepository;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Components.Controllers
{
    [Sauerova]
    public class ShortcutGenerator
    {
        private const string JSONFilePath = @"data/commands.json";
        static ShortcutGenerator()
        {
            //Creates JSON file with available commands as keys and their corresponding shortcuts
            if (!System.IO.File.Exists(JSONFilePath))
            {
                using (StreamWriter sw = System.IO.File.CreateText(JSONFilePath))
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
        
        /// <summary>
        /// Rewrites a shortcut for a specific command in commands.json file.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="newShortcut"></param>
        /// <returns></returns>
        public bool ChangeShortcut(string command, string newShortcut)
        {
            if (!System.IO.File.Exists(JSONFilePath))
            {
                string content;
                using (StreamReader sr = new StreamReader(JSONFilePath))
                {
                    content = sr.ReadToEnd();
                }
                if (EnsureUnambiguityAsync(content, newShortcut))
                {
                    Dictionary<string, string> commandList = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
                    if (commandList.ContainsValue(command))
                    {
                        commandList[command] = newShortcut;
                        System.IO.File.WriteAllText(JSONFilePath, SerializeToFile(commandList));
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Rewrites asynchronously a shortcut for a specific command in commands.json file.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="newShortcut"></param>
        /// <returns></returns>
        public async Task<bool> ChangeShortcutAsync(string command, string newShortcut)
        {
            if (!System.IO.File.Exists(JSONFilePath))
            {
                string content;
                using (StreamReader sr = new StreamReader(JSONFilePath))
                {
                    content = await sr.ReadToEndAsync();
                }
                if (EnsureUnambiguityAsync(content, newShortcut))
                {
                    Dictionary<string, string> commandList = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
                    if (commandList.ContainsValue(command))
                    {
                        commandList[command] = newShortcut;
                        await System.IO.File.WriteAllTextAsync(JSONFilePath, SerializeToFile(commandList));
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        private bool EnsureUnambiguityAsync(string content, string newShortcut)
        {
            if (content.Contains(newShortcut))
            {
                return false;
            }
            return true;
        }
    }
}
