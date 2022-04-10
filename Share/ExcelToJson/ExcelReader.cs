
using ExcelToJson.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using DataTable = System.Data.DataTable;

namespace ExcelToJson
{
    public class ExcelReader
    {
        private Dictionary<string, string> findParent = new Dictionary<string, string>();


        private const string DataSheetName = "Data$";
        private const string DefineSheetName = "Define$";
        private const string ConnectString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=NO;IMEX=1;MAXSCANROWS=0""";
        public void Read(string excelPath, string outputPath)
        {
            var fileName = Path.GetFileNameWithoutExtension(excelPath);

            var defineTable = GetDataTableFromDataSheet(excelPath, DefineSheetName);

            if(defineTable == null)
            {
                throw new Exception($"Not found Define DataSheet");
            }

            var dataTable = GetDataTableFromDataSheet(excelPath, DataSheetName);

            if (dataTable == null)
            {
                throw new Exception($"Not found Data DataSheet");
            }
            var define = GetDefineFromDataTable(defineTable);

            //ValidateDataTable(define, dataTable);

            var jsonDatas = MakeJsonObjectFromDataTable(define, dataTable);

            ExportJsonFile(outputPath, fileName, jsonDatas);
        }
        //private void ValidateDataTable(Dictionary<string, Define> defineDatas, DataTable dataTable)
        //{
        //    if (defineDatas.ContainsKey("Id") == false)
        //    {
        //        throw new FormatException("Id는 필수입니다.");
        //    }
        //    else if (defineDatas.ContainsKey("Name") == false)
        //    {
        //        throw new FormatException("Name는 필수입니다.");
        //    }

        //    for (int i = 1; i < dataTable.Rows.Count; ++i)
        //    {
        //        for(int ii=0; ii<defineDatas.Values.Count; ++i)
        //        {
        //            var value = dataTable.Rows[i][ii];
        //        }
                
        //    }




        //        var maxDummyLine = 0;
        //    foreach (var kv in defineDatas)
        //    {
        //        maxDummyLine += GetDummyLine(kv.Value);
        //    }

        //    if(dataTable.Rows[maxDummyLine].ItemArray[0].ToString().Equals("Id") == false)
        //    {
        //        throw new FormatException("Id는 첫번째 필드에 존재해야합니다.");
        //    }

        //    if (dataTable.Rows[maxDummyLine].ItemArray[1].ToString().Equals("Name") == false)
        //    {
        //        throw new FormatException("Name는 두번째 필드에 존재해야합니다.");
        //    }

        //    var columnIndex = 0;
        //    foreach(var kv in defineDatas)
        //    {
        //        var dummyLine = GetDummyLine(kv.Value);

        //        var columnName = dataTable.Rows[maxDummyLine - dummyLine].ItemArray[columnIndex].ToString();

        //        if(columnName.Equals(kv.Key) == false)
        //        {
        //            throw new FormatException($"Data 필드와 Define 필드 동일하지 않습니다. Define : {kv.Key} Data : {columnName}");
        //        }

        //        columnIndex += kv.Value.Count;
        //    }

        //    var dataIndex = 1 + maxDummyLine;
        //    for (int i= dataIndex; i<dataTable.Rows.Count; ++i)
        //    {
        //        var idDataValue = dataTable.Rows[i][0].ToString();

        //        var nameDataValue = dataTable.Rows[i][1].ToString();

        //        for (int ii = i + 1; ii < dataTable.Rows.Count; ++ii)
        //        {
        //            if (dataTable.Rows[ii][0].ToString().Equals(idDataValue))
        //            {
        //                throw new FormatException($"Row {ii} Id 값이 중복입니다. id : {idDataValue}");
        //            }

        //            if (dataTable.Rows[ii][1].ToString().Equals(nameDataValue))
        //            {
        //                throw new FormatException($"Row {ii} Name 값이 중복입니다. name : {nameDataValue}");
        //            }
        //        }
        //    }
        //}
        //private int GetDummyLine(Define define)
        //{
        //    var dummyLine = 0;

        //    if(define.Members.Count > 0)
        //    {
        //        dummyLine++;
        //    }

        //    foreach(var memberDefine in define.Members.Values)
        //    {
        //        if(memberDefine == null)
        //        {
        //            throw new FormatException($"member Name {memberDefine.Name} 를 찾을 수 없습니다.");
        //        }
        //        dummyLine += GetDummyLine(memberDefine);
        //    }

        //    return dummyLine;
        //}
        private List<Dictionary<string, object>>  MakeJsonObjectFromDataTable(Dictionary<string, Define> defineDatas,
                                                                                DataTable dataTable)
        {
            var jsonObjects = new List<Dictionary<string, object>>();

            //for (int i = 1; i < dataTable.Rows.Count; ++i)
            //{
            //    var dic = new Dictionary<string, object>();
            //    var columnIndex = 0;

            //    foreach (var kv in defineDatas)
            //    {
            //        object value = null;

            //        if (kv.Value.Count > 1)
            //        {
            //            var listType = typeof(List<>);
            //            Type constructed = listType.MakeGenericType(kv.Value.Type);
            //            var list = (System.Collections.IList)Activator.CreateInstance(constructed);
            //            for (int ii = 0; ii < kv.Value.Count; ++ii)
            //            {
            //                var dataValue = Convert.ChangeType(dataTable.Rows[i].ItemArray[columnIndex + ii], kv.Value.Type);
            //                list.Add(dataValue);
            //            }
            //            value = list;
            //        }
            //        else
            //        {
            //            value = Convert.ChangeType(dataTable.Rows[i].ItemArray[columnIndex], kv.Value.Type);
            //        }
            //        if (kv.Value.Required == true)
            //        {
            //            if (value == null)
            //            {
            //                throw new Exception($"{kv.Key} is required field");
            //            }
            //        }
            //        dic.Add(kv.Key, value);
            //        columnIndex += kv.Value.Count;
            //    }
            //    jsonObjects.Add(dic);
            //}
            return jsonObjects;
        }
        private void ExportJsonFile(string outputPath, string fileName, List<Dictionary<string, object>> jsonDatas)
        {
            if (Directory.Exists(outputPath) == false)
            {
                Directory.CreateDirectory(outputPath);
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonDatas, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText($"{outputPath}{fileName}.json", json);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{outputPath}{fileName}.json 파일 생성");
        }
        private Dictionary<string,  Define> GetDefineFromDataTable(DataTable dataTable)
        {
            Dictionary<string, Define> defines = new Dictionary<string, Define>();
            List<DefinitionColumnType> defineColumns = new List<DefinitionColumnType>();

            for (int i = 0; i < dataTable.Rows[0].ItemArray.Length; ++i)
            {
                var value = dataTable.Rows[0].ItemArray[i].ToString();
                var definitionColumnType = (DefinitionColumnType)Enum.Parse(typeof(DefinitionColumnType), value);
                if (value.ToString() == definitionColumnType.ToString())
                {
                    defineColumns.Add(definitionColumnType);
                }
            }

            for(int i=1; i< dataTable.Rows.Count; ++i)
            {
                var row = dataTable.Rows[i];
                var define = GetDefineData(row, defineColumns);

                //for (int ii=0; ii< define.Members.Count; ++ii)
                //{
                //    var memberRow = i + 1 + ii;
                //    var memberDefine = GetDefineData(dataTable.Rows[memberRow], defineColumns);

                //    if(define.Members.ContainsKey(memberDefine.Name) == false)
                //    {
                //        throw new Exception($"Not Found Member Field! Key : {memberDefine.Name}");
                //    }
                //    define.Members[memberDefine.Name] = memberDefine;
                //    ++i;
                //}

                defines.Add(define.Name, define);
            }

            return defines;
        }
        private Define GetDefineData(DataRow row, List<DefinitionColumnType> defineMembers)
        {
            var defineData = new Define();
            var index = 0;
            foreach (var defineMember in defineMembers)
            {
                var columnType = defineMember;

                if (columnType == DefinitionColumnType.Name)
                {
                    defineData.Name = row.ItemArray[index].ToString();
                }
                else if (columnType == DefinitionColumnType.Required)
                {
                    defineData.Required = bool.Parse(row.ItemArray[index].ToString());
                }
                else if (columnType == DefinitionColumnType.Count)
                {
                    defineData.Count = int.Parse(row.ItemArray[index].ToString());

                    if(defineData.Count <= 0)
                    {
                        throw new Exception($"Must be greater than 0. {row.ItemArray[index]}");
                    }

                }
                else if (columnType == DefinitionColumnType.Type)
                {
                    if(GetDataType(row.ItemArray[index].ToString(), defineData) == false)
                    {
                        throw new Exception($"Data Type Invalid! type : {row.ItemArray[index]}");
                    }
                }
                else if (columnType == DefinitionColumnType.Member)
                {
                    var value = row.ItemArray[index].ToString();

                    if(string.IsNullOrEmpty(value) == true)
                    {
                        continue;
                    }

                    if (defineData.DataType != DataType.Class)
                    {
                        throw new Exception($"Type is not class! {defineData.DataType}");
                    }
                    
                    defineData.Members.Add(row.ItemArray[index].ToString());


                }
                index++;
            }
            return defineData;
        }

        private bool GetDataType(string type, Define define)
        {
            for(int i=0; i< (int)DataType.Max; ++i)
            {
                if(type.ToLower() == DataHelper.DataTypeToString[i])
                {
                    define.DataType = (DataType)i;
                    return true;
                }
            }

            return false;
        }
        private DataTable GetDataTableFromDataSheet(string path, string sheetName)
        {
            DataTable dataTable = null;
            try
            {
                using (OleDbConnection conn = new OleDbConnection(string.Format(ConnectString, path)))
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand
                    {
                        Connection = conn
                    };

                    DataTable dataTableSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    foreach (DataRow row in dataTableSheet.Rows)
                    {
                        string currentSheetName = row["TABLE_NAME"].ToString();
                        if (currentSheetName.Equals(sheetName))
                        {
                            cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                            dataTable = new DataTable();
                            dataTable.TableName = sheetName;

                            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmd);
                            dataAdapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FormatException(ex.Message);
            }
            return dataTable;
        }
    }
}
