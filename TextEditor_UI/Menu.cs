using Components;
using Components.Commands;
using Components.Controllers;
using Components.Models;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;

namespace OurTextEditor
{
    public static class Menu
    {
        public static event EventHandler ShortcutsChanged;
        static ShortcutGenerator ShortcutGenerator = new ShortcutGenerator();
        static Dictionary<string, string> Commands { get; set; }

        public static void RaiseEvent(object sender)
        {
            ShortcutsChanged?.Invoke(sender ,EventArgs.Empty);
        }

        static async void UpdateShortcuts(object sender, EventArgs args)
        {
            Commands = await ShortcutGenerator.GetCurrentCommandListAsync();
            SetMenu();
        }

        public static async void OpenMainWindow()
        {
            ShortcutsChanged += UpdateShortcuts;
            Commands = await ShortcutGenerator.GetCurrentCommandListAsync();

            var options = new BrowserWindowOptions()
            {
                Show = false,
            };

            var mainWindow = await Electron.WindowManager.CreateWindowAsync(options);
            mainWindow.OnReadyToShow += () =>
            {
                mainWindow.Show();
                var openFiles = ApplicationState.Instance.FileHandlerInstance.GetOpenFilePaths();

                // If history is present, set the first file as selected. Otherwise open a new file and set it as selected.
                if (openFiles.Count > 0)
                {
                    MenuActions.SetCurrentFilePath(openFiles.First());
                }
                else
                {
                    ApplicationState.Instance.FileHandlerInstance.OpenFile("new.txt");
                    ApplicationState.Instance.FileHandlerInstance.GetFileBuffer("new.txt").FillBufferFromFile();
                    MenuActions.SetCurrentFilePath("new.txt");
                }

            };

            mainWindow.OnClosed += () => { Electron.App.Quit(); };

            SetMenu();
        } 

