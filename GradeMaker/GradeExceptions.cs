using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeScores
{
    public class GradeBadFormatException : Exception
    {
        public GradeBadFormatException(int lineNumber) 
            :base(String.Format("Error in inputfile at line {0}: incorrect number of data.", lineNumber))
        {
        }
    }

    public class GradeBadDataFormatException : Exception
    {
        public GradeBadDataFormatException(int lineNumber)
            : base(String.Format("Error in inputfile at line {0}: Grade data not in correct format. It cannot be converted to double.", lineNumber))
        {
        }
    }


}
