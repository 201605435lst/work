using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    /// <summary>
    /// 检修设备dto
    /// </summary>
    public class MaintenanceRecordEquipDto : IRepairTagDto
    {
        /// <summary>
        /// 派工作业Id
        /// </summary>
        public Guid WorkOrderId { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipType { get; set; }

        public Guid? EquipNameId { get; set; }
        /// <summary>
        /// 设备名称/类别
        /// </summary>
        public string EquipName { get; set; }

        /// <summary>
        /// 关联设备Id
        /// </summary>
        public Guid? ResourceEquipId { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelNumber { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipModelCode { get; set; }

        /// <summary>
        /// 维护单位
        /// </summary>
        //public string MaintenanceOrg { get; set; }

        /// <summary>
        /// 安装地点
        /// </summary>
        public string InstallationSite { get; set; }

        public Guid? RepairTagId { get; set; }

        public DataDictionaryDto RepairTag { get; set; }

        public List<Guid> MaintenanceRecordRltWorkOrders { get; set; } = new List<Guid>();
    }
}
