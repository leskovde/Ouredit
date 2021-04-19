using Components.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Models
{
    /// <summary>
    /// A wrapper object for the text keeping data structure and an interface for operating with its content.
    /// </summary>
    [Leskovar]
    public abstract class Buffer : IDisposable
    {
        public File FileInstance { get; }
        public TextCounter Counter { get; protected set; }
        protected GapBuffer Storage;
        protected Dictionary<string, int> WordFrequencies;
        protected static object Mutex = new object();
        
        // Position in file.
        protected ulong LinePosition;
        protected ulong ByteOffset;

        // Position in buffer.
        protected int BufferPosition;

        protected Buffer(File file)
        {
            FileInstance = file;
            Storage = new GapBuffer();
            WordFrequencies = new Dictionary<string, int>();

            LinePosition = 0;
            ByteOffset = 0;

            BufferPosition = 0;
        }

        public abstract void UpdateCursorPosition(int numberOfCharactersFromStart);
        public abstract (int, int) ParseCursorPosition();
        public abstract void InsertAtCursor(char content);
        public abstract void InsertAtCursor(string content);
        public abstract void DeleteAtCursorLeft(int numberOfCharacters);
        public abstract void DeleteAtCursorRight(int numberOfCharacters);
        public abstract void FillBufferFromFile();
        public abstract void DumpBufferToCurrentFile();
        public abstract void DumpBufferToFile(File file);
        public abstract string GetBufferContent();
        public abstract List<string> GetMostFrequentWords();
        public abstract void Clear();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                FileInstance?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
