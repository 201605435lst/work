using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using NPOI.XSSF.UserModel.Helpers;
using NPOI.XSSF.Streaming;

namespace SnAbp.Utils.ExcelHelper
{
    /// <summary>
    /// Excel基础操作类
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// Excel数据导出DataTable
        /// </summary>
        public static DataTable ExcelToDataTable(Stream fs, string fileName, int sheetIndex = 0)
        {
            DataTable dataTable = null;
            IWorkbook workbook = null;
            try
            {
                // 2007版本
                if (fileName.IndexOf(".xlsx") > 0)
                    workbook = new XSSFWorkbook(fs);
                // 2003版本
                else if (fileName.IndexOf(".xls") > 0)
                    workbook = new HSSFWorkbook(fs);
                else
                {
                    return null;
                }

                if (workbook != null)
                {
                    var sheet = workbook.GetSheetAt(sheetIndex);//读取第一个sheet，当然也可以循环读取每个sheet
                    dataTable = GetSheetDataTable(sheet);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                throw ex;
            }
            finally
            {
                workbook.Close();
            }
        }

        /// <summary>
        /// Excel数据导出DataSet
        /// </summary>
        public static DataSet ImportBaseDataToDataSet(Stream fs, string fileName)
        {
            var ds = new DataSet();
            DataTable dataTable = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            try
            {
                // 2007版本
                if (fileName.IndexOf(".xlsx") > 0)
                    workbook = new XSSFWorkbook(fs);
                // 2003版本
                else if (fileName.IndexOf(".xls") > 0)
                    workbook = new HSSFWorkbook(fs);
                else
                {
                    return null;
                }
                var startRow = -1;
                var celVal = "";
                var isFindCol = false;
                if (workbook != null)
                {
                    for (var v = 0; v < workbook.NumberOfSheets; v++)
                    {
                        startRow = -1;
                        isFindCol = false;
                        sheet = workbook.GetSheetAt(v);//读取第一个sheet，当然也可以循环读取每个sheet
                        if (sheet == null) continue;
                        dataTable = new DataTable(sheet.SheetName);

                        var rowCount = sheet.LastRowNum;//总行数
                        if (rowCount == 0) continue;

                        #region 询找标记列
                        for (int i = 0; i <= rowCount; ++i)
                        {
                            row = sheet.GetRow(i);
                            if (row == null) continue;
                            for (int u = row.FirstCellNum; u < row.LastCellNum; ++u)
                            {
                                cell = row.GetCell(u);
                                if (cell != null)
                                {
                                    switch (cell.CellType)
                                    {
                                        case CellType.String:
                                            {
                                                celVal = cell.StringCellValue;
                                                break;
                                            }
                                        case CellType.Numeric:
                                            {
                                                celVal = cell.NumericCellValue.ToString();
                                                break;
                                            }
                                        case CellType.Blank:
                                            {
                                                celVal = "";
                                                break;
                                            }
                                        default:
                                            {
                                                throw new Exception("导入的内容数据格式错误");
                                            }
                                    }

                                    if (string.IsNullOrWhiteSpace(celVal)) continue;
                                    if (celVal.Contains('[') && celVal.Contains(']'))
                                    {
                                        var arr = celVal.Replace("]", "[").Split('[');
                                        if (arr.Length == 3)
                                        {
                                            isFindCol = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (isFindCol)
                            {
                                startRow = i;
                                break;
                            }
                        }

                        if (startRow < 0) continue;
                        #endregion

                        #region 按标记列编码构造DataTable列名
                        if (startRow >= 0)
                        {
                            row = sheet.GetRow(startRow);
                            var rowIndex = (int)0;
                            for (int u = row.FirstCellNum; u < row.LastCellNum; ++u)
                            {
                                cell = row.GetCell(u);
                                if (cell != null)
                                {
                                    switch (cell.CellType)
                                    {
                                        case CellType.String:
                                            {
                                                celVal = cell.StringCellValue;
                                                break;
                                            }
                                        case CellType.Numeric:
                                            {
                                                celVal = cell.NumericCellValue.ToString();
                                                break;
                                            }
                                        case CellType.Blank:
                                            {
                                                celVal = "";
                                                break;
                                            }
                                        default:
                                            {
                                                throw new Exception("导入的内容数据格式错误");
                                            }
                                    }
                                    if (string.IsNullOrWhiteSpace(celVal))
                                    {
                                        rowIndex++;
                                        column = new DataColumn(rowIndex.ToString());
                                        dataTable.Columns.Add(column);
                                    }
                                    else if (celVal.Contains('[') && celVal.Contains(']'))
                                    {
                                        var arr = celVal.Replace("]", "[").Split('[');
                                        if (arr.Length != 3)
                                        {
                                            throw new Exception("导入的数据标记名错误");
                                        }
                                        column = new DataColumn(arr[1]);
                                        dataTable.Columns.Add(column);
                                    }
                                }
                            }
                            startRow++;

                        }
                        else
                        {
                            throw new Exception("未找到标记列");
                        }
                        #endregion

                        #region 填充数据行
                        //填充行
                        for (int i = startRow; i <= rowCount; ++i)
                        {
                            row = sheet.GetRow(i);
                            if (row == null) continue;

                            dataRow = dataTable.NewRow();
                            for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
                            {
                                cell = row.GetCell(j);
                                if (cell != null)
                                {
                                    switch (cell.CellType)
                                    {
                                        case CellType.Blank:
                                            dataRow[j] = "";
                                            break;
                                        case CellType.Numeric:
                                            short format = cell.CellStyle.DataFormat;
                                            //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
                                            if (format == 14 || format == 31 || format == 57 || format == 58)
                                                dataRow[j] = DateTime.FromOADate(cell.NumericCellValue);
                                            else
                                                dataRow[j] = cell.NumericCellValue;
                                            if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
                                                dataRow[j] = cell.NumericCellValue.ToString("#0.00");
                                            break;
                                        case CellType.String:
                                            dataRow[j] = cell.StringCellValue;
                                            break;
                                        case CellType.Formula:
                                            try
                                            {
                                                dataRow[j] = cell.NumericCellValue;
                                                if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 88)
                                                    dataRow[j] = cell.NumericCellValue.ToString("#0.00");
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    dataRow[j] = cell.StringCellValue;
                                                }
                                                catch { }
                                            }
                                            break;
                                        default:
                                            dataRow[j] = cell.StringCellValue;
                                            break;
                                    }
                                }
                            }
                            dataTable.Rows.Add(dataRow);
                        }
                        #endregion

                        ds.Tables.Add(dataTable);
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close();
                }
            }
            return ds;
        }

        /// <summary>
        /// Excel数据导出DataTable
        /// </summary>
        public static DataTable ImportBaseDataToDataTable(Stream fs, string fileName, out IWorkbook workbook, int sheetIndex = 0)
        {
            DataTable dataTable = null;
            DataColumn column = null;
            DataRow dataRow = null;
            //IWorkbook workbook = null;
            workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            try
            {
                // 2007版本
                if (fileName.IndexOf(".xlsx") > 0)
                    workbook = new XSSFWorkbook(fs);
                // 2003版本
                else if (fileName.IndexOf(".xls") > 0)
                    workbook = new HSSFWorkbook(fs);
                else
                {
                    return null;
                }
                var startRow = -1;
                var celVal = "";
                var isFindCol = false;
                if (workbook != null)
                {
                    sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet
                    dataTable = new DataTable();
                    if (sheet != null)
                    {
                        var rowCount = sheet.LastRowNum;//总行数
                        if (rowCount > 0)
                        {
                            #region 询找标记列
                            for (int i = startRow; i <= rowCount; ++i)
                            {
                                row = sheet.GetRow(i);
                                if (row == null) continue;
                                for (int u = row.FirstCellNum; u < row.LastCellNum; ++u)
                                {
                                    cell = row.GetCell(u);
                                    if (cell != null)
                                    {
                                        switch (cell.CellType)
                                        {
                                            case CellType.String:
                                                {
                                                    celVal = cell.StringCellValue;
                                                    break;
                                                }
                                            case CellType.Numeric:
                                                {
                                                    celVal = cell.NumericCellValue.ToString();
                                                    break;
                                                }
                                            case CellType.Blank:
                                                {
                                                    celVal = "";
                                                    break;
                                                }
                                            default:
                                                {
                                                    throw new Exception("导入的内容数据格式错误");
                                                }
                                        }

                                        if (string.IsNullOrWhiteSpace(celVal)) continue;
                                        if (celVal.Contains('[') && celVal.Contains(']'))
                                        {
                                            var arr = celVal.Replace("]", "[").Split('[');
                                            if (arr.Length == 3)
                                            {
                                                isFindCol = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (isFindCol)
                                {
                                    startRow = i;
                                    break;
                                }
                            }
                            #endregion

                            #region 按标记列编码构造DataTable列名
                            if (startRow >= 0)
                            {
                                row = sheet.GetRow(startRow);
                                var rowIndex = (int)0;
                                for (int u = row.FirstCellNum; u < row.LastCellNum; ++u)
                                {
                                    cell = row.GetCell(u);
                                    if (cell != null)
                                    {
                                        switch (cell.CellType)
                                        {
                                            case CellType.String:
                                                {
                                                    celVal = cell.StringCellValue;
                                                    break;
                                                }
                                            case CellType.Numeric:
                                                {
                                                    celVal = cell.NumericCellValue.ToString();
                                                    break;
                                                }
                                            case CellType.Blank:
                                                {
                                                    celVal = "";
                                                    break;
                                                }
                                            default:
                                                {
                                                    throw new Exception("导入的内容数据格式错误");
                                                }
                                        }
                                        if (string.IsNullOrWhiteSpace(celVal))
                                        {
                                            rowIndex++;
                                            column = new DataColumn(rowIndex.ToString());
                                            dataTable.Columns.Add(column);
                                        }
                                        else if (celVal.Contains('[') && celVal.Contains(']'))
                                        {
                                            var arr = celVal.Replace("]", "[").Split('[');
                                            if (arr.Length != 3)
                                            {
                                                throw new Exception("导入的数据标记名错误");
                                            }
                                            column = new DataColumn(arr[1]);
                                            dataTable.Columns.Add(column);
                                        }
                                    }
                                }
                                startRow++;

                            }
                            else
                            {
                                throw new Exception("未找到标记列");
                            }
                            #endregion

                            #region 填充数据行
                            //填充行
                            for (int i = startRow; i <= rowCount; ++i)
                            {
                                row = sheet.GetRow(i);
                                if (row == null) continue;

                                dataRow = dataTable.NewRow();
                                for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
                                {
                                    cell = row.GetCell(j);
                                    switch (cell.CellType)
                                    {
                                        case CellType.Blank:
                                            dataRow[j] = "";
                                            break;
                                        case CellType.Numeric:
                                            short format = cell.CellStyle.DataFormat;
                                            //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
                                            if (format == 14 || format == 31 || format == 57 || format == 58)
                                                dataRow[j] = DateTime.FromOADate(cell.NumericCellValue); //cell.DateCellValue;第二次用到未引用错误。换成DateTime.FromOADate(double b)
                                            else
                                                dataRow[j] = cell.NumericCellValue;
                                            if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
                                                dataRow[j] = cell.NumericCellValue.ToString("#0.00");
                                            break;
                                        case CellType.String:
                                            dataRow[j] = cell.StringCellValue;
                                            break;
                                        case CellType.Formula:
                                            try
                                            {
                                                dataRow[j] = cell.NumericCellValue;
                                                if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 88)
                                                    dataRow[j] = cell.NumericCellValue.ToString("#0.00");
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    dataRow[j] = cell.StringCellValue;
                                                }
                                                catch { }
                                            }
                                            break;
                                        default:
                                            dataRow[j] = cell.StringCellValue;
                                            break;
                                    }
                                }
                                dataTable.Rows.Add(dataRow);
                            }
                        }
                    }
                    #endregion
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close();
                }
            }
            return dataTable;
        }

        /// <summary>
        /// excel文件添加错误信息并返回
        /// </summary>
        /// <param name="workbook">WorkBook</param>
        /// <param name="wrongInfos">错误信息</param>
        /// <param name="resMsg">导入结果信息</param>
        public static byte[] AddWrongInfos(IWorkbook workbook, List<WrongInfo> wrongInfos, string resMsg = null)
        {
            byte[] res = null;
            if (workbook == null) return null;
            var sheet = workbook.GetSheetAt(0);
            int startRow = -1;
            var celVal = "";
            bool isFindCol = false;

            if (sheet != null)
            {
                var cellCount = sheet.GetRow(sheet.LastRowNum - 1).LastCellNum;
                InsertRow(sheet, 0, 1, sheet.GetRow(sheet.LastRowNum - 1));
                var newRow = sheet.GetRow(0);
                ICellStyle style = workbook.CreateCellStyle();
                style.WrapText = true;
                style.VerticalAlignment = VerticalAlignment.Top;
                newRow.CreateCell(0).SetCellValue(resMsg);
                newRow.GetCell(0).CellStyle = style;
                CellRangeAddress cellRangeAddress = new CellRangeAddress(0, 0, 0, cellCount);
                sheet.AddMergedRegion(cellRangeAddress);

                var rowCount = sheet.LastRowNum;//总行数
                if (rowCount > 0)
                {
                    #region 询找标记列
                    for (int i = 0; i <= rowCount; ++i)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null) continue;
                        for (int u = row.FirstCellNum; u < row.LastCellNum; ++u)
                        {
                            var cell = row.GetCell(u);
                            if (cell != null)
                            {
                                switch (cell.CellType)
                                {
                                    case CellType.String:
                                        {
                                            celVal = cell.StringCellValue;
                                            break;
                                        }
                                    case CellType.Numeric:
                                        {
                                            celVal = cell.NumericCellValue.ToString();
                                            break;
                                        }
                                    case CellType.Blank:
                                        {
                                            celVal = "";
                                            break;
                                        }
                                    default:
                                        {
                                            throw new Exception("导入的内容数据格式错误");
                                        }
                                }

                                if (string.IsNullOrWhiteSpace(celVal)) continue;
                                if (celVal.Contains('[') && celVal.Contains(']'))
                                {
                                    var arr = celVal.Replace("]", "[").Split('[');
                                    if (arr.Length == 3)
                                    {
                                        isFindCol = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (isFindCol)
                        {
                            startRow = i;
                            break;
                        }
                    }
                    #endregion

                    if (startRow > -1)
                    {
                        ICellStyle cellStyle = null;
                        bool isGetStyle = false;

                        foreach (var item in wrongInfos)
                        {
                            var row = sheet.GetRow(startRow + item.RowIndex + 1);
                            var cell = row.CreateCell(row.LastCellNum);
                            cell.SetCellValue(item.WrongMsg);
                            for (int i = 0; i < row.LastCellNum; i++)
                            {
                                var theCell = row.GetCell(i);
                                if (isGetStyle == false)
                                {
                                    cellStyle = GetCopyCellStyle(workbook, theCell.CellStyle);
                                    cellStyle.FillPattern = FillPattern.SolidForeground;
                                    cellStyle.FillForegroundColor = HSSFColor.Red.Index;
                                    isGetStyle = true;
                                }
                                theCell.CellStyle = cellStyle;
                            }
                        }
                    }
                }
            }
            return res;
        }


        /// <summary>
        /// DataTable导出到Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strFileName"></param>
        /// <param name="hideCol"></param>
        /// <param name="tableTitle">表头</param>
        /// <returns></returns>
        public static byte[] DataTableToExcel(DataTable dt, string strFileName, List<int> hideCol = null, string tableTitle = null, bool bottom = false, object bootmData = null)
        {
            IWorkbook workbook = null;
            ICell cell = null;
            try
            {
                // 2007版本
                if (strFileName.IndexOf(".xlsx") > 0)
                    workbook = new XSSFWorkbook();
                // 2003版本
                else if (strFileName.IndexOf(".xls") > 0)
                    workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();

                //隐藏列
                if (hideCol?.Count > 0)
                {
                    foreach (var item in hideCol)
                    {
                        sheet.SetColumnHidden(item, true);
                    }
                }

                var tableTitlefont = workbook.CreateFont();
                //设置标题
                if (tableTitle != null)
                {
                    var tableTitleRow = sheet.CreateRow(0);
                    var titleCell = tableTitleRow.CreateCell(0);
                    titleCell.SetCellValue(tableTitle);
                    var style = workbook.CreateCellStyle();
                    style.Alignment = HorizontalAlignment.Center;
                    tableTitlefont.FontHeight = 20 * 20;
                    tableTitlefont.IsBold = true;
                    style.SetFont(tableTitlefont);
                    titleCell.CellStyle = style;
                    var region = new CellRangeAddress(0, 0, 0, dt.Rows[0].ItemArray.Length - 1);
                    sheet.AddMergedRegion(region);
                    SetBorder(region, sheet);
                }

                ICellStyle titleStyle = workbook.CreateCellStyle();
                IFont titleFont = workbook.CreateFont();
                titleFont.FontHeightInPoints = 11;//设置字体高度
                titleFont.FontName = "宋体";//设置字体
                titleFont.IsBold = true;
                titleStyle.BorderBottom = BorderStyle.Thin;
                titleStyle.BorderLeft = BorderStyle.Thin;
                titleStyle.BorderRight = BorderStyle.Thin;
                titleStyle.BorderTop = BorderStyle.Thin;
                titleStyle.WrapText = true;
                titleStyle.Alignment = HorizontalAlignment.Center;
                titleStyle.VerticalAlignment = VerticalAlignment.Center;
                titleStyle.SetFont(titleFont);
                //填充表头
                var dataRow = sheet.CreateRow(1);
                foreach (DataColumn column in dt.Columns)
                {
                    cell = dataRow.CreateCell(column.Ordinal);
                    cell.CellStyle = titleStyle;
                    cell.SetCellValue(column.ColumnName);
                }

                titleStyle = workbook.CreateCellStyle();
                titleStyle.BorderBottom = BorderStyle.Thin;
                titleStyle.BorderLeft = BorderStyle.Thin;
                titleStyle.BorderRight = BorderStyle.Thin;
                titleStyle.BorderTop = BorderStyle.Thin;
                //titleStyle.Alignment = HorizontalAlignment.Center;
                titleStyle.VerticalAlignment = VerticalAlignment.Center;
                //填充内容
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    dataRow = sheet.CreateRow(i + 2);
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = dataRow.CreateCell(j);
                        cell.CellStyle = titleStyle;
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }

                //表格底部数据(天窗计划维修作业表)
                if (bottom)
                {
                    ICellStyle bottomStyle = workbook.CreateCellStyle();
                    bottomStyle.VerticalAlignment = VerticalAlignment.Center;
                    var bottomFont = workbook.CreateFont();
                    bottomFont.FontHeightInPoints = 10;//设置字体高度
                    bottomFont.FontName = "宋体";//设置字体
                    bottomStyle.SetFont(bottomFont);
                    //var a = @bootmData.CreateMember;  
                    dynamic aa = "";
                    SetBottom(sheet, dt, bottomStyle, 2, $"编制人:{bootmData.GetObjectMember("CreateMember")}");
                    CellRangeAddress region;
                    region = new CellRangeAddress(dt.Rows.Count + 2, dt.Rows.Count + 3, 0, (dt.Rows[0].ItemArray.Length - 1));
                    sheet.AddMergedRegion(region);

                    var row = SetBottom(sheet, dt, bottomStyle, 4, $"技术科（高铁办）负责人:{bootmData.GetObjectMember("TechnicalMember")}");
                    region = new CellRangeAddress(dt.Rows.Count + 4, dt.Rows.Count + 5, 0, (dt.Rows[0].ItemArray.Length - 1) / 2);
                    sheet.AddMergedRegion(region);

                    var bottomCell = row.CreateCell((dt.Rows[0].ItemArray.Length - 1) / 2 + 1);
                    bottomCell.SetCellValue($"安全调度科负责人:{bootmData.GetObjectMember("SafeMember")}");
                    bottomCell.CellStyle = bottomStyle;
                    region = new CellRangeAddress(dt.Rows.Count + 4, dt.Rows.Count + 5, ((dt.Rows[0].ItemArray.Length - 1) / 2) + 1, (dt.Rows[0].ItemArray.Length - 1));
                    sheet.AddMergedRegion(region);

                    SetBottom(sheet, dt, bottomStyle, 6, $"主管领导:{bootmData.GetObjectMember("ChiefMember")}");
                    region = new CellRangeAddress(dt.Rows.Count + 6, dt.Rows.Count + 7, 0, (dt.Rows[0].ItemArray.Length - 1));
                    sheet.AddMergedRegion(region);

                    var row2 = SetBottom(sheet, dt, bottomStyle, 8, "西安通信段");
                    region = new CellRangeAddress(dt.Rows.Count + 8, dt.Rows.Count + 9, 0, (dt.Rows[0].ItemArray.Length - 1) / 2);
                    sheet.AddMergedRegion(region);

                    //var bottomCell2 = row2.CreateCell((dt.Rows[0].ItemArray.Length - 1) / 2 + 1);
                    //bottomCell2.SetCellValue("负责人:");
                    //bottomCell2.CellStyle = bottomStyle;
                    //region = new CellRangeAddress(dt.Rows.Count + 8, dt.Rows.Count + 9, ((dt.Rows[0].ItemArray.Length - 1) / 2) + 1, ((dt.Rows[0].ItemArray.Length - 1) / 2) + 4);
                    //sheet.AddMergedRegion(region);

                    //var bottomCell3 = row2.CreateCell((dt.Rows[0].ItemArray.Length - 1) / 2 + 5);
                    //bottomCell3.SetCellValue("联系电话:");
                    //bottomCell3.CellStyle = bottomStyle;
                    //region = new CellRangeAddress(dt.Rows.Count + 8, dt.Rows.Count + 9, ((dt.Rows[0].ItemArray.Length - 1) / 2) + 5, dt.Rows[0].ItemArray.Length - 1);
                    //sheet.AddMergedRegion(region);
                }
                //转为字节数组  
                MemoryStream stream = new MemoryStream();
                workbook.Write(stream);
                var buf = stream.ToArray();
                return buf;

            }
            catch (Exception ex)
            {
            }
            finally
            {
                workbook.Close();
                //bookStream.Close();
            }
            return null;
        }

        private static string GetObjectMember(this object obj, string key) => obj.GetType().GetProperty(key).GetValue(obj).ToString();

        private static IRow SetBottom(ISheet sheet, DataTable dt, ICellStyle style, int row, string name)
        {
            var tablBottomRow1 = sheet.CreateRow(dt.Rows.Count + row);
            var botttomCell = tablBottomRow1.CreateCell(0);
            botttomCell.SetCellValue(name);
            botttomCell.CellStyle = style;
            return tablBottomRow1;
        }

        private static void SetBorder(CellRangeAddress region, ISheet sheet)
        {

            RegionUtil.SetBorderLeft(1, region, sheet);//左
            RegionUtil.SetBorderRight(1, region, sheet);//右
            RegionUtil.SetBorderTop(1, region, sheet);//顶
            RegionUtil.SetBorderBottom(1, region, sheet);//底
        }


        private static DataTable GetSheetDataTable(ISheet sheet)
        {
            DataTable dt = new DataTable();
            string sheetName = sheet.SheetName;
            int startIndex = 0;// sheet.FirstRowNum;
            int lastIndex = sheet.LastRowNum;
            //最大列数
            int cellCount = 0;
            IRow maxRow = sheet.GetRow(0);
            for (int i = startIndex; i <= lastIndex; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null && cellCount < row.LastCellNum)
                {
                    cellCount = row.LastCellNum;
                    maxRow = row;
                }
            }
            //列名设置
            try
            {
                for (int i = 0; i < maxRow.LastCellNum; i++)//maxRow.FirstCellNum
                {
                    dt.Columns.Add(Convert.ToChar(((int)'A') + i).ToString());
                    //DataColumn column = new DataColumn("Column" + (i + 1).ToString());
                    //dt.Columns.Add(column);
                }
            }
            catch
            {
                throw new Exception("工作表" + sheetName + "中无数据");
            }
            //数据填充
            for (int i = startIndex; i <= lastIndex; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow drNew = dt.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
                    {
                        if (row.GetCell(j) != null)
                        {
                            ICell cell = row.GetCell(j);
                            switch (cell.CellType)
                            {
                                case CellType.Blank:
                                    drNew[j] = "";
                                    break;
                                case CellType.Numeric:
                                    short format = cell.CellStyle.DataFormat;
                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                        drNew[j] = cell.DateCellValue;
                                    else
                                        drNew[j] = cell.NumericCellValue;
                                    if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
                                        drNew[j] = cell.NumericCellValue.ToString("#0.00");
                                    break;
                                case CellType.String:
                                    drNew[j] = cell.StringCellValue;
                                    break;
                                case CellType.Formula:
                                    try
                                    {
                                        drNew[j] = cell.NumericCellValue;
                                        if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 88)
                                            drNew[j] = cell.NumericCellValue.ToString("#0.00");
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            drNew[j] = cell.StringCellValue;
                                        }
                                        catch { }
                                    }
                                    break;
                                default:
                                    drNew[j] = cell.StringCellValue;
                                    break;
                            }
                        }
                    }
                }
                dt.Rows.Add(drNew);
            }
            return dt;
        }


        /// <summary>
        /// 复制单元格样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="cs"></param>
        /// <returns></returns>
        public static ICellStyle GetCopyCellStyle(IWorkbook workbook, ICellStyle cs)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = cs.Alignment;
            cellStyle.BorderBottom = cs.BorderBottom;
            cellStyle.BorderDiagonal = cs.BorderDiagonal;
            cellStyle.BorderDiagonalColor = cs.BorderDiagonalColor;
            cellStyle.BorderDiagonalLineStyle = cs.BorderDiagonalLineStyle;
            cellStyle.BorderLeft = cs.BorderLeft;
            cellStyle.BorderRight = cs.BorderRight;
            cellStyle.BorderTop = cs.BorderTop;
            cellStyle.BottomBorderColor = cs.BottomBorderColor;
            cellStyle.DataFormat = cs.DataFormat;
            //cellStyle.FillBackgroundColor = cs.FillBackgroundColor;
            //cellStyle.SetFont(cs.GetFont(workbook));
            //cellStyle.FontIndex = ss.FontIndex;
            cellStyle.Indention = cs.Indention;
            //cellStyle.Index = ss.Index;
            cellStyle.IsHidden = cs.IsHidden;
            cellStyle.IsLocked = cs.IsLocked;
            cellStyle.LeftBorderColor = cs.LeftBorderColor;
            cellStyle.RightBorderColor = cs.RightBorderColor;
            cellStyle.Rotation = cs.Rotation;
            cellStyle.ShrinkToFit = cs.ShrinkToFit;
            cellStyle.TopBorderColor = cs.TopBorderColor;
            cellStyle.VerticalAlignment = cs.VerticalAlignment;
            cellStyle.WrapText = cs.WrapText;
            //cellStyle.FillPattern = FillPattern.SolidForeground;
            //cellStyle.FillForegroundColor = HSSFColor.Red.Index;
            return cellStyle;
        }

        /// <summary>
        /// 插入新的行
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="insertTargetRowIndex"></param>
        /// <param name="insertCount"></param>
        /// <param name="sourceStyleRow"></param>
        private static void InsertRow(ISheet sheet, int insertTargetRowIndex, int insertCount, IRow sourceStyleRow)
        {
            #region 批量移动行
            sheet.ShiftRows(insertTargetRowIndex, sheet.LastRowNum, insertCount);
            #endregion

            #region 对批量移动后空出的空行插，创建相应的行，并以插入行的上一行为格式源(即：插入行-1的那一行)
            for (int i = insertTargetRowIndex; i < insertTargetRowIndex + insertCount - 1; i++)
            {

                var targetRow = sheet.CreateRow(i + 1);

                for (int m = sourceStyleRow.FirstCellNum; m < sourceStyleRow.LastCellNum; m++)
                {
                    var sourceCell = sourceStyleRow.GetCell(m);
                    if (sourceCell == null)
                        continue;
                    var targetCell = targetRow.CreateCell(m);

                    targetCell.CellStyle = sourceCell.CellStyle;
                    targetCell.SetCellType(sourceCell.CellType);

                }
                //CopyRow(sourceRow, targetRow);

                //Util.CopyRow(sheet, sourceRow, targetRow);
            }

            var firstTargetRow = sheet.GetRow(insertTargetRowIndex);

            for (int m = sourceStyleRow.FirstCellNum; m < sourceStyleRow.LastCellNum; m++)
            {
                var firstSourceCell = sourceStyleRow.GetCell(m);
                if (firstSourceCell == null)
                    continue;
                var firstTargetCell = firstTargetRow.CreateCell(m);

                firstTargetCell.CellStyle = firstSourceCell.CellStyle;
                firstTargetCell.SetCellType(firstSourceCell.CellType);
            }
            #endregion
        }

        /// <summary>
        /// 获取工作表空间
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static IWorkbook GetWorkbookByStream(Stream stream, string fileName)
        {
            // 判断是不是文件
            if (fileName.EndsWith(".xlsx"))
            {
                return new XSSFWorkbook(stream);
            }
            else if (fileName.EndsWith(".xls"))
            {
                return new HSSFWorkbook(stream);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 工作表转List
        /// </summary>
        /// <typeparam name="T">需要转换的泛型类</typeparam>
        /// <param name="sheet">工作表</param>
        /// <param name="rowStartIndex">起始索引</param>
        /// <param name="fileType">文件类型</param>
        /// <returns></returns>
        public static List<T> SheetToList<T>(ISheet sheet, int rowStartIndex = 1) where T : class, new()
        {
            var list = new List<T>();
            var workType = sheet.GetWorkbookType();
            var rows = sheet.GetRowEnumerator();
            if (rows == null) return null;
            IRow row = null;
            IRow firstRow = null;
            while (rows.MoveNext())
            {
                switch (workType)
                {
                    case ExcelFileType.Xlsx:
                        row = (XSSFRow)rows.Current;
                        break;
                    case ExcelFileType.Xls:
                        row = (HSSFRow)rows.Current;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (row == null) continue;
                if (row.RowNum == 0)
                {
                    firstRow = row;
                    continue;
                }
                if (row.RowNum < rowStartIndex)
                {
                    continue;
                }
                var t = Activator.CreateInstance(typeof(T)) as T;
                var props = t.GetType().GetProperties().ToArray();
                // 逻辑修改，根据实体的属性顺序读取表格行中的内容

                var cells = row.Cells;// 获取所有的列

                // 获取是否有扩展的内容
                var properties = props.FirstOrDefault(a => a.Name == "Properties");

                if (properties != null)
                {
                    // 存在扩展属性
                    var propertyCells = cells.Skip(props.Length - 1).ToList();
                    if (propertyCells != null && propertyCells.Any())
                    {
                        var dic = new Dictionary<string, string>();
                        propertyCells.ForEach(cell =>
                        {
                            // 获取第一行的数据
                            var cindex = propertyCells.FindIndex(cell);
                            var key = firstRow.GetCell(cell.ColumnIndex).ToString(); // 获取表头的名称
                            var value = cell.ToString();
                            dic.Add(key, value);
                        });
                        properties.SetValue(t, dic);
                    }
                }

                var length = properties != null ? props.Length - 1 : props.Length;
                for (int i = 0; i < length; i++)
                {
                    var cell = cells[i];
                    if (cell == null || string.IsNullOrEmpty(cell.ToString())) continue;
                    var type = props[i].PropertyType;
                    var name = props[i].Name;
                    //if (name == "Properties") continue;
                    var value = GetCellValue(type, cell.ToString());
                    if (name == "Index")
                    {
                        value = rowStartIndex++;
                    }
                    props[i].SetValue(t, value);
                }

                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// 给指定工作表添加信息列，默认信息列背景色为黄
        /// </summary>
        /// <param name="sheet">信息表</param>
        /// <param name="infos">错误信息</param>
        /// <param name="colTitle">列标题</param>
        /// <param name="titleRowIndex">列标题行起始索引</param>
        /// <returns></returns>
        public static void AddInfoColumn(
            ISheet sheet,
            List<WrongInfo> infos,
            string colTitle,
            int titleRowIndex = 0
            )
        {
            if (sheet == null) return;
            try
            {
                ICellStyle cellStyle = sheet.Workbook.CreateCellStyle();
                bool isAddTitle = false;
                var font = sheet.Workbook.CreateFont();
                font.Color = HSSFColor.Red.Index;
                cellStyle.WrapText = true;
                cellStyle.FillForegroundColor = HSSFColor.Orange.Index;
                cellStyle.SetFont(font);
                // 根据错误信息中行索引进行添加数据
                foreach (var item in infos)
                {
                    var row = sheet.GetRow(item.RowIndex);
                    //根据数据行的列位置创建 异常信息列 列头
                    if (!isAddTitle)
                    {
                        isAddTitle = true;
                        var titleRow = sheet.GetRow(titleRowIndex);
                        var titleCell = titleRow.CreateCell(row.LastCellNum);
                        titleCell.SetCellValue(colTitle);
                        sheet.SetColumnWidth(row.LastCellNum, 30 * 256);
                    }
                    var cell = row.CreateCell(row.LastCellNum);
                    cell.SetCellValue(item.WrongMsg);
                    cell.CellStyle = cellStyle;
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        private static ICellStyle BuildCellStyle(this ICellStyle cellStyle, WrongInfoType type)
        {
            cellStyle.FillPattern = FillPattern.SolidForeground;
            switch (type)
            {
                case WrongInfoType.Error:
                    cellStyle.FillForegroundColor = HSSFColor.Yellow.Index;
                    break;
                case WrongInfoType.Warning:
                    cellStyle.FillForegroundColor = HSSFColor.Orange.Index;
                    break;
                case WrongInfoType.Success:
                    cellStyle.FillForegroundColor = HSSFColor.Green.Index;
                    break;
                default:
                    cellStyle.FillForegroundColor = HSSFColor.Yellow.Index;
                    break;
            }
            return cellStyle;
        }


        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="row"></param>
        private static void SetCellStyle(
            IRow row)
        {
            if (row == null) return;
            for (int i = row.FirstCellNum; i < row.LastCellNum; i++)
            {
                //row.GetCell(i).CellStyle.FillBackgroundColor = 8;
                row.GetCell(i).CellStyle.FillForegroundColor = 10; // 设置前景色
                row.GetCell(i).CellStyle.FillPattern = FillPattern.SolidForeground;
            }
        }
        /// <summary>
        /// 获取单元格的值
        /// </summary>
        /// <param name="valueType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object GetCellValue(Type valueType, string value)
        {
            if (valueType == typeof(int))
            {
                return int.Parse(value);
            }
            else if (valueType == typeof(float))
            {
                return float.Parse(value);
            }
            else if (valueType == typeof(DateTime))
            {
                return DateTime.Parse(value);
            }
            else if (valueType == typeof(decimal))
            {
                return decimal.Parse(value);
            }
            else
            {
                return value;
            }
        }

        public static WrongInfo SetError(this WrongInfo wrongInfo)
        {
            wrongInfo.Type = WrongInfoType.Error;
            return wrongInfo;
        }
        public static WrongInfo SetWarn(this WrongInfo wrongInfo)
        {
            wrongInfo.Type = WrongInfoType.Warning;
            return wrongInfo;
        }
        public static WrongInfo SetSuccess(this WrongInfo wrongInfo)
        {
            wrongInfo.Type = WrongInfoType.Success;
            return wrongInfo;
        }

        /// <summary>
        /// 导出excel 文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="templateName"></param>
        /// <param name="rowIndex"></param>
        /// <param name="imageConfig"></param>
        /// <returns></returns>
        public static Stream ExcelExportStream<T>(List<T> dataList, string templateName, int rowIndex = 0, FileImageConfig imageConfig = null)
        {
            // 判断实体T 的结构与模板的结构是否相同
            //获取文件模板
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(),
                path2: $"import_templates\\import_templates_{templateName}.xls");
            MemoryStream fileMemoryStream;
            if (File.Exists(templatePath))
            {
                var fileStream = File.OpenRead(templatePath);
                var sheet = ExcelHelper.GetWorkbookByStream(
                             fileStream,
                        $"import_templates_{templateName}.xls")
                    .GetSheetAt(0)                             // 获取工作表
                    .CheckColumnAccordTempleModel<T>(rowIndex) // 校验工作表
                    .WriteData(dataList, rowIndex);            // 写入数据
                if (imageConfig != null)
                {
                    // 添加图片
                    var drawing = sheet.CreateDrawingPatriarch();
                    var pic = sheet.Workbook.AddPicture(imageConfig.ImageBytes, PictureType.PNG);
                    var anchor = new HSSFClientAnchor();
                    anchor.SetAnchor(
                        imageConfig.Col1,
                        imageConfig.Row1,
                        imageConfig.X1,
                        imageConfig.Y1,
                        imageConfig.Col2,
                        imageConfig.Row2,
                        imageConfig.X2,
                        imageConfig.Y2);
                    anchor.AnchorType = AnchorType.MoveAndResize;
                    drawing.CreatePicture(anchor, pic);
                }
                // 写入数据
                fileMemoryStream = sheet.Workbook
                    .ConvertToBytes()
                    .BytesToStream();
            }
            else
            {
                throw new ArgumentException($"import_templates_{templateName}.xls文件模板不存在");
            }
            return fileMemoryStream;
        }


    }

    /// <summary>
    /// 错误信息模板
    /// </summary>
    public class WrongInfo
    {
        public WrongInfo()
        { }
        public WrongInfo(int rowIndex, string msg = null)
        {
            RowIndex = rowIndex;
            WrongMsg = msg;
        }
        /// <summary>
        /// 数据行号（去除表头后，此数据的行号）
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string WrongMsg { get; set; }

        /// <summary>
        /// 异常类型
        /// </summary>
        public WrongInfoType Type { get; set; }

    }

    public enum WrongInfoType
    {
        Error,
        Warning,
        Success
    }

    public class FileImageConfig
    {
        public byte[] ImageBytes { get; set; }
        public short Col1 { get; set; }
        public int Row1 { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public short Col2 { get; set; }
        public int Row2 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
    }
}
