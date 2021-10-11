using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Project.Dtos
{
   public class DossierTemplateDto
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 文件编号
        /// </summary>
        public string 文件编号 { get; set; }
        /// <summary>
        /// 文件题名
        /// </summary>
        public string 文件题名 { get; set; }
        /// <summary>
        /// 页号
        /// </summary>
        public int 页号 { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public string 责任人 { get; set; }

        /// <summary>
        /// 文件日期
        /// </summary>
        public DateTime 文件日期 { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string 文件分类 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string 备注 { get; set; }

    }
}
