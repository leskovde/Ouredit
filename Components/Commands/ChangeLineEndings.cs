using System;
using System.Collections.Generic;
using System.Text;
using Components.Models;
using Buffer = Components.Models.Buffer;

namespace Components.Commands
{
    /// <summary>
    /// The command wrapper for changing the current line ending character.
    /// The Execute operation changes the EOL property in a given file buffer to a given EOL style.
    /// The Undo operation changes the EOL property in said file to the value it had when the command was created.
    /// </summary>
    [Leskovar]
    public class ChangeLineEndings : ICommand
    {
        public string Name { get; set; }
        private readonly Buffer _buffer;
        private readonly LineEndings _newLineEndingCharacter;
        private readonly LineEndings _previousLineEndingsCharacter;

        /// <summary>
        /// Looks up the buffer for a given file and saves the its current line ending character for the undo operation.
        /// </summary>
        /// <param name="filePath">The path to the open file whose line endings should change.</param>
        /// <param name="newLineEnding">The EOL style to which the file should change.</param>
        public ChangeLineEndings(string filePath, LineEndings newLineEnding)
        {
            Name = "Change EOLs to " + newLineEnding;

            _buffer = ApplicationState.Instance.FileHandlerInstance.GetFileBuffer(filePath);
            _newLineEndingCharacter = newLineEnding;
            _previousLineEndingsCharacter = _buffer.FileInstance.EndOfLineCharacter;
        }

        public void Execute()
        {
            _buffer.FileInstance.SetLineEndings(_newLineEndingCharacter);
        }

        public void Undo()
        {
            _buffer.FileInstance.SetLineEndings(_previousLineEndingsCharacter);
        }
    }
}
