using System;
using System.Collections.Generic;
using System.Text;
using Components.Controllers;

namespace Components.Models
{
    /// <summary>
    /// A wrapper object for the text keeping data structure. Keeps the entire text file in the memory.
    /// </summary>
    [Leskovar]
    public class ImmediateBuffer : Buffer
    {
        //private const int _blockSize = 4096 * 1024;

        public ImmediateBuffer(File file) : base(file)
        {
            Counter = new TextCounter();
        }

        // TODO: Create an abstraction for the cursor OR get position info in any other way.
        public override void UpdateCursorPosition(int numberOfCharactersFromStart)
        {
            // MOCK for test purposes.
            BufferPosition = numberOfCharactersFromStart;
        }

        /// <summary>
        /// Inserts a single character at the cursors position and moves the cursor.
        /// </summary>
        /// <param name="content">The character to be inserted.</param>
        public override void InsertAtCursor(char content)
        {
            lock (Mutex)
            {
                Storage.Insert(content, BufferPosition);
                Counter.UpdateCountsAdd(content);
                BufferPosition++;
            }
        }

        /// <summary>
        /// Inserts a string at the cursors position and moves the cursor.
        /// </summary>
        /// <param name="content">The string to be inserted.</param>
        public override void InsertAtCursor(string content)
        {
            lock (Mutex)
            {
                Storage.Insert(content, BufferPosition);
                Counter.UpdateCountsAdd(content);
                BufferPosition += content.Length;
            }
        }

        /// <summary>
        /// Deletes a given number of characters to the left of the cursor (backspace).
        /// </summary>
        /// <param name="numberOfCharacters">The number of characters to be removed.</param>
        public override void DeleteAtCursorLeft(int numberOfCharacters)
        {
            lock (Mutex)
            {
                var content = Storage.GetText(BufferPosition - numberOfCharacters, BufferPosition);
                Storage.Delete(BufferPosition - numberOfCharacters, BufferPosition);
                Counter.UpdateCountsRemove(content);
                BufferPosition -= numberOfCharacters;
            }
        }

        /// <summary>
        /// Deletes a given number of characters to the right of the cursor (delete).
        /// </summary>
        /// <param name="numberOfCharacters">The number of characters to be removed.</param>
        public override void DeleteAtCursorRight(int numberOfCharacters)
        {
            lock (Mutex)
            {
                var content = Storage.GetText(BufferPosition, BufferPosition + numberOfCharacters);
                Storage.Delete(BufferPosition, BufferPosition + numberOfCharacters);
                Counter.UpdateCountsRemove(content);
            }
        }

        /// <summary>
        /// Loads the content of the file to the buffer.
        /// </summary>
        public override void FillBufferFromFile()
        {
            lock (Mutex)
            {
                if (Storage.GetLength() > 0)
                {
                    Storage.Delete(0, Storage.GetLength() - 1);
                }

                /*
                for (var i = 0; i <= _file.FileSize / blockSize; i++)
                {
                    var fileContent = _file.ReadFromFile(i * blockSize, blockSize); 
                    _storage.Insert(fileContent, i * blockSize);
                }
                */

                var fileContent = FileInstance.ReadFromFile(0, (int)FileInstance.FileSize);
                Storage.Insert(fileContent, 0);
                Counter.CountFileContent(this);
            }
        }

        /// <summary>
        /// Saves the content of the buffer to the current file.
        /// </summary>
        public override void DumpBufferToCurrentFile()
        {
            string bufferContent;

            lock (Mutex)
            { 
                bufferContent = Storage.GetText(0, Storage.GetLength() - 1);
            }

            FileInstance.WriteToFile(0, bufferContent);
        }

        /// <summary>
        /// Saves the content of the buffer to a given file.
        /// </summary>
        public override void DumpBufferToFile(File file)
        {
            string bufferContent;

            lock (Mutex)
            {
                bufferContent = Storage.GetText(0, Storage.GetLength() - 1);
            }

            file.WriteToFile(0, bufferContent);
        }

        /// <summary>
        /// Gets the entire text stored in the buffer.
        /// </summary>
        /// <returns>A string representation of the text stored in the buffer.</returns>
        public override string GetBufferContent()
        {
            string content;

            lock (Mutex)
            {
                content = Storage.GetText(0, Storage.GetLength() - 1);
            }

            return content;
        }

        public override void Clear()
        {
            lock (Mutex)
            {
                if (Storage.GetLength() > 0)
                {
                    Storage.Delete(0, Storage.GetLength() - 1);
                }
                BufferPosition = 0;
            }
        }
    }
}
