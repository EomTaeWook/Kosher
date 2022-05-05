using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToJson.Data
{
    public class ClassMemeberInfoData
    {
        public string MemberName { get; set; }
        public string ParentName { get; set; }
        public ClassMemeberInfoData Parent { get; set; }
    }
}
