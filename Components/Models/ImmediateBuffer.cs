using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Models
{
    /// <summary>
    /// A wrapper object for the text keeping data structure. Keeps the entire text file in the memory.
    /// </summary>
    [Leskovar]
    internal class ImmediateBuffer : Buffer
    {
        //private const int _blockSize = 4096 * 1024;

        public ImmediateBuffer(File file) : base(file) { }

        // TODO: Create an abstraction for the cursor OR get position info in any other way.
        public override void UpdateCursorPosition()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a single character at the cursors position and moves the cursor.
        /// </summary>
        /// <param name="content">The character to be inserted.</param>
        public override void InsertAtCursor(char content)
        {
            _storage.Insert(content, _bufferPosition);
            _bufferPosition++;
        }

        /// <summary>
        /// Inserts a string at the cursors position and moves the cursor.
        /// </summary>
        /// <param name="content">The string to be inserted.</param>
        public override void InsertAtCursor(string content)
        {
            _storage.Insert(content, _bufferPosition);
            _bufferPosition += content.Length;
        }

        /// <summary>
        /// Deletes a given number of characters to the left of the cursor (backspace).
        /// </summary>
        /// <param name="numberOfCharacters">The number of characters to be removed.</param>
        public override void DeleteAtCursorLeft(int numberOfCharacters)
        {
            _storage.Delete(_bufferPosition - numberOfCharacters, _bufferPosition);
            _bufferPosition -= numberOfCharacters;
        }

        /// <summary>
        /// Deletes a given number of characters to the right of the cursor (delete).
        /// </summary>
        /// <param name="numberOfCharacters">The number of characters to be removed.</param>
        public override void DeleteAtCursorRight(int numberOfCharacters)
        {
            _storage.Delete(_bufferPosition, _bufferPosition + numberOfCharacters);
        }

        /// <summary>
        /// Loads the content of the file to the buffer.
        /// </summary>
        public override void FillBufferFromFile()
        {
            if (_storage.GetLength() > 0)
            {
                _storage.Delete(0, _storage.GetLength() - 1);
            }

            /*
            for (var i = 0; i <= _file.FileSize / blockSize; i++)
            {
                var fileContent = _file.ReadFromFile(i * blockSize, blockSize); 
                _storage.Insert(fileContent, i * blockSize);
            }
            */

            var fileContent = FileInstance.ReadFromFile(0, (int)FileInstance.FileSize);
            _storage.Insert(fileContent, 0);

        }

        /// <summary>
        /// Saves the content of the buffer to the current file.
        /// </summary>
        public override void DumpBufferToCurrentFile()
        {
            var bufferContent = _storage.GetText(0, _storage.GetLength() - 1);
            FileInstance.WriteToFile(0, bufferContent);
        }

        /// <summary>
        /// Saves the content of the buffer to a given file.
        /// </summary>
        public override void DumpBufferToFile(File file)
        {
            var bufferContent = _storage.GetText(0, _storage.GetLength() - 1);
            file.WriteToFile(0, bufferContent);
        }
    }
}
