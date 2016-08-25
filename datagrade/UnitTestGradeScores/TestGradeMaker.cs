using Microsoft.VisualStudio.TestTools.UnitTesting;
using GradeScores;
using System.IO;

namespace UnitTestGradeScores
{
    [TestClass]
    public class TestGradeMaker
    {
        [TestMethod]
        public void TestMethodGradeScores()
        {
            GradeMaker gm = new GradeMaker();
            using (StreamWriter sw = new StreamWriter("c:\\UnitTest.txt"))
            {
                sw.WriteLine("BUNDY, TERESSA, 88");
                sw.WriteLine("KING, MADISON, 88");
                sw.WriteLine("SMITH, FRANCIS, 85");
                sw.WriteLine("SMITH, ALLAN, 70");
            }
            gm.CreateGradeFile("c:\\test.txt");
            Assert.IsTrue(File.Exists("c:\\UnitTest-graded.txt"));
            Assert.AreEqual<string>("BUNDY, TERESSA, 88", gm.GradedList[0]);
            Assert.AreEqual<string>("KING, MADISON, 88", gm.GradedList[1]);
            Assert.AreEqual<string>("SMITH, FRANCIS, 85", gm.GradedList[2]);
            Assert.AreEqual<string>("SMITH, ALLAN, 70", gm.GradedList[3]);
            File.Delete(gm.GradedFileName);
        }
    }
        
}
