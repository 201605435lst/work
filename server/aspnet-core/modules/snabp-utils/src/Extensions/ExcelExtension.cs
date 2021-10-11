/**********************************************************************
*******命名空间： SnAbp.Utils.Extensions
*******类 名 称： ExcelExtension
*******类 说 明： 表单扩展方法
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/11 10:32:01
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SnAbp.Utils.ExcelHelper;

namespace SnAbp.Utils
{
    public static class ExcelExtension
    {
        /// <summary>
        /// 将表格<seealso cref="IWorkbook"/>转为<seealso cref="Byte[]"/>流 
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(this IWorkbook workbook)
        {
            try
            {
                var stream = new MemoryStream();
                workbook.Write(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                return new byte[0] { };
            }
        }

        /// <summary>
        /// 将对象转化为字节数字
        /// </summary>
        /// <param name="obj">需要转化对象</param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(this object obj) => obj.ConvertToStream().GetBuffer();

        /// <summary>
        /// 对象转为数据流
        /// </summary>
        /// <param name="obj">需要转化的对象</param>
        /// <returns></returns>
        public static MemoryStream ConvertToStream(this object obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream;
            }
        }
        /// <summary>
        /// 字节数组转数据流
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static MemoryStream BytesToStream(this byte[] datas)
        {
            return new MemoryStream(datas);
        }
        /// <summary>
        /// 获取工作表的了类型
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static ExcelFileType GetWorkbookType(this ISheet sheet)
        {
            var workType = sheet.Workbook.GetType().Name;
            switch (workType)
            {
                case "XSSFWorkbook":
                    return ExcelFileType.Xlsx;
                case "HSSFWorkbook":
                    return ExcelFileType.Xls;
                default: return default;
            }
        }

        public static ExcelFileType GetWorkbookType(this IRow row) => row.Sheet.GetWorkbookType();

        /// <summary>
        /// 检查需要导入的工作表是和模板实体对应
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet">工作表</param>
        /// <param name="validDataIndex">有效数据索引</param>
        /// <returns></returns>
        public static ISheet CheckColumnAccordTempleModel<T>(this ISheet sheet, int validDataIndex = 0)
        {
            if (sheet == null) throw new ArgumentNullException(paramName: nameof(CheckColumnAccordTempleModel), "工作表为空");
            var cellCount = sheet.GetRow(validDataIndex).PhysicalNumberOfCells;
            var properties = typeof(T).GetProperties();

            // 逻辑优化,如果实体包含扩展内容，则不需要再判断
            if (properties.Any(a => a.Name == "Properties"))
            {
                return sheet;
            }
            else
            {
                var modelPropsCount = typeof(T).GetProperties().Length;
                return cellCount == modelPropsCount ? sheet : throw new Exception("导入文件与对应的模板不符，请检查");
            };
        }

        public static ISheet WriteData<T>(this ISheet sheet, List<T> datas, int rowIndex = 0)
        {
            // 判断T中是否有扩展属性
            var props = typeof(T).GetProperties();
            if (props.Count(a => a.Name != "Properties") != sheet.GetRow(rowIndex).PhysicalNumberOfCells)
            {
                throw new ArgumentException("模板与数据格式不符");
            }

            bool isGetStyle = false;
            ICellStyle cellStyle = null;
            var rowNumber = 1;

            ICell theCell;
            //cellStyle = ExcelHelper.ExcelHelper.GetCopyCellStyle(sheet.Workbook, theCell.CellStyle);
            cellStyle = sheet.Workbook.CreateCellStyle();

            datas?.ForEach(a =>
            {
                var colIndex = 0;

                var row = sheet
                    .CreateRow(rowIndex);
                foreach (var prop in props)
                {
                    row
                    .CreateCell(colIndex)
                    .SetCellValue(prop.GetValue(a)?.ToString() == "0" && colIndex == 0 ? rowNumber.ToString() : prop.GetValue(a)?.ToString());

                    theCell = row.GetCell(colIndex);
                    if (isGetStyle == false)
                    {
                        cellStyle.BorderTop = BorderStyle.Thin;
                        cellStyle.BorderRight = BorderStyle.Thin;
                        cellStyle.BorderBottom = BorderStyle.Thin;
                        cellStyle.BorderLeft = BorderStyle.Thin;
                        cellStyle.WrapText = true;
                        isGetStyle = true;
                    }
                    theCell.CellStyle = cellStyle;

                    colIndex++;
                }
                /*for (int i = 0; i < row.LastCellNum; i++)
                {
                    var theCell = row.GetCell(i);
                    if (isGetStyle == false)
                    {
                        cellStyle = ExcelHelper.ExcelHelper.GetCopyCellStyle(sheet.Workbook, theCell.CellStyle);
                        cellStyle.BorderTop = BorderStyle.Thin;
                        cellStyle.BorderRight = BorderStyle.Thin;
                        cellStyle.BorderBottom = BorderStyle.Thin;
                        cellStyle.BorderLeft = BorderStyle.Thin;
                        isGetStyle = true;
                    }
                    theCell.CellStyle = cellStyle;
                }*/
                isGetStyle = false;
                rowNumber++;
                rowIndex++;
            });
            return sheet;
        }


        /// <summary>
        /// 错误信息扩展
        /// </summary>
        /// <param name="wrongInfo"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static WrongInfo AppendInfo(this WrongInfo wrongInfo, string content)
        {
            wrongInfo.WrongMsg += content;
            return wrongInfo;
        }

        /// <summary>
        /// 新增列扩展
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="infos"></param>
        /// <param name="title"></param>

        public static void CreateInfoColumn(this ISheet sheet, List<WrongInfo> infos, string title = "异常信息")
        {
            if (sheet != null)
            {
                ExcelHelper.ExcelHelper.AddInfoColumn(sheet, infos, title);
            }
        }

        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellStyle"></param>
        /// <returns></returns>
        public static ICell SetCellStyle(this ICell cell, ICellStyle cellStyle)
        {
            cell.CellStyle = cellStyle;
            return cell;
        }
    }
}
