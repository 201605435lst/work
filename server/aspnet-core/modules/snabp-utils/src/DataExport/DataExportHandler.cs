/**********************************************************************
*******命名空间： SnAbp.Utils.DataExport
*******类 名 称： DataExportHandler
*******类 说 明： 数据导出帮助类,通过创建不同的行列样式来快速生成文件
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/11 9:14:46
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF;
using NPOI.XSSF.UserModel;

using SnAbp.Utils.ExcelHelper;

namespace SnAbp.Utils.DataExport
{
    /// <summary>
    /// 数据导出帮助类
    /// </summary>
    public class DataExportHandler
    {

        private ExcelFileType _fileType;
        private IWorkbook workbook;
        private ISheet sheet;
        private List<ISheet> sheets;

        private static DataExportHandler dataExportHandler { get; set; }
        public DataExportHandler() { }
        private DataExportHandler(ExcelFileType type)
        {
            _fileType = type;
            switch (type)
            {
                case ExcelFileType.Xls:
                    workbook = new HSSFWorkbook();
                    break;
                case ExcelFileType.Xlsx:
                    workbook = new XSSFWorkbook();
                    break;
                default:
                    throw new ArgumentException("参数异常");
            }
        }

        /// <summary>
        /// 创建一个excel工作表
        /// </summary>
        /// <param name="type">工作表类型</param>
        /// <param name="multipleSheet">是否有多个sheet页</param>
        public static DataExportHandler CreateExcelFile(ExcelFileType type, bool multipleSheet = false)
        {
            dataExportHandler = new DataExportHandler(type);
            if (multipleSheet)
            {
                dataExportHandler.sheets = new List<ISheet>();
            }
            return dataExportHandler;
        }

        /// <summary>
        /// 创建一个sheet
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public ISheet CreateSheet(string sheetName)
        {
            sheet = workbook.CreateSheet(sheetName);
            if (sheets != null)
            {
                sheets.Add(sheet);
            }
            return sheet;
        }
        public IRow CreateRow(int index)
        {
            var row = sheet.CreateRow(index);
            return row;
        }

        /// <summary>
        /// 创建合并单元格
        /// </summary>
        /// <param name="firstRow">从第几行开始合并</param>
        /// <param name="lastRow">到第几行结束合并</param>
        /// <param name="firstCol">从第几列开始合并</param>
        /// <param name="lastCol">到第几列结束合并</param>
        /// <returns></returns>
        public CellRangeAddress CellRangeAddress(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            return new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
        }

        /// <summary>
        /// 创建单元格样式
        /// </summary>
        /// <param name="border"></param>
        /// <param name="borderColor"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public ICellStyle CreateCellStyle(CellBorder border = null, short borderColor = 0,CellFont font=null)
        {
            var cellStyle = workbook.CreateCellStyle();
            if (border == null)
            {
                border = CellBorder.CreateBorder(BorderStyle.Thin, borderColor);
            }

            if (font == null)
            {
                font = CellFont.CreateCellFont();
            }
            border.ConfigBorder(cellStyle);
            font.ConfigCellFont(workbook,cellStyle);
            return cellStyle;
        }

        public byte[] GetExcelDataBuffer()
        {
            var stream = new MemoryStream();
            workbook.Write(stream);
            return stream.ToArray();
        }
    }
    /// <summary>
    /// 单元格边框样式
    /// </summary>
    public class CellBorder
    {
        private BorderStyle BorderLeft { get; set; }
        private BorderStyle BorderBottom { get; set; }
        private BorderStyle BorderTop { get; set; }
        private BorderStyle BorderRight { get; set; }

        private short BottomBorderColor { get; set; }
        private short TopBorderColor { get; set; }
        private short RightBorderColor { get; set; }
        private short LeftBorderColor { get; set; }

        /// <summary>
        /// 绑定和配置单元格边框及颜色
        /// </summary>
        /// <param name="cellStyle"></param>
        /// <returns></returns>
        public ICellStyle ConfigBorder(ICellStyle cellStyle)
        {
            cellStyle.BorderBottom = this.BorderBottom;
            cellStyle.BorderRight = this.BorderRight;
            cellStyle.BorderTop = this.BorderTop;
            cellStyle.BorderLeft = this.BorderLeft;
            cellStyle.RightBorderColor = this.RightBorderColor;
            cellStyle.LeftBorderColor = this.LeftBorderColor;
            cellStyle.TopBorderColor = this.TopBorderColor;
            cellStyle.BottomBorderColor = this.BottomBorderColor;
            return cellStyle;
        }

        private static CellBorder border;
        private HSSFColor HSSFColor;
        public CellBorder(BorderStyle borderTop,
            BorderStyle borderRight,
            BorderStyle borderBottom,
            BorderStyle borderLeft,
            short lineColor = default)
        {
            if (lineColor == 0)
            {
                lineColor = HSSFColor.Black.Index;
            }
            this.BorderLeft = borderLeft;
            this.BorderRight = borderRight;
            this.BorderTop = borderTop;
            this.BorderBottom = borderBottom;
            this.BottomBorderColor = lineColor;
            this.TopBorderColor = lineColor;
            this.LeftBorderColor = lineColor;
            this.RightBorderColor = lineColor;
        }
        public static CellBorder CreateBorder(
            BorderStyle borderTop,
            BorderStyle borderRight,
            BorderStyle borderBottom,
            BorderStyle borderLeft,
            short lineColor = 0
        )
        {
            border = new CellBorder(borderTop, borderRight, borderBottom, borderLeft, lineColor);
            return border;
        }

        public static CellBorder CreateBorder(BorderStyle borderStyle, short lineColor = 0) => CreateBorder(borderStyle, borderStyle, borderStyle, borderStyle, lineColor);
        public static CellBorder CreateBorder(BorderStyle topAndBottomStyle, BorderStyle leftAndRightStyle, short lineColor = 0) => CreateBorder(topAndBottomStyle, leftAndRightStyle, topAndBottomStyle, leftAndRightStyle, lineColor);
    }
    public class CellFont
    {
        string FontName { get; set; }
        double FontHeight { get; set; }
        double FontHeightInPoints { get; set; }
        bool IsItalic { get; set; }
        bool IsStrikeout { get; set; }
        short Color { get; set; }
        FontSuperScript TypeOffset { get; set; }
        FontUnderlineType Underline { get; set; }
        short Charset { get; set; }
        short Index { get; }
        [Obsolete("deprecated POI 3.15 beta 2. Use IsBold instead.")]
        short Boldweight { get; set; }
        bool IsBold { get; set; }
        private IFont font;
        private static CellFont cellFont;
        private CellFont() { }

        /// <summary>
        /// 配置单元格字体
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="cellStlye"></param>
        /// <returns></returns>
        public ICellStyle ConfigCellFont(IWorkbook workbook,ICellStyle cellStlye)
        {
            font = workbook.CreateFont();
            font.FontName = this.FontName;
            font.FontHeight = this.FontHeight;
            font.FontHeightInPoints = this.FontHeightInPoints;
            font.IsItalic = this.IsItalic;
            font.Color = this.Color;
            font.IsBold = this.IsBold;
            cellStlye.SetFont(font);
            return cellStlye;
        }

        public static CellFont CreateCellFont(
            string fontName = "宋体",
            double fontHeight = 11d,
            double fontHeightInPoints=0.0,
            bool isItalic=false,
            short color=0,
            bool isBold=false
            )
        {
            cellFont = new CellFont();
            cellFont.FontName = fontName;
            cellFont.FontHeight = fontHeight;
            cellFont.FontHeightInPoints = fontHeightInPoints;
            cellFont.IsItalic = isItalic;
            cellFont.Color = color;
            cellFont.IsBold = isBold;
            return cellFont;
        }
    }
}
