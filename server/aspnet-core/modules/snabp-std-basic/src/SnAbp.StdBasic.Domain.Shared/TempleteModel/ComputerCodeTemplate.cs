using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic
{
  public  class ComputerCodeTemplate
    {
        public int Index { get; set; }
        /// <summary>
        /// 电算代号
        /// </summary>
        public  string Code { get; set; }
        /// <summary>
        /// 名称及规格
        /// </summary>
        public  string Name { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public  string Unit { get; set; }
        /// <summary>
        /// 电算代号类型
        /// </summary>
        public string Type { get; set; }
       
        /// <summary>
        /// 单位重量
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
