using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SnAbp.CrPlan.Dto.WorkOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Volo.Abp;

namespace SnAbp.CrPlan.Commons
{
    /// <summary>
    /// Excel操作类
    /// </summary>
    public class ExcelHepler
    {

        #region Excel文件导出DataTable===========================================
        /// <summary>
        /// Excel数据导出DataTable
        /// </summary>
        public static DataTable ExcelToDataTable(Stream fs, string fileName, int startRow = 0, string sheetName = null)
        {
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

                if (workbook != null)
                {
                    sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet
                    dataTable = new DataTable();
                    if (sheet != null)
                    {
                        var rowCount = sheet.LastRowNum;//总行数
                        if (rowCount > 0)
                        {
                            IRow firstRow = sheet.GetRow(0);//第一行
                            int cellCount = firstRow.LastCellNum;//列数

                            for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                            {
                                cell = firstRow.GetCell(i);
                                if (cell != null)
                                {
                                    switch (cell.CellType)
                                    {
                                        case CellType.String:
                                            {
                                                column = new DataColumn(cell.StringCellValue);
                                                dataTable.Columns.Add(column);
                                                break;
                                            }
                                        case CellType.Numeric:
                                            {
                                                column = new DataColumn(cell.NumericCellValue.ToString());
                                                dataTable.Columns.Add(column);
                                                break;
                                            }
                                        default:
                                            {
                                                throw new UserFriendlyException("导入的内容数据格式错误");
                                            }
                                    }
                                }
                            }


                            //填充行
                            for (int i = startRow; i <= rowCount; ++i)
                            {
                                row = sheet.GetRow(i);
                                if (row == null) continue;

                                dataRow = dataTable.NewRow();
                                for (int j = row.FirstCellNum; j < cellCount; ++j)
                                {
                                    cell = row.GetCell(j);
                                    if (cell == null)
                                    {
                                        dataRow[j] = "";
                                    }
                                    else
                                    {
                                        //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)
                                        switch (cell.CellType)
                                        {
                                            case CellType.Blank:
                                                dataRow[j] = "";
                                                break;
                                            case CellType.Numeric:
                                                short format = cell.CellStyle.DataFormat;
                                                //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
                                                if (format == 14 || format == 31 || format == 57 || format == 58)
                                                    dataRow[j] = cell.DateCellValue;
                                                else
                                                    dataRow[j] = cell.NumericCellValue;
                                                break;
                                            case CellType.String:
                                                dataRow[j] = cell.StringCellValue;
                                                break;
                                        }
                                    }
                                }
                                dataTable.Rows.Add(dataRow);
                            }
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }
        #endregion

        #region DataTable导出到Excel=============================================
        public static byte[] DataTableToExcel(DataTable dt, string strFileName, List<int> hideCol = null, bool monthPlanCompletion = false)
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

                ICellStyle titleStyle = workbook.CreateCellStyle();
                IFont titleFont = workbook.CreateFont();
                titleFont.FontHeightInPoints = 11;//设置字体高度
                titleFont.FontName = "宋体";//设置字体
                titleFont.IsBold = true;
                titleStyle.BorderBottom = BorderStyle.Thin;
                titleStyle.BorderLeft = BorderStyle.Thin;
                titleStyle.BorderRight = BorderStyle.Thin;
                titleStyle.BorderTop = BorderStyle.Thin;
                titleStyle.Alignment = HorizontalAlignment.Center;
                titleStyle.VerticalAlignment = VerticalAlignment.Center;
                titleStyle.SetFont(titleFont);
                //填充表头
                var dataRow = sheet.CreateRow(0);
                var k = 0;
                foreach (DataColumn column in dt.Columns)
                {
                    cell = dataRow.CreateCell(column.Ordinal);
                    cell.CellStyle = titleStyle;
                    cell.SetCellValue(column.ColumnName);
                    if (monthPlanCompletion && k != 6)
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(0, 1, k, k));
                    }
                    k++;
                }


                titleStyle = workbook.CreateCellStyle();
                titleStyle.BorderBottom = BorderStyle.Thin;
                titleStyle.BorderLeft = BorderStyle.Thin;
                titleStyle.BorderRight = BorderStyle.Thin;
                titleStyle.BorderTop = BorderStyle.Thin;
                //填充内容
                var x = 2;
                var y = 3;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataRow = sheet.CreateRow(i + 1);

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = dataRow.CreateCell(j);
                        cell.CellStyle = titleStyle;
                        cell.CellStyle.Alignment = HorizontalAlignment.CenterSelection;
                        cell.SetCellValue(dt.Rows[i][j].ToString());

                        if (monthPlanCompletion && j <= 5)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(x, y, j, j));
                        }
                    }
                    x = y + 1;
                    y = x + 1;

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
        #endregion

        /// <summary>
        /// 检修、验收表导出专用（有线）
        /// </summary>
        /// <param name="dt">检修表数据</param>
        /// <param name="dt2">验收表数据</param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static byte[] ReparAndCheckToExcel(DataTable dt, DataTable dt2, string strFileName, string workPlace, string workTime)
        {
            IWorkbook workbook = null;
            try
            {
                bool isHSSF = true;
                // 2007版本
                if (strFileName.IndexOf(".xlsx") > 0)
                {
                    isHSSF = false;
                    workbook = new XSSFWorkbook();
                }
                // 2003版本
                else if (strFileName.IndexOf(".xls") > 0)
                    workbook = new HSSFWorkbook();
                for (int x = 0; x < 2; x++)
                {
                    DataTable data = x == 0 ? dt : dt2;
                    string sheetName = x == 0 ? "检修作业表" : "验收表";
                    var sheet = workbook.CreateSheet(sheetName);
                    //设置列宽
                    if (x == 0)
                    {
                        sheet.SetColumnWidth(0, 10 * 256);
                        sheet.SetColumnWidth(1, 9 * 256);
                        sheet.SetColumnWidth(2, 10 * 256);
                        sheet.SetColumnWidth(3, 8 * 256);
                        sheet.SetColumnWidth(4, 15 * 256);
                        sheet.SetColumnWidth(5, 15 * 256);
                        sheet.SetColumnWidth(6, 10 * 256);
                        sheet.SetColumnWidth(7, 8 * 256);
                    }
                    else
                    {
                        sheet.SetColumnWidth(0, 8 * 256);
                        sheet.SetColumnWidth(1, 9 * 256);
                        sheet.SetColumnWidth(2, 8 * 256);
                        sheet.SetColumnWidth(3, 15 * 256);
                        sheet.SetColumnWidth(4, 15 * 256);
                        sheet.SetColumnWidth(5, 10 * 256);
                        sheet.SetColumnWidth(6, 8 * 256);
                    }

                    //表格标题
                    var dataRow = sheet.CreateRow(0);
                    dataRow.Height = 30 * 20;   //设置行高30
                    CellRangeAddress region = new CellRangeAddress(0, 0, 0, 7);
                    sheet.AddMergedRegion(region);
                    var titleCell = dataRow.CreateCell(0);
                    string titleName = x == 0 ? "检修作业表" : "检修验收表";
                    titleCell.SetCellValue(titleName);

                    ICellStyle style = workbook.CreateCellStyle();//创建样式对象
                    IFont font = workbook.CreateFont(); //创建一个字体样式对象
                    font.FontName = "宋体"; //和excel里面的字体对应
                    font.FontHeightInPoints = 16;//字体大小
                    font.Boldweight = (short)FontBoldWeight.Bold;//字体加粗
                    style.SetFont(font); //将字体样式赋给样式对象
                    style.Alignment = HorizontalAlignment.Center;
                    style.VerticalAlignment = VerticalAlignment.Center;
                    titleCell.CellStyle = style; //把样式赋给单元格

                    ICellStyle borderCellStyle = workbook.CreateCellStyle();
                    borderCellStyle.BorderTop = BorderStyle.Thin;
                    borderCellStyle.BorderBottom = BorderStyle.Thin;
                    borderCellStyle.BorderLeft = BorderStyle.Thin;
                    borderCellStyle.BorderRight = BorderStyle.Thin;
                    //创建所有数据单元格(带边框)
                    int columnCount = x == 0 ? 7 : 6;
                    for (int i = 1; i < data.Rows.Count + 3; i++)
                    {
                        var row = sheet.CreateRow(i);
                        row.Height = 30 * 20;
                        for (int j = 0; j <= columnCount; j++)
                        {
                            var singleCell = row.CreateCell(j);
                            if (i != 17)
                                singleCell.CellStyle = borderCellStyle;
                        }
                    }

                    //时间填写行
                    dataRow = sheet.GetRow(1);
                    region = new CellRangeAddress(1, 1, 0, 4);
                    sheet.AddMergedRegion(region);
                    var timeCell = dataRow.GetCell(0);
                    timeCell.SetCellValue("时间：" + workTime);

                    var rightCell = dataRow.GetCell(5);  //单元格的赋值是原来的单元格，并不是合并之后的
                    region = new CellRangeAddress(1, 1, 5, 7);
                    sheet.AddMergedRegion(region);
                    rightCell.SetCellValue("作业处所：" + workPlace);

                    ICellStyle tmStyle = workbook.CreateCellStyle();//创建样式对象
                    IFont tmFont = workbook.CreateFont(); //创建一个字体样式对象
                    tmFont.FontName = "宋体"; //和excel里面的字体对应
                    tmFont.FontHeightInPoints = 12;//字体大小
                    tmFont.Boldweight = (short)FontBoldWeight.Bold;//字体加粗
                    tmStyle.SetFont(tmFont); //将字体样式赋给样式对象
                    tmStyle.Alignment = HorizontalAlignment.Left;
                    tmStyle.VerticalAlignment = VerticalAlignment.Center;
                    tmStyle.BorderTop = BorderStyle.Thin;
                    tmStyle.BorderBottom = BorderStyle.Thin;
                    tmStyle.BorderLeft = BorderStyle.Thin;
                    tmStyle.BorderRight = BorderStyle.Thin;
                    timeCell.CellStyle = tmStyle;
                    rightCell.CellStyle = tmStyle;

                    //数据单元格样式
                    ICellStyle cellStyle = workbook.CreateCellStyle();//创建样式对象
                    IFont cellFont = workbook.CreateFont(); //创建一个字体样式对象
                    cellFont.FontName = "宋体"; //和excel里面的字体对应
                    cellFont.FontHeightInPoints = 12;//字体大小
                    cellStyle.SetFont(cellFont); //将字体样式赋给样式对象
                    cellStyle.WrapText = true;  //换行
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cellStyle.BorderTop = BorderStyle.Thin;
                    cellStyle.BorderBottom = BorderStyle.Thin;
                    cellStyle.BorderLeft = BorderStyle.Thin;
                    cellStyle.BorderRight = BorderStyle.Thin;

                    //创建表头
                    dataRow = sheet.GetRow(2);
                    foreach (DataColumn column in data.Columns)
                    {
                        var headCell = dataRow.GetCell(column.Ordinal);
                        headCell.SetCellValue(column.ColumnName);
                        headCell.CellStyle = cellStyle;
                    }
                    //填充内容并合并前三列相同单元格
                    int start0 = 0;//记录同组开始行号
                    int end0 = 0;//记录同组结束行号
                    string temp0 = "";
                    int start1 = 0;//记录同组开始行号
                    int end1 = 0;//记录同组结束行号
                    string temp1 = "";
                    int numIndex = x == 0 ? 3 : 2;//序号所在列Index
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        IRow row = sheet.GetRow(i + 3);
                        for (int j = 0; j < data.Columns.Count; j++)
                        {
                            if (data.Columns[j].ColumnName == "序号") continue;
                            string cellText = data.Rows[i][j].ToString();
                            var dataCell = row.GetCell(j);
                            dataCell.SetCellValue(cellText);
                            dataCell.CellStyle = cellStyle;
                            if (j == 0)     //年月表类型列合并
                            {
                                if (cellText == temp0)//上下行相等，记录要合并的最后一行
                                {
                                    end0 = i;
                                    if (end0 == data.Rows.Count - 1)
                                    {
                                        region = new CellRangeAddress(start0 + 3, end0 + 3, j, j);
                                        sheet.AddMergedRegion(region);
                                    }
                                }
                                else//上下行不等，记录
                                {
                                    if (start0 != end0)
                                    {
                                        region = new CellRangeAddress(start0 + 3, end0 + 3, j, j);
                                        sheet.AddMergedRegion(region);
                                    }
                                    start0 = i;
                                    end0 = i;
                                    temp0 = cellText;
                                }
                            }
                            else if (j == 1)   //类别及设备列合并
                            {
                                if (cellText == temp1)//上下行相等，记录要合并的最后一行
                                {
                                    end1 = i;
                                    if (end1 == data.Rows.Count - 1)
                                    {
                                        region = new CellRangeAddress(start1 + 3, end1 + 3, j, j);
                                        sheet.AddMergedRegion(region);
                                        //检修表关联设备列合并
                                        //if (x == 0)
                                        //{
                                        //    region = new CellRangeAddress(start1 + 3, end1 + 3, 2, 2);
                                        //    sheet.AddMergedRegion(region);
                                        //}
                                        //序号赋值
                                        for (int index = start1; index <= end1; index++)
                                        {
                                            IRow numRow = sheet.GetRow(index + 3);
                                            var numCell = numRow.GetCell(numIndex);
                                            numCell.SetCellValue((index - start1 + 1).ToString());
                                            numCell.CellStyle = cellStyle;
                                        }
                                    }
                                }
                                else//上下行不等，记录
                                {
                                    if (start1 != end1)
                                    {
                                        region = new CellRangeAddress(start1 + 3, end1 + 3, j, j);
                                        sheet.AddMergedRegion(region);
                                        //检修表关联设备列合并
                                        //if (x == 0)
                                        //{
                                        //    region = new CellRangeAddress(start1 + 3, end1 + 3, 2, 2);
                                        //    sheet.AddMergedRegion(region);
                                        //}
                                        //序号赋值
                                        for (int index = start1; index <= end1; index++)
                                        {
                                            IRow numRow = sheet.GetRow(index + 3);
                                            var numCell = numRow.GetCell(numIndex);
                                            numCell.SetCellValue((index - start1 + 1).ToString());
                                            numCell.CellStyle = cellStyle;
                                        }
                                        //最后一行则给本行添加序号1
                                        if (i == data.Rows.Count - 1)
                                        {
                                            var numCell = row.GetCell(numIndex);
                                            numCell.SetCellValue("1");
                                            numCell.CellStyle = cellStyle;
                                        }
                                    }
                                    else
                                    {
                                        //序号赋值
                                        var numCell = row.GetCell(numIndex);
                                        numCell.SetCellValue("1");
                                        numCell.CellStyle = cellStyle;
                                    }
                                    start1 = i;
                                    end1 = i;
                                    temp1 = cellText;
                                }
                            }
                        }
                    }

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

        /// <summary>
        /// 导出检修表（检修人与验收人不分表）高铁
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static byte[] RepairWithCheckToExcel(DataTable dt, string strFileName, string workTime)
        {
            IWorkbook workbook = null;
            try
            {
                bool isHSSF = true;
                // 2007版本
                if (strFileName.IndexOf(".xlsx") > 0)
                {
                    isHSSF = false;
                    workbook = new XSSFWorkbook();
                }
                // 2003版本
                else if (strFileName.IndexOf(".xls") > 0)
                    workbook = new HSSFWorkbook();

                DataTable data = dt;
                string sheetName = "检修作业表";
                var sheet = workbook.CreateSheet(sheetName);
                //设置列宽
                sheet.SetColumnWidth(0, 10 * 256);
                sheet.SetColumnWidth(1, 9 * 256);
                sheet.SetColumnWidth(2, 10 * 256);
                sheet.SetColumnWidth(3, 8 * 256);
                sheet.SetColumnWidth(4, 15 * 256);
                sheet.SetColumnWidth(5, 15 * 256);
                sheet.SetColumnWidth(6, 10 * 256);
                sheet.SetColumnWidth(7, 15 * 256);
                sheet.SetColumnWidth(8, 10 * 256);
                sheet.SetColumnWidth(9, 8 * 256);

                //表格标题
                var dataRow = sheet.CreateRow(0);
                dataRow.Height = 30 * 20;   //设置行高30
                CellRangeAddress region = new CellRangeAddress(0, 0, 0, 9);
                sheet.AddMergedRegion(region);
                var titleCell = dataRow.CreateCell(0);
                string titleName = "检修作业记录/验收表";
                titleCell.SetCellValue(titleName);

                ICellStyle style = workbook.CreateCellStyle();//创建样式对象
                IFont font = workbook.CreateFont(); //创建一个字体样式对象
                font.FontName = "宋体"; //和excel里面的字体对应
                font.FontHeightInPoints = 16;//字体大小
                font.Boldweight = (short)FontBoldWeight.Bold;//字体加粗
                style.SetFont(font); //将字体样式赋给样式对象
                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;
                titleCell.CellStyle = style; //把样式赋给单元格

                ICellStyle borderCellStyle = workbook.CreateCellStyle();
                borderCellStyle.BorderTop = BorderStyle.Thin;
                borderCellStyle.BorderBottom = BorderStyle.Thin;
                borderCellStyle.BorderLeft = BorderStyle.Thin;
                borderCellStyle.BorderRight = BorderStyle.Thin;
                //创建所有数据单元格(带边框)
                int columnCount = 9;
                for (int i = 1; i < data.Rows.Count + 3; i++)
                {
                    var row = sheet.CreateRow(i);
                    row.Height = 30 * 20;
                    for (int j = 0; j <= columnCount; j++)
                    {
                        var singleCell = row.CreateCell(j);
                        if (i != 17)
                            singleCell.CellStyle = borderCellStyle;
                    }
                }

                //时间填写行
                dataRow = sheet.GetRow(1);
                var timeCell = dataRow.GetCell(0);
                region = new CellRangeAddress(1, 1, 0, 9);
                sheet.AddMergedRegion(region);
                timeCell.SetCellValue("时间：" + workTime);

                /*var rightCell = dataRow.GetCell(7);  //单元格的赋值是原来的单元格，并不是合并之后的
                region = new CellRangeAddress(1, 1, 7, 9);
                sheet.AddMergedRegion(region);
                rightCell.SetCellValue("作业处所：");*/

                ICellStyle tmStyle = workbook.CreateCellStyle();//创建样式对象
                IFont tmFont = workbook.CreateFont(); //创建一个字体样式对象
                tmFont.FontName = "宋体"; //和excel里面的字体对应
                tmFont.FontHeightInPoints = 12;//字体大小
                tmFont.Boldweight = (short)FontBoldWeight.Bold;//字体加粗
                tmStyle.SetFont(tmFont); //将字体样式赋给样式对象
                tmStyle.Alignment = HorizontalAlignment.Left;
                tmStyle.VerticalAlignment = VerticalAlignment.Center;
                tmStyle.BorderTop = BorderStyle.Thin;
                tmStyle.BorderBottom = BorderStyle.Thin;
                tmStyle.BorderLeft = BorderStyle.Thin;
                tmStyle.BorderRight = BorderStyle.Thin;
                timeCell.CellStyle = tmStyle;
                //rightCell.CellStyle = tmStyle;



                //数据单元格样式
                ICellStyle cellStyle = workbook.CreateCellStyle();//创建样式对象
                IFont cellFont = workbook.CreateFont(); //创建一个字体样式对象
                cellFont.FontName = "宋体"; //和excel里面的字体对应
                cellFont.FontHeightInPoints = 12;//字体大小
                cellStyle.SetFont(cellFont); //将字体样式赋给样式对象
                cellStyle.WrapText = true;  //换行
                cellStyle.Alignment = HorizontalAlignment.Center;
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;

                //创建表头
                dataRow = sheet.GetRow(2);
                foreach (DataColumn column in data.Columns)
                {
                    var headCell = dataRow.GetCell(column.Ordinal);
                    headCell.SetCellValue(column.ColumnName);
                    headCell.CellStyle = cellStyle;
                }
                //填充内容并合并前三列相同单元格
                int start0 = 0;//记录同组开始行号
                int end0 = 0;//记录同组结束行号
                string temp0 = "";
                int start1 = 0;//记录同组开始行号
                int end1 = 0;//记录同组结束行号
                string temp1 = "";
                int numIndex = 3;//序号所在列Index
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    IRow row = sheet.GetRow(i + 3);
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        if (data.Columns[j].ColumnName == "序号") continue;
                        var cellText = data.Rows[i][j].ToString();
                        var dataCell = row.GetCell(j);
                        dataCell.SetCellValue(cellText);
                        dataCell.CellStyle = cellStyle;
                        if (j == 0)     //年月表类型列合并
                        {
                            if (cellText == temp0)//上下行相等，记录要合并的最后一行
                            {
                                end0 = i;
                                if (end0 == data.Rows.Count - 1 && data.Rows.Count > 1)
                                {
                                    region = new CellRangeAddress(start0 + 3, end0 + 3, j, j);
                                    sheet.AddMergedRegion(region);
                                }
                            }
                            else//上下行不等，记录
                            {
                                if (start0 != end0)
                                {
                                    region = new CellRangeAddress(start0 + 3, end0 + 3, j, j);
                                    sheet.AddMergedRegion(region);
                                }
                                start0 = i;
                                end0 = i;
                                temp0 = cellText;
                            }
                        }
                        else if (j == 1)   //类别及设备列合并
                        {
                            if (cellText == temp1)//上下行相等，记录要合并的最后一行
                            {
                                end1 = i;
                                if (end1 == data.Rows.Count - 1)
                                {
                                    region = new CellRangeAddress(start1 + 3, end1 + 3, j, j);
                                    sheet.AddMergedRegion(region);
                                    //检修表关联设备列合并
                                    //if (x == 0)
                                    //{
                                    //    region = new CellRangeAddress(start1 + 3, end1 + 3, 2, 2);
                                    //    sheet.AddMergedRegion(region);
                                    //}
                                    //序号赋值
                                    for (int index = start1; index <= end1; index++)
                                    {
                                        IRow numRow = sheet.GetRow(index + 3);
                                        var numCell = numRow.GetCell(numIndex);
                                        numCell.SetCellValue((index - start1 + 1).ToString());
                                        numCell.CellStyle = cellStyle;
                                    }
                                }
                            }
                            else//上下行不等，记录
                            {
                                if (start1 != end1)
                                {
                                    region = new CellRangeAddress(start1 + 3, end1 + 3, j, j);
                                    sheet.AddMergedRegion(region);
                                    //检修表关联设备列合并
                                    //if (x == 0)
                                    //{
                                    //    region = new CellRangeAddress(start1 + 3, end1 + 3, 2, 2);
                                    //    sheet.AddMergedRegion(region);
                                    //}
                                    //序号赋值
                                    for (int index = start1; index <= end1; index++)
                                    {
                                        IRow numRow = sheet.GetRow(index + 3);
                                        var numCell = numRow.GetCell(numIndex);
                                        numCell.SetCellValue((index - start1 + 1).ToString());
                                        numCell.CellStyle = cellStyle;
                                    }
                                    //最后一行则给本行添加序号1
                                    if (i == data.Rows.Count - 1)
                                    {
                                        var numCell = row.GetCell(numIndex);
                                        numCell.SetCellValue("1");
                                        numCell.CellStyle = cellStyle;
                                    }
                                }
                                else
                                {
                                    //序号赋值
                                    var numCell = row.GetCell(numIndex);
                                    numCell.SetCellValue("1");
                                    numCell.CellStyle = cellStyle;
                                }
                                start1 = i;
                                end1 = i;
                                temp1 = cellText;
                            }
                        }
                    }
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

        /// <summary>
        /// 派工单导出专用
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static byte[] WorkOrderToExcel(WorkOrderExportDto input, string strFileName)
        {
            IWorkbook workbook = null;
            try
            {
                bool isHSSF = true;
                // 2007版本
                if (strFileName.IndexOf(".xlsx") > 0)
                {
                    isHSSF = false;
                    workbook = new XSSFWorkbook();
                }
                // 2003版本
                else if (strFileName.IndexOf(".xls") > 0)
                    workbook = new HSSFWorkbook();

                var sheet = workbook.CreateSheet("派工单");
                sheet.PrintSetup.PaperSize = 11;
                sheet.FitToPage = false;
                sheet.SetMargin(MarginType.LeftMargin, 1 / 2.54);
                sheet.SetMargin(MarginType.RightMargin, 1 / 2.54);
                sheet.SetMargin(MarginType.BottomMargin, 1 / 2.54);
                sheet.SetMargin(MarginType.TopMargin, 1 / 2.54);

                //创建每个单元格，带边框
                ICellStyle borderStyle = workbook.CreateCellStyle();
                borderStyle.BorderTop = BorderStyle.Thin;
                borderStyle.BorderBottom = BorderStyle.Thin;
                borderStyle.BorderLeft = BorderStyle.Thin;
                borderStyle.BorderRight = BorderStyle.Thin;
                for (int i = 2; i <= 17; i++)
                {
                    var row = sheet.CreateRow(i);
                    row.Height = 31 * 20;
                    for (int j = 0; j <= 6; j++)
                    {
                        var singleCell = row.CreateCell(j);
                        if (i != 17)
                            singleCell.CellStyle = borderStyle;
                    }
                }

                //设置列宽
                sheet.SetColumnWidth(0, 9 * 256); //列宽9*256=8.38
                sheet.SetColumnWidth(1, 16 * 256);
                sheet.SetColumnWidth(2, 7 * 256);
                sheet.SetColumnWidth(3, 5 * 256);
                sheet.SetColumnWidth(4, 5 * 256);
                sheet.SetColumnWidth(5, 4 * 256);
                sheet.SetColumnWidth(6, 19 * 256);
                //表格标题
                var dataRow = sheet.CreateRow(0);
                dataRow.Height = 31 * 20; //设置行高31磅
                CellRangeAddress region = new CellRangeAddress(0, 0, 0, 6);
                sheet.AddMergedRegion(region);
                var titleCell = dataRow.CreateCell(0);
                titleCell.SetCellValue("通信日常作业派工单");

                ICellStyle style = workbook.CreateCellStyle();//创建样式对象
                IFont font = workbook.CreateFont(); //创建一个字体样式对象
                font.FontName = "宋体"; //和excel里面的字体对应
                font.FontHeightInPoints = 14;//字体大小
                font.Boldweight = (short)FontBoldWeight.Bold;//字体加粗
                style.SetFont(font); //将字体样式赋给样式对象
                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;
                titleCell.CellStyle = style; //把样式赋给单元格

                #region 数据单元格样式
                ICellStyle leftCenterStyle = workbook.CreateCellStyle();//创建样式对象
                IFont cellFont = workbook.CreateFont(); //创建一个字体样式对象
                cellFont.FontName = "宋体"; //和excel里面的字体对应
                cellFont.FontHeightInPoints = 10;//字体大小
                leftCenterStyle.SetFont(cellFont); //将字体样式赋给样式对象
                leftCenterStyle.WrapText = true;  //换行
                leftCenterStyle.Alignment = HorizontalAlignment.Left;
                leftCenterStyle.VerticalAlignment = VerticalAlignment.Center;

                ICellStyle leftCenterBorderStyle = workbook.CreateCellStyle();
                leftCenterBorderStyle.SetFont(cellFont);
                leftCenterBorderStyle.WrapText = true;
                leftCenterBorderStyle.Alignment = HorizontalAlignment.Left;
                leftCenterBorderStyle.VerticalAlignment = VerticalAlignment.Center;
                leftCenterBorderStyle.BorderLeft = BorderStyle.Thin;
                leftCenterBorderStyle.BorderBottom = BorderStyle.Thin;
                leftCenterBorderStyle.BorderRight = BorderStyle.Thin;
                leftCenterBorderStyle.BorderTop = BorderStyle.Thin;

                ICellStyle leftTopStyle = workbook.CreateCellStyle();
                leftTopStyle.SetFont(cellFont);
                leftTopStyle.WrapText = true;
                leftTopStyle.Alignment = HorizontalAlignment.Left;
                leftTopStyle.VerticalAlignment = VerticalAlignment.Top;
                leftTopStyle.BorderLeft = BorderStyle.Thin;
                leftTopStyle.BorderBottom = BorderStyle.Thin;
                leftTopStyle.BorderRight = BorderStyle.Thin;
                leftTopStyle.BorderTop = BorderStyle.Thin;

                ICellStyle leftTopNoBorderStyle = workbook.CreateCellStyle();
                leftTopNoBorderStyle.SetFont(cellFont);
                leftTopNoBorderStyle.WrapText = true;
                leftTopNoBorderStyle.Alignment = HorizontalAlignment.Left;
                leftTopNoBorderStyle.VerticalAlignment = VerticalAlignment.Top;

                ICellStyle cenCenterStyle = workbook.CreateCellStyle();
                cenCenterStyle.SetFont(cellFont);
                cenCenterStyle.WrapText = true;
                cenCenterStyle.Alignment = HorizontalAlignment.Center;
                cenCenterStyle.VerticalAlignment = VerticalAlignment.Center;
                cenCenterStyle.BorderLeft = BorderStyle.Thin;
                cenCenterStyle.BorderBottom = BorderStyle.Thin;
                cenCenterStyle.BorderRight = BorderStyle.Thin;
                cenCenterStyle.BorderTop = BorderStyle.Thin;
                #endregion

                //车间、工区、命令票号
                dataRow = sheet.CreateRow(1);
                dataRow.Height = 31 * 20;
                var workShopCell = dataRow.CreateCell(0);
                workShopCell.SetCellValue("车间：" + input.WorkShop);
                workShopCell.CellStyle = leftCenterStyle;
                region = new CellRangeAddress(1, 1, 0, 1);
                sheet.AddMergedRegion(region);
                var workAreaCell = dataRow.CreateCell(2);
                workAreaCell.SetCellValue("工区：" + input.WorkArea);
                workAreaCell.CellStyle = leftCenterStyle;
                region = new CellRangeAddress(1, 1, 2, 4);
                sheet.AddMergedRegion(region);
                var ticketCell = dataRow.CreateCell(5);
                ticketCell.SetCellValue("命令票号：" + input.OrderNo);
                ticketCell.CellStyle = leftCenterStyle;
                region = new CellRangeAddress(1, 1, 5, 6);
                sheet.AddMergedRegion(region);

                //创建派工单表格
                //作业小组长
                dataRow = sheet.GetRow(2);
                dataRow.Height = 23 * 20;
                var workLeaderCell = dataRow.GetCell(0);
                workLeaderCell.SetCellValue("作业小组长：" + input.WorkLeader);
                workLeaderCell.CellStyle = leftCenterBorderStyle;
                region = new CellRangeAddress(2, 3, 0, 2);
                sheet.AddMergedRegion(region);
                //成员
                var workerCell = dataRow.GetCell(3);
                workerCell.SetCellValue("成员：" + input.WorkMemberList);
                workerCell.CellStyle = leftCenterBorderStyle;
                region = new CellRangeAddress(2, 3, 3, 6);
                sheet.AddMergedRegion(region);

                //工作地点内容及影响
                dataRow = sheet.GetRow(4);
                dataRow.Height = 15 * 20;
                var workContentCell = dataRow.GetCell(0);
                workContentCell.SetCellValue("工作内容及影响：\n" + input.WorkSiteAndContent);
                workContentCell.CellStyle = leftTopStyle;
                region = new CellRangeAddress(4, 7, 0, 6);
                sheet.AddMergedRegion(region);

                //驿站联络员
                dataRow = sheet.GetRow(8);
                dataRow.Height = 23 * 20;
                var contacterCell = dataRow.GetCell(0);
                contacterCell.SetCellValue("驻站联络员：\n" + input.StationLiaisonOfficerList);
                contacterCell.CellStyle = leftTopStyle;
                region = new CellRangeAddress(8, 9, 0, 1);
                sheet.AddMergedRegion(region);
                //现场联络员
                var sceneContacterCell = dataRow.GetCell(2);
                sceneContacterCell.SetCellValue("现场联络员：\n" + input.FieldGuardList);
                sceneContacterCell.CellStyle = leftTopStyle;
                region = new CellRangeAddress(8, 9, 2, 3);
                sheet.AddMergedRegion(region);
                //通信工具检查试验情况
                var toolTestCell = dataRow.GetCell(4);
                toolTestCell.SetCellValue("通信工具检查试验情况：\n" + input.ToolSituation);
                toolTestCell.CellStyle = leftTopStyle;
                region = new CellRangeAddress(8, 9, 4, 6);
                sheet.AddMergedRegion(region);

                //作业时间
                dataRow = sheet.GetRow(10);
                dataRow.Height = 31 * 20;
                var workTimeTitleCell = dataRow.GetCell(0);
                workTimeTitleCell.SetCellValue("作   业     时   间");
                workTimeTitleCell.CellStyle = cenCenterStyle;
                var workTimeCell = dataRow.GetCell(1);
                workTimeCell.SetCellValue(input.WorkTime);
                workTimeCell.CellStyle = cenCenterStyle;
                region = new CellRangeAddress(10, 10, 1, 2);
                sheet.AddMergedRegion(region);
                //天窗时间
                var skylightTitleCell = dataRow.GetCell(3);
                skylightTitleCell.SetCellValue("天   窗              时   间");
                skylightTitleCell.CellStyle = cenCenterStyle;
                region = new CellRangeAddress(10, 10, 3, 4);
                sheet.AddMergedRegion(region);
                var skylightCell = dataRow.GetCell(5);
                skylightCell.SetCellValue(input.SkylightTime);
                skylightCell.CellStyle = cenCenterStyle;
                region = new CellRangeAddress(10, 10, 5, 6);
                sheet.AddMergedRegion(region);

                //作业注意事项（固定）
                dataRow = sheet.GetRow(11);
                dataRow.Height = 40 * 20;
                var attentionCell = dataRow.GetCell(0);
                attentionCell.SetCellValue("作业注意事项：\n1、上线前进网必须设置好防护并掌握列车运行情况，做好人身安全防护。\n2、严格执行“三不动”、“三不离”制度，检修完后必须按规定测试和试验。\n3、其它注意事项（负责人要研判作业中的安全风险并明确措施责任到人）：\n机房作业人员带齐作业用品，进入机房与网管联系，做好登记，严格执行标准化作业，天窗点内才能作业，不得干与派工单无关的工作。作业时确保其它设备安全，严格按照年月表检修内容，不得抄表、漏表，作业完毕与网管联系，确认设备运行正常后才能离开机房。");
                attentionCell.CellStyle = leftTopStyle;
                region = new CellRangeAddress(11, 13, 0, 6);
                sheet.AddMergedRegion(region);

                //作业完成情况
                dataRow = sheet.GetRow(14);
                dataRow.Height = 23 * 20;
                var completeCell = dataRow.GetCell(0);
                completeCell.SetCellValue("作业完成情况：\n" + input.CompletionStatus);
                completeCell.CellStyle = leftTopStyle;
                region = new CellRangeAddress(14, 15, 0, 6);
                sheet.AddMergedRegion(region);

                //负责人（签字）
                dataRow = sheet.GetRow(16);
                dataRow.Height = 31 * 20;
                var headCell = dataRow.GetCell(0);
                headCell.SetCellValue("负责人：" + input.WorkAreaLeader);
                headCell.CellStyle = leftCenterBorderStyle;
                region = new CellRangeAddress(16, 16, 0, 2);
                sheet.AddMergedRegion(region);
                //作业小组长
                var gpLeaderCell = dataRow.GetCell(3);
                gpLeaderCell.SetCellValue("作业小组长：" + input.WorkLeader);
                gpLeaderCell.CellStyle = leftCenterBorderStyle;
                region = new CellRangeAddress(16, 16, 3, 6);
                sheet.AddMergedRegion(region);

                //备注（固定）
                dataRow = sheet.GetRow(17);
                dataRow.Height = 37 * 20;
                var remarkCell = dataRow.GetCell(0);
                remarkCell.SetCellValue("注：1、此表由工区负责人、作业小组长共同签字后生效。\n2、作业人员只填写实际给点时间栏和工作完后情况反馈栏，其它栏目由负责人填写。");
                remarkCell.CellStyle = leftTopNoBorderStyle;
                region = new CellRangeAddress(17, 17, 0, 6);
                sheet.AddMergedRegion(region);

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

    }
}
