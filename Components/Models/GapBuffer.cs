using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Models
{
    /// <summary>
    /// Data structure for storing the contents of a file. Storage is done via lists of chars.
    /// </summary>
    [Leskovar]
    // TODO: Possibly use the IList interface.
    public class GapBuffer
    {
        // TODO: Possibly change int to uint or ulong.
        private List<char> _leftSide;
        private List<char> _rightSide;

        public GapBuffer()
        {
            _leftSide = new List<char>();
            _rightSide = new List<char>();
        }

        /// <summary>
        /// Inserts a single character into a given position.
        /// </summary>
        /// <param name="content">The inserted character.</param>
        /// <param name="offset">The position to which the character will be inserted. Lowest possible is zero.</param>
        public void Insert(char content, int offset)
        {
            MoveGap(offset);
            _leftSide.Add(content);
        }

        /// <summary>
        /// Inserts a string into a position given by a starting index.
        /// </summary>
        /// <param name="content">The inserted string.</param>
        /// <param name="offset">The starting index of the position to which the string will be inserted. Lowest possible is zero.</param>
        public void Insert(string content, int offset)
        {
            MoveGap(offset);
            _leftSide.AddRange(content);
        }

        /// <summary>
        /// Deletes characters inside a given range if that range is correct.
        /// </summary>
        /// <param name="startingOffset">The starting index of the range. Lowest possible is zero.</param>
        /// <param name="endingOffset">The ending index of the range. Highest possible is one less than the size of the buffer.</param>
        public void Delete(int startingOffset, int endingOffset)
        {
            if (startingOffset < 0 || startingOffset > endingOffset)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (endingOffset > GetLength() - 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            // Remove range without moving the gap if possible.
            if (endingOffset < _leftSide.Count)
            {
                _leftSide.RemoveRange(startingOffset, endingOffset - startingOffset + 1);
            }
            else if (startingOffset >= _leftSide.Count)
            {
                _rightSide.RemoveRange(startingOffset - _leftSide.Count, endingOffset - startingOffset + 1);
            }
            // Range goes over the gap, move the gap.
            else
            {
                MoveGap(GetLength());
                _leftSide.RemoveRange(startingOffset, endingOffset - startingOffset + 1);
            }
        }

        /// <summary>
        /// Returns the text stored in a given range if that range is correct.
        /// </summary>
        /// <param name="startingOffset">The starting index of the range. Lowest possible is zero.</param>
        /// <param name="endingOffset">The ending index of the range. Highest possible is one less than the size of the buffer.</param>
        /// <returns>The sequence of characters in the range represented by a string.</returns>
        public string GetText(int startingOffset, int endingOffset)
        {
            // TODO: Benchmark and possibly rewrite this method to make it run faster.

            if (GetLength() == 0)
            {
                return default;
            }

            var continuousBuffer = _leftSide.Concat(_rightSide).ToList();
            var selection = continuousBuffer.GetRange(startingOffset, endingOffset - startingOffset + 1);

            return new string(selection.ToArray());
        }

        /// <summary>
        /// Gets the size of the buffer storage.
        /// </summary>
        /// <returns>The number of characters stored in the internal buffer.</returns>
        public int GetLength()
        {
            return _leftSide.Count + _rightSide.Count;
        }

        /// <summary>
        /// Creates a gap in the internal storage on a given offset if the offset is correct. The gap is used for efficient text insertion.
        /// </summary>
        /// <param name="offset">The index on which the gap should be created. Lowest possible is zero, highest possible is one less than the size of the buffer.</param>
        private void MoveGap(int offset)
        {
            // TODO: Benchmark and possibly rewrite this method to make it run faster.

            if (offset == _leftSide.Count)
            {
                return;
            }

            if (offset < 0 || offset > GetLength())
            {
                throw new ArgumentOutOfRangeException();
            }

            var continuousBuffer = _leftSide.Concat(_rightSide);
            var subLists = continuousBuffer
                .Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index < offset)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToArray();

            if (subLists.Length != 2)
            {
                if (offset == 0)
                {
                    _rightSide = subLists.First();
                    _leftSide = new List<char>();

                    return;
                }

                if (offset == GetLength())
                {
                    _leftSide = subLists.First();
                    _rightSide = new List<char>();
                    return;
                }
                
                throw new NotSupportedException();
            }

            _leftSide = subLists.First();
            _rightSide = subLists.Last();
        }
    }
}
