using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Components.Models;
using ElectronNET.API;

namespace OurTextEditor
{
    public static class MenuActions
    {
        public static string CurrentFilePath { get; private set;  } = "new.txt";

        /// <summary>
        /// Upon changing the CurrentFilePath, razor components need to be re-rendered.
        /// This setter sends a notification message to the service that handles the page refreshing.
        /// </summary>
        /// <param name="filePath">The file path to the new current file.</param>
        public static void SetCurrentFilePath(string filePath)
        {
            CurrentFilePath = filePath;
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
            Console.WriteLine($"#DEBUG: OpenFiles have been changed to: {string.Join(" ",ApplicationState.Instance.FileHandlerInstance.GetOpenFileNames())}. Sending the notification further.");
        }

        /// <summary>
        /// Serves as a worker function of the FileContentChangeService.
        /// </summary>
        public static void FileContentChanged(object sender, FileContentChangeArgs args)
        {
            Console.WriteLine($"#DEBUG: Performing clear and insert actions on the {args.FileBuffer.FileInstance.FileName} file buffer.");
            args.FileBuffer.Clear();
            args.FileBuffer.InsertAtCursor(args.FileContent);
        }

        public static async Task NewFileAsync()
        {
            var task = new Task(() =>
            {
                try
                {
                    ApplicationState.Instance.FileHandlerInstance.NewFile("new-file2.txt");
                    ApplicationState.Instance.FileHandlerInstance.GetFileBuffer("new-file2.txt").FillBufferFromFile();
                    var handler = OpenFilesChanged;
                    handler?.Invoke(ApplicationState.Instance.FileHandlerInstance.GetOpenFileNames(), EventArgs.Empty);
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
                    ApplicationState.Instance.FileHandlerInstance.GetFileBuffer("new-file1.txt").FillBufferFromFile();
                    var handler = OpenFilesChanged;
                    handler?.Invoke(ApplicationState.Instance.FileHandlerInstance.GetOpenFileNames(), EventArgs.Empty);

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
