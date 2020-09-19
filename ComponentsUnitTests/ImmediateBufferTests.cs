using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using Components;
using Components.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using File = Components.Models.File;

namespace ComponentsUnitTests
{
    [Leskovar]
    [TestClass]
    public class ImmediateBufferTests
    {
        private const string _fileName = "_TempFile.txt";

        [TestMethod]
        public void ReadUTF7FileTest()
        {
            // Arrange.
            var fileName = "UTF7" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using var sw = new StreamWriter(fs, Encoding.UTF7);
                fs.Write(new byte[] { 0x2b, 0x2f, 0x76, 0x38 });
                sw.Write(expectedResult);
                sw.Flush();
            }

            // Act.
            using var immediateBuffer = new ImmediateBuffer(new File(fileName));

            immediateBuffer.FillBufferFromFile();

            var actualResult = immediateBuffer.GetBufferContent();

            // Assert.
            Assert.AreEqual(Encoding.UTF7.ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
            Assert.AreEqual(Encoding.UTF7.ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            Assert.IsTrue(string.Equals(expectedResult, actualResult, StringComparison.InvariantCulture), "The text returned by the buffer differs.");
        }

        [TestMethod]
        public void ReadUTF8FileTest()
        {
            // Arrange.
            var fileName = "UTF8" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using var sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(expectedResult);
                sw.Flush();
            }

            // Act.
            using var immediateBuffer = new ImmediateBuffer(new File(fileName));
            immediateBuffer.FillBufferFromFile();

            var actualResult = immediateBuffer.GetBufferContent();

            // Assert.
            Assert.AreEqual(Encoding.UTF8.ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
            Assert.AreEqual(Encoding.UTF8.ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            Assert.IsTrue(string.Equals(expectedResult, actualResult, StringComparison.InvariantCulture), "The text returned by the buffer differs.");
        }

        [TestMethod]
        public void ReadUTF16LEFileTest()
        {
            // Arrange.
            var fileName = "UTF16LE" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using var sw = new StreamWriter(fs, Encoding.Unicode);
                sw.Write(expectedResult);
                sw.Flush();
            }

            // Act.
            using var immediateBuffer = new ImmediateBuffer(new File(fileName));
            immediateBuffer.FillBufferFromFile();

            var actualResult = immediateBuffer.GetBufferContent();

            // Assert.
            Assert.AreEqual(Encoding.Unicode.ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
            Assert.AreEqual(Encoding.Unicode.ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            Assert.IsTrue(string.Equals(expectedResult, actualResult, StringComparison.InvariantCulture), "The text returned by the buffer differs.");
        }

        [TestMethod]
        public void ReadUTF16BEFileTest()
        {
            // Arrange.
            var fileName = "UTF16BE" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using var sw = new StreamWriter(fs, Encoding.BigEndianUnicode);
                sw.Write(expectedResult);
                sw.Flush();
            }

            // Act.
            using var immediateBuffer = new ImmediateBuffer(new File(fileName));
            immediateBuffer.FillBufferFromFile();

            var actualResult = immediateBuffer.GetBufferContent();

            // Assert.
            Assert.AreEqual(Encoding.BigEndianUnicode.ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
            Assert.AreEqual(Encoding.BigEndianUnicode.ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            Assert.IsTrue(string.Equals(expectedResult, actualResult, StringComparison.InvariantCulture), "The text returned by the buffer differs.");
        }

        [TestMethod]
        public void ReadUTF32LEFileTest()
        {
            // Arrange.
            var fileName = "UTF32LE" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using var sw = new StreamWriter(fs, Encoding.UTF32);
                sw.Write(expectedResult);
                sw.Flush();
            }

            // Act.
            using var immediateBuffer = new ImmediateBuffer(new File(fileName));
            immediateBuffer.FillBufferFromFile();

            var actualResult = immediateBuffer.GetBufferContent();

            // Assert.
            Assert.AreEqual(Encoding.UTF32.ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
            Assert.AreEqual(Encoding.UTF32.ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            Assert.IsTrue(string.Equals(expectedResult, actualResult, StringComparison.InvariantCulture), "The text returned by the buffer differs.");
        }

        [TestMethod]
        public void ReadUTF32BEFileTest()
        {
            // Arrange.
            var fileName = "UTF32BE" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using var sw = new StreamWriter(fs, new UTF32Encoding(true, true));
                sw.Write(expectedResult);
                sw.Flush();
            }

            // Act.
            using var immediateBuffer = new ImmediateBuffer(new File(fileName));
            immediateBuffer.FillBufferFromFile();

            var actualResult = immediateBuffer.GetBufferContent();

            // Assert.
            Assert.AreEqual(new UTF32Encoding(true, true).ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
            Assert.AreEqual(new UTF32Encoding(true, true).ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            Assert.IsTrue(string.Equals(expectedResult, actualResult, StringComparison.InvariantCulture), "The text returned by the buffer differs.");
        }

        [TestMethod]
        public void WriteUTF7FileTest()
        {
            // Arrange.
            var fileName = "UTF7" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            // Act.
            using (var immediateBuffer = new ImmediateBuffer(new File(fileName)))
            {
                immediateBuffer.FileInstance.SetInputEncoding(EncodingType.UTF7);
                immediateBuffer.FileInstance.SetOutputEncoding(EncodingType.UTF7);

                immediateBuffer.InsertAtCursor(expectedResult);
                immediateBuffer.DumpBufferToCurrentFile();

                Assert.AreEqual(Encoding.UTF7.ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
                Assert.AreEqual(Encoding.UTF7.ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            }

            string actualResult;

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                using var sw = new StreamReader(fs, Encoding.UTF7);
                actualResult = sw.ReadToEnd();
            }

            // Assert.
            Assert.IsTrue(expectedResult.Contains(actualResult.Remove(0, 4)), "The text returned by the buffer differs.");
        }

        [TestMethod]
        public void WriteUTF8FileTest()
        {
            // Arrange.
            var fileName = "UTF8" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            // Act.
            using (var immediateBuffer = new ImmediateBuffer(new File(fileName)))
            {
                immediateBuffer.FileInstance.SetInputEncoding(EncodingType.UTF8);
                immediateBuffer.FileInstance.SetOutputEncoding(EncodingType.UTF8);

                immediateBuffer.InsertAtCursor(expectedResult);
                immediateBuffer.DumpBufferToCurrentFile();

                Assert.AreEqual(Encoding.UTF8.ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
                Assert.AreEqual(Encoding.UTF8.ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            }

            string actualResult;

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                using var sw = new StreamReader(fs, Encoding.UTF8);
                actualResult = sw.ReadToEnd();
            }

            // Assert.
            Assert.IsTrue(string.Equals(expectedResult, actualResult, StringComparison.InvariantCulture), "The text returned by the buffer differs.");
        }

        [TestMethod]
        public void WriteUTF16LEFileTest()
        {
            // Arrange.
            var fileName = "UTF16LE" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            // Act.
            using (var immediateBuffer = new ImmediateBuffer(new File(fileName)))
            {
                immediateBuffer.FileInstance.SetInputEncoding(EncodingType.UTF16LE);
                immediateBuffer.FileInstance.SetOutputEncoding(EncodingType.UTF16LE);

                immediateBuffer.InsertAtCursor(expectedResult);
                immediateBuffer.DumpBufferToCurrentFile();

                Assert.AreEqual(Encoding.Unicode.ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
                Assert.AreEqual(Encoding.Unicode.ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            }

            string actualResult;

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                using var sw = new StreamReader(fs, Encoding.Unicode);
                actualResult = sw.ReadToEnd();
            }

            // Assert.
            Assert.IsTrue(string.Equals(expectedResult, actualResult, StringComparison.InvariantCulture), "The text returned by the buffer differs.");
        }

        [TestMethod]
        public void WriteUTF16BEFileTest()
        {
            // Arrange.
            var fileName = "UTF16BE" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            // Act.
            using (var immediateBuffer = new ImmediateBuffer(new File(fileName)))
            {
                immediateBuffer.FileInstance.SetInputEncoding(EncodingType.UTF16BE);
                immediateBuffer.FileInstance.SetOutputEncoding(EncodingType.UTF16BE);

                immediateBuffer.InsertAtCursor(expectedResult);
                immediateBuffer.DumpBufferToCurrentFile();

                Assert.AreEqual(Encoding.BigEndianUnicode.ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
                Assert.AreEqual(Encoding.BigEndianUnicode.ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            }

            string actualResult;

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                using var sw = new StreamReader(fs, Encoding.BigEndianUnicode);
                actualResult = sw.ReadToEnd();
            }

            // Assert.
            Assert.IsTrue(string.Equals(expectedResult, actualResult, StringComparison.InvariantCulture), "The text returned by the buffer differs.");
        }

        [TestMethod]
        public void WriteUTF32LEFileTest()
        {
            // Arrange.
            var fileName = "UTF32LE" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            // Act.
            using (var immediateBuffer = new ImmediateBuffer(new File(fileName)))
            {
                immediateBuffer.FileInstance.SetInputEncoding(EncodingType.UTF32LE);
                immediateBuffer.FileInstance.SetOutputEncoding(EncodingType.UTF32LE);

                immediateBuffer.InsertAtCursor(expectedResult);
                immediateBuffer.DumpBufferToCurrentFile();

                Assert.AreEqual(Encoding.UTF32.ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
                Assert.AreEqual(Encoding.UTF32.ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            }

            string actualResult;

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                using var sw = new StreamReader(fs, Encoding.UTF32);
                actualResult = sw.ReadToEnd();
            }

            // Assert.
            Assert.IsTrue(string.Equals(expectedResult, actualResult, StringComparison.InvariantCulture), "The text returned by the buffer differs.");
        }

        [TestMethod]
        public void WriteUTF32BEFileTest()
        {
            // Arrange.
            var fileName = "UTF32BE" + _fileName;
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            // Act.
            using (var immediateBuffer = new ImmediateBuffer(new File(fileName)))
            {
                immediateBuffer.FileInstance.SetInputEncoding(EncodingType.UTF32BE);
                immediateBuffer.FileInstance.SetOutputEncoding(EncodingType.UTF32BE);
                
                immediateBuffer.InsertAtCursor(expectedResult);
                immediateBuffer.DumpBufferToCurrentFile();

                Assert.AreEqual(new UTF32Encoding(true, true).ToString(), immediateBuffer.FileInstance.InputEncoding.ToString());
                Assert.AreEqual(new UTF32Encoding(true, true).ToString(), immediateBuffer.FileInstance.OutputEncoding.ToString());
            }

            string actualResult;

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                using var sw = new StreamReader(fs, new UTF32Encoding(true, true));
                actualResult = sw.ReadToEnd();
            }

            // Assert.
            Assert.IsTrue(string.Equals(expectedResult, actualResult, StringComparison.InvariantCulture), "The text returned by the buffer differs.");
        }
    }
}