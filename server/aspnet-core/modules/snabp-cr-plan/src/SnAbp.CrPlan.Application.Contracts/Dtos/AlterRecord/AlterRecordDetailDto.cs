using SnAbp.CrPlan.Enumer;
using System;
using System.Collections.Generic;
using SnAbp.Identity;
using Volo.Abp.Application.Dtos;
using SnAbp.CrPlan.Dtos;

namespace SnAbp.CrPlan.Dto.AlterRecord
{
    public class AlterRecordDetailDto : EntityDto<Guid>, IRepairTagDto
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
        public OrganizationDto Organization { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 完整编号
        /// </summary>
        public string FullNumber { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public YearMonthPlanState State { get; set; }
        public string StateStr { get; set; }

        /// <summary>
        /// 变更类型
        /// </summary>
        public SelectablePlanType AlterType { get; set; }
        public string AlterTypeStr { get; set; }

        /// <summary>
        /// 审批批号
        /// </summary>
        public Guid? AR_Key { get; set; }

        /// <summary>
        /// 变更关联
        /// </summary>
        public List<DailyPlanAlterDetailDto> DailyPlanAlters { get; set; } = new List<DailyPlanAlterDetailDto>();

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
