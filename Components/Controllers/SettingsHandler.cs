using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Components.Controllers
{
    /// <summary>
    /// Keeps the settings data in the memory. Each category has its container.
    /// </summary>
    [Leskovar]
    public class Settings
    {
        public CategoryContainer General { get; private set; }
        public CategoryContainer Editing { get; private set; }
        public CategoryContainer NewDocument { get; private set; }
        public CategoryContainer DefaultDirectory { get; private set; }
        public CategoryContainer RecentFilesHistory { get; private set; }
        public CategoryContainer Highlighting { get; private set; }

        /// <summary>
        /// Contains names and values of different types of UI controls for a category.
        /// The data is stored in tuples, the first item represents the name of the control and the second item is its value.
        /// </summary>
        public class CategoryContainer
        {
            public readonly List<(string, string)> ComboBoxes;
            public readonly List<(string, bool)> CheckBoxes;
            public readonly List<(string, string)> TextBoxes;

            public CategoryContainer(List<(string, string)> comboBoxes, List<(string, bool)> checkBoxes, List<(string, string)> textBoxes)
            {
                ComboBoxes = comboBoxes;
                CheckBoxes = checkBoxes;
                TextBoxes = textBoxes;
            }
        }

        /// <summary>
        /// Handles interactions between the settings data and the user or the application.
        /// </summary>
        [Leskovar]
        public class SettingsHandler
        {
            public Settings SettingsInstance { get; private set; }
            public const string FileHistoryPath = @"Settings\fileHistory.json";
            private const string _settingsPath = @"Settings\settingsDump.json";
            private const string _defaultSettingsPath = @"Reference\defaultSettings.json";

            public SettingsHandler()
            {
                try
                {
                    var jsonString =
                        File.ReadAllText(File.Exists(_settingsPath) ? _settingsPath : _defaultSettingsPath);

                    SettingsInstance = JsonSerializer.Deserialize<Settings>(jsonString);
                }
                catch
                {
                    SettingsInstance = new Settings
                    {
                        General = new CategoryContainer(new List<(string, string)>(), new List<(string, bool)>(),
                            new List<(string, string)>()),
                        Editing = new CategoryContainer(new List<(string, string)>(), new List<(string, bool)>(),
                            new List<(string, string)>()),
                        NewDocument = new CategoryContainer(new List<(string, string)>(), new List<(string, bool)>(),
                            new List<(string, string)>()),
                        DefaultDirectory = new CategoryContainer(new List<(string, string)>(), new List<(string, bool)>(),
                            new List<(string, string)>()),
                        RecentFilesHistory = new CategoryContainer(new List<(string, string)>(), new List<(string, bool)>(),
                            new List<(string, string)>()),
                        Highlighting = new CategoryContainer(new List<(string, string)>(), new List<(string, bool)>(),
                            new List<(string, string)>())
                    };
                }
            }

            /// <summary>
            /// Resets the current settings configuration to the one saved in a default settings file.
            /// </summary>
            public async Task ResetSettings()
            {
                await using var fs = File.Create(_defaultSettingsPath);
                SettingsInstance = await JsonSerializer.DeserializeAsync<Settings>(fs);
            }

            /// <summary>
            /// Changes a check box of a given name to a given value.
            /// </summary>
            /// <param name="category">The category the UI control belongs to.</param>
            /// <param name="name">The name of the UI control containing the given setting.</param>
            /// <param name="value">The value of the setting to be set.</param>
            public void ChangeSettings(CategoryContainer category, string name, bool value)
            {
                var checkBoxMatches = category.CheckBoxes.Where(x => x.Item1 == name).ToList();

                if (!checkBoxMatches.Any()) throw new InvalidOperationException();
                
                var index = category.CheckBoxes.IndexOf(checkBoxMatches.First());
                category.CheckBoxes[index] = (name, value);
            }

            /// <summary>
            /// Changes a text box or a combo box of a given name to a given value.
            /// </summary>
            /// <param name="category">The category the UI control belongs to.</param>
            /// <param name="name">The name of the UI control containing the given setting.</param>
            /// <param name="value">The value of the setting to be set.</param>
            public void ChangeSettings(CategoryContainer category, string name, string value)
            {
                var comboBoxMatches = category.ComboBoxes.Where(x => x.Item1 == name).ToList();

                if (comboBoxMatches.Any())
                {
                    var index = category.ComboBoxes.IndexOf(comboBoxMatches.First());
                    category.ComboBoxes[index] = (name, value);
                }
                else
                {
                    var textBoxMatches = category.TextBoxes.Where(x => x.Item1 == name).ToList();

                    if (!comboBoxMatches.Any())
                    {
                        throw new InvalidOperationException();
                    }
                    var index = category.TextBoxes.IndexOf(textBoxMatches.First());
                    category.TextBoxes[index] = (name, value);
                }
            }

            /// <summary>
            /// Dumps the current settings to a strictly set path on the disc.
            /// </summary>
            public async Task SaveSettingsToDisc()
            {
                await using var fs = File.Create(_settingsPath);
                await JsonSerializer.SerializeAsync(fs, SettingsInstance);
            }
        }
    }
}
