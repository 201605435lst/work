using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic
{
  public  class QuotaTemplate
    {
        public int Index { get; set; }

        /// <summary>
        /// 定额名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 定额编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// 人工费
        /// </summary>
        public float LaborCost { get; set; }
        /// <summary>
        /// 材料费
        /// </summary>
        public float MaterialCost { get; set; }
        /// <summary>
        /// 机械使用费
        /// </summary>
        public float MachineCost { get; set; }

       
        /// <summary>
        /// 专业
        /// </summary>
        public string SpecialtyName { get; set; }
        /// <summary>
        ///标准编号 Code 来源于数据字典
        /// </summary>
        public string StandardCode { get; set; }

        /// <summary>
        /// 行政区划名称
        /// </summary>
        public string AreaName { get; set; }


        /// <summary>
        /// 定额分类名称
        /// </summary>
        public string QuotaCategoryName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
