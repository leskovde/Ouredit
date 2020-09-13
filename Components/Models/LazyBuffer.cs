using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Models
{
    // TODO: Implement the lazy evaluation.
    [Leskovar]
    internal class LazyBuffer : Buffer
    {
        public LazyBuffer(File file) : base(file) { }

        public override void UpdateCursorPosition()
        {
            throw new NotImplementedException();
        }

        public override void InsertAtCursor(char content)
        {
            throw new NotImplementedException();
        }

        public override void InsertAtCursor(string content)
        {
            throw new NotImplementedException();
        }

        public override void DeleteAtCursorLeft(int numberOfCharacters)
        {
            throw new NotImplementedException();
        }

        public override void DeleteAtCursorRight(int numberOfCharacters)
        {
            throw new NotImplementedException();
        }

        public override void FillBufferFromFile()
        {
            throw new NotImplementedException();
        }

        public override void DumpBufferToCurrentFile()
        {
            throw new NotImplementedException();
        }

        public override void DumpBufferToFile(File file)
        {
            throw new NotImplementedException();
        }
    }
}
