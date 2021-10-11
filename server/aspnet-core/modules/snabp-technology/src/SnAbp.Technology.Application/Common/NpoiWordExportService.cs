using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using NPOI.XWPF.UserModel;
using SnAbp.Technology.Dtos;
using SnAbp.Technology.Entities;
using SnAbp.Technology.enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace SnAbp.Technology
{
    public class NpoiWordExportService
    {
        /// <summary>
        /// word生成并保存服务
        /// </summary>
        private static IHostingEnvironment _environment;

        public NpoiWordExportService(IHostingEnvironment iEnvironment)
        {
            _environment = iEnvironment;
        }

        #region 生成报告信息    word

        /// <summary>
        ///  生成word案卷文档
        /// </summary>
        /// <param name="savePath">保存路径</param>
        public static Stream SaveConstructInterfaceWordFile(ConstructInterface Data, int Count, string Date, string TableName, string WorkOrganization)
        {
            string workFileName = TableName;
            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();

            //文本标题
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, workFileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);
            //TODO:这里一行需要显示两个文本
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"单位名称：{WorkOrganization}", false, 12, "宋体", ParagraphAlignment.LEFT, true, $"                                      日期：{Date}"), 1);


            #region 文档第一个表格对象实例
            //创建文档中的表格对象实例
            XWPFTable firstXwpfTable = document.CreateTable(3, 4);//显示的行列数rows:4行,cols:4列
            firstXwpfTable.Width = 5200;//总宽度
            firstXwpfTable.SetColumnWidth(0, 1000); /* 设置列宽 */
            firstXwpfTable.SetColumnWidth(1, 1600); /* 设置列宽 */
            firstXwpfTable.SetColumnWidth(2, 1000); /* 设置列宽 */
            firstXwpfTable.SetColumnWidth(3, 1600); /* 设置列宽 */


            //Table 表格第一行展示...后面的都是一样，只改变GetRow中的行数

            var position = JObject.Parse(Data.Position);

            var positionString = position["lon"].ToString() + "," + "\n" + position["lat"].ToString() + "," + "\n" + position["alt"].ToString();

            firstXwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "接口名称", ParagraphAlignment.CENTER, 22, true));
            firstXwpfTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, $"{Data.Name}", ParagraphAlignment.CENTER, 22, false));
            firstXwpfTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "接口编号", ParagraphAlignment.CENTER, 22, true));
            firstXwpfTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, $"{Data.Code}", ParagraphAlignment.CENTER, 22, false));

            //Table 表格第二行
            firstXwpfTable.GetRow(1).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "接口位置", ParagraphAlignment.CENTER, 22, true));
            firstXwpfTable.GetRow(1).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, $"{positionString}", ParagraphAlignment.CENTER, 22, false, 10));
            firstXwpfTable.GetRow(1).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "专业", ParagraphAlignment.CENTER, 22, true));
            firstXwpfTable.GetRow(1).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, $"{Data?.Profession?.Name}", ParagraphAlignment.CENTER, 22, false));


            //Table 表格第三行
            firstXwpfTable.GetRow(2).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "土建单位", ParagraphAlignment.CENTER, 22, true));
            firstXwpfTable.GetRow(2).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, $"{Data?.Builder?.Name}", ParagraphAlignment.CENTER, 22, false));
            firstXwpfTable.GetRow(2).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "检查情况", ParagraphAlignment.CENTER, 22, true));
            firstXwpfTable.GetRow(2).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, $"{NpoiWordParagraphTextStyleHelper._.transformMarkType(Data.MarkType)}", ParagraphAlignment.CENTER, 22, false));

            ////Table 表格第四行
            //firstXwpfTable.GetRow(3).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "土建单位", ParagraphAlignment.CENTER, 22, true));
            //firstXwpfTable.GetRow(3).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, $"{Data.Builder.Name}", ParagraphAlignment.CENTER, 22, false));
            //firstXwpfTable.GetRow(3).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "检查情况", ParagraphAlignment.CENTER, 22, true));
            //firstXwpfTable.GetRow(3).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, $"{Data.Builder.Name}", ParagraphAlignment.CENTER, 22, false));




            #endregion


            #region 文档第二个表格对象实例
            //创建文档中的表格对象实例
            XWPFTable secondXwpfTable = document.CreateTable(1, 6);//显示的行列数rows:1行,cols:6列
            secondXwpfTable.Width = 5200;//总宽度
            secondXwpfTable.SetColumnWidth(0, 700); /* 设置列宽 */
            secondXwpfTable.SetColumnWidth(1, 1000); /* 设置列宽 */
            secondXwpfTable.SetColumnWidth(2, 700); /* 设置列宽 */
            secondXwpfTable.SetColumnWidth(3, 1100); /* 设置列宽 */
            secondXwpfTable.SetColumnWidth(4, 700); /* 设置列宽 */
            secondXwpfTable.SetColumnWidth(5, 1000); /* 设置列宽 */

            //Table 表格第一行展示...后面的都是一样，只改变GetRow中的行数
            secondXwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, secondXwpfTable, "材料名称", ParagraphAlignment.CENTER, 22, true));
            secondXwpfTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, secondXwpfTable, $"{Data.MarerialName}", ParagraphAlignment.CENTER, 22, false));
            secondXwpfTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, secondXwpfTable, "规格", ParagraphAlignment.CENTER, 22, true));
            secondXwpfTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, $"{Data.MaterialSpec}", ParagraphAlignment.CENTER, 22, false));
            secondXwpfTable.GetRow(0).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, secondXwpfTable, "数量", ParagraphAlignment.CENTER, 22, true));
            secondXwpfTable.GetRow(0).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, secondXwpfTable, $"{Data.MarerialCount}", ParagraphAlignment.CENTER, 22, false));
            #endregion

            #region 文档第三个表格对象实例（遍历表格项）
            //创建文档中的表格对象实例
            XWPFTable thirdXwpfTable = document.CreateTable(Count + 2, 9);//显示的行列数rows:n+1行,cols:9列
            thirdXwpfTable.Width = 5200;//总宽度
            thirdXwpfTable.SetColumnWidth(0, 500); /* 设置列宽 */
            thirdXwpfTable.SetColumnWidth(1, 500); /* 设置列宽 */
            thirdXwpfTable.SetColumnWidth(2, 500); /* 设置列宽 */
            thirdXwpfTable.SetColumnWidth(3, 500); /* 设置列宽 */
            thirdXwpfTable.SetColumnWidth(4, 500); /* 设置列宽 */
            thirdXwpfTable.SetColumnWidth(5, 500); /* 设置列宽 */
            thirdXwpfTable.SetColumnWidth(6, 500); /* 设置列宽 */
            thirdXwpfTable.SetColumnWidth(7, 500); /* 设置列宽 */
            thirdXwpfTable.SetColumnWidth(8, 1200); /* 设置列宽 */

            thirdXwpfTable.GetRow(0).MergeCells(0, 8);//合并6列
            thirdXwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "接口维护记录", ParagraphAlignment.CENTER, 10, false));
            //遍历表格标题
            thirdXwpfTable.GetRow(1).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "序号", ParagraphAlignment.CENTER, 22, true, 10));
            thirdXwpfTable.GetRow(1).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "检查人员", ParagraphAlignment.CENTER, 22, true, 10));
            thirdXwpfTable.GetRow(1).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "检查情况", ParagraphAlignment.CENTER, 22, true, 10));
            thirdXwpfTable.GetRow(1).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "检查时间", ParagraphAlignment.CENTER, 22, true, 10));
            thirdXwpfTable.GetRow(1).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "土建单位", ParagraphAlignment.CENTER, 22, true, 10));
            thirdXwpfTable.GetRow(1).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "状况原因", ParagraphAlignment.CENTER, 22, true, 10));
            thirdXwpfTable.GetRow(1).GetCell(6).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "整改人", ParagraphAlignment.CENTER, 22, true, 10));
            thirdXwpfTable.GetRow(1).GetCell(7).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "整改时间", ParagraphAlignment.CENTER, 22, true, 10));
            thirdXwpfTable.GetRow(1).GetCell(8).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "整改说明", ParagraphAlignment.CENTER, 22, true, 10));
            //遍历数据
            var i = 2;
            foreach (var item in Data.ConstructInterfaceInfos)
            {
                thirdXwpfTable.GetRow(i).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, (i - 1).ToString(), ParagraphAlignment.CENTER, 22, false, 10));
                thirdXwpfTable.GetRow(i).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, item.Marker == null ? "" : $"{item.Marker.Name}", ParagraphAlignment.CENTER, 22, false, 10));
                thirdXwpfTable.GetRow(i).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, NpoiWordParagraphTextStyleHelper._.transformMarkType(item.MarkType), ParagraphAlignment.CENTER, 22, false, 10));
                thirdXwpfTable.GetRow(i).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, string.Format("{0:d}", item.MarkDate), ParagraphAlignment.CENTER, 22, false, 10));
                thirdXwpfTable.GetRow(i).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, item.Builder == null ? "" : $"{item.Builder.Name}", ParagraphAlignment.CENTER, 22, false, 10));
                thirdXwpfTable.GetRow(i).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, item.Reason == null ? "" : $"{item.Reason}", ParagraphAlignment.CENTER, 22, false, 10));
                thirdXwpfTable.GetRow(i).GetCell(6).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, item.Reformer == null ? "" : $"{item?.Reformer?.Name}", ParagraphAlignment.CENTER, 22, false, 10));
                thirdXwpfTable.GetRow(i).GetCell(7).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, string.Format("{0:d}", item.ReformDate), ParagraphAlignment.CENTER, 22, false, 10));
                thirdXwpfTable.GetRow(i).GetCell(8).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, item.ReformExplain == null ? "" : $"{item.ReformExplain}", ParagraphAlignment.CENTER, 22, false, 10));
                i++;
            }

            #endregion
            //向文档流中写入内容，生成word
            MemoryStream stream = new MemoryStream();
            document.Write(stream);
            var buf = stream.ToArray();
            stream = new MemoryStream(buf);
            return stream;
        }

        #endregion

    }
}

