using System;
using System.Collections.Generic;
using System.Text;
using Buffer = Components.Models.Buffer;

namespace Components.Controllers
{
    /// <summary>
    /// Keeps the current character, word and line counts of a given file.
    /// </summary>
    [Leskovar]
    public class TextCounter
    {
        public int WordCount { get; private set; }
        public int CharacterCount { get; private set; }
        public int LineCount { get; private set; }

        private char _previousChar;

        /// <summary>
        /// Scans the entire file and sets the counts according to its content.
        /// </summary>
        /// <param name="fileBuffer">The buffer of a given file.</param>
        public void CountFileContent(Buffer fileBuffer)
        {
            WordCount = 0;
            CharacterCount = 0;
            LineCount = 1;

            _previousChar = '\0';

            UpdateCountsAdd(fileBuffer.GetBufferContent());
        }

        /// <summary>
        /// Updates current counts upon inserting a character into the file.
        /// </summary>
        /// <param name="inputChar">The character inserted.</param>
        public void UpdateCountsAdd(char inputChar)
        {
            CharacterCount++;

            if (WordCount == 0 && !Char.IsWhiteSpace(_previousChar))
            {
                WordCount++;
            }

            if (Char.IsWhiteSpace(inputChar) && !Char.IsWhiteSpace(_previousChar))
            {
                WordCount++;
            }


            if (inputChar == '\n')
            {
                if (_previousChar != '\r')
                {
                    LineCount++;
                }
            }

            if (inputChar == '\r')
            {
                LineCount++;
            }

            _previousChar = inputChar;
        }

        /// <summary>
        /// Updates current counts upon inserting a string into the file.
        /// </summary>
        /// <param name="inputString">The string of text to be inserted.</param>
        public void UpdateCountsAdd(string inputString)
        {
            foreach (var character in inputString)
            {
                UpdateCountsAdd(character);
            }
        }

        /// <summary>
        /// Updates current counts upon inserting a character into the file.
        /// </summary>
        /// <param name="inputChar">The character inserted.</param>
        public void UpdateCountsRemove(char inputChar)
        {
            CharacterCount--;

            if (Char.IsWhiteSpace(inputChar) && !Char.IsWhiteSpace(_previousChar))
            {
                WordCount--;
            }


            if (inputChar == '\r')
            {
                if (_previousChar != '\n')
                {
                    LineCount--;
                }
            }

            if (inputChar == '\r')
            {
                LineCount--;
            }

            _previousChar = inputChar;
        }

        /// <summary>
        /// Updates current counts upon inserting a string into the file.
        /// </summary>
        /// <param name="inputString">The string of text to be inserted.</param>
        public void UpdateCountsRemove(string inputString)
        {
            foreach (var character in inputString)
            {
                UpdateCountsRemove(character);
            }
        }
    }
}
