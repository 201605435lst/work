using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.AlterRecord
{
    public class AlterRecordDto : EntityDto<Guid>, IRepairTagDto
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
        /// 申请车间 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public YearMonthPlanState State { get; set; }

        /// <summary>
        /// 变更类型
        /// </summary>
        public SelectablePlanType AlterType { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
