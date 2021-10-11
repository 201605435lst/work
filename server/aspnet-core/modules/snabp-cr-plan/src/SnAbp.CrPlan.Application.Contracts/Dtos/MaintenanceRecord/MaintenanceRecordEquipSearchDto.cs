using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.MaintenanceRecord
{
    public class MaintenanceRecordEquipSearchDto : PagedAndSortedResultRequestDto, IRepairTagKeyDto
    {
        /// <summary>
        /// 维修项标签
        /// </summary>
        public string RepairTag { get; set; }

        /// <summary>
        /// 维护单位
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public Guid? EquipTypeId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public Guid? EquipNameId { get; set; }

        /// <summary>
        /// 安装地点
        /// </summary>
        public Guid? InstallationSiteId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        /// <summary>
        /// 关键字  设备型号，编码 
        /// </summary>
        public string Keywords { get; set; }

        public string RepairTagKey { get; set; }

        public Guid? RailwayId { get; set; }
        public Guid? StationId { get; set; }
    }
}