        static void SetMenu()
        {
            var encodingCheckedValues = new bool[6];
            encodingCheckedValues[1] = true;

            var indexMenu = new MenuItem[]
            {
                new MenuItem
                {
                    Label = "File",
                    Submenu = new MenuItem[]
                    {
                        new MenuItem
                        {
                            Label = "New",
                            Accelerator = Commands["New file"],
                            Click = async () => { await MenuActions.NewFileAsync(); }
                        },
                        new MenuItem
                        {
                            Label = "Open...",
                            Accelerator = Commands["Open file"],
                            Click = async () => { await MenuActions.OpenFileAsync(); }
                        },
                        new MenuItem
                        {
                            Label = "Save",
                            Accelerator = Commands["Save file"],
                            Click = async () => { await MenuActions.SaveFileAsync(); }
                        },
                        new MenuItem
                        {
                            Label = "Save as...",
                            Accelerator = Commands["Save file as"],
                            Click = async () => { await MenuActions.SaveFileAsAsync(); }

                        },
                        new MenuItem
                        {
                            Label = "Close",
                            Accelerator = Commands["Close file"],
                            Click = async () => { await MenuActions.CloseAsync(); }
                        },
                        new MenuItem
                        {
                            Label = "Exit",
                            Accelerator = Commands["Exit"],
                            Click = () =>
                            {
                                var openFiles = ApplicationState.Instance.FileHandlerInstance.GetOpenFilePaths();
                                Console.WriteLine("#DEBUG: Serializing: " + string.Join(", ", openFiles));

                                var jsonString = JsonSerializer.Serialize(openFiles);
                                System.IO.File.WriteAllText(Settings.SettingsHandler.FileHistoryPath, jsonString); 

                                Electron.WindowManager.BrowserWindows.First().Close();
                            }
                        },
                    }
                },
                new MenuItem
                {
                    Label = "Edit",
                    Submenu = new MenuItem[]
                    {
                        new MenuItem
                        {
                            Label = "[PI]Undo",
                            Click = async () => { await CommandInvoker.Instance.Undo(); }
                        },
                        new MenuItem
                        {
                            Label = "[PI]Redo",
                            Click = async () => { await CommandInvoker.Instance.Redo(); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Indent",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Convert Case to",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Comment/Uncomment",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "Change EOLs to",
                            Submenu = new MenuItem[]
                            {
                                new MenuItem
                                {
                                    Label = "LF",
                                    Click = async () =>
                                    {
                                        await CommandInvoker.Instance.Execute(new ChangeLineEndings(MenuActions.CurrentFilePath, LineEndings.LF));
                                    }
                                },
                                new MenuItem
                                {
                                    Label = "CR",
                                    Click = async () =>
                                    {
                                        await CommandInvoker.Instance.Execute(new ChangeLineEndings(MenuActions.CurrentFilePath, LineEndings.CR));
                                    }
                                },
                                new MenuItem
                                {
                                    Label = "CR LF",
                                    Click = async () =>
                                    {
                                        await CommandInvoker.Instance.Execute(new ChangeLineEndings(MenuActions.CurrentFilePath, LineEndings.CRLF));
                                    }
                                },
                            }
                        }
                    }
                },
                new MenuItem
                {
                    Label = "Search",
                    Submenu = new MenuItem[]
                    {
                        new MenuItem
                        {
                            Label = "[NI]Find...",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Find in Files...",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Find Next",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Find Previous",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Replace",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Go to...",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                    }
                },
                new MenuItem
                {
                    Label = "View",
                    Submenu = new MenuItem[]
                    {
                        new MenuItem
                        {
                            Label = "Toggle Full Screen Mode",
                            Click = async () =>
                            {
                                var isFullScreen = await Electron.WindowManager.BrowserWindows.First().IsFullScreenAsync();
                                Electron.WindowManager.BrowserWindows.First().SetFullScreen(!isFullScreen);

                            }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Show Symbol",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Fold All",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Unfold All",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Type = MenuType.separator
                        },
                        new MenuItem
                        {
                            Label = "Open Developer Tools",
                            Accelerator = "CmdOrCtrl+I",
                            Click = () => Electron.WindowManager.BrowserWindows.First().WebContents.OpenDevTools()
                        },
                    }
                },
                new MenuItem
                {
                    Label = "Encoding",
                    Submenu = new MenuItem[]
                    {
                        new MenuItem
                        {
                            Label = "UTF-7",
                            Click = async () =>
                            {
                                await CommandInvoker.Instance.Execute(new ConvertEncoding(MenuActions.CurrentFilePath, EncodingType.UTF7));
                            }
                        },
                        new MenuItem
                        {
                            Label = "UTF-8",
                            Click = async () =>
                            {
                                await CommandInvoker.Instance.Execute(new ConvertEncoding(MenuActions.CurrentFilePath, EncodingType.UTF8));
                            }
                        },
                        new MenuItem
                        {
                            Label = "UTF-16 LE",
                            Click = async () =>
                            {
                                await CommandInvoker.Instance.Execute(new ConvertEncoding(MenuActions.CurrentFilePath, EncodingType.UTF16LE));
                            }
                        },
                        new MenuItem
                        {
                            Label = "UTF-16 BE",
                            Click = async () =>
                            {
                                await CommandInvoker.Instance.Execute(new ConvertEncoding(MenuActions.CurrentFilePath, EncodingType.UTF16BE));
                            }
                        },
                        new MenuItem
                        {
                            Label = "UTF-32 LE",
                            Click = async () =>
                            {
                                await CommandInvoker.Instance.Execute(new ConvertEncoding(MenuActions.CurrentFilePath, EncodingType.UTF32LE));
                            }
                        },
                        new MenuItem
                        {
                            Label = "UTF-32 BE",
                            Click = async () =>
                            {
                                await CommandInvoker.Instance.Execute(new ConvertEncoding(MenuActions.CurrentFilePath, EncodingType.UTF32BE));
                            }
                        },
                        new MenuItem
                        {
                            Type = MenuType.separator
                        },
                        new MenuItem
                        {
                            Label = "Interpret File as ...",
                            Submenu = new MenuItem[]
                            {
                                new MenuItem
                                {
                                    Type = MenuType.radio,
                                    Checked = encodingCheckedValues[0],
                                    Label = "UTF-7",
                                    Click = async () =>
                                    {
                                        encodingCheckedValues.Clear();
                                        encodingCheckedValues[0] = true;
                                        await CommandInvoker.Instance.Execute(new ChangeEncodingInterpretation(MenuActions.CurrentFilePath, EncodingType.UTF7));
                                    }
                                },
                                new MenuItem
                                {
                                    Type = MenuType.radio,
                                    Checked = encodingCheckedValues[1],
                                    Label = "UTF-8",
                                    Click = async () =>
                                    {
                                        encodingCheckedValues.Clear();
                                        encodingCheckedValues[1] = true;
                                        await CommandInvoker.Instance.Execute(new ChangeEncodingInterpretation(MenuActions.CurrentFilePath, EncodingType.UTF8));
                                    }
                                },
                                new MenuItem
                                {
                                    Type = MenuType.radio,
                                    Checked = encodingCheckedValues[2],
                                    Label = "UTF-16 LE",
                                    Click = async () =>
                                    {
                                        encodingCheckedValues.Clear();
                                        encodingCheckedValues[2] = true;
                                        await CommandInvoker.Instance.Execute(new ChangeEncodingInterpretation(MenuActions.CurrentFilePath, EncodingType.UTF16LE));
                                    }
                                },
                                new MenuItem
                                {
                                    Type = MenuType.radio,
                                    Checked = encodingCheckedValues[3],
                                    Label = "UTF-16 BE",
                                    Click = async () =>
                                    {
                                        encodingCheckedValues.Clear();
                                        encodingCheckedValues[3] = true;
                                        await CommandInvoker.Instance.Execute(new ChangeEncodingInterpretation(MenuActions.CurrentFilePath, EncodingType.UTF16BE));
                                    }
                                },
                                new MenuItem
                                {
                                    Type = MenuType.radio,
                                    Checked = encodingCheckedValues[4],
                                    Label = "UTF-32 LE",
                                    Click = async () =>
                                    {
                                        encodingCheckedValues.Clear();
                                        encodingCheckedValues[4] = true;
                                        await CommandInvoker.Instance.Execute(new ChangeEncodingInterpretation(MenuActions.CurrentFilePath, EncodingType.UTF32LE));
                                    }
                                },
                                new MenuItem
                                {
                                    Type = MenuType.radio,
                                    Checked = encodingCheckedValues[5],
                                    Label = "UTF-32 BE",
                                    Click = async () =>
                                    {
                                        encodingCheckedValues.Clear();
                                        encodingCheckedValues[5] = true;
                                        await CommandInvoker.Instance.Execute(new ChangeEncodingInterpretation(MenuActions.CurrentFilePath, EncodingType.UTF32BE));
                                    }
                                },
                            }
                        },
                    }
                },
                new MenuItem
                {
                    Label = "Settings",
                    Submenu = new MenuItem[]
                    {
                        new MenuItem
                        {
                            Label = "Change Settings",
                            Accelerator="F10",
                            Click = async () =>
                            {
                                var path = $"http://localhost:{BridgeSettings.WebPort}/settings";

                                var settingsWindow = await Electron.WindowManager.CreateWindowAsync(path);
                                settingsWindow.RemoveMenu();
                            }
                        }
                    }
                },
                new MenuItem
                {
                    Label = "Shortcuts",
                    Click = async () =>
                    {
                        var path = $"http://localhost:{BridgeSettings.WebPort}/shortcuts";

                        var settingsWindow = await Electron.WindowManager.CreateWindowAsync(path);
                        settingsWindow.RemoveMenu();
                    }
                }
            };

            Electron.Menu.SetApplicationMenu(indexMenu);
        }
    }
}
