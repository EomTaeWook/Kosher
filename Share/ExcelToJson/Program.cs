using ExcelToJson.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToJson
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            ExcelReader excelReader = new ExcelReader();
            {
                excelReader.Read($@"C:\Users\trim0\Documents\source\Handy\Share\ExcelTemplate\UserGroup.xlsx");
            }
            
            Console.WriteLine();
        }
        
    }
}
