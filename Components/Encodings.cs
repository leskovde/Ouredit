using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Components
{
    /// <summary>
    /// Converts alphanumerical encoding names to System.Text.Encoding objects (if the name is valid).
    /// </summary>
    [Leskovar]
    public static class Encodings
    {
        private static readonly Dictionary<EncodingType, Encoding> _encodings = new Dictionary<EncodingType, Encoding>
        {
            {EncodingType.UTF7, Encoding.UTF7},
            {EncodingType.UTF8, Encoding.UTF8},
            {EncodingType.UTF16LE, Encoding.Unicode},
            {EncodingType.UTF16BE, Encoding.BigEndianUnicode},
            {EncodingType.UTF32LE, Encoding.UTF32},
            {EncodingType.UTF32BE, new UTF32Encoding(true, true)},
        };

        /// <summary>
        /// Searches the registered encoding for the given type.
        /// </summary>
        /// <param name="type">The name of the type of a given encoding.</param>
        /// <returns>A System.Text.Encoding representing the given encoding.</returns>
        public static Encoding GetEncoding(EncodingType type)
        {
            if (!_encodings.ContainsKey(type))
            {
                throw new InvalidOperationException();
            }

            return _encodings[type];
        }
    }
}
