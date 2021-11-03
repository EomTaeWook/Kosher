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
            try
            {
                var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));

                DirectoryInfo directoryInfo = new DirectoryInfo(config.ReadPath);

                var files = new List<FileInfo>();
                foreach(var file in directoryInfo.GetFiles())
                {
                    if(file.Extension.Equals("xlsx") == true)
                    {
                        files.Add(file);
                    }
                }

                ExcelReader excelReader = new ExcelReader();

                foreach(var file in files)
                {
                    excelReader.Read(file.FullName, config.OutputPath);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($@"작업이 완료되었습니다. 파일을 확인해주세요.");
            Console.ReadLine();
        }
        
    }
}
