using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Components;
using Components.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComponentsUnitTests
{
    [Leskovar]
    [TestClass]
    public class GapBufferTests
    {
        [TestMethod]
        public void InsertCharTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";

            // Act.
            var index = 0;
            foreach (var character in expectedResult)
            {
                gapBuffer.Insert(character, index);
                index++;
            }

            var actualResult = gapBuffer.GetText(0, gapBuffer.GetLength() - 1);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The buffer storage content differs.");
        }

        [TestMethod]
        public void InsertStringTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";

            // Act.
            gapBuffer.Insert(expectedResult, 0);
            var actualResult = gapBuffer.GetText(0, gapBuffer.GetLength() - 1);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The buffer storage content differs.");
        }

        [TestMethod]
        public void DeleteFirstCharTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = "orem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";
            gapBuffer.Insert("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.", 0);

            // Act.
            gapBuffer.Delete(0, 0);
            var actualResult = gapBuffer.GetText(0, gapBuffer.GetLength() - 1);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The buffer storage content differs.");
        }

        [TestMethod]
        public void DeleteLastCharTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl";
            gapBuffer.Insert("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.", 0);

            // Act.
            gapBuffer.Delete(gapBuffer.GetLength() - 1, gapBuffer.GetLength() - 1);
            var actualResult = gapBuffer.GetText(0, gapBuffer.GetLength() - 1);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The buffer storage content differs.");
        }

        [TestMethod]
        public void DeleteMiddleCharTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = "Loremipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";
            gapBuffer.Insert("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.", 0);

            // Act.
            gapBuffer.Delete(5, 5);
            var actualResult = gapBuffer.GetText(0, gapBuffer.GetLength() - 1);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The buffer storage content differs.");
        }

        [TestMethod]
        public void DeleteFirstStringTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = "ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";
            gapBuffer.Insert("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.", 0);

            // Act.
            gapBuffer.Delete(0, 5);
            var actualResult = gapBuffer.GetText(0, gapBuffer.GetLength() - 1);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The buffer storage content differs.");
        }

        [TestMethod]
        public void DeleteLastStringTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse";
            gapBuffer.Insert("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.", 0);

            // Act.
            gapBuffer.Delete(gapBuffer.GetLength() - 6, gapBuffer.GetLength() - 1);
            var actualResult = gapBuffer.GetText(0, gapBuffer.GetLength() - 1);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The buffer storage content differs.");
        }

        [TestMethod]
        public void DeleteMiddleStringTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = "Lorem dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.";
            gapBuffer.Insert("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.", 0);

            // Act.
            gapBuffer.Delete(5, 10);
            var actualResult = gapBuffer.GetText(0, gapBuffer.GetLength() - 1);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The buffer storage content differs.");
        }

        [TestMethod]
        public void GetLengthInsertTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = 5;

            // Act.
            gapBuffer.Insert("Lorem", 0);
            var actualResult = gapBuffer.GetLength();

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The buffer storage length differs.");
        }

        [TestMethod]
        public void GetLengthDeleteTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = 0;
            gapBuffer.Insert("Lorem", 0);

            // Act.
            gapBuffer.Delete(0, gapBuffer.GetLength() - 1);
            var actualResult = gapBuffer.GetLength();

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The buffer storage length differs.");
        }

        [TestMethod]
        public void GetTextFirstStringTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = "Lorem ";
            gapBuffer.Insert("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.", 0);

            // Act.
            var actualResult = gapBuffer.GetText(0, 5);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The returned text differs.");
        }

        [TestMethod]
        public void GetTextLastStringTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = " nisl.";
            gapBuffer.Insert("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.", 0);

            // Act.
            var actualResult = gapBuffer.GetText(gapBuffer.GetLength() - 6, gapBuffer.GetLength() - 1);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The returned text differs.");
        }

        [TestMethod]
        public void GetTextMiddleStringTest()
        {
            // Arrange.
            var gapBuffer = new GapBuffer();
            var expectedResult = "ipsum";
            gapBuffer.Insert("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Suspendisse nisl.", 0);

            // Act.
            var actualResult = gapBuffer.GetText(6, 10);

            // Assert.
            Assert.AreEqual(expectedResult, actualResult, "The returned text differs.");
        }

        [TestMethod]
        public void ComplexOperationTests()
        {
            var gapBuffer = new GapBuffer();
            const string lorem = "Lorem";

            gapBuffer.Insert(' ', 0);

            foreach (var character in lorem)
            {
                gapBuffer.Insert(character, gapBuffer.GetLength());
            }

            var actualResult = gapBuffer.GetText(0, gapBuffer.GetLength() - 1);
            Assert.AreEqual(" Lorem", actualResult, "The returned text differs.");


            foreach (var character in lorem)
            {
                gapBuffer.Insert(character, 0);
            }

            actualResult = gapBuffer.GetText(0, gapBuffer.GetLength() - 1);
            Assert.AreEqual("meroL Lorem", actualResult, "The returned text differs.");

            gapBuffer.Delete(3,3);
            actualResult = gapBuffer.GetText(0, gapBuffer.GetLength() - 1);
            Assert.AreEqual("merL Lorem", actualResult, "The returned text differs.");

            gapBuffer.Delete(0, gapBuffer.GetLength() - 1);
            actualResult = gapBuffer.GetText(0, 0);
            Assert.AreEqual(default, actualResult, "The returned text differs.");
        }
    }
}
