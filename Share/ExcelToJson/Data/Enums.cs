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
}
