using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 检修/验收表导出Dto
    /// </summary>
    public class RepairCheckExportDto
    {
        /// <summary>
        /// 年/月表
        /// </summary>
        public string PlanType { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 设备类型序号
        /// </summary>
        public int DeviceNumber { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }
        /// <summary>
        /// 维护项目
        /// </summary>
        public string WorkContent { get; set; }
        /// <summary>
        /// 维护项目序号
        /// </summary>
        public string WorkContentNumber { get; set; }
        /// <summary>
        /// 检修记录/验收情况
        /// </summary>
        public string RepairRecord { get; set; }
        /// <summary>
        /// 检修人/验收人
        /// </summary>
        public string RepairUser { get; set; }
    }
}
