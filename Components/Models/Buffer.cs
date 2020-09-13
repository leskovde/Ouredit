using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Models
{
    [Leskovar]
    enum BufferType
    {
        Immediate,
        Lazy
    }

    [Leskovar]
    internal abstract class Buffer
    {
        public File FileInstance { get; }
        protected GapBuffer _storage;

        // Position in file.
        protected ulong _linePosition;
        protected ulong _byteOffset;

        // Position in buffer.
        protected int _bufferPosition;

        protected Buffer(File file)
        {
            FileInstance = file;
            _storage = new GapBuffer();
            
            _linePosition = 0;
            _byteOffset = 0;

            _bufferPosition = 0;
        }

        public abstract void UpdateCursorPosition();
        public abstract void InsertAtCursor(char content);
        public abstract void InsertAtCursor(string content);
        public abstract void DeleteAtCursorLeft(int numberOfCharacters);
        public abstract void DeleteAtCursorRight(int numberOfCharacters);
        public abstract void FillBufferFromFile();
        public abstract void DumpBufferToCurrentFile();
        public abstract void DumpBufferToFile(File file);
    }
}
