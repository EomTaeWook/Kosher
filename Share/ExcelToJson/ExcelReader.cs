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
            var workBook = app.Workbooks.Open(path);
            if(Validate(workBook) == false)
            {
                return;
            }

            Sheets excelSheet = workBook.Sheets[definitionSheetName];




            var definitionDataTable = new System.Data.DataTable();
                
        }
        private bool Validate(Workbook workBook)
        {
            var sheetNames = new List<string>();
            foreach (Worksheet sheet in workBook.Worksheets)
            {
                sheetNames.Add(sheet.Name.ToLower());
            }

            if(sheetNames.Contains(dataSheetName) == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Data Sheet Not Found!");
                return false;
            }

            if (sheetNames.Contains(definitionSheetName) == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Definition Sheet Not Found!");
                return false;
            }

            return true;
        }
    }
}
