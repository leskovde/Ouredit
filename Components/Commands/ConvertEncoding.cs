using System;
using System.Collections.Generic;
using System.Text;
using Components.Controllers;
using Components.Models;
using Buffer = Components.Models.Buffer;

namespace Components.Commands
{
    /// <summary>
    /// The command wrapper for changing the current output encoding.
    /// The Execute operation changes the file encoding property in a given file buffer to a given encoding type.
    /// The Undo operation changes the encoding property in said file to the value it had when the command was created.
    /// </summary>
    [Leskovar]
    public class ConvertEncoding : ICommand
    {
        public string Name { get; set; }
        private readonly Buffer _buffer;
        private readonly EncodingType _newEncoding;
        private readonly Encoding _previousEncoding;

        /// <summary>
        /// Looks up the buffer for a given file and saves the its current encoding for the undo operation.
        /// </summary>
        /// <param name="filePath">The path to the open file whose encoding should change.</param>
        /// <param name="newEncoding">The encoding to which the file should change.</param>
        public ConvertEncoding(string filePath, EncodingType newEncoding)
        {
            Name = "Convert Encoding to " + newEncoding;

            _buffer = ApplicationState.Instance.FileHandlerInstance.GetFileBuffer(filePath);
            _newEncoding = newEncoding;
            _previousEncoding = _buffer.FileInstance.OutputEncoding;
        }

        public void Execute()
        {
            _buffer.FileInstance.SetOutputEncoding(_newEncoding);
        }

        public void Undo()
        {
            _buffer.FileInstance.SetOutputEncoding(_previousEncoding);
        }
    }
}
