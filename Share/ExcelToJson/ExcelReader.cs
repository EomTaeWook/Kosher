
using ExcelToJson.Data;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using DataTable = System.Data.DataTable;

namespace ExcelToJson
{
    public class ExcelReader
    {
        private const string DataSheetName = "Data$";
        private const string DefineSheetName = "Define$";
        private const string ConnectString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
        public void Read(string path)
        {
            var defineTable = GetDataTableFromDataSheet(path, DefineSheetName);

            if(defineTable == null)
            {
                throw new Exception($"Not found Define DataSheet");
            }

            var dataTable = GetDataTableFromDataSheet(path, DataSheetName);

            if (dataTable == null)
            {
                throw new Exception($"Not found Data DataSheet");
            }
            var define = GetDefintFromDataTable(defineTable);
        }


        private Dictionary<string,  Define> GetDefintFromDataTable(DataTable dataTable)
        {
            Dictionary<string, Define> defines = new Dictionary<string, Define>();

            foreach(DataRow row in dataTable.Rows)
            {
                Define define = new Define();
                define.Name = row["Name"].ToString();
                define.Required = bool.Parse(row["Required"].ToString());
                define.Count = int.Parse(row["Count"].ToString());
                define.Type = Type.GetType($"System.{row["Type"]}", true, true);

                defines.Add(define.Name, define);
            }

            return defines;
        }

        private DataTable GetDataTableFromDataSheet(string path, string sheetName)
        {
            DataTable dataTable = null;
            try
            {
                using (OleDbConnection conn = new OleDbConnection(string.Format(ConnectString, path)))
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand
                    {
                        Connection = conn
                    };

                    DataTable dataTableSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    foreach (DataRow row in dataTableSheet.Rows)
                    {
                        string currentSheetName = row["TABLE_NAME"].ToString();
                        if (currentSheetName.Equals(sheetName))
                        {
                            cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                            dataTable = new DataTable
                            {
                                TableName = sheetName
                            };
                            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmd);
                            dataAdapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FormatException(ex.Message);
            }
            return dataTable;
        }

        //private bool Validate(string path)
        //{
        //    Application app = new Application();
        //    Workbook workbook = null;
        //    try
        //    {
        //        workbook = app.Workbooks.Open(path);

        //        var sheetNames = new List<string>();
        //        foreach (Worksheet sheet in workbook.Worksheets)
        //        {
        //            sheetNames.Add(sheet.Name.ToLower());
        //        }

        //        if (sheetNames.Contains(DataSheetName.ToLower()) == false)
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine($"{path} Data Sheet Not Found!");
        //            return false;
        //        }

        //        if (sheetNames.Contains(DefinitionSheetName.ToLower()) == false)
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine($"{path} Definition Sheet Not Found!");
        //            return false;
        //        }

        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new FormatException(ex.Message);
        //    }
        //    finally
        //    {
        //        workbook?.Close();
        //        app.Quit();
        //    }
            
        //}
        //public System.Data.DataTable MakeDataTable(System.Data.DataTable definitionDataTable)
        //{
        //    var dataTable = new System.Data.DataTable();

        //    for (int i = 0; i < definitionDataTable.Rows.Count; ++i)
        //    {
        //        var column = new System.Data.DataColumn();
        //        for (int ii = 0; ii < definitionDataTable.Columns.Count; ++ii)
        //        {
        //            var columnType = (DefinitionColumnType)Enum.Parse(typeof(DefinitionColumnType), definitionDataTable.Columns[ii].ColumnName);
        //            if (columnType == DefinitionColumnType.Name)
        //            {
        //                column.ColumnName = definitionDataTable.Rows[i][ii].ToString();
        //            }
        //            else if (columnType == DefinitionColumnType.Type)
        //            {
        //                column.DataType = Type.GetType(definitionDataTable.Rows[i][ii].ToString());
        //            }
        //            else if (columnType == DefinitionColumnType.Count)
        //            {
        //                var count = int.Parse(definitionDataTable.Rows[i][definitionDataTable.Columns[ii].ColumnName].ToString());
        //                if(count > 1)
        //                {
        //                    column.DataType = Type.GetType($"System.Collections.Generic.List`1[{column.DataType}]");
        //                }
        //            }
        //        }
        //        dataTable.Columns.Add(column);
        //    }

        //    return dataTable;
        //}
    }
}
