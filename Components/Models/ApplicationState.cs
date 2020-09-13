using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Components.Models
{
    /// <summary>
    /// Stores the current state of the text editor.
    /// </summary>
    [Leskovar]
    internal class ApplicationState
    {
        private readonly List<Buffer> _fileBuffers;
        public FileHandler FileHandlerInstance { get; }

        public ApplicationState(List<string> filePaths)
        {
            _fileBuffers = new List<Buffer>();
            FileHandlerInstance = new FileHandler(this);

            foreach (var filePath in filePaths)
            {
                FileHandlerInstance.OpenFile(filePath);
            }
        }

        /// <summary>
        /// Performs basic file handling tasks such as opening, saving and closing a given file while updating the state of the application.
        /// </summary>
        [Leskovar]
        internal class FileHandler
        {
            private readonly ApplicationState _applicationState;

            public FileHandler(ApplicationState applicationState)
            {
                _applicationState = applicationState;
            }

            /// <summary>
            /// Attempts to open a given file and adds it to the application state.
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
                if (new FileInfo(filePath).Length > 20 * 1024 * 1024)
                {
                    type = BufferType.Lazy;
                }

                _applicationState._fileBuffers.Add(BufferFactoryGetter.GetBuffer(type, new File(filePath)));
            }

            /// <summary>
            /// Saves the user changes to the file.
            /// </summary>
            /// <param name="filePath"></param>
            public void SaveFile(string filePath)
            {
                var fileBuffer = _applicationState._fileBuffers.First(x => x.FileInstance.FilePath == filePath);
                
                // TODO: Add the 'save' logic.
            }

            /// <summary>
            /// Closes an open file and removes it from the application status.
            /// </summary>
            /// <param name="filePath"></param>
            public void CloseFile(string filePath)
            {
                if (!_applicationState._fileBuffers.Exists(x => x.FileInstance.FilePath != filePath))
                {
                    throw new InvalidOperationException();
                }

                var fileBuffer = _applicationState._fileBuffers.First(x => x.FileInstance.FilePath == filePath);
                
                fileBuffer.FileInstance.Dispose();
                _applicationState._fileBuffers.Remove(fileBuffer);
            }
        }
    }
}
