using SnAbp.CrPlan.Dto.Worker;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 派工单实体
    /// 修改派工单使用
    /// </summary>
    public class WorkOrderAcceptanceDto : EntityDto<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 计划内容类型
        /// </summary>
        public WorkContentType? WorkContentType { get; set; }
        /// <summary>
        /// 相关设备列表
        /// </summary>
        public List<EquipmentPlanDetailUpdateDto> EquipmentList { get; set; }
        public string RepairTagKey { get ; set ; }
    }
}
