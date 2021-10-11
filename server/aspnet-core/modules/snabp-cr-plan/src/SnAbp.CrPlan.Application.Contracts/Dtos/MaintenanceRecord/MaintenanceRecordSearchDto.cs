using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.MaintenanceRecord
{
    public class MaintenanceRecordSearchDto : PagedAndSortedResultRequestDto, IRepairTagKeyDto
    {
        /// <summary>
        /// 维修项标签
        /// </summary>
        public string RepairTag { get; set; }

        public Guid? OrganizationId { get; set; }
        public Guid? EquipmentId { get; set; }
        public Guid? RepairGroupId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        public bool IsAll { get; set; }

        public string RepairTagKey { get; set; }

        public List<Guid> WorkOrderIds { get; set; }
    }
}
