using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GradeScores;
namespace grade_scores
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                Console.WriteLine("No correct usage of grade-scores \n usage: grade-scores <filename>");
            else
            {
                GradeMaker gm = new GradeMaker();
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
