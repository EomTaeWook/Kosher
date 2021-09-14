using ExcelToJson.Data;
using ExcelToJson.Data.Interface;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ExcelToJson
{
    public class ExcelReader
    {
        private Application app = new Application();
        private string dataSheetName = "Data";
        private string definitionSheetName = "Definition";
        public void Read(string path)
        {
            Workbook workbook = null;
            try
            {
                workbook = app.Workbooks.Open(path);
                if (Validate(workbook) == false)
                {
                    return;
                }

                Worksheet definitionSheet = workbook.Sheets.Item[definitionSheetName];
                var definitionDataTable = definitionSheet.ExportToDataTable();
                var dataTable = MakeDataTable(definitionDataTable);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                workbook?.Close();
                app.Quit();
            }
        }
        private bool Validate(Workbook workBook)
        {
            var sheetNames = new List<string>();
            foreach (Worksheet sheet in workBook.Worksheets)
            {
                sheetNames.Add(sheet.Name.ToLower());
            }

            if(sheetNames.Contains(dataSheetName.ToLower()) == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Data Sheet Not Found!");
                return false;
            }

            if (sheetNames.Contains(definitionSheetName.ToLower()) == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Definition Sheet Not Found!");
                return false;
            }

            return true;
        }
        public System.Data.DataTable MakeDataTable(System.Data.DataTable definitionDataTable)
        {
            var dataTable = new System.Data.DataTable();

            for (int i = 0; i < definitionDataTable.Rows.Count; ++i)
            {
                var column = new System.Data.DataColumn();
                for (int ii = 0; ii < definitionDataTable.Columns.Count; ++ii)
                {
                    var columnType = (DefinitionColumnType)Enum.Parse(typeof(DefinitionColumnType), definitionDataTable.Columns[ii].ColumnName);
                    if (columnType == DefinitionColumnType.Name)
                    {
                        column.ColumnName = definitionDataTable.Rows[i][ii].ToString();
                    }
                    else if (columnType == DefinitionColumnType.Type)
                    {
                        column.DataType = Type.GetType(definitionDataTable.Rows[i][ii].ToString());
                    }
                    else if (columnType == DefinitionColumnType.Count)
                    {
                        var count = int.Parse(definitionDataTable.Rows[i][definitionDataTable.Columns[ii].ColumnName].ToString());
                        if(count > 1)
                        {
                            column.DataType = Type.GetType($"System.Collections.Generic.List`1[{column.DataType}]");
                        }
                    }
                }
                dataTable.Columns.Add(column);
            }

            return dataTable;
        }
    }
}
