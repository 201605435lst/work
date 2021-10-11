using ICSharpCode.SharpZipLib.Zip;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SnAbp.Utils.WordHelper
{
    public static class WordHelper
    {
        public static Stream CreateDocxTable()
        {
            XWPFDocument document = new XWPFDocument();

            CreateDocx(document);

            MemoryStream stream = new MemoryStream();
            document.Write(stream);
            var buf = stream.ToArray();
            stream = new MemoryStream(buf);
            return stream;

        }

        public static Stream CreateDocxRAR()
        {
            byte[] buffer = new byte[6500];
            Stream returnStream;// = new MemoryStream();
            var zipMs = new MemoryStream();

            Dictionary<string, Stream> StreamList = new Dictionary<string, Stream>();

            StreamList.Add("测试1.docx", CreateDocxTable());
            StreamList.Add("测试2.docx", CreateDocxTable());

            using (ZipOutputStream zipStream = new ZipOutputStream(zipMs))
            {
                zipStream.SetLevel(9);//压缩等级1-9
                //zipStream.Password = "";//压缩密码
                foreach (var kv in StreamList)
                {
                    using (var streamInput = kv.Value)
                    {
                        var fileName = new ZipEntry(kv.Key);
                        fileName.DateTime = DateTime.Now;
                        fileName.IsUnicodeText = true;
                        zipStream.PutNextEntry(fileName);
                        while (true)
                        {
                            var readCount = streamInput.Read(buffer, 0, buffer.Length);
                            if (readCount > 0)
                            {
                                zipStream.Write(buffer, 0, readCount);
                            }
                            else
                            {
                                break;
                            }
                        }
                        zipStream.Flush();
                    }
                }
                zipStream.Finish();
                //zipMs.Position = 0;
                //zipMs.CopyTo(returnStream, 5600);
            }

            var bytes = zipMs.ToArray();
             returnStream = new MemoryStream(bytes);


            //FileStream aFile = new FileStream("C:\\222222222\\aasd.zip", FileMode.Create);
            //var bytes = zipMs.ToArray();
            //aFile.Seek(0, SeekOrigin.Begin);
            //aFile.Write(bytes, 0, bytes.Length);
            //zipMs.Close();
            //aFile.Close();
            return returnStream;
        }


        public static void CreateDocx(XWPFDocument document)
        {
            for (var i = 0;i < 2;i++)
            {
                XWPFParagraph paragraph = document.CreateParagraph(); //创建段落
                paragraph.Alignment = ParagraphAlignment.CENTER;//居中显示
                XWPFRun run = paragraph.CreateRun();
                run.FontFamily = "宋体"; //设置字体
                run.FontSize = 18; //设置字体大小
                run.IsBold = true; //字体加粗
                run.SetText("施\n工\n维\n修\n工\n作\n票");

                paragraph = document.CreateParagraph(); //创建段落
                paragraph.Alignment = ParagraphAlignment.LEFT;
                run = paragraph.CreateRun();
                run.FontSize = 12; //设置字体大小 小四
                run.SetText("命令号：\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                run.AppendText("\n\n\n\n\n\n\n填报时间：\n\n\n\n\n年\n\n\n月\n\n\n日");

                //创建表
                XWPFTable table = document.CreateTable(1, 7);//创建12行7列表
                table.RemoveRow(0);//去掉第一行,下面重新建行
                table.SetColumnWidth(0, 1059);//设置列的宽度
                table.SetColumnWidth(1, 1058);
                table.SetColumnWidth(2, 2117);
                table.SetColumnWidth(3, 530);
                table.SetColumnWidth(4, 1587);
                table.SetColumnWidth(5, 530);
                table.SetColumnWidth(6, 2117);
                CT_Tbl ctTbl = document.Document.body.GetTblArray()[i]; //获取文档第i张表
                                                                        //设置表水平居中
                ctTbl.AddNewTblPr().jc = new CT_Jc();
                ctTbl.AddNewTblPr().jc.val = ST_Jc.center;
                //设置表宽
                ctTbl.AddNewTblPr().AddNewTblW().w = "9000";
                ctTbl.AddNewTblPr().AddNewTblW().type = ST_TblWidth.dxa;

                //创建第一行
                CT_Row row = new CT_Row();
                XWPFTableRow tableRow = new XWPFTableRow(row, table);
                tableRow.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)480;//设置行高
                row.AddNewTrPr().AddNewTrHeight().val = (ulong)480;//设置行高（这两行都得有）
                table.AddRow(tableRow);//将行添加到table中
                //合并单元格
                XWPFTableCell cell = tableRow.CreateCell();//创建一个单元格,创建单元格时就创建了一个CT_P
                CT_Tc cttc = cell.GetCTTc();
                CT_TcPr ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "4";
                CT_VerticalJc va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "\n命令号：";

                cell = tableRow.CreateCell();//创建一个单元格
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "3";
                va = ctTcPr.AddNewVAlign();
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.right;//单元格内容水平居右显示
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "填报时间：\n\n\n\n\n年\n\n\n月\n\n\n日\n";


                //创建第二行
                CT_Row row1 = new CT_Row();
                XWPFTableRow tableRow1 = new XWPFTableRow(row1, table);
                tableRow1.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)480;//设置行高
                row1.AddNewTrPr().AddNewTrHeight().val = (ulong)480;//设置行高（这两行都得有）
                table.AddRow(tableRow1);//将行添加到table中
                                        //合并单元格
                cell = tableRow1.CreateCell();//创建一个单元格,创建单元格时就创建了一个CT_P
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "2";//合并2列
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "\n作业名称";

                cell = tableRow1.CreateCell();//创建一个单元格
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "5";//合并5列
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "\n核心机房郑西10G传输主扩架调整";

                //创建第二行
                CT_Row row2 = new CT_Row();
                XWPFTableRow tableRow2 = new XWPFTableRow(row2, table);
                tableRow2.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)480;//设置行高
                row2.AddNewTrPr().AddNewTrHeight().val = (ulong)480;//设置行高（这两行都得有）
                table.AddRow(tableRow2);//将行添加到table中
                                        //合并单元格
                cell = tableRow2.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "2";//合并2列
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "\n作业地点";

                cell = tableRow2.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "2";//合并2列
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "\n技术支持中心";

                cell = tableRow2.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "2";//合并2列
                va = ctTcPr.AddNewVAlign();
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "施工维修等级";

                cell = tableRow2.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                va = ctTcPr.AddNewVAlign();
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "天窗内I级维修";

                //创建第三行
                CT_Row row3 = new CT_Row();
                XWPFTableRow tableRow3 = new XWPFTableRow(row3, table);
                table.AddRow(tableRow3);//将行添加到table中

                cell = tableRow3.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "7";//合并2列
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cell.SetBorderBottom(XWPFTable.XWPFBorderType.NONE, 0, 0, null);
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "\n作业内容：";

                //创建第四行
                CT_Row row4 = new CT_Row();
                XWPFTableRow tableRow4 = new XWPFTableRow(row4, table);
                tableRow4.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)3000;//设置行高
                row4.AddNewTrPr().AddNewTrHeight().val = (ulong)3000;//设置行高（这两行都得有）
                table.AddRow(tableRow4);//将行添加到table中

                //table.GetRow(3).MergeCells(0, 7);//从第一列起,合并7列           -- 待优化代码

                cell = tableRow4.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "7";//合并2列
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                string parp = "将郑西客专10G主架11槽OL1板3口的电路调整至该设备13槽位2口。</ br>拔出郑西客专调度楼10G主架11槽位OL1板，插入郑西10G扩6槽位。</ br>将郑西客专调度楼10G扩架4槽位OL4板拔出，插入郑西客专调度楼10G主架11槽位。";
                table.GetRow(3).GetCell(0).SetParagraph(SetTableParagraphInstanceSetting(table, parp, ParagraphAlignment.LEFT, false));


                //创建第五行
                CT_Row row5 = new CT_Row();
                XWPFTableRow tableRow5 = new XWPFTableRow(row5, table);
                tableRow5.GetCTRow().AddNewTrPr().AddNewTrHeight().val = 750;//设置行高
                row5.AddNewTrPr().AddNewTrHeight().val = 750;//设置行高（这两行都得有）
                table.AddRow(tableRow5);//将行添加到table中

                cell = tableRow5.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                va = ctTcPr.AddNewVAlign();
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "影响范围";

                cell = tableRow5.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "6";
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "\n影响范围";

                //创建第六行
                CT_Row row6 = new CT_Row();
                XWPFTableRow tableRow6 = new XWPFTableRow(row6, table);
                tableRow6.GetCTRow().AddNewTrPr().AddNewTrHeight().val = 700;//设置行高
                row6.AddNewTrPr().AddNewTrHeight().val = 700;//设置行高（这两行都得有）
                table.AddRow(tableRow6);//将行添加到table中

                cell = tableRow6.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                va = ctTcPr.AddNewVAlign();
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "施工时间";

                cell = tableRow6.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "6";
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                parp = "计划时间\n\n\n\n年\n\n月\n\n日\n\n时\n\n分至\n\n\n月\n\n日\n\n时    分</ br>实际时间\n\n\n\n年\n\n月\n\n日\n\n时\n\n分至\n\n\n月\n\n日\n\n时\n\n分";
                table.GetRow(5).GetCell(1).SetParagraph(SetTableParagraphInstanceSetting(table, parp, ParagraphAlignment.LEFT, false));


                //创建第七行
                CT_Row row7 = new CT_Row();
                XWPFTableRow tableRow7 = new XWPFTableRow(row7, table);
                table.AddRow(tableRow7);//将行添加到table中

                cell = tableRow7.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "7";
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cell.SetBorderBottom(XWPFTable.XWPFBorderType.NONE, 0, 0, null);
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "\n安全技术措施及注意事项：";


                //创建第八行
                CT_Row row8 = new CT_Row();
                XWPFTableRow tableRow8 = new XWPFTableRow(row8, table);
                tableRow8.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)4000;//设置行高
                row8.AddNewTrPr().AddNewTrHeight().val = (ulong)4000;//设置行高（这两行都得有）
                table.AddRow(tableRow8);//将行添加到table中

                cell = tableRow8.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "7";//合并2列
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                parp = "将郑西客专10G主架11槽OL1板3口的电路调整至该设备13槽位2口。</ br>拔出郑西客专调度楼10G主架11槽位OL1板，插入郑西10G扩6槽位。</ br>将郑西客专调度楼10G扩架4槽位OL4板拔出，插入郑西客专调度楼10G主架11槽位。";
                table.GetRow(7).GetCell(0).SetParagraph(SetTableParagraphInstanceSetting(table, parp, ParagraphAlignment.LEFT, false));

                //创建第九行
                CT_Row row9 = new CT_Row();
                XWPFTableRow tableRow9 = new XWPFTableRow(row9, table);
                tableRow9.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)500;//设置行高
                row9.AddNewTrPr().AddNewTrHeight().val = (ulong)500;//设置行高（这两行都得有）
                table.AddRow(tableRow9);//将行添加到table中
                                        //合并单元格
                cell = tableRow9.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "2";//合并2列
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "制表人";

                cell = tableRow9.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "作业负责人";

                cell = tableRow9.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "2";//合并2列
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "技术科审核人";

                cell = tableRow9.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "2";//合并2列
                va = ctTcPr.AddNewVAlign();
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "安调科审核人";

                //创建第十行
                CT_Row row10 = new CT_Row();
                XWPFTableRow tableRow10 = new XWPFTableRow(row10, table);
                tableRow10.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)500;//设置行高
                row10.AddNewTrPr().AddNewTrHeight().val = (ulong)500;//设置行高（这两行都得有）
                table.AddRow(tableRow10);//将行添加到table中
                                         //合并单元格
                cell = tableRow10.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "2";//合并2列
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "***";

                cell = tableRow10.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "***";

                cell = tableRow10.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "2";//合并2列
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "***";

                cell = tableRow10.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "2";//合并2列
                va = ctTcPr.AddNewVAlign();
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;//单元格内容水平居中显示
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "***";

                //创建第十一行
                CT_Row row11 = new CT_Row();
                XWPFTableRow tableRow11 = new XWPFTableRow(row11, table);
                tableRow11.GetCTRow().AddNewTrPr().AddNewTrHeight().val = (ulong)500;//设置行高
                row11.AddNewTrPr().AddNewTrHeight().val = (ulong)500;//设置行高（这两行都得有）
                table.AddRow(tableRow11);//将行添加到table中

                cell = tableRow11.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "7";
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cell.SetBorderBottom(XWPFTable.XWPFBorderType.NONE, 0, 0, null);
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "工作完成情况：";

                //创建第十二行
                CT_Row row12 = new CT_Row();
                XWPFTableRow tableRow12 = new XWPFTableRow(row12, table);
                table.AddRow(tableRow12);//将行添加到table中

                cell = tableRow12.CreateCell();
                cttc = cell.GetCTTc();
                ctTcPr = cttc.AddNewTcPr();
                ctTcPr.gridSpan = new CT_DecimalNumber();
                ctTcPr.gridSpan.val = "7";
                va = ctTcPr.AddNewVAlign();
                va.val = ST_VerticalJc.center;//垂直居中
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.right;//单元格内容水平居右显示
                cttc.GetPList()[0].AddNewR().AddNewT().Value = "年\n\n\n\n月\n\n\n\n日\n\n\n\n\n";

                paragraph = document.CreateParagraph(); //创建段落
                paragraph.Alignment = ParagraphAlignment.LEFT;
                run = paragraph.CreateRun();
                run.SetText("注：该工作票用于通信施工和I级维修作业项目；由段技术科、安调科会签批准后执行；施工结束后作业主体部门填写工作完成情况并保存，同时通过段施工管理系统反馈完成情况。");
            }
            
        }

        public static XWPFParagraph SetTableParagraphInstanceSetting(XWPFTable table, string fillContent, ParagraphAlignment paragraphAlign, bool isBold)
        {
            var para = new CT_P();
            string[] strArr_FG = null;
            //设置单元格文本对齐
            para.AddNewPPr().AddNewTextAlignment();

            XWPFParagraph paragraph = new XWPFParagraph(para, table.Body);//创建表格中的段落对象
            //XWPFParagraph paragraph = document.CreateParagraph(); //创建段落
            paragraph.Alignment = paragraphAlign;//文字显示位置,段落排列（左对齐，居中，右对齐）
            if (fillContent.IndexOf("</ br>") > 0)
            {
                strArr_FG = Regex.Split(fillContent, "</ br>");//根据换行符分隔字符串
                XWPFRun xwpfRun = paragraph.CreateRun();//创建段落文本对象
                for (var i = 0; i < strArr_FG.Length; i++)
                {
                    if (i == 0)
                    {
                        xwpfRun.SetText(strArr_FG[0]);
                    }
                    else
                    {
                        xwpfRun.AddCarriageReturn();
                        xwpfRun.AppendText(strArr_FG[i]);
                        xwpfRun.SetFontFamily("宋体", FontCharRange.None);//设置字体（如：微软雅黑,华文楷体,宋体）
                    }
                }
            }
            else
            {
                XWPFRun xwpfRun = paragraph.CreateRun();//创建段落文本对象
                xwpfRun.SetText(fillContent);
                xwpfRun.IsBold = isBold;//文字加粗
                xwpfRun.SetFontFamily("宋体", FontCharRange.None);//设置字体（如：微软雅黑,华文楷体,宋体）
            }
            return paragraph;
        }

        private static void addTableRow(XWPFTable table, ulong height, string text, bool isStJc, bool isBlod)
        {
            CT_Row ctRow = new CT_Row();
            XWPFTableRow tableRow = new XWPFTableRow(ctRow, table);
            tableRow.GetCTRow().AddNewTrPr().AddNewTrHeight().val = height;//设置行高
            ctRow.AddNewTrPr().AddNewTrHeight().val = height;//设置行高（这两行都得有）
            table.AddRow(tableRow);//将行添加到table中
            XWPFTableCell cell = tableRow.CreateCell();//创建一个单元格,创建单元格时就创建了一个CT_P
            CT_Tc cttc = cell.GetCTTc();
            CT_TcPr ctTcPr = cttc.AddNewTcPr();
            CT_VerticalJc va = ctTcPr.AddNewVAlign();
            va.val = ST_VerticalJc.center;//垂直居中
            if (isStJc == true)
            {
                cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;
            }
            tableRow.GetCell(0).SetParagraph(SetTableParagraphInstanceSetting(table, text, ParagraphAlignment.LEFT, isBlod));
        }
  


   
    }
}
