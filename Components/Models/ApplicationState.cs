using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Components.Controllers;

namespace Components.Models
{
    /// <summary>
    /// Stores the current state of the text editor.
    /// </summary>
    [Leskovar]
    public class ApplicationState
    {
        private static ApplicationState _instance;
        private static readonly object Mutex = new object();
        private readonly List<Buffer> _fileBuffers;
        public FileHandler FileHandlerInstance { get; }
        public Settings.SettingsHandler SettingsHandlerInstance { get; }

        private ApplicationState()
        {
            _fileBuffers = new List<Buffer>();
            FileHandlerInstance = new FileHandler(this);
            SettingsHandlerInstance = new Settings.SettingsHandler();

            if (!System.IO.File.Exists(Settings.SettingsHandler.FileHistoryPath)) return;

            var jsonString =
                System.IO.File.ReadAllText(Settings.SettingsHandler.FileHistoryPath);

            var filePaths = JsonSerializer.Deserialize<List<string>>(jsonString);

            foreach (var filePath in filePaths)
            {
                FileHandlerInstance.OpenFile(filePath);
                FileHandlerInstance.GetFileBuffer(filePath).FillBufferFromFile();
            }
        }

        /// <summary>
        /// Makes sure only one instance is ever created.
        /// </summary>
        /// <returns>Reference to the singleton instance.</returns>
        public static ApplicationState Instance
        {
            get
            {
                lock (Mutex)
                {
                    return _instance ??= new ApplicationState();
                }
            }
        }

        /// <summary>
        /// Performs basic file handling tasks such as opening, saving and closing a given file while updating the state of the application.
        /// </summary>
        [Leskovar]
        public class FileHandler
        {
            private readonly ApplicationState _applicationState;

            internal FileHandler(ApplicationState applicationState)
            {
                _applicationState = applicationState;
            }

            /// <summary>
            /// Offers the same functionality as Open file but with a file existence check.
            /// </summary>
            /// <param name="filePath">The name of the file to be created.</param>
            public void NewFile(string filePath)
            {
                if (new FileInfo(filePath).Exists)
                {
                    throw new InvalidOperationException();
                }

                OpenFile(filePath);
            }

            /// <summary>
            /// Attempts to open a given file and adds it to the application state.
            /// Side effect: In case the file does not exist, the method creates a new file.
            /// </summary>
            /// <param name="filePath">The file to be opened.</param>
            public void OpenFile(string filePath)
            {
                if (_applicationState._fileBuffers.Any(x => x.FileInstance.FilePath == filePath))
                {
                    throw new InvalidOperationException();
                }

                var type = BufferType.Immediate;

                // Files larger than 20 MB are automatically set to the lazy buffering mode.
                var fileInfo = new FileInfo(filePath);

                if (fileInfo.Exists && fileInfo.Length > 20 * 1024 * 1024)
                {
                    type = BufferType.Lazy;
                }

                _applicationState._fileBuffers.Add(BufferInstantiator.GetBuffer(type, new File(filePath)));
            }

            /// <summary>
            /// Saves the user changes to the file.
            /// </summary>
            /// <param name="currentFilePath">The file path of the file whose content will be saved.</param>
            /// <param name="destinationFilePath">The destination to which the file fill be saved.</param>
            public void SaveFile(string currentFilePath, string destinationFilePath)
            {
                var currentFileBuffer = GetFileBuffer(currentFilePath);

                if (currentFilePath == destinationFilePath)
                {
                    currentFileBuffer.DumpBufferToCurrentFile();
                }
                else
                {
                    var destinationFileBuffer = _applicationState._fileBuffers.FirstOrDefault(x => x.FileInstance.FilePath == destinationFilePath);

                    if (destinationFileBuffer == default)
                    {
                        OpenFile(destinationFilePath);
                        destinationFileBuffer = GetFileBuffer(destinationFilePath);
                    }

                    currentFileBuffer.DumpBufferToFile(destinationFileBuffer.FileInstance);
                }
            }

            /// <summary>
            /// Closes an open file and removes it from the application status.
            /// </summary>
            /// <param name="filePath"></param>
            public void CloseFile(string filePath)
            {
                var fileBuffer = _applicationState._fileBuffers.FirstOrDefault(x => x.FileInstance.FilePath == filePath);

                if (fileBuffer == default)
                {
                    throw new InvalidOperationException();
                }
                
                fileBuffer.FileInstance.Dispose();
                _applicationState._fileBuffers.Remove(fileBuffer);
            }

            /// <summary>
            /// Searches through all open files to get the underlying data structure handler for a given file.
            /// </summary>
            /// <param name="filePath">The file path to the searched file.</param>
            /// <returns>The Buffer object that contains the text of a given file.</returns>
            public Buffer GetFileBuffer(string filePath)
            {
                var fileBuffer = _applicationState._fileBuffers.FirstOrDefault(x => x.FileInstance.FilePath == filePath);

                if (fileBuffer == default)
                {
                    throw new InvalidOperationException();
                }

                return fileBuffer;
            }

            /// <summary>
            /// Creates a collection of all opened files. The collection contains file names (i. e. not full paths, just names with extensions).
            /// </summary>
            /// <returns></returns>
            public List<string> GetOpenFilePaths()
            {
                return _applicationState._fileBuffers.Select(x => x.FileInstance.FilePath).ToList();
            }
        }
    }
}
