using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToJson
{
    public static class WorksheetExtension
    {
        public static System.Data.DataTable ExportToDataTable(this Worksheet worksheet)
        {
            var dataTable = new System.Data.DataTable();

            for(int i=0; i< worksheet.UsedRange.Columns.Count; ++i)
            {
                dataTable.Columns.Add(((Range)worksheet.Cells[1, 1 + i]).Value);
            }

            for(int i=1; i< worksheet.UsedRange.Rows.Count; ++i)
            {
                var row = dataTable.NewRow();
                for (int ii=0; ii<dataTable.Columns.Count; ++ii)
                {
                    row[ii] = ((Range)worksheet.Cells[1 + i, 1 + ii]).Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
