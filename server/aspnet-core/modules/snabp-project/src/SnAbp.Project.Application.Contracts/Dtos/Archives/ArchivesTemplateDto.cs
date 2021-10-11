using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Project.Dtos
{
    public class ArchivesTemplateDto
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 宗号
        /// </summary>
        public string 宗号 { get; set; }
        /// <summary>
        /// 档号
        /// </summary>
        public string 档号 { get; set; }
        /// <summary>
        /// 案卷号
        /// </summary>
        public string 案卷号 { get; set; }
        /// <summary>
        /// 案卷题名
        /// </summary>
        public string 案卷题名 { get; set; }
        /// <summary>
        /// 编制日期
        /// </summary>
        public DateTime 编制日期 { get; set; }
        /// <summary>
        /// 编制单位
        /// </summary>
        public string 编制单位 { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public string 年度 { get; set; }
      

       
        /// <summary>
        /// 页数
        /// </summary>
        public int 页数 { get; set; }
        /// <summary>
        /// 份数
        /// </summary>
        public int 份数 { get; set; }
        /// <summary>
        /// 案卷分类
        /// </summary>
        public string 案卷分类 { get; set; }
        /// <summary>
        /// 密级
        /// </summary>
        public int 密级 { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string 备注 { get; set; }

    }
}
