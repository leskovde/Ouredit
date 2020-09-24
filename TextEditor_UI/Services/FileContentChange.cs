using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Microsoft.Extensions.Configuration;
using OurTextEditor.Component;
using Buffer = Components.Models.Buffer;

namespace OurTextEditor
{
    public delegate void FileContentChangeDelegate(object sender, FileContentChangeArgs args);

    /// <summary>
    /// Hold the FileContentChange arguments, i.e. the updated file content and its destination buffer.
    /// </summary>
    [Leskovar]
    public class FileContentChangeArgs : EventArgs
    {
        public Buffer FileBuffer { get; }
        public string FileContent { get; }

        public FileContentChangeArgs(Buffer fileBuffer, string content)
        {
            FileBuffer = fileBuffer;
            FileContent = content;
        }
    }

    /// <summary>
    /// Provides the contract for the FileContentChange service.
    /// </summary>
    [Leskovar]
    public interface IFileContentChangeBroadcastService
    {
        event FileContentChangeDelegate OnFileContentChanged;
    }

    /// <summary>
    /// Serves as the middle man between the FileContent.FileContentChanged notification event and the buffer that needs to be updated.
    /// </summary>
    [Leskovar]
    public class FileContentChangeBroadcastService : IFileContentChangeBroadcastService
    {
        public event FileContentChangeDelegate OnFileContentChanged;
        private IConfiguration _configuration;

        public FileContentChangeBroadcastService(IConfiguration configuration)
        {
            _configuration = configuration;
            FileContent.FileContentChanged += FileContent_Changed;
        }

        /// <summary>
        /// Redirects the notification event and its data further.
        /// </summary>
        /// <param name="sender">The sender object of the notification event.</param>
        /// <param name="e">The notification event argument, i.e. Buffer object and its new content.</param>
        private void FileContent_Changed(object sender, FileContentChangeArgs e)
        {
            Console.WriteLine("#DEBUG: The notification has been received by the FileContentChangedBroadcastService.");
            OnFileContentChanged?.Invoke(this, e);
        }
    }
}
