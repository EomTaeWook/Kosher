using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToJson.Data
{
    public enum DefinitionColumnType
    {
        Name,
        Type,
        Count,
        Required,

        Member,
        RefTemplate,
        RefTemplateField,

        Max,
    }

    public enum DataType
    {
        Class,
        Int32,
        Int64,
        String,
        

        Max,
    }
    public class DataHelper
    {
        public static string[] DataTypeToString { get; set; } = new string[(int)DataType.Max]
        {
            "class",
            "int32",
            "int64",
            "string",
        };
    }

}
