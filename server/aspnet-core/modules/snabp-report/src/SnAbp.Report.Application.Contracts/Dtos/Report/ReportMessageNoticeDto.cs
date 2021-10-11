using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Report.Dtos
{
    public class ReportMessageNoticeDto
    {
        //提交报告的人
        public string name { get; set; }
        /// <summary>
        /// 汇报的类型
        /// </summary>
        public string reportType { get; set; }
    }
}
