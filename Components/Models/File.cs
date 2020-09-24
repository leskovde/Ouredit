using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Components.Models
{
    /// <summary>
    /// An abstraction for a single opened file. Holds data needed for the UI and supports basic IO operations.
    /// </summary>
    [Leskovar]
    public class File : IDisposable
    {
        public string FileName { get; }
        public string FilePath { get; }
        public long FileSize { get; private set; }
        public LineEndings EndOfLineCharacter { get; private set; }
        
        /// <summary>
        /// Input encoding represents the encoding in which the file on the disc is interpreted when being read.
        /// </summary>
        public Encoding InputEncoding { get; private set; }
        
        /// <summary>
        /// Output encoding represents the encoding in which the file will be dumped to the disc upon saving it.
        /// </summary>
        public Encoding OutputEncoding { get; private set; }

        private static readonly object Mutex = new object();
        private readonly FileStream _fileStream;

        public File(string filePath)
        {
            _fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            FileName = Path.GetFileName(filePath);
            FilePath = filePath;

            var fileInfo = new FileInfo(filePath);
            
            if (fileInfo.Exists && fileInfo.Length > 0)
            {
                FileSize = fileInfo.Length;
                InputEncoding = GetEncodingFromMetadata(filePath);
            }
            else
            {
                FileSize = 0;
                InputEncoding = Encoding.UTF8;
            }

            OutputEncoding = InputEncoding;

            Console.WriteLine($"#DEBUG: Created a new file: Name: {FileName}, Path: {FilePath}, Size: {FileSize}, Encoding: {InputEncoding}.");
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

            if (content == null)
            {
                content = string.Empty;
            }

            var byteArray = InputEncoding.GetBytes(content);

            if (byteOffset == 0 && InputEncoding is UTF7Encoding)
            {
                // UTF-7 does not have a BOM by default in C#. Add the BOM.
                byteArray = new byte[] { 0x2b, 0x2f, 0x76, 0x38 }.Concat(byteArray).ToArray();
            }

            lock (Mutex)
            {
                _fileStream.Seek(byteOffset, SeekOrigin.Begin);
                _fileStream.Write(byteArray);
                _fileStream.Flush();
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

            if (byteOffset == 0 && InputEncoding is UTF7Encoding)
            {
                // UTF-7 does not expect a BOM by default in C#. Skip the BOM.
                byteOffset = 4;
            }

            lock (Mutex)
            {
                _fileStream.Seek(byteOffset, SeekOrigin.Begin);
                _fileStream.Read(byteArray, 0, byteCount);
            }

            return InputEncoding.GetString(byteArray);
        }

        /// <summary>
        /// Sets the file size to a new value.
        /// </summary>
        /// <param name="newFileSize">File size in bytes.</param>
        public void SetFileSize(long newFileSize)
        {
            lock (Mutex)
            {
                FileSize = newFileSize;
            }
        }

        /// <summary>
        /// Substitutes the current line ending character for a valid given one.
        /// </summary>
        /// <param name="lineEndings">The type of line ending to which the file will be changed.</param>
        public void SetLineEndings(LineEndings lineEndings)
        {
            var previousEndOfLineCharacter = EndOfLineCharacter;
            EndOfLineCharacter = lineEndings;

            // TODO: Implement the substitution logic via search (or keep a table of all EOL positions).
        }

        /// <summary>
        /// Sets the encoding in which the file will be interpreted upon reading it from the disc.
        /// To see a change in the representation, the content needs to be read again.
        /// </summary>
        /// <param name="encoding">The System.Text.Encoding object representing the given input encoding.</param>
        public void SetInputEncoding(Encoding encoding)
        {
            InputEncoding = encoding;
        }

        /// <summary>
        /// Sets the encoding in which the file will be interpreted upon reading it from the disc.
        /// To see a change in the representation, the content needs to be read again.
        /// </summary>
        /// <param name="type">The given type of input encoding.</param>
        public void SetInputEncoding(EncodingType type)
        {
            InputEncoding = Encodings.GetEncoding(type);
        }

        /// <summary>
        /// Sets the encoding in which the file will be saved upon dumping it to the disc.
        /// </summary>
        /// <param name="encoding">The System.Text.Encoding object representing the given output encoding.</param>
        public void SetOutputEncoding(Encoding encoding)
        {
            OutputEncoding = encoding;
        }

        /// <summary>
        /// Sets the encoding in which the file will be saved upon dumping it to the disc.
        /// </summary>
        /// <param name="type">The given type of output encoding.</param>
        public void SetOutputEncoding(EncodingType type)
        {
            OutputEncoding = Encodings.GetEncoding(type);
        }

        /// <summary>
        /// Clears the current file, i.e deletes its content.
        /// </summary>
        public void Clear()
        {
            Console.WriteLine($"#DEBUG: Clearing the file {FilePath}.");

            lock (Mutex)
            {
                _fileStream.SetLength(0);
            }
            
            if (InputEncoding is UTF7Encoding)
            {
                // UTF-7 does not have a BOM by default in C#. Add the BOM.
                var byteArray = new byte[] { 0x2b, 0x2f, 0x76, 0x38 };
                lock (Mutex)
                {
                    _fileStream.Seek(0, SeekOrigin.Begin);
                    _fileStream.Write(byteArray);
                }
            }

            lock (Mutex)
            {
                _fileStream.Flush();
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
        private Encoding GetEncodingFromMetadata(string filePath)
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
