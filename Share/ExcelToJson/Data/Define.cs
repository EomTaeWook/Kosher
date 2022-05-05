using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToJson.Data
{
    public class Define
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public bool Required { get; set; }

        public DataType DataType { get; set; }

        public List<string> Members { get; set; } = new List<string>();
    }
}
