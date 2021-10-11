using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic
{
  public  class ProcessTemplateTemplate
    {
        public int Index { get; set; }
        /// <summary>
        /// 父级工序编码
        /// </summary>
        public string ParentCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public  string Name { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 工作项单位
        /// </summary>
        public  string Unit { get; set; }
        /// <summary>
        /// 工期
        /// </summary>
        public decimal Duration { get; set; }
        /// <summary>
        /// 工期单位
        /// </summary>
        public string DurationUnit { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 工序类别
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 前置任务Code
        /// </summary>
        public string PrepositionCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public  string Remark { get; set; }
    }
}
