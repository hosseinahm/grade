﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace GradeScores
{
    public class GradeMaker
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
        private void LoadInputFile( string inputFileName)
        {
            string line;
            GradesDataTable.Clear();
            using (StreamReader fileData = new StreamReader(inputFileName))
            {
                int lineNumber = 0;
                while ((line = fileData.ReadLine()) != null)
                {
                    lineNumber++;
                    var lineData = line.Split(',');
                    if (lineData.Length < 3)
                        throw (new Exception(String.Format("Error in inputfile at line {0}: incorrect number of data.", lineNumber)));
                    var row = GradesDataTable.NewRow();
                    try
                    {
                        row["Grade"] = Convert.ToDouble(lineData[2]);
                    }
                    catch
                    {
                        throw (new Exception(String.Format("Error in inputfile at line {0}: Grade data not in correct format. It cannot be converted to double.", lineNumber)));
                    }
                    row["FirstName"] = lineData[1].Trim();
                    row["LastName"] = lineData[0].Trim();
                    GradesDataTable.Rows.Add(row);
                }
            }
        }
        public void CreateGradeFile(string dataFileName)
        {
            GradedFileName = "";
            LoadInputFile(dataFileName);
            MakeOutputFile(dataFileName);
        }
        private void MakeOutputFile(string dataFileName)
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
            string filePath = Path.GetDirectoryName(dataFileName);
            filePath = (filePath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? filePath : filePath + Path.DirectorySeparatorChar.ToString());
            GradedFileName = String.Format("{0}{1}-graded{2}",filePath
                                                            , Path.GetFileNameWithoutExtension(dataFileName)
                                                            , Path.GetExtension(dataFileName));
            if (query.Count() > 0)
            {
                using (StreamWriter outputFile = new StreamWriter(GradedFileName))
                {
                    foreach (var item in query)
                    {
                        string lineData = String.Format("{0}, {1}, {2}",
                                                                    item.LastName,
                                                                    item.FirstName,
                                                                    item.Grade);
                        GradedList.Add(lineData);
                        outputFile.WriteLine(lineData);
                    }
                }
            }
        }
    }
}
