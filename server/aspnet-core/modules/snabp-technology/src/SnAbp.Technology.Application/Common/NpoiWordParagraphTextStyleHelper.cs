using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using SnAbp.Technology.enums;

namespace SnAbp.Technology
{
    public class NpoiWordParagraphTextStyleHelper
    {
        private static NpoiWordParagraphTextStyleHelper _exportHelper;

        public static NpoiWordParagraphTextStyleHelper _
        {
            get => _exportHelper ?? (_exportHelper = new NpoiWordParagraphTextStyleHelper());
            set => _exportHelper = value;
        }


        /// <summary>
        /// 创建word文档中的段落对象和设置段落文本的基本样式（字体大小，字体，字体颜色，字体对齐位置）
        /// </summary>
        /// <param name="document">document文档对象</param>
        /// <param name="fillContent">段落第一个文本对象填充的内容</param>
        /// <param name="isBold">是否加粗</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="fontFamily">字体</param>
        /// <param name="paragraphAlign">段落排列（左对齐，居中，右对齐）</param>
        /// <param name="isStatement">是否在同一段落创建第二个文本对象（解决同一段落里面需要填充两个或者多个文本值的情况，多个文本需要自己拓展，现在最多支持两个）</param>
        /// <param name="secondFillContent">第二次声明的文本对象填充的内容，样式与第一次的一致</param>
        /// <param name="fontColor">字体颜色--十六进制</param>
        /// <param name="isItalic">是否设置斜体（字体倾斜）</param>
        /// <returns></returns>
        public XWPFParagraph ParagraphInstanceSetting(XWPFDocument document, string fillContent, bool isBold, int fontSize, string fontFamily, ParagraphAlignment paragraphAlign, bool isStatement = false, string secondFillContent = "", string fontColor = "000000", bool isItalic = false)
        {
            XWPFParagraph paragraph = document.CreateParagraph();//创建段落对象
            paragraph.Alignment = paragraphAlign;//文字显示位置,段落排列（左对齐，居中，右对齐）

            XWPFRun xwpfRun = paragraph.CreateRun();//创建段落文本对象
            xwpfRun.IsBold = isBold;//文字加粗
            xwpfRun.SetText(fillContent);//填充内容
            xwpfRun.FontSize = fontSize;//设置文字大小
            xwpfRun.IsItalic = isItalic;//是否设置斜体（字体倾斜）
            xwpfRun.SetColor(fontColor);//设置字体颜色--十六进制
            xwpfRun.SetFontFamily(fontFamily, FontCharRange.None); //设置标题样式如：（微软雅黑，隶书，楷体）根据自己的需求而定

            if (!isStatement) return paragraph;

            XWPFRun secondXwpfRun = paragraph.CreateRun();//创建段落文本对象
            secondXwpfRun.IsBold = isBold;//文字加粗
            secondXwpfRun.SetText(secondFillContent);//填充内容
            secondXwpfRun.FontSize = fontSize;//设置文字大小
            secondXwpfRun.IsItalic = isItalic;//是否设置斜体（字体倾斜）
            secondXwpfRun.SetColor(fontColor);//设置字体颜色--十六进制
            secondXwpfRun.SetFontFamily(fontFamily, FontCharRange.None); //设置标题样式如：（微软雅黑，隶书，楷体）根据自己的需求而定


            return paragraph;
        }

