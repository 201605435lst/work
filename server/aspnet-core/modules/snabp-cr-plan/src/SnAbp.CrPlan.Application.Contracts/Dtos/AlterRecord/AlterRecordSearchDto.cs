using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.AlterRecord
{
    public class AlterRecordSearchDto : PagedAndSortedResultRequestDto, IRepairTagKeyDto
    {
        /// <summary>
        /// 审批状态
        /// </summary>
        public YearMonthPlanState? State { get; set; }

        /// <summary>
        /// 变更类型
        /// </summary>
        public SelectablePlanType? AlterType { get; set; }

        /// <summary>
        /// 所属组织机构id
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// 关键字  变更原因模糊搜索
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanStartTime { get; set; }
       
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndTime { get; set; }

        /// <summary>
        /// 变更开始时间
        /// </summary>
        public DateTime AlterStartTime { get; set; }

        /// <summary>
        /// 变更结束时间
        /// </summary>
        public DateTime AlterEndTime { get; set; }

        /// <summary>
        /// 是否全部返回
        /// </summary>
        public bool IsAll { get; set; }
        public string RepairTagKey { get ; set ; }
    }
}
