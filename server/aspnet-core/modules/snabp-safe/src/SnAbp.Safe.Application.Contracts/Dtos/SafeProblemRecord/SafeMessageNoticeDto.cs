using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Safe.Dtos
{
   public class SafeMessageNoticeDto
    {
        //提交报告的人
        public string Name { get; set; }
        /// <summary>
        /// 汇报的类型
        /// </summary>
        public string RecordType { get; set; }
        /// <summary>
        /// 汇报的内容
        /// </summary>
        public string RecordContent { get; set; }
    }
}
