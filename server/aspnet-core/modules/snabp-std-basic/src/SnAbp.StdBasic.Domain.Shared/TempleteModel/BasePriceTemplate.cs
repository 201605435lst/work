using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic
{
    public class BasePriceTemplate
    {
        public int Index { get; set; }

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
        public string StandardCodeName { get; set; }
        /// <summary>
        /// 行政区划
        /// </summary>
        public string AreaName { get; set; }

    }
}
