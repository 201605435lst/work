using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic
{
  public  class QuotaItemTemplate
    {
        public int Index { get; set; }

        /// <summary>
        /// 电算代号
        /// </summary>
        public virtual string ComputerCodeCode { get; set; }

        /// <summary>
        /// 电算代号名称及规格
        /// </summary>
        public string ComputerCodeName { get; set; }

        /// <summary>
        /// 基础单价
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        /// 标准编号
        /// </summary>
        public string StandardCode { get; set; }

        /// <summary>
        /// 行政区划Name
        /// </summary>
        public string AreaName { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        public float Number { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
