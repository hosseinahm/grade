using System;
namespace GradeScores
{
    public interface IGradeStreamWriter
    {
        void WriteLine(string value);
        void OpenFile(string fileName);
        void CloseFile();
    }

    public interface IGradeStreamReader
    {
        string ReadLine();
        void OpenFile(string fileName);
        void CloseFile();
    }

}