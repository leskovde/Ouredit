using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Components.Controllers;
using Components.Models;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace OurTextEditor
{
    public static class MenuActions
    {
        public static string CurrentFilePath { get; private set; }

        /// <summary>
        /// Upon changing the CurrentFilePath, razor components need to be re-rendered.
        /// This setter sends a notification message to the service that handles the page refreshing.
        /// </summary>
        /// <param name="filePath">The file path to the new current file.</param>
        public static void SetCurrentFilePath(string filePath)
        {
            CurrentFilePath = filePath;
            AutoSaver.Instance.Update(filePath);
            var handler = CurrentFileChanged;
            handler?.Invoke(CurrentFilePath, EventArgs.Empty);
        }

        public static event EventHandler CurrentFileChanged = CurrentFileChangeNotify;
        public static event EventHandler OpenFilesChanged = OpenFilesChangeNotify;

        /// <summary>
        /// Serves as a notification message for the CurrentFileChangeService.
        /// </summary>
        public static void CurrentFileChangeNotify(object sender, EventArgs e)
        {
            Console.WriteLine($"#DEBUG: The CurrentFile has been changed to: {CurrentFilePath}. Sending the notification further.");
        }

        /// <summary>
        /// Serves as a notification message for the OpenFilesChangeService.
        /// </summary>
        public static void OpenFilesChangeNotify(object sender, EventArgs e)
        {
            Console.WriteLine($"#DEBUG: OpenFiles have been changed to: {string.Join(" ",ApplicationState.Instance.FileHandlerInstance.GetOpenFilePaths())}. Sending the notification further.");
        }

        /// <summary>
        /// Serves as a worker function of the FileContentChangeService.
        /// </summary>
        public static void FileContentChanged(object sender, FileContentChangeArgs args)
        {
            Console.WriteLine($"#DEBUG: Performing clear and insert actions on the {args.FileBuffer.FileInstance.FileName} file buffer.");
            args.FileBuffer.Clear();
            args.FileBuffer.InsertAtCursor(args.FileContent);
            AutoSaver.Instance.Trigger();
        }

        /// <summary>
        /// Serves as a worker function of the FileContentChangeService.
        /// </summary>
        public static void CursorPositionChanged(object sender, CursorPositionChangeArgs args)
        {
            Console.WriteLine($"#DEBUG: Performing update cursor position action to {args.CursorPosition}.");
            var buffer = ApplicationState.Instance.FileHandlerInstance.GetFileBuffer(CurrentFilePath);
            buffer.UpdateCursorPosition(args.CursorPosition);
        }

        public static async Task NewFileAsync()
        {
            var mainWindow = Electron.WindowManager.BrowserWindows.First();

            var options = new OpenDialogOptions
            {
                Title = "New File",
                Properties = new OpenDialogProperty[] {
                    OpenDialogProperty.openFile,
                    OpenDialogProperty.promptToCreate,
                }
            };

            var files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);
            var filePath = files.FirstOrDefault();

            if (filePath == default)
            {
                return;
            }

            if (ApplicationState.Instance.FileHandlerInstance.GetOpenFilePaths().Contains(filePath))
            {
                Electron.Dialog.ShowErrorBox("File Error", "File already exists.");
                return;
            }

            var task = new Task(() =>
            {
                try
                {
                    ApplicationState.Instance.FileHandlerInstance.NewFile(filePath);
                    ApplicationState.Instance.FileHandlerInstance.GetFileBuffer(filePath).FillBufferFromFile();
                    var handler = OpenFilesChanged;
                    handler?.Invoke(ApplicationState.Instance.FileHandlerInstance.GetOpenFilePaths(), EventArgs.Empty);
                }
                catch (Exception e)
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
            var mainWindow = Electron.WindowManager.BrowserWindows.First();

            var options = new OpenDialogOptions
            {
                Title = "Open File",
                Properties = new OpenDialogProperty[] {
                    OpenDialogProperty.openFile
                }
            };

            var files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);
            var filePath = files.FirstOrDefault();

            if (filePath == default)
            {
                return;
            }

            if (ApplicationState.Instance.FileHandlerInstance.GetOpenFilePaths().Contains(filePath))
            {
                Electron.Dialog.ShowErrorBox("File Error", "File is already open or doesn't exists.");
                return;
            }

            var task = new Task(() =>
            {
                try
                {
                    ApplicationState.Instance.FileHandlerInstance.OpenFile(filePath);
                    ApplicationState.Instance.FileHandlerInstance.GetFileBuffer(filePath).FillBufferFromFile();
                    var handler = OpenFilesChanged;
                    handler?.Invoke(ApplicationState.Instance.FileHandlerInstance.GetOpenFilePaths(), EventArgs.Empty);

                }
                catch (Exception e)
                {
                    Electron.Dialog.ShowErrorBox("File Error", "File is already open or doesn't exists.");
                    Console.WriteLine(e.StackTrace);
                }
            });

            task.Start();
            await task;
        }

        public static async Task SaveFileAsync()
        {
            var task = new Task(() =>
            {
                try
                {
                    ApplicationState.Instance.FileHandlerInstance.SaveFile(CurrentFilePath, CurrentFilePath);
                }
                catch (InvalidOperationException e)
                {
                    Electron.Dialog.ShowErrorBox("File Error", "File couldn't be saved.");
                    Console.WriteLine(e.StackTrace);
                }
            });

            task.Start();
            await task;
        }

        public static async Task SaveFileAsAsync()
        {
            var mainWindow = Electron.WindowManager.BrowserWindows.First();

            var options = new SaveDialogOptions
            {
                Title = "Save As..."
            };

            var filePath = await Electron.Dialog.ShowSaveDialogAsync(mainWindow, options);

            var task = new Task(() =>
            {
                try
                {
                    ApplicationState.Instance.FileHandlerInstance.SaveFile(CurrentFilePath, filePath);
                }
                catch (InvalidOperationException e)
                {
                    Electron.Dialog.ShowErrorBox("File Error", "File couldn't be saved.");
                    Console.WriteLine(e.StackTrace);
                }
            });

            task.Start();
            await task;
        }

        public static async Task CloseAsync()
        {
            var task = new Task(() =>
            {
                try
                {
                    var openFiles = ApplicationState.Instance.FileHandlerInstance.GetOpenFilePaths();
                    if (openFiles.Count <= 1)
                    {
                        Electron.Dialog.ShowErrorBox("Close Error", "Last File Tab cannot be closed.");
                        return;
                    }

                    ApplicationState.Instance.FileHandlerInstance.CloseFile(CurrentFilePath);
                    CurrentFilePath = ApplicationState.Instance.FileHandlerInstance.GetOpenFilePaths().First();

                    var currentFileHandler = CurrentFileChanged;
                    currentFileHandler?.Invoke(openFiles, EventArgs.Empty);

                    var openFilesHandler = OpenFilesChanged;
                    openFilesHandler?.Invoke(openFiles, EventArgs.Empty);
                }
                catch (InvalidOperationException e)
                {
                    Electron.Dialog.ShowErrorBox("File Error", "File couldn't be closed.");
                    Console.WriteLine(e.StackTrace);
                }
            });

            task.Start();
            await task;
        }
    }
}
