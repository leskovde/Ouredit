using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Components.Models;
using Buffer = Components.Models.Buffer;

namespace Components
{
    /// <summary>
    /// An abstraction for a single opened file. Holds data needed for the UI and supports basic IO operations.
    /// </summary>
    [Leskovar]
    internal class File : IDisposable
    {
        public string FileName { get; }
        public string FilePath { get; }
        public long FileSize { get; private set; }

        private static readonly object Mutex = new object();
        private readonly FileStream _fileStream;
        private readonly Encoding _encoding; 

        public File(string filePath)
        {
            FileName = Path.GetFileName(filePath);
            FilePath = filePath;
            FileSize = new FileInfo(filePath).Length;

            _fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _encoding = GetEncoding(filePath);
        }

        /// <summary>
        /// Writes a string to the file at the given offset from the beginning of the file. Any present data is overwritten.
        /// </summary>
        /// <param name="byteOffset">The byte offset relative to the beginning of the file.</param>
        /// <param name="content">The text to be written represented via a string.</param>
        public void WriteToFile(long byteOffset, string content)
        {
            lock (Mutex)
            {
                if (!_fileStream.CanSeek)
                {
                    throw new NotSupportedException();
                }
            }

            var byteArray = _encoding.GetBytes(content);

            lock (Mutex)
            {
                _fileStream.Seek(byteOffset, SeekOrigin.Begin);
                _fileStream.Write(byteArray);
            }
        }

        /// <summary>
        /// Reads continuous text of given length of bytes from a given offset from the file and returns it as a string.
        /// </summary>
        /// <param name="byteOffset">The byte offset relative to the beginning of the file.</param>
        /// <param name="byteCount">The number of bytes that should be read from the file.</param>
        /// <returns></returns>
        public string ReadFromFile(long byteOffset, int byteCount)
        {
            lock (Mutex)
            {
                if (!_fileStream.CanSeek)
                {
                    throw new NotSupportedException();
                }
            }

            var byteArray = new byte[byteCount];

            lock (Mutex)
            {
                _fileStream.Seek(byteOffset, SeekOrigin.Begin);
                _fileStream.Read(byteArray, 0, byteCount);
            }

            return _encoding.GetString(byteArray);
        }

        /// <summary>
        /// Sets the file size to a new value.
        /// </summary>
        /// <param name="newFileSize">File size in bytes.</param>
        public void UpdateFileSize(long newFileSize)
        {
            lock (Mutex)
            {
                FileSize = newFileSize;
            }
        }

        public void Dispose()
        {
            lock (Mutex)
            {
                _fileStream?.Dispose();
            }
        }

        /// <summary>
        /// Looks up the encoding of a given file by analyzing the byte order mark. If none is found, the file is treated as an UTF-8 encoded file.
        /// </summary>
        /// <param name="filePath">The file from which the encoding should be taken.</param>
        /// <returns>A System.Text.Encoding object according to the BOM.</returns>
        private Encoding GetEncoding(string filePath)
        {
            // Read the BOM
            var bom = new byte[4];
            lock (Mutex)
            {
                _fileStream.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // BOM was not recognized, default to UTF-8.
            return Encoding.UTF8;
        }
    }
}
