using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToJson.Data
{
    public class Definition
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
        public bool Required { get; set; }
    }
}
