using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 派工作业、已完成的模块所使用查询条件实体
    /// </summary>
    public class WorkOrderSearchInputDto : PagedAndSortedResultRequestDto, IRepairTagKeyDto
    {
        /// <summary>
        /// 维修项标签
        /// </summary>
        public string RepairTag { get; set; }

        /// <summary>
        /// 计划日期(开始，默认本月1号)
        /// </summary>
        public DateTime StartPlanTime { get; set; }

        /// <summary>
        /// 计划日期（结束，默认本月最后一天）
        /// </summary>
        public DateTime EndPlanTime { get; set; }

        /// <summary>
        /// 作业机房(默认为null)
        /// </summary>
        public Guid InstallationSiteId { get; set; }

        /// <summary>
        /// 作业单位(默认为用户组织机构)
        /// </summary>
        public Guid WorkUnitId { get; set; }

        /// <summary>
        /// 其他（作业内容、作业位置里程）
        /// </summary>
        public string OtherConditions { get; set; }

        /// <summary>
        /// 是否为已完成模块
        /// true 只获取状态为已验收的派工单
        /// </summary>
        public bool IsDispatching { get; set; }

        public string RepairTagKey { get ; set ; }

        /// <summary>
        /// 维修等级
        /// </summary>
        public RepairLevel RepairLevel { get; set; }
    }
}
