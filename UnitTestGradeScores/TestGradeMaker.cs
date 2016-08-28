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
        private int lineNumber = -1;
        private string[] lines = new string[] { "BUNDY, TERESSA, 88",
                                                "SMITH, ALLAN, 70" ,
                                                "KING, MADISON, 88",
                                                "SMITH, FRANCIS, 85" };

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
            string inputFileName = "dummyFileName.txt";
            var gm = new GradeMaker<FakeGradeStreamReader, FakeGradeStreamWriter>();
            string desiredGradFileName = String.Format("{0}-graded{1}", Path.GetFileNameWithoutExtension(inputFileName),
                                                                        Path.GetExtension(inputFileName));
            desiredGradFileName = Path.Combine(Path.GetDirectoryName(inputFileName), desiredGradFileName);
            gm.CreateGradeFile(inputFileName);
            Assert.AreEqual<string>(gm.GradedFileName, desiredGradFileName);
                       
        }

        [TestMethod]
        public void SortGrades()
        {
            var gm = new GradeMaker<FakeGradeStreamReader, FakeGradeStreamWriter>();
            gm.CreateGradeFile("dummyFileName.txt");

            Assert.AreEqual<string>(sortedGrades[0], gm.GradedList[0]);
            Assert.AreEqual<string>(sortedGrades[1], gm.GradedList[1]);
            Assert.AreEqual<string>(sortedGrades[2], gm.GradedList[2]);
            Assert.AreEqual<string>(sortedGrades[3], gm.GradedList[3]);
        }


    }

}
