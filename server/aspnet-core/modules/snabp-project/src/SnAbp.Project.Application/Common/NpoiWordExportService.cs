using Microsoft.AspNetCore.Hosting;
using NPOI.XWPF.UserModel;
using SnAbp.Project.Dtos;
using SnAbp.Project.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace SnAbp.Project
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

        #region 生成Archives    word

        /// <summary>
        ///  生成word案卷文档
        /// </summary>
        /// <param name="savePath">保存路径</param>
        public static Stream SaveArchivesWordFile(List<Archives> DataList, int Count, string FileName, string TableName, string WorkOrganization)
        {
            string workFileName = FileName;
            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();

            //文本标题
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, workFileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);

            //TODO:这里一行需要显示两个文本
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"工程名称：{TableName}", false, 16, "宋体", ParagraphAlignment.LEFT, true, $"                                  施工单位：{WorkOrganization}"), 1);

            #region 文档第表格对象实例（遍历表格项）
            //创建文档中的表格对象实例
            XWPFTable xwpfTable = document.CreateTable(Count + 2, 7);//显示的行列数rows:8行,cols:4列
            xwpfTable.Width = 5200;//总宽度
            xwpfTable.SetColumnWidth(0, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(1, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(2, 1000); /* 设置列宽 */
            xwpfTable.SetColumnWidth(3, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(4, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(5, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(6, 1400); /* 设置列宽 */

            //遍历表格标题
            xwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "序号", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "案卷编号", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "案卷题名", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "编制日期", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "卷内张数", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "份数", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(6).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "备注", ParagraphAlignment.CENTER, 22, true));
            var i = 1;
            //遍历数据
            foreach (var item in DataList)
            {
                xwpfTable.GetRow(i).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{i}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.ArchivesFilesCode}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Name}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Date.ToString("yyyy-MM-dd")}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Page}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Copies}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(6).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Remark}", ParagraphAlignment.CENTER, 22, false));
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

        #region 生成Dossier    word

        /// <summary>
        ///  生成word案卷文档
        /// </summary>
        /// <param name="savePath">保存路径</param>
        public static Stream SaveDossierWordFile(List<Dossier> DataList, int Count, string FileName, string TableName, string WorkOrganization)
        {
            string workFileName = FileName;
            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();

            //文本标题
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, workFileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);

            //TODO:这里一行需要显示两个文本
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"工程名称：{TableName}", false, 10, "宋体", ParagraphAlignment.LEFT, true, $"                                  施工单位：{WorkOrganization}"), 1);

            #region 文档第表格对象实例（遍历表格项）
            //创建文档中的表格对象实例
            XWPFTable xwpfTable = document.CreateTable(Count + 2, 7);//显示的行列数rows:8行,cols:4列
            xwpfTable.Width = 5200;//总宽度
            xwpfTable.SetColumnWidth(0, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(1, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(2, 1000); /* 设置列宽 */
            xwpfTable.SetColumnWidth(3, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(4, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(5, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(6, 1400); /* 设置列宽 */

            //遍历表格标题
            xwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "序号", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "文件编号", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "文件题名", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "文件日期", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "文件分类", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "页号", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(6).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "备注", ParagraphAlignment.CENTER, 22, true));
            var i = 1;
            //遍历数据
            foreach (var item in DataList)
            {
                xwpfTable.GetRow(i).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{i}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Code}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Name}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Date.ToString("yyyy-MM-dd")}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.FileCategory.Name}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Page}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(6).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Remark}", ParagraphAlignment.CENTER, 22, false));
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

