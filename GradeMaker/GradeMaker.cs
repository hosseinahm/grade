using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace GradeScores
{
    public class GradeMaker<GradeStreamReader, GradeStreamWriter> where  GradeStreamWriter : IGradeStreamWriter, new()
                                                                  where GradeStreamReader: IGradeStreamReader, new()
    {
        public List<string> GradedList;
        private DataSet GradesDataSet;
        private DataTable GradesDataTable;
        public string GradedFileName;

        public GradeMaker()
        {
            DataTable Table = new DataTable("Grades");

            Table.Columns.Add(new DataColumn
            {
                DataType = typeof(double),
                ColumnName = "Grade",
                ReadOnly = true
            });

            Table.Columns.Add(new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "FirstName",
                ReadOnly = true
            });

            Table.Columns.Add(new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "LastName",
                ReadOnly = true
            });

            DataColumn[] PrimaryKeyColumns = new DataColumn[2];
            PrimaryKeyColumns[0] = Table.Columns["Grade"];
            PrimaryKeyColumns[1] = Table.Columns["LastName"];
            Table.PrimaryKey = PrimaryKeyColumns;

            GradesDataSet = new DataSet();
            GradesDataSet.Tables.Add(Table);
            GradesDataTable = GradesDataSet.Tables["Grades"];
            GradedList = new List<string>();
        }
        private void PopulateData(string inputFileName)
        {
            string line;
            GradesDataTable.Clear();

            int lineNumber = 0;
            GradeStreamReader sr = new GradeStreamReader();
            sr.OpenFile(inputFileName);
            while ((line = sr.ReadLine()) != null)
            {
                lineNumber++;
                var lineData = line.Split(',');
                if (lineData.Length < 3)
                    throw (new GradeBadFormatException(lineNumber));
                var row = GradesDataTable.NewRow();
                try
                {
                    row["Grade"] = Convert.ToDouble(lineData[2]);
                }
                catch
                {
                    throw (new GradeBadDataFormatException(lineNumber));
                }
                row["FirstName"] = lineData[1].Trim();
                row["LastName"] = lineData[0].Trim();
                GradesDataTable.Rows.Add(row);
            }
            sr.CloseFile();
        }

        public void CreateGradeFile(string inputFileName)
        {
            string gradeFileName = String.Format("{0}-graded{1}",  Path.GetFileNameWithoutExtension(inputFileName)
                                                            , Path.GetExtension(inputFileName));
            GradedFileName = Path.Combine(Path.GetDirectoryName(inputFileName), gradeFileName);
            PopulateData(inputFileName);
            MakeOutputFile();
        }
        private void MakeOutputFile()
        {
            GradedList.Clear();
            var dataTable = GradesDataSet.Tables["Grades"];
            var query = from grade in dataTable.AsEnumerable() 
                        orderby grade.Field<double>("Grade") descending,
                                grade.Field<string>("LastName") ascending,
                                grade.Field<string>("FirstName") ascending
                        select new
                        {
                            FirstName = grade.Field<string>("FirstName"),
                            LastName = grade.Field<string>("LastName"),
                            Grade = grade.Field<double>("Grade")
                        };
            if (query.Count() > 0)
            {
                GradeStreamWriter sw = new GradeStreamWriter();
                sw.OpenFile(GradedFileName);
                foreach (var item in query)
                {
                    string lineData = String.Format("{0}, {1}, {2}",
                                                                item.LastName,
                                                                item.FirstName,
                                                                item.Grade);
                    GradedList.Add(lineData);
                    sw.WriteLine(lineData);
                }
                sw.CloseFile();
            }
        }
    }
}
