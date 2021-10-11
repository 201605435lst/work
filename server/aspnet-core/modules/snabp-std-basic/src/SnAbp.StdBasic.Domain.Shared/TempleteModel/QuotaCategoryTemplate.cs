using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic
{
    public class QuotaCategoryTemplate
    {
        public int Index { get; set; }
        /// <summary>
        /// 定额分类中的章节名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string SpecialtyName { get; set; }
        /// <summary>
        ///标准编号 Code 来源于数据字典
        /// </summary>
        public string StandardCode { get; set; }

        /// <summary>
        /// 定额的内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 行政区划名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 父级分类编码
        /// </summary>
        public string ParentCode { get; set; }
    }
}
