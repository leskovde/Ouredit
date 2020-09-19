using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Models;
using ElectronNET.API;
using TextEditor.Controllers;

namespace TextEditor
{
    public static class MenuActions
    {
        public static string CurrentFileName = "Mock";

        public static async Task NewFileAsync()
        {
            var task = new Task(() =>
            {
                try
                {
                    ApplicationState.Instance.FileHandlerInstance.NewFile("new-file2.txt");
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, "async-tab-select-cs-caller", "new-file2.txt");
                }
                catch (InvalidOperationException e)
                {
                    Electron.Dialog.ShowErrorBox("File Error", "File already exists.");
                    Console.WriteLine(e.StackTrace);
                }
            });

            task.Start();
            await task;
        }

        public static async Task OpenFileAsync()
        {
            var task = new Task(() =>
            {
                try
                {
                    ApplicationState.Instance.FileHandlerInstance.OpenFile("new-file1.txt");
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, "async-tab-select-cs-caller", "new-file1.txt");
                    
                }
                catch (InvalidOperationException e)
                {
                    Electron.Dialog.ShowErrorBox("File Error", "File is already open or doesn't exists.");
                    Console.WriteLine(e.StackTrace);
                }
            });

            task.Start();
            await task;
        }
    }
}
