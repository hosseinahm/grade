using Microsoft.VisualStudio.TestTools.UnitTesting;
using GradeScores;
using System.IO;
using System;

namespace unittestGradeMaker
{
    class FakeGradeStreamWriter : IGradeStreamWriter
    {
        public void CloseFile() {}
        public void OpenFile(string fileName) {}
        public void WriteLine(string value) {}
    }

    class FakeGradeStreamReader : IGradeStreamReader
    {
        public FakeGradeStreamReader()
        {
            lines = new string[] { "" };
        }
        protected int lineNumber = -1;
        protected string[] lines;

        public void CloseFile()
        {
            lineNumber = -1;
        }
        public void OpenFile(string fileName)
        {
            lineNumber = -1;
        }

        public string ReadLine()
        {
            lineNumber++;
            if (lineNumber < lines.Length)
                return lines[lineNumber];
            else
                return null;
        }
    }

    class FakeGradeStreamReaderCorrectFormat : FakeGradeStreamReader
    {
        public FakeGradeStreamReaderCorrectFormat()
        {
            lines = new string[] { "BUNDY, TERESSA, 88",
                                    "SMITH, ALLAN, 70" ,
                                    "KING, MADISON, 88",
                                    "SMITH, FRANCIS, 85" };
        }
    }

    class FakeGradeStreamReaderBadFormat : FakeGradeStreamReader
    {
        public FakeGradeStreamReaderBadFormat()
        {
            lines = new string[] { "BUNDY, TERESSA, 88",
                                    "SMITH, ALLAN"     ,            // Bad format line
                                    "KING, MADISON, 88",
                                    "SMITH, FRANCIS, 85" };
        }
    }
    class FakeGradeStreamReaderBadDataFormat : FakeGradeStreamReader
    {
        public FakeGradeStreamReaderBadDataFormat()
        {
            lines = new string[] { "BUNDY, TERESSA, 88",
                                    "SMITH, ALLAN, 88.3e"     ,            // Bad dta format line
                                    "KING, MADISON, 88",
                                    "SMITH, FRANCIS, 85" };
        }
    }

    [TestClass]
    public class testGradeMaker
    {
        private string[] sortedGrades = new string[] { "BUNDY, TERESSA, 88",
                                                       "KING, MADISON, 88",
                                                       "SMITH, FRANCIS, 85",
                                                       "SMITH, ALLAN, 70" };
        [TestMethod]
        public void GradeOutputFileName()
        {
            string inputFileName = "dmy.txt";
            var gm = new GradeMaker<FakeGradeStreamReaderCorrectFormat, FakeGradeStreamWriter>();
            string desiredGradFileName = String.Format("{0}-graded{1}", Path.GetFileNameWithoutExtension(inputFileName),
                                                                        Path.GetExtension(inputFileName));
            desiredGradFileName = Path.Combine(Path.GetDirectoryName(inputFileName), desiredGradFileName);
            gm.CreateGradeFile(inputFileName);
            Assert.AreEqual<string>(gm.GradedFileName, desiredGradFileName);
                       
        }

        [TestMethod]
        public void SortGrades()
        {
            var gm = new GradeMaker<FakeGradeStreamReaderCorrectFormat, FakeGradeStreamWriter>();
            gm.CreateGradeFile("dmy.txt");

            Assert.AreEqual<string>(sortedGrades[0], gm.GradedList[0]);
            Assert.AreEqual<string>(sortedGrades[1], gm.GradedList[1]);
            Assert.AreEqual<string>(sortedGrades[2], gm.GradedList[2]);
            Assert.AreEqual<string>(sortedGrades[3], gm.GradedList[3]);
        }
        [TestMethod]
        public void BadFormatFileException()
        {
            var gm = new GradeMaker<FakeGradeStreamReaderBadFormat, FakeGradeStreamWriter>();
            try
            {
                gm.CreateGradeFile("dmy.txt");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is GradeBadFormatException);
            }
        }

        [TestMethod]
        public void BadDataFormatFileException()
        {
            var gm = new GradeMaker<FakeGradeStreamReaderBadDataFormat, FakeGradeStreamWriter>();
            try
            {
                gm.CreateGradeFile("dmy.txt");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is GradeBadDataFormatException);
            }
        }

    }

}