        /// <summary>  
        /// 创建Word文档中表格段落实例和设置表格段落文本的基本样式（字体大小，字体，字体颜色，字体对齐位置）
        /// </summary>  
        /// <param name="document">document文档对象</param>  
        /// <param name="table">表格对象</param>  
        /// <param name="fillContent">要填充的文字</param>  
        /// <param name="paragraphAlign">段落排列（左对齐，居中，右对齐）</param>
        /// <param name="textPosition">设置文本位置（设置两行之间的行间,从而实现表格文字垂直居中的效果），从而实现table的高度设置效果  </param>
        /// <param name="isBold">是否加粗（true加粗，false不加粗）</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="fontColor">字体颜色--十六进制</param>
        /// <param name="isItalic">是否设置斜体（字体倾斜）</param>
        /// <returns></returns>  
        public XWPFParagraph SetTableParagraphInstanceSetting(XWPFDocument document, XWPFTable table, string fillContent, ParagraphAlignment paragraphAlign, int textPosition = 24, bool isBold = false, int fontSize = 12, string fontColor = "000000", bool isItalic = false)
        {
            var para = new CT_P();
            //设置单元格文本对齐
            para.AddNewPPr().AddNewTextAlignment();


            XWPFParagraph paragraph = new XWPFParagraph(para, table.Body);//创建表格中的段落对象
            paragraph.Alignment = paragraphAlign;//文字显示位置,段落排列（左对齐，居中，右对齐）
            paragraph.VerticalAlignment = TextAlignment.CENTER;//字体垂直对齐
            //paragraph.FontAlignment =Convert.ToInt32(ParagraphAlignment.CENTER);
            //字体在单元格内显示位置与 paragraph.Alignment效果相似

            XWPFRun xwpfRun = paragraph.CreateRun();//创建段落文本对象
            xwpfRun.SetText(fillContent);//内容填充
            xwpfRun.FontSize = fontSize;//字体大小
            xwpfRun.SetColor(fontColor);//设置字体颜色--十六进制
            xwpfRun.IsItalic = isItalic;//是否设置斜体（字体倾斜）
            xwpfRun.IsBold = isBold;//是否加粗
            xwpfRun.SetFontFamily("宋体", FontCharRange.None);//设置字体（如：微软雅黑,华文楷体,宋体）
            return paragraph;
        }

        public string transformMarkType(MarkType Type)
        {
            string typeName = null;

            if (Type == MarkType.NoCheck)
            {
                typeName = "未检查";
            }
            if (Type == MarkType.Qualified)
            {
                typeName = "合格";
            }
            if (Type == MarkType.NoQualified)
            {
                typeName = "不合格";
            }
            return typeName;
        }
        public string transformConstructType(ConstructType Type)
        {
            string typeName = null;

            if (Type == ConstructType.Civil)
            {
                typeName = "土建工程";
            }
            if (Type == ConstructType.Electric)
            {
                typeName = "四电工程";
            }
            return typeName;
        }

    }
}
//runX.setText("舜发于畎亩之中， 傅说举于版筑之间， 胶鬲举于鱼盐之中， 管夷吾举于士...");
//runX.setBold(false);//加粗
//runX.setCapitalized(false);//我也不知道这个属性做啥的
////runX.setCharacterSpacing(5);//这个属性报错
//runX.setColor("BED4F1");//设置颜色--十六进制
//runX.setDoubleStrikethrough(false);//双删除线
//runX.setEmbossed(false);//浮雕字体----效果和印记（悬浮阴影）类似
////runX.setFontFamily("宋体");//字体
//runX.setFontFamily("华文新魏", FontCharRange.cs);//字体，范围----效果不详
//runX.setFontSize(14);//字体大小
//runX.setImprinted(false);//印迹（悬浮阴影）---效果和浮雕类似
//runX.setItalic(false);//斜体（字体倾斜）
////runX.setKerning(1);//字距调整----这个好像没有效果
//runX.setShadow(true);//阴影---稍微有点效果（阴影不明显）
////runX.setSmallCaps(true);//小型股------效果不清楚
////runX.setStrike(true);//单删除线（废弃）
//runX.setStrikeThrough(false);//单删除线（新的替换Strike）
////runX.setSubscript(VerticalAlign.SUBSCRIPT);//下标(吧当前这个run变成下标)---枚举
////runX.setTextPosition(20);//设置两行之间的行间距//runX.setUnderline(UnderlinePatterns.DASH_LONG);//各种类型的下划线（枚举）
////runX0.addBreak();//类似换行的操作（html的  br标签）
//runX0.addTab();//tab键
//runX0.addCarriageReturn();//回车键