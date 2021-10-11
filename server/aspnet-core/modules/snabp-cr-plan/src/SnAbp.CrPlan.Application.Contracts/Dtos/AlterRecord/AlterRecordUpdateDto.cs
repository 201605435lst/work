using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.AlterRecord
{
    public class AlterRecordUpdateDto : EntityDto<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 变更原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 原计划时间
        /// </summary>
        public DateTime PlanTime { get; set; }

        /// <summary>
        /// 申请变更时间
        /// </summary>
        public DateTime AlterTime { get; set; }

        /// <summary>
        /// 变更类型
        /// </summary>
        public SelectablePlanType AlterType { get; set; }

        /// <summary>
        /// 组织机构id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 变更关联
        /// </summary>
        public List<DailyPlanAlterCreateDto> DailyPlanAlters { get; set; } = new List<DailyPlanAlterCreateDto> { };
        public string RepairTagKey { get ; set ; }
    }
}
