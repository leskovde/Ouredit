using Components;
using Components.Commands;
using Components.Controllers;
using Components.Models;
using ElectronNET.API;
using ElectronNET.API.Entities;
using System;
using System.Linq;
using System.Threading;

namespace OurTextEditor
{
    public static class Menu
    {
        public static async void SetMenu()
        {
            var options = new BrowserWindowOptions()
            {
                Show = false,
            };

            var mainWindow = await Electron.WindowManager.CreateWindowAsync(options);
            mainWindow.OnReadyToShow += () =>
            {
                mainWindow.Show();
                var openFiles = ApplicationState.Instance.FileHandlerInstance.GetOpenFileNames();

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

            var encodingCheckedValues = new bool[6];

            var indexMenu = new MenuItem[]
            {
                new MenuItem
                {
                    Label = "File",
                    Submenu = new MenuItem[]
                    {
                        new MenuItem
                        {
                            Label = "[NI]New",
                            Click = async () => { await MenuActions.NewFileAsync(); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Open...",
                            Click = async () => { await MenuActions.OpenFileAsync(); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Save",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Save as...",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Close",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Close All",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "Exit",
                            Click = () => { Electron.WindowManager.BrowserWindows.First().Close(); }
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
                            Label = "[NI]Cut",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Copy",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Paste",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Delete",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                        },
                        new MenuItem
                        {
                            Label = "[NI]Select All",
                            Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
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
                            Label = "[NI]Auto-Completion",
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
                                encodingCheckedValues.Clear();
                                encodingCheckedValues[0] = true;
                                await CommandInvoker.Instance.Execute(new ConvertEncoding(MenuActions.CurrentFilePath, EncodingType.UTF7));
                            }
                        },
                        new MenuItem
                        {
                            Label = "UTF-8",
                            Click = async () =>
                            {
                                encodingCheckedValues.Clear();
                                encodingCheckedValues[1] = true;
                                await CommandInvoker.Instance.Execute(new ConvertEncoding(MenuActions.CurrentFilePath, EncodingType.UTF8));
                            }
                        },
                        new MenuItem
                        {
                            Label = "UTF-16 LE",
                            Click = async () =>
                            {
                                encodingCheckedValues.Clear();
                                encodingCheckedValues[2] = true;
                                await CommandInvoker.Instance.Execute(new ConvertEncoding(MenuActions.CurrentFilePath, EncodingType.UTF16LE));
                            }
                        },
                        new MenuItem
                        {
                            Label = "UTF-16 BE",
                            Click = async () =>
                            {
                                encodingCheckedValues.Clear();
                                encodingCheckedValues[3] = true;
                                await CommandInvoker.Instance.Execute(new ConvertEncoding(MenuActions.CurrentFilePath, EncodingType.UTF16BE));
                            }
                        },
                        new MenuItem
                        {
                            Label = "UTF-32 LE",
                            Click = async () =>
                            {
                                encodingCheckedValues.Clear();
                                encodingCheckedValues[4] = true;
                                await CommandInvoker.Instance.Execute(new ConvertEncoding(MenuActions.CurrentFilePath, EncodingType.UTF32LE));
                            }
                        },
                        new MenuItem
                        {
                            Label = "UTF-32 BE",
                            Click = async () =>
                            {
                                encodingCheckedValues.Clear();
                                encodingCheckedValues[5] = true;
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
                            Label = "[NI]Change Settings",
                            Click = async () =>
                            {
                                var path = $"http://localhost:{BridgeSettings.WebPort}/settings";

                                var settingsWindowOptions = new BrowserWindowOptions
                                {
                                    SkipTaskbar = true,
                                };

                                var settingsWindow = await Electron.WindowManager.CreateWindowAsync(settingsWindowOptions, path);
                                settingsWindow.RemoveMenu();
                            }
                        }
                    }
                }
            };

            Electron.Menu.SetApplicationMenu(indexMenu);
        }
    }
}
