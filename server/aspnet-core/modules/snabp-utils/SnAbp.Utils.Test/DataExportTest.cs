/**********************************************************************
*******命名空间： SnAbp.Utils.Test
*******类 名 称： DataExportTest
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/11 9:34:57
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NUnit.Framework;

using SnAbp.Utils.DataExport;

using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace SnAbp.Utils.Test
{
    [TestClass]
    public class DataExportTest
    {
        public DataExportTest() { }

        [TestMethod]
        public void Test()
        {
            var handler = DataExportHandler.CreateExcelFile(ExcelHelper.ExcelFileType.Xlsx);
            _= handler.CreateSheet("ceshi");
            var cellStyle = handler.CreateCellStyle();
            handler
                .CreateRow(1)
                .CreateCell(1)
                .SetCellStyle(cellStyle)
                .SetCellValue("123");

        }
    }
}
