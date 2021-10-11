using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic
{
    public class StandardEquipmentModel
    {
        public int Index { get; set; }

        /// <summary>
        /// 设备编码（标准设备表）
        /// </summary>
        public string CSRGCode { get; set; }

        /// <summary>
        /// 产品分类
        /// </summary>
        public string IFDParent { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string IFD { get; set; }

        /// <summary>
        /// 产品型号（标准设备表）
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 产品厂商（标准设备表）
        /// </summary>
        public string Manufacture { get; set; }

        /// <summary>
        /// 组件
        /// </summary>
        public string Element { get; set; }

        /// <summary>
        /// 录入单位
        /// </summary>
        public string EnterDepartment { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateRole { get; set; }
    }
}
