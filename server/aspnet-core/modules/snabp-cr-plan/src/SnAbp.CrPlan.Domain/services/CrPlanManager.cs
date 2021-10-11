using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SnAbp.File.Services;
using SnAbp.Utils.ExcelHelper;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace SnAbp.CrPlan.Services
{
    public class CrPlanManager : DomainService
    {
        private readonly FileManager _fileManager;

        public CrPlanManager(FileManager fileManager)
        {
            _fileManager = fileManager;
        }

        /// <summary>
        /// Json 转为 DataTable
        /// </summary>
        /// <param name="jsonData">需要导入到表格的json数据</param>
        /// <param name="tableName">表格名称</param>
        /// <returns></returns>
        public async Task<JObject> JsonToDataTable(JArray jsonData, string tableName,bool isRepaire)
        {
            var tableData = new DataTable();
            var colnames = ((JObject) (jsonData.First))?.Properties();
            var colNames = new List<string>();
            try
            {
                foreach (var item in colnames)
                {
                    if (!colNames.Contains(item.Name))
                    {
                        colNames.Add(item.Name);
                        tableData.Columns.Add(item.Name, typeof(string));
                    }
                }

                foreach (var jToken in jsonData)
                {
                    var data = (JObject) jToken;
                    var row = tableData.NewRow();
                    foreach (var colName in colNames)
                    {
                        if (colName == null)
                        {
                            data.Add(colName);
                            row[colName] = data[colName];
                        }

                        row[colName] = data[colName];
                    }

                    tableData.Rows.Add(row);
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException("提交失败");
            }

            byte[] excel;
            if (isRepaire)
            {
                excel = ExcelHelper.DataTableToExcel(tableData, tableName + ".xlsx", null, tableName, true);
            }
            else
            {
                excel = ExcelHelper.DataTableToExcel(tableData, tableName + ".xlsx", null, null, false);
            }
            var dataStream = new MemoryStream(excel);

            var file = await _fileManager.CreateFileByStream(dataStream, tableName + ".xlsx");

            var fileJObject = new JObject
            {
                ["id"] = file.Id,
                ["name"] = file.Name,
                ["type"] = file.Type,
                ["url"] = file.Url,
                ["size"] = file.Size
            };

            return fileJObject;
        }
    }
}
