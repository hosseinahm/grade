using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GradeScores;
using System.IO;

namespace grade_scores
{
    class GradeFileStreamWriter : IGradeStreamWriter
    {
        private StreamWriter sw;

        public void CloseFile()
        {
            sw.Dispose();
        }
        public void OpenFile(string fileName)
        {
            sw = new StreamWriter(fileName);
        }

        public void WriteLine(string value)
        {
            sw.WriteLine(value);
        }
    }

    class GradeFileStreamReader : IGradeStreamReader
    {
        private StreamReader sr;
        public void CloseFile()
        {
            sr.Dispose();
        }
        public void OpenFile(string fileName)
        {
            sr = new StreamReader(fileName);
        }

        public string ReadLine()
        {
            string lineData = sr.ReadLine();
            while ((lineData != null) && (lineData.Trim() == ""))
            {
                lineData = sr.ReadLine();
            }
            return lineData;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                Console.WriteLine("No correct usage of grade-scores \n usage: grade-scores <filename>");
            else
            {
               var gm = new GradeMaker<GradeFileStreamReader, GradeFileStreamWriter>();
                try
                {
                    gm.CreateGradeFile(args[0]);
                    var gmResult = gm.GradedList.AsEnumerable<string>();
                    foreach (var item in gmResult)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine(String.Format("Finished: created {0}", gm.GradedFileName));
                }
                catch (Exception e)
                {
                    Console.WriteLine(String.Format("Failed: {0}", e.Message));
                }
    
            }
        }
    }
}
