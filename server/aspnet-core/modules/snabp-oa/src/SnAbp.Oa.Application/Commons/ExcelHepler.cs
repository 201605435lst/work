using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace SnAbp.Oa.Commons
{
    public class ExcelHepler
    {
        #region DataTable导出到Excel=============================================
        public static byte[] DataTableToExcel(DataTable dt, string strFileName, List<int> hideCol = null)
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
                string sheetName = "合同表";
                var sheet = workbook.CreateSheet(sheetName);
                sheet.SetColumnWidth(0, 15 * 310);
                sheet.SetColumnWidth(1, 18 * 310);
                sheet.SetColumnWidth(2, 12 * 310);
                sheet.SetColumnWidth(3, 12 * 310);
                sheet.SetColumnWidth(4, 15 * 310);
                sheet.SetColumnWidth(5, 15 * 310);
                sheet.SetColumnWidth(6, 12 * 310);
                sheet.SetColumnWidth(7, 12 * 310);
                sheet.SetColumnWidth(8, 15 * 310);
                sheet.SetColumnWidth(9, 15 * 310);
                sheet.SetColumnWidth(10, 15 * 310);
                sheet.SetColumnWidth(11, 18 * 400);
                sheet.SetColumnWidth(12, 18 * 400);

                ICellStyle titleStyle = workbook.CreateCellStyle();
                IFont titleFont = workbook.CreateFont();
                titleFont.FontHeightInPoints = 11;//设置字体高度
                titleFont.FontName = "宋体";//设置字体
                titleFont.IsBold = true;
                titleStyle.BorderBottom = BorderStyle.Thin;
                titleStyle.BorderLeft = BorderStyle.Thin;
                titleStyle.BorderRight = BorderStyle.Thin;
                titleStyle.BorderTop = BorderStyle.Thin;
                titleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                titleStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                titleStyle.SetFont(titleFont);
                //填充表头
                var dataRow = sheet.CreateRow(0);
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
                //填充内容
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataRow = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = dataRow.CreateCell(j);
                        cell.CellStyle = titleStyle;
                        cell.SetCellValue(dt.Rows[i][j].ToString());
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
        #endregion
    }
}
