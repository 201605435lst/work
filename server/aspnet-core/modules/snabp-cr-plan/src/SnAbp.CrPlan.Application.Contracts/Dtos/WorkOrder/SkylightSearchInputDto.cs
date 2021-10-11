using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 单位待办、已派工的模块查询条件实体
    /// </summary>

    public class SkylightSearchInputDto : PagedAndSortedResultRequestDto, IRepairTagKeyDto
    {
        /// <summary>
        /// 计划日期(开始，默认本月1号)
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
        public List<Guid> InstallationSiteIds { get; set; }

        /// <summary>
        /// 作业单位(默认为用户组织机构)
        /// </summary>
        public Guid WorkUnitId { get; set; }

        /// <summary>
        /// 计划类型(垂直、综合、点外)
        /// 默认为全部
        /// </summary>
        public PlanType PlanType { get; set; }

        /// <summary>
        /// 其他（作业内容、作业位置里程）
        /// </summary>
        public string OtherConditions { get; set; }

        public string RepairTagKey { get ; set ; }
    }
}
