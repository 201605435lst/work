using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Alarm.TemplateModel
{
    public class ImportEquipmentIdModel
    {
        public int Index { get; set; }

        /// <summary>
        /// 安装位置名称
        /// </summary>
        public string InstallationSiteName { get; set; }

        /// <summary>
        /// 设备分组
        /// </summary>
        public string EquipmentGroup { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 第三方 ID 3424235,2342342 以英文逗号隔开
        /// </summary>
        public string EquipmentThirdIds { get; set; }
    }
}
